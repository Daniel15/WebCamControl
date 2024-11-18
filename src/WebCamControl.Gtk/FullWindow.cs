using Adw;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - expanded view
/// </summary>
public class FullWindow : Adw.Window
{
	private readonly ICamera _camera;
	private readonly IPresets _presets;
	
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly ListBox _controls = default!;
	[Connect] private readonly ActionRow _exampleRow = default!;
	[Connect] private readonly ListBox _presetsList = default!;
	[Connect] private readonly Box _panAndTiltButtons = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

	public FullWindow(
		Adw.Application app,
		ICamera camera,
		IPresets presets
	) : this(new Builder("FullWindow.ui"), camera, presets)
	{
		Application = app;
	}

	private FullWindow(
		Builder builder,
		ICamera camera,
		IPresets presets
	) : base(builder.GetPointer("full_window"), false)
	{
		_camera = camera;
		_presets = presets;
		builder.Connect(this);
		// TODO: Configure proper icon
		
		InitializeWidgets();
	}

	/// <summary>
	/// Initialize all widgets in the form
	/// </summary>
	private void InitializeWidgets()
	{
		_panAndTiltButtons.Append(new PanAndTiltButtons(_camera));
		
		_controls.Remove(_exampleRow);
		
		var potentialControls = new Widget?[]
		{
			CameraControlSlider.TryCreate(_camera.Zoom),
			CameraControlSlider.TryCreate(_camera.Brightness),
			CameraControlSwitch.TryCreate(_camera.AutoWhiteBalance),
			CameraControlSlider.TryCreate(_camera.Temperature),
		};

		foreach (var control in potentialControls.Where(x => x != null))
		{
			_controls.Append(control!);
		}
		
		InitializePresets();
		_presets.OnChange += (_, _) => InitializePresets();
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
