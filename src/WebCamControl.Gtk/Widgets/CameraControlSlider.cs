using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A slider to modify a camera control.
/// </summary>
[GObject.Subclass<ActionRow>(qualifiedName: nameof(CameraControlSlider))]
public partial class CameraControlSlider
{
	private ICameraControl<int> _control = null!;
	private Scale _scale = null!;
	private EventHandler? _controlChangedHandler;

	private static CameraControlSlider Create(ICameraControl<int> control)
	{
		var slider = NewWithProperties([]);
		slider.Configure(control);
		return slider;
	}

	private void Configure(ICameraControl<int> control)
	{
		_control = control;
		Title = control.Name;
		
		var adjustment = Adjustment.New(
			value: control.Value,
			lower: control.Minimum,
			upper: control.Maximum,
			stepIncrement: control.Step,
			pageIncrement: control.Step,
			pageSize: 0
		);
		_scale = Scale.New(Orientation.Horizontal, adjustment);
		_scale.Halign = Align.Fill;
		_scale.Hexpand = true;
		_scale.OnChangeValue += (_, args) =>
		{
			control.Value = (int)args.Value;
			return false;
		};
		AddSuffix(_scale);

		_controlChangedHandler = (_, _) => UpdateState();
		control.Changed += _controlChangedHandler;
		UpdateState();
	}

	public override void Dispose()
	{
		if (_controlChangedHandler != null)
		{
			_control.Changed -= _controlChangedHandler;
			_controlChangedHandler = null;
		}

		base.Dispose();
	}

	/// <summary>
	/// Update the widget to match the camera control's value.
	/// </summary>
	private void UpdateState()
	{
		_scale.SetValue(_control.Value);
		var userFriendlyValue = _control.UserFriendlyValue;
		if (userFriendlyValue != null)
		{
			Subtitle = userFriendlyValue;
		}
		Sensitive = _control.IsEnabled;
	}

	public static CameraControlSlider? TryCreate(ICameraControl<int>? control)
	{
		return control == null ? null : Create(control);
	}
}
