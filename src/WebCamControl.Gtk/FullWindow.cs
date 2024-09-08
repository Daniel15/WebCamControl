using Adw;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Core.Linux;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - expanded view
/// </summary>
public class FullWindow : Adw.Window
{
	private readonly ICamera _camera;
	
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly ListBox _controls = default!;
	[Connect] private readonly ActionRow _exampleRow = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

	public FullWindow(
		Adw.Application app,
		ICamera camera
	) : this(new Builder("FullWindow.ui"), camera)
	{
		Application = app;
	}

	private FullWindow(
		Builder builder,
		ICamera camera
	) : base(builder.GetPointer("full_window"), false)
	{
		_camera = camera;
		builder.Connect(this);
		// TODO: Configure proper icon
		
		InitializeWidgets();
	}

	/// <summary>
	/// Initialize all widgets in the form
	/// </summary>
	private void InitializeWidgets()
	{
		_controls.Remove(_exampleRow);
		
		var potentialControls = new Widget?[]
		{
			CameraControlSlider.TryCreate(_camera.Brightness),
			CameraControlSwitch.TryCreate(_camera.AutoWhiteBalance),
			CameraControlSlider.TryCreate(_camera.Temperature),
		};

		foreach (var control in potentialControls.Where(x => x != null))
		{
			_controls.Append(control!);
		}
	}
}
