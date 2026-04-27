using Adw;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebCamControl.Core;
using WebCamControl.Gtk;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;
using WebCamControl.GtkWidgets;

namespace WebCamControl.GtkViews;

/// <summary>
/// Main window for the app - expanded view
/// </summary>
[GObject.Subclass<Adw.ApplicationWindow>(qualifiedName: nameof(FullWindow))]
[global::Gtk.Template<global::Gtk.AssemblyResource>("FullWindow.ui")]
public partial class FullWindow
{
	private ICameraManager _cameraManager = null!;
	private IPresets _presets = null!;
	private ILogger<FullWindow> _logger = null!;
	private CustomComboRow<ICamera> _cameraComboComponent = null!;
	private EventHandler? _presetsChangedHandler;

	[Connect] private ComboRow _cameraCombo; 
	[Connect] private ListBox _controls;
	[Connect] private ActionRow _exampleRow;
	[Connect] private ListBox _presetsList;
	[Connect] private ActionRow _panAndTiltRow;
	[Connect] private Box _panAndTiltButtons;

	public static FullWindow Create(IServiceProvider provider)
	{
		var app = provider.GetRequiredService<Adw.Application>();
		var cameraManager = provider.GetRequiredService<ICameraManager>();
		var presets = provider.GetRequiredService<IPresets>();
		var logger = provider.GetRequiredService<ILogger<FullWindow>>();
		var window = NewWithProperties([]);
		window.Configure(app, cameraManager, presets, logger);
		return window;
	}

	private void Configure(
		Adw.Application app,
		ICameraManager cameraManager,
		IPresets presets,
		ILogger<FullWindow> logger
	)
	{
		_cameraManager = cameraManager;
		_presets = presets;
		_logger = logger;
		Application = app;
		// TODO: Configure proper icon
		
		InitializeWidgets();
	}

	/// <summary>
	/// Initialize all widgets in the form
	/// </summary>
	private void InitializeWidgets()
	{
		InitializeCameras();
		InitializeCamera();
		InitializePresets();
		_presetsChangedHandler = (_, _) => InitializePresets();
		_presets.OnChange += _presetsChangedHandler;
	}
	
	/// <summary>
	/// Builds the list of all available cameras.
	/// </summary>
	private void InitializeCameras()
	{
		var cameras = _cameraManager.Cameras.ToArray();
		_cameraComboComponent = new CustomComboRow<ICamera>(_cameraCombo)
		{
			LabelCallback = camera => $"{camera.Name} ({camera.RawName})",
			Items = cameras,
			SelectedItem = _cameraManager.SelectedCamera,
		};
		_cameraComboComponent.OnSelectionChanged += (_, _) =>
		{
			var newCamera = _cameraComboComponent.SelectedItem;
			if (newCamera == null)
			{
				return;
			}
			
			_logger.LogInformation("Changing camera to {CameraName}", newCamera.Name);
			_cameraManager.SelectedCamera = newCamera;
			InitializeCamera();
		};
	}

	/// <summary>
	/// Initialize the controls for the camera. This is called both when the dialog is initially
	/// created, and when the selected camera is changed.
	/// </summary>
	private void InitializeCamera()
	{
		var camera = _cameraManager.SelectedCamera;
		_logger.LogInformation("Initializing controls for {CameraName}", camera.Name);

		// Remove any existing controls so we don't end up with duplicate ones when changing camera.
		_panAndTiltButtons.RemoveChildren();
		//_controls.Remove(_exampleRow);
		CleanupCameraControls();
		
		// Create controls for the selected camera
		_panAndTiltButtons.Append(PanAndTiltButtons.Create(camera));
		
		var potentialControls = new Widget?[]
		{
			CameraControlSlider.TryCreate(camera.Zoom),
			CameraControlSlider.TryCreate(camera.Brightness),
			CameraControlSwitch.TryCreate(camera.AutoWhiteBalance),
			CameraControlSlider.TryCreate(camera.Temperature),
		};

		foreach (var control in potentialControls.Where(x => x != null))
		{
			_controls.Append(control!);
		}
	}

	private void InitializePresets()
	{
		_presetsList.RemoveAll();
		foreach (var preset in _presets.PresetConfigs)
		{
			var row = PresetRow.Create(preset);
			row.OnDelete += (_, _) => _presets.Delete(preset);
			_presetsList.Append(row);
		}
	}
	
	private void CleanupCameraControls()
	{
		foreach (var child in _controls.GetChildren())
		{
			if (child == _panAndTiltRow)
			{
				continue;
			}

			_controls.Remove(child);
			child.Dispose();
		}
	}
	
	public override void Dispose()
	{
		CleanupCameraControls();

		if (_presetsChangedHandler != null)
		{
			_presets.OnChange -= _presetsChangedHandler;
			_presetsChangedHandler = null;
		}

		base.Dispose();
	}
}
