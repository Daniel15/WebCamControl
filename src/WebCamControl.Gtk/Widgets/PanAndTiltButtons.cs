// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Renders up, down, left, and right buttons to adjust the pan and tilt.
/// </summary>
[GObject.Subclass<Grid>(qualifiedName: nameof(PanAndTiltButtons))]
[Template<EntryAssemblyResource>("PanAndTiltButtons.ui")]
public partial class PanAndTiltButtons
{
	private const float _panTiltAdjustmentAmount = 2f;

	private ICamera _camera = null!;

	[Connect] private PressAndHoldButton _up;
	[Connect] private PressAndHoldButton _down;
	[Connect] private PressAndHoldButton _left;
	[Connect] private PressAndHoldButton _right;
	
	public static PanAndTiltButtons New(ICamera camera)
	{
		// Ensure the PressAndHoldButton GType is registered before creating the PanAndTiltButtons.
		// Otherwise, an error will be thrown on creation.
		// https://github.com/gircore/gir.core/issues/1517
		PressAndHoldButton.GetGType();
		
		var buttons = NewWithProperties([]);
		buttons.Configure(camera);
		return buttons;
	}

	private void Configure(ICamera camera)
	{
		_camera = camera;
		AttachEvents();
	}

	private void AttachEvents()
	{
		_left.DisableCameraControlIfUnsupported(_camera.Pan);
		_right.DisableCameraControlIfUnsupported(_camera.Pan);
		if (_camera.Pan != null)
		{
			_left.OnHeld += (_, _) => _camera.Pan.Value -= _panTiltAdjustmentAmount;
			_right.OnHeld += (_, _) => _camera.Pan.Value += _panTiltAdjustmentAmount;
		}
	
		_down.DisableCameraControlIfUnsupported(_camera.Tilt);
		_up.DisableCameraControlIfUnsupported(_camera.Tilt);
		if (_camera.Tilt != null)
		{
			_down.OnHeld += (_, _) => _camera.Tilt.Value -= _panTiltAdjustmentAmount;
			_up.OnHeld += (_, _) => _camera.Tilt.Value += _panTiltAdjustmentAmount;
		}
	}

}
