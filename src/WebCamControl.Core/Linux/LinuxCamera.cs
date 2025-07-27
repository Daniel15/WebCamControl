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
public class LinuxCamera : ICamera
{
	private const string _generalDeviceDir = "/dev";
	
	private readonly ILogger<LinuxCamera> _logger;
	private readonly ILogger<LinuxCameraControl> _controlLogger;
	private readonly FileStream _deviceFile;
	private readonly IntPtr _fd;
	private readonly Capability _caps;
	private readonly LinuxCameraEvents _events;
	private readonly List<LinuxCameraControl> _allRawControls = [];

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
		VideoModes = ComputeVideoModes();
		
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
	public string Name
	{
		get
		{
			// Some cameras repeat the name twice for some reason
			// (e.g. "Insta360 Link: Insta360 Link"). In that case, only show the name once.
			var pieces = _caps.Device.Split(":", StringSplitOptions.TrimEntries);
			if (pieces.Length == 2 && pieces[0] == pieces[1])
			{
				return pieces[0];
			}

			return _caps.Device;
		}
	}

	/// <summary>
	/// Gets the video modes supported by the webcam.
	/// </summary>
	public IReadOnlyList<VideoMode> VideoModes { get; private init; }

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
	/// Gets or sets the zoom.
	/// </summary>
	public ICameraControl<int>? Zoom { get; private set; }

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
			_allRawControls.Add(control);

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

		integers.Remove(ControlID.ZoomAbsolute, out var zoom);
		if (zoom != null)
		{
			zoom.UserFriendlyValueDelegate = value => $"{value}%";
		}
		Zoom = zoom;

		booleans.Remove(ControlID.AutoWhiteBalance, out var autoWhiteBalance);
		AutoWhiteBalance = autoWhiteBalance;
		
		RawControls = new LinuxCameraAdvancedControls
		{
			Booleans = booleans.ToImmutableDictionary(),
			Integers = integers.ToImmutableDictionary(),
		};
	}

	private IReadOnlyList<VideoMode> ComputeVideoModes()
	{
		var videoModes = new List<VideoMode>();
		var fmtDesc = new FormatDescription
		{
			Index = 0,
			Type = BufferType.VideoCapture, 
		};
		while (ioctl(_fd, IoctlCommand.EnumerateFormats, ref fmtDesc) == 0)
		{
			++fmtDesc.Index;
			var frameSize = new FrameSize
			{
				Index = 0,
				PixelFormat = fmtDesc.PixelFormat,
			};
			while (ioctl(_fd, IoctlCommand.EnumerateFrameSizes, ref frameSize) == 0)
			{
				++frameSize.Index;
				// Only supports discrete frame sizes for now
				if (frameSize.Type != FrameSizeType.Discrete)
				{
					continue;
				}

				var frameInterval = new FrameInterval
				{
					Height = frameSize.Size.Discrete.Height,
					Index = 0,
					PixelFormat = fmtDesc.PixelFormat,
					Width = frameSize.Size.Discrete.Width,
				};
				while (ioctl(_fd, IoctlCommand.EnumerateFrameIntervals, ref frameInterval) == 0)
				{
					++frameInterval.Index;
					// Only supports discrete frame intervals for now
					if (frameInterval.Type != FrameIntervalType.Discrete)
					{
						continue;
					}

					var videoMode = new VideoMode
					{
						FrameRate = frameInterval.Interval.Discrete.Denominator /
						            frameInterval.Interval.Discrete.Numerator,
						Height = frameSize.Size.Discrete.Height,
						PixelFormatName = fmtDesc.Description,
						PixelFormatId = fmtDesc.PixelFormat,
						Width = frameSize.Size.Discrete.Width,
					};
					videoModes.Add(videoMode);
					_logger.LogInformation(
						"{Name}: Supports {FormatName} {Width}x{Height} {FrameRate} fps",
						Name,
						videoMode.PixelFormatName,
						videoMode.Width,
						videoMode.Height,
						videoMode.FrameRate
					);
				}
			}
		}

		return videoModes
			// Sort better modes to the top
			.OrderByDescending(x => x.Width)
			.ThenByDescending(x => x.Height)
			.ThenByDescending(x => x.FrameRate)
			.ThenBy(mode => mode.PixelFormatName switch
			{
				"H.264" => 1,
				"YUYV 4:2:2" => 2,
				"Motion-JPEG" => 4,
				_ => 3
			})
			.ToImmutableList();
	}
	
	private Capability GetCapabilities()
	{
		var cap = new Capability();
		ioctl(_fd, IoctlCommand.QueryCapabilities, ref cap);
		InteropException.ThrowIfError();
		return cap;
	}

	public void Dispose()
	{
		foreach (var control in _allRawControls)
		{
			control.Dispose();
		}
		_logger.LogInformation("Disposing camera");
		_events.Dispose();
		_deviceFile.Dispose();
		
		GC.SuppressFinalize(this);
	}
}
