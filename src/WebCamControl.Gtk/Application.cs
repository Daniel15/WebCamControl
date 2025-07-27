// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Reflection;
using Gio;
using GLib;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebCamControl.Core;
using WebCamControl.Core.Extensions;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Linux.Interop;
using Window = Adw.Window;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

/// <summary>
/// Root of the application. Creates the main window.
/// </summary>
public class Application
{
	private const int _returnCodeExceptionThrown = 1;
	
	private readonly IServiceProvider _provider;
	private readonly Adw.Application _app;
	private readonly ILogger<Application> _logger;
	private Window? _mainWindow;
	private bool _wasExceptionThrown;

	public Application(
		Adw.Application app,
		ICameraManager cameraManager,
		ILogger<Application> logger,
		IServiceProvider provider
	)
	{
		_app = app;
		_logger = logger;
		_provider = provider;

		_app.OnStartup += OnStartup;
		_app.OnActivate += OnActivate;
		
		NativeGettext.Init();
	}

	private int Run(string[] args)
	{
		var version = Assembly.GetEntryAssembly()
			?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			?.InformationalVersion ?? "Unknown";
		_logger.LogInformation("==== WebCamControl v{Version} ====", version);
		
		UnhandledException.SetHandler(OnUnhandledException);
		var errorCode = _app.RunWithSynchronizationContext(args);
		// `g_application_quit` (`_app.Quit()`) doesn't let us specify an exit code, so we have to
		// handle that ourselves.
		return _wasExceptionThrown ? _returnCodeExceptionThrown : errorCode;
	}

	private void OnUnhandledException(Exception ex)
	{
		_logger.LogError(ex, "Unhandled exception");
		_wasExceptionThrown = true;
		var dialog = new ErrorDialog(ex);
		dialog.OnResponse += (_, __) =>
		{
			_app.Release();
			_app.Quit();
		};
		// .Hold() ensures the app does not close until the dialog is closed
		_app.Hold();
		dialog.Present(_mainWindow);
	}
	
	private void OnStartup(Gio.Application sender, EventArgs args)
	{
		LoadResources();
		
		_app.ConfigureAction("quit", (_, _) => _app.Quit(), "<Ctrl>Q");
		_app.ConfigureAction("save_preset", (_, _) =>
		{
			var dialog = ActivatorUtilities.CreateInstance<SavePresetDialog>(
				_provider
			);
			dialog.Present(_mainWindow);
		}, "<Ctrl>S");
		
		_app.ConfigureAction("toggle_view", (_, _) =>
		{
			if (_mainWindow is MiniWindow)
			{
				ShowWindow<FullWindow>();	
			}
			else
			{
				ShowWindow<MiniWindow>();
			}
		}, "<Ctrl>T");
		
		_app.ConfigureAction("about", (_, _) => AboutWindow.Show(_mainWindow));
		
		var display = Gdk.Display.GetDefault()!;
		var cssProvider = CssProvider.New();
		cssProvider.LoadFromString(@"
			.settings-page {
				margin: 10px;
			}"
		);
		StyleContext.AddProviderForDisplay(
			display, 
			cssProvider, 
			global::Gtk.Constants.STYLE_PROVIDER_PRIORITY_APPLICATION
		);
	}

	private void OnActivate(Gio.Application application, EventArgs eventArgs)
	{
		ShowWindow<MiniWindow>();
	}

	private void ShowWindow<T>() where T: Window
	{
		if (_mainWindow != null)
		{
			_mainWindow.Destroy();
			_mainWindow.Dispose();
			_mainWindow = null;
		}
		
		_logger.LogInformation("Creating new {WindowType}", typeof(T).Name);
		_mainWindow = ActivatorUtilities.CreateInstance<T>(
			_provider
		);
		_mainWindow.Present();
	}
	
	/// <summary>
	/// Loads GLib resources. One of the reasons this is required is for the app icon.
	/// </summary>
	private void LoadResources()
	{
		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WebCamControl.Gtk.webcamcontrol.gresource");
		if (stream == null)
		{
			throw new Exception("Could not load resources");
		}
		var data = new byte[stream.Length];
		stream.Read(data, 0, data.Length);

		var resource = Resource.NewFromData(Bytes.New(data));
		resource.Register();
		
		// Add the resource path to icon theme
		var theme = IconTheme.GetForDisplay(Gdk.Display.GetDefault()!);
		theme.AddResourcePath("/com/daniel15/webcamcontrol/icons");
		
	}

	public static int Main(string[] args)
	{
		using var services = new ServiceCollection()
			.AddLogging(builder =>
			{
			    builder.ClearProviders();
			    builder.AddConsole();
			})
			.AddWebCamControl()
			.AddSingleton<Application>()
			.AddSingleton<Adw.Application>(
				_ => Adw.Application.New("com.daniel15.wcc", ApplicationFlags.DefaultFlags)
			)
			.BuildServiceProvider();
		
		var app = services.GetRequiredService<Application>();
		var returnCode = app.Run(args);
		Console.WriteLine(_("Exiting..."));
		return returnCode;
	}
}
