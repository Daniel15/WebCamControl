using Adw;
using Gtk;
using Microsoft.Extensions.Logging;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - expanded view
/// </summary>
public class FullWindow : Adw.Window
{
	private readonly ICameraManager _cameraManager;
	private readonly IPresets _presets;
	private readonly ILogger<FullWindow> _logger;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly CustomComboRow<ICamera> _cameraCombo = default!; 
	[Connect] private readonly ListBox _controls = default!;
	[Connect] private readonly ActionRow _exampleRow = default!;
	[Connect] private readonly ListBox _presetsList = default!;
	[Connect] private readonly ActionRow _panAndTiltRow = default!;
	[Connect] private readonly Box _panAndTiltButtons = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

	public FullWindow(
		Adw.Application app,
		ICameraManager cameraManager,
		IPresets presets,
		ILogger<FullWindow> logger
	) : this(new Builder("FullWindow.ui"), cameraManager, presets, logger)
	{
		Application = app;
	}

	private FullWindow(
		Builder builder,
		ICameraManager cameraManager,
		IPresets presets,
		ILogger<FullWindow> logger
	) : base(builder.GetPointer("full_window"), false)
	{
		_cameraManager = cameraManager;
		_presets = presets;
		_logger = logger;
		builder.Connect(this);
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
		_presets.OnChange += (_, _) => InitializePresets();
	}
	
	/// <summary>
	/// Builds the list of all available cameras.
	/// </summary>
	private void InitializeCameras()
	{
		_cameraCombo.LabelCallback = camera => $"{camera.Name} ({camera.RawName})";
		_cameraCombo.Items = _cameraManager.Cameras;
		_cameraCombo.SelectedItem = _cameraManager.SelectedCamera;
		NotifySignal.Connect(
			_cameraCombo,
			(_, _) =>
			{
				var newCamera = _cameraCombo.SelectedItem;
				if (newCamera == null)
				{
					return;
				}
				
				_logger.LogInformation("Changing camera to {CameraName}", newCamera.Name);
				_cameraManager.SelectedCamera = newCamera;
				InitializeCamera();
			},
			detail: ComboRow.SelectedPropertyDefinition.UnmanagedName
		);
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
		foreach (var child in _controls.GetChildren())
		{
			if (child != _panAndTiltRow)
			{
				_controls.Remove(child);
			}
		}
		
		// Create controls for the selected camera
		_panAndTiltButtons.Append(new PanAndTiltButtons(camera));
		
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
			var row = new PresetRow(preset);
			row.OnDelete += (_, _) => _presets.Delete(preset);
			_presetsList.Append(row);
		}
	}
}
