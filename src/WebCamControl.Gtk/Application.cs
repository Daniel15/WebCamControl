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
	private MainWindow? _mainWindow;
	private readonly ICamera _selectedCamera;

	public Application(
		Adw.Application app,
		ICameraManager cameraManager,
		IServiceProvider provider
	)
	{
		_app = app;
		_selectedCamera = cameraManager.GetDefaultCamera();
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
		_app.ConfigureMenuItem("quit", (_, _) => _app.Quit(), "<Ctrl>Q");
		_app.ConfigureMenuItem("save_preset", (_, _) =>
		{
			var dialog = ActivatorUtilities.CreateInstance<SavePresetDialog>(
				_provider,
				_selectedCamera
			);
			dialog.Present(_mainWindow);
		}, "<Ctrl>S");
	}

	private void OnActivate(Gio.Application application, EventArgs eventArgs)
	{
		_mainWindow ??= ActivatorUtilities.CreateInstance<MainWindow>(
			_provider,
			_selectedCamera
		);
		_mainWindow.Present();
	}

	public static void Main(string[] args)
	{
		var services = new ServiceCollection()
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
	}
}
