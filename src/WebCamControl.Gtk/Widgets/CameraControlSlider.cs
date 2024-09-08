using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A slider to modify a camera control.
/// </summary>
public class CameraControlSlider : ActionRow
{
	private readonly ICameraControl<int> _control;
	private readonly Scale _scale;

	private CameraControlSlider(ICameraControl<int> control)
		: base(Adw.Internal.ActionRow.New(), false)
	{
		_control = control;
		Title = control.Name;
		
		_scale = Scale.New(Orientation.Horizontal, new Adjustment
		{
			Lower = control.Minimum,
			Upper = control.Maximum,
			StepIncrement = control.Step
		});
		_scale.Halign = Align.Fill;
		_scale.Hexpand = true;
		_scale.OnChangeValue += (_, args) =>
		{
			control.Value = (int)args.Value;
			return false;
		};
		AddSuffix(_scale);

		control.Changed += (_, _) => UpdateState();
		UpdateState();
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
		// TODO: Disable widget if control is readonly/inactive/grabbed
	}

	public static CameraControlSlider? TryCreate(ICameraControl<int>? control)
	{
		return control == null ? null : new CameraControlSlider(control);
	}
}
