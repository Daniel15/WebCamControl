// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WebCamControl.Core.Exceptions;
using WebCamControl.Linux.Interop;
using static WebCamControl.Linux.Interop.Ioctl;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Represents a V4L2 webcam.
/// </summary>
public class LinuxCamera : ICamera, IAsyncDisposable
{
	private const string _generalDeviceDir = "/dev";
	
	private readonly ILogger<LinuxCamera> _logger;
	private readonly ILogger<LinuxCameraControl> _controlLogger;
	private readonly FileStream _deviceFile;
	private readonly IntPtr _fd;
	private readonly Capability _caps;
	private readonly LinuxCameraEvents _events;

	/// <summary>
	/// Creates a new <see cref="LinuxCamera"/>.
	/// </summary>
	/// <param name="rawName">e.g. "video0", "video1"</param>
	/// <param name="logger">Logger for debug logging</param>
	/// <param name="controlLogger">Logging for controls</param>
	/// <param name="eventsLogger">Logging for events</param>
	public LinuxCamera(
		string rawName,
		ILogger<LinuxCamera> logger,
		ILogger<LinuxCameraControl> controlLogger,
		ILogger<LinuxCameraEvents> eventsLogger
	)
	{
		_logger = logger;
		_controlLogger = controlLogger;
		RawName = rawName;
		var devicePath = Path.Join(_generalDeviceDir, rawName);
		_deviceFile = File.Open(devicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		_fd = _deviceFile.SafeFileHandle.DangerousGetHandle();
		_caps = GetCapabilities();
		_events = new LinuxCameraEvents(eventsLogger, _fd);
		
		logger.LogInformation(
			"Found camera: {name}. \nCapabilities: 0x{caps:X}\nDevice Capabilities: 0x{deviceCaps:X}\nSupported? {isSupported}", 
			Name,
			_caps.Capabilities,
			_caps.DeviceCapabilities,
			IsSupported
		);

		if (IsSupported)
		{
			CreateControls();
			_events.StartAsync();
		}
	}
	
	/// <summary>
	/// Whether this camera is supported by WebCamControl.
	/// </summary>
	public bool IsSupported => _caps.DeviceCapabilities.HasFlag(Capabilities.VideoCapture);

	/// <summary>
	/// Raw device name of the webcam (e.g. "video0")
	/// </summary>
	public string RawName { get; }

	/// <summary>
	/// User-friendly name of the webcam (e.g. "Insta360 Link")
	/// </summary>
	public string Name => $"{_caps.Device} ({RawName})";
	
	/// <summary>
	/// Gets or sets if automatic white balance is enabled.
	/// </summary>
	public BooleanControl? AutoWhiteBalance { get; private set; }

	/// <summary>
	/// Gets or sets the brightness
	/// </summary>
	public PercentControl? Brightness { get; private set; }
	
	/// <summary>
	/// Gets or sets the pan. This is a number of degrees between -180 and 180.
	/// </summary>
	public AngleControl? Pan { get; private set; }
	
	/// <summary>
	/// Gets or sets the white balance temperature.
	/// </summary>
	public ICameraControl<int>? Temperature { get; private set; }
	
	/// <summary>
	/// Gets or sets the tilt. This is a number of degrees between -180 and 180.
	/// </summary>
	public AngleControl? Tilt { get; private set; }

	/// <summary>
	/// Any controls that are not supported as one of the high-level fields above.
	/// </summary>
	public LinuxCameraAdvancedControls RawControls { get; private set; } = new();
	
	private void CreateControls()
	{
		var integers = new Dictionary<ControlID, LinuxCameraControl>();
		var booleans = new Dictionary<ControlID, BooleanControl>();
		
		// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/control.html#example-enumerating-all-controls
		var controlData = new QueryControl
		{
			ID = ControlID.NextControl
		};
		while (ioctl(_fd, IoctlCommand.QueryControl, ref controlData) == 0)
		{
			_logger.LogInformation(
				"Supports control: {Name} ({ControlID}). Type = {Type}, Flags = {Flags}",
				controlData.Name,
				controlData.ID,
				controlData.Type,
				controlData.Flags
			);

			var control = new LinuxCameraControl(_fd, controlData, _controlLogger, _events);

			switch (controlData.Type)
			{
				case ControlType.Integer:
					integers.Add(controlData.ID, control);
					break;
					
				case ControlType.Boolean:
					booleans.Add(controlData.ID, new BooleanControl(control));
					break;
				
				case ControlType.ControlClass:
					// Intentionally not supported
					break;

				case ControlType.Menu:
				case ControlType.Button:
				case ControlType.Integer64:
				case ControlType.String:
				default:
					_logger.LogInformation("=> Unsupported control type {Type}", controlData.Type);
					break;
			}
			
			// ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
			controlData.ID |= ControlID.NextControl;
		}
		
		_logger.LogInformation(
			"Finished enumerating controls. Errno = {Errno}", 
			Marshal.GetLastPInvokeError()
		);

		integers.Remove(ControlID.Brightness, out var brightness);
		Brightness = PercentControl.CreateIfNotNull(brightness);
		
		integers.Remove(ControlID.PanAbsolute, out var pan);
		Pan = AngleControl.CreateIfNotNull(pan);
		
		integers.Remove(ControlID.WhiteBalanceTemperature, out var temperature);
		if (temperature != null)
		{
			temperature.UserFriendlyValueDelegate = value => $"{value}K";			
		}
		Temperature = temperature;
		
		integers.Remove(ControlID.TiltAbsolute, out var tilt);
		Tilt = AngleControl.CreateIfNotNull(tilt);

		booleans.Remove(ControlID.AutoWhiteBalance, out var autoWhiteBalance);
		AutoWhiteBalance = autoWhiteBalance;
		
		RawControls = new LinuxCameraAdvancedControls
		{
			Booleans = booleans.ToImmutableDictionary(),
			Integers = integers.ToImmutableDictionary(),
		};
	}
	
	private Capability GetCapabilities()
	{
		var cap = new Capability();
		ioctl(_fd, IoctlCommand.QueryCapabilities, ref cap);
		InteropException.ThrowIfError();
		return cap;
	}

	public async ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		await _deviceFile.DisposeAsync();
	}
}
