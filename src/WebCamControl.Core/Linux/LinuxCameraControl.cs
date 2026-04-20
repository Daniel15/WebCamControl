// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WebCamControl.Core.Exceptions;
using WebCamControl.Linux.Interop;
using static WebCamControl.Linux.Interop.Ioctl;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Implementation of <see cref="ICameraControl"/> that uses V4L2.
/// </summary>
public class LinuxCameraControl : ICameraControl, IDisposable
{
	private readonly IntPtr _fd;
	private QueryControl _controlData;
	private readonly ILogger<LinuxCameraControl> _logger;
	private readonly LinuxCameraEvents _events;
	private readonly ControlID _id;

	// TODO: Subscribe to changes and fire this event when changes occur outside the app 
	// (e.g. volatile controls)
	public event EventHandler? Changed;

	public LinuxCameraControl(
		IntPtr fd,
		QueryControl controlData,
		ILogger<LinuxCameraControl> logger,
		LinuxCameraEvents events
	)
	{
		_fd = fd;
		_controlData = controlData;
		_logger = logger;
		_events = events;
		_id = controlData.ID;

		_events.Subscribe(this, (evt) =>
		{
			var result = ioctl(_fd, IoctlCommand.QueryControl, ref _controlData);
			InteropException.ThrowIfError(result);
			Changed?.Invoke(this, EventArgs.Empty);
		});
	}
	
	public ControlID ID => _controlData.ID;
	public string Name => _controlData.Name;
	public int Minimum => _controlData.Minimum;
	public int Maximum => _controlData.Maximum;
	public int Step => _controlData.Step;

	public bool IsEnabled =>
		!_controlData.Flags.HasFlag(ControlFlags.Disabled) &&
		!_controlData.Flags.HasFlag(ControlFlags.Grabbed) &&
		!_controlData.Flags.HasFlag(ControlFlags.Inactive) &&
		!_controlData.Flags.HasFlag(ControlFlags.ReadOnly);

	public int Value
	{
		get
		{
			var control = new Control
			{
				ID = _id,
			};
			ioctl(_fd, IoctlCommand.GetControl, ref control);
			InteropException.ThrowIfError();
			_logger.LogInformation("GetControl({id}) = {value}", _id, control.Value);
			return control.Value;
		}

		set
		{
			var oldValue = Value;
			var clampedValue = ClampValue(value, oldValue);
			var control = new Control
			{
				ID = _id,
				Value = clampedValue,
			};
			ioctl(_fd, IoctlCommand.SetControl, ref control);
			var errno = Marshal.GetLastPInvokeError();
			if (errno != 0)
			{
				var errMessage = Marshal.GetPInvokeErrorMessage(errno);
				throw new Exception($"""
					Could not set `{_id}`: {errMessage} ({errno}). 
				    Min = {Minimum}, Max = {Maximum}, Step = {Step}
				    Current = {Value}, New = {value}, ClampedNew = {clampedValue}.
				    This could be caused by a bug in your camera's driver or firmware. Make sure the firmware is up-to-date."
				""" );
			}
			_logger.LogInformation("SetControl({id}, {value})", _id, clampedValue);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
	
	public string? UserFriendlyValue => UserFriendlyValueDelegate?.Invoke(Value);

	/// <summary>
	/// Sets the deriver to use to determine the user-friendly value.
	/// </summary>
	internal Func<int, string>? UserFriendlyValueDelegate { private get; set; }

	private int ClampValue(int newValue, int oldValue)
	{
		if (newValue > Maximum)
		{
			_logger.LogWarning(
				"SetControl({id}): {value} is above the maximum of {maximum}!",
				_id,
				newValue,
				Maximum
			);
			return Maximum;
		}
			
		if (newValue < Minimum)
		{
			_logger.LogWarning(
				"SetControl({id}): {value} is below the minimum of {minimum}!",
				_id,
				newValue,
				Minimum
			);
			return Minimum;
		}

		if (newValue % Step != 0)
		{
			// If we get here, it means the value isn't a multiple of `Step`. This shouldn't happen.
			// If value is going up, round up.
			// If value is going down, round down.
			var rounding = newValue > oldValue
				? MidpointRounding.ToPositiveInfinity
				: MidpointRounding.ToNegativeInfinity;
			var fixedNewValue = (int)(Math.Round(newValue / (double)Step, rounding) * Step);
			_logger.LogWarning(
				"SetControl({id}): {value} was not a multiple of {step}. Rounded to {fixedValue}",
				_id,
				newValue,
				Step,
				fixedNewValue
			);
			return fixedNewValue;
		}

		return newValue;
	}

	public void Dispose()
	{
		_events.Unsubscribe(this);
		GC.SuppressFinalize(this);
	}
}
