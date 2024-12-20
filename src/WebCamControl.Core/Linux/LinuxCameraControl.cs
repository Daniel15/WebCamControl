// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

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
			var clampedValue = ClampValue(value);
			var control = new Control
			{
				ID = _id,
				Value = clampedValue,
			};
			ioctl(_fd, IoctlCommand.SetControl, ref control);
			InteropException.ThrowIfError();
			_logger.LogInformation("SetControl({id}, {value})", _id, clampedValue);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
	
	public string? UserFriendlyValue => UserFriendlyValueDelegate?.Invoke(Value);

	/// <summary>
	/// Sets the deriver to use to determine the user-friendly value.
	/// </summary>
	internal Func<int, string>? UserFriendlyValueDelegate { private get; set; }

	private int ClampValue(int value)
	{
		if (value > Maximum)
		{
			_logger.LogWarning(
				"SetControl({id}): {value} is above the maximum of {maximum}!",
				_id,
				value,
				Maximum
			);
			return Maximum;
		}
			
		if (value < Minimum)
		{
			_logger.LogWarning(
				"SetControl({id}): {value} is below the minimum of {minimum}!",
				_id,
				value,
				Minimum
			);
			return Minimum;
		}

		return value;
	}

	public void Dispose()
	{
		_events.Unsubscribe(this);
		GC.SuppressFinalize(this);
	}
}
