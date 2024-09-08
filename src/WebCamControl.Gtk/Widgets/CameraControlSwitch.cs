using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A switch to modify a boolean camera control.
/// </summary>
public class CameraControlSwitch : Box
{
	private readonly ICameraControl<bool> _control;
	private readonly SwitchRow _switch;

	private CameraControlSwitch(ICameraControl<bool> control)
		: base(global::Gtk.Internal.Box.New(Orientation.Horizontal, 0), false)
	{
		_control = control;
		// SwitchRow is `final`, so we can't inherit from it. Instead, append the switch
		// as a child.
		_switch = SwitchRow.New();
		_switch.Title = control.Name;
		_switch.Hexpand = true;
		NotifySignal.Connect(
			_switch,
			(_, _) =>
			{
				control.Value = _switch.Active;
			},
			detail: SwitchRow.ActivePropertyDefinition.UnmanagedName
		);
		InsertChildAfter(_switch, null);

		control.Changed += (_, _) => UpdateState();
		UpdateState();
	}
	
	public static CameraControlSwitch? TryCreate(ICameraControl<bool>? control)
	{
		return control == null ? null : new CameraControlSwitch(control);
	}

	private void UpdateState()
	{
		_switch.Active = _control.Value;
	}
}
