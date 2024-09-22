// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using Adw;
using Gio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebCamControl.Core;
using WebCamControl.Core.Extensions;
using WebCamControl.Gtk.Extensions;

namespace WebCamControl.Gtk;

/// <summary>
/// Root of the application. Creates the main window.
/// </summary>
public class Application
{
	private readonly IServiceProvider _provider;
	private readonly Adw.Application _app;
	private readonly ILogger<Application> _logger;
	private Window? _mainWindow;
	private readonly ICamera _selectedCamera;

	public Application(
		Adw.Application app,
		ICameraManager cameraManager,
		ILogger<Application> logger,
		IServiceProvider provider
	)
	{
		_app = app;
		_logger = logger;
		_selectedCamera = cameraManager.DefaultCamera;
		_provider = provider;

		_app.OnStartup += OnStartup;
		_app.OnActivate += OnActivate;
	}

	private void Run(string[] args)
	{
		// TODO: Show error dialog on error
		_app.RunWithSynchronizationContext(args);
	}
	
	private void OnStartup(Gio.Application sender, EventArgs args)
	{
		_app.ConfigureAction("quit", (_, _) => _app.Quit(), "<Ctrl>Q");
		_app.ConfigureAction("save_preset", (_, _) =>
		{
			var dialog = ActivatorUtilities.CreateInstance<SavePresetDialog>(
				_provider,
				_selectedCamera
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
			_provider,
			_selectedCamera
		);
		_mainWindow.Present();
	}

	public static void Main(string[] args)
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
		app.Run(args);

		Console.WriteLine("Exiting...");
	}
}
