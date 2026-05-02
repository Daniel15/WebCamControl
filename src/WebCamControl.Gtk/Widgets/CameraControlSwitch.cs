// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A switch to modify a boolean camera control.
/// </summary>
[GObject.Subclass<Box>(qualifiedName: nameof(CameraControlSwitch))]
public partial class CameraControlSwitch
{
	private ICameraControl<bool> _control = null!;
	private SwitchRow _switch = null!;
	private EventHandler? _controlChangedHandler;

	private static CameraControlSwitch New(ICameraControl<bool> control)
	{
		var controlSwitch = NewWithProperties([]);
		controlSwitch.Configure(control);
		return controlSwitch;
	}

	private void Configure(ICameraControl<bool> control)
	{
		_control = control;
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
	
	public static CameraControlSwitch? TryCreate(ICameraControl<bool>? control)
	{
		return control == null ? null : New(control);
	}

	private void UpdateState()
	{
		_switch.Active = _control.Value;
		_switch.Sensitive = _control.IsEnabled;
	}
}
