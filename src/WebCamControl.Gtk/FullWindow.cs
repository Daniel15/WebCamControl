using Adw;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Core.Linux;
using WebCamControl.Gtk.Extensions;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - expanded view
/// </summary>
public class FullWindow : Adw.Window
{
	private readonly ICamera _camera;
	
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly SwitchRow _autoWhiteBalance = default!;
	[Connect] private readonly Scale _brightness = default!;
	[Connect] private readonly ActionRow _temperatureRow = default!;	
	[Connect] private readonly Scale _temperature = default!;
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
		UpdateFromCameraControls();
	}

	/// <summary>
	/// Initialize all widgets in the form
	/// </summary>
	private void InitializeWidgets()
	{
		AttachSwitchToCameraControl(_autoWhiteBalance, _camera.AutoWhiteBalance);
		AttachSliderToCameraControl(_temperature, _camera.Temperature);
		AttachSliderToCameraControl(_brightness, _camera.Brightness);
	}

	/// <summary>
	/// Sync camera settings to the widgets.
	/// </summary>
	private void UpdateFromCameraControls()
	{
		_autoWhiteBalance.Active = _camera.AutoWhiteBalance?.Value ?? false;
		_temperature.SetValue(_camera.Temperature?.Value ?? 0);
		_temperature.Sensitive = _camera.Temperature != null && !_autoWhiteBalance.Active;
		if (_camera.Temperature != null)
		{
			_temperatureRow.Subtitle = _camera.Temperature.Value + "K";
		}
		_brightness.SetValue(_camera.Brightness?.Value ?? 0);
	}

	/// <summary>
	/// Configures a <see cref="SwitchRow"/> to control a boolean camera control.
	/// </summary>
	private void AttachSwitchToCameraControl(SwitchRow widget, ICameraControl<bool>? control)
	{
		widget.DisableCameraControlIfUnsupported(control);
		if (control == null)
		{
			return;
		}
		
		NotifySignal.Connect(
			widget,
			(_, _) =>
			{
				control.Value = widget.Active;
				UpdateFromCameraControls();
			},
			detail: SwitchRow.ActivePropertyDefinition.UnmanagedName
		);
	}

	/// <summary>
	/// Configures a <see cref="Scale"/> to control an integer camera control.
	/// </summary>
	private void AttachSliderToCameraControl(Scale widget, ICameraControl<int>? control)
	{
		widget.DisableCameraControlIfUnsupported(control);
		if (control == null)
		{
			return;
		}
		
		widget.Adjustment = new()
		{
			Lower = control.Minimum,
			Upper = control.Maximum,
			StepIncrement = control.Step,
			Value = control.Value,
		};
		widget.OnChangeValue += (_, args) =>
		{
			control.Value = (int)args.Value;
			UpdateFromCameraControls();
			return false;
		};
	}
}
