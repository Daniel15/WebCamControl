// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Renders up, down, left, and right buttons to adjust the pan and tilt.
/// </summary>
public class PanAndTiltButtons : Box
{
	private const float _panTiltAdjustmentAmount = 2f;

	private readonly ICamera _camera;
	private readonly Builder _builder;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly PressAndHoldButton _up = default!;
	[Connect] private readonly PressAndHoldButton _down = default!;
	[Connect] private readonly PressAndHoldButton _left = default!;
	[Connect] private readonly PressAndHoldButton _right = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
	
	public PanAndTiltButtons(ICamera camera)
	{
		_camera = camera;
		_builder = new Builder("PanAndTiltButtons.ui");
		var rootWidget = (Grid)_builder.GetObject("pan_and_tilt_buttons")!;
		Append(rootWidget);
		_builder.Connect(this);

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

	public override void Dispose()
	{
		GC.SuppressFinalize(this);
		base.Dispose();
		_builder.Dispose();
	}
}
