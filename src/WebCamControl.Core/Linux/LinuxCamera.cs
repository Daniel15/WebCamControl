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

	/// <summary>
	/// Creates a new <see cref="LinuxCamera"/>.
	/// </summary>
	/// <param name="rawName">e.g. "video0", "video1"</param>
	/// <param name="logger">Logger for debug logging</param>
	/// <param name="controlLogger">Logging for controls</param>
	public LinuxCamera(
		string rawName,
		ILogger<LinuxCamera> logger,
		ILogger<LinuxCameraControl> controlLogger
	)
	{
		_logger = logger;
		_controlLogger = controlLogger;
		RawName = rawName;
		var devicePath = Path.Join(_generalDeviceDir, rawName);
		_deviceFile = File.Open(devicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		_fd = _deviceFile.SafeFileHandle.DangerousGetHandle();
		_caps = GetCapabilities();
		CreateControls();

		logger.LogInformation(
			"Found camera: {name}. \nCapabilities: 0x{caps:X}\nDevice Capabilities: 0x{deviceCaps:X}\nSupported? {isSupported}", 
			Name,
			_caps.Capabilities,
			_caps.DeviceCapabilities,
			IsSupported
		);
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
	/// Gets or sets the brightness
	/// TODO: should be a percentage
	/// </summary>
	public ICameraControl? Brightness { get; private set; }
	
	/// <summary>
	/// Gets or sets the pan. This is a number of degrees between -180 and 180.
	/// TODO: Should be an angle
	/// </summary>
	public AngleControl? Pan { get; private set; }
	
	/// <summary>
	/// Gets or sets the tilt. This is a number of degrees between -180 and 180.
	/// </summary>
	public AngleControl? Tilt { get; private set; }


	private void CreateControls()
	{
		LinuxCameraControl? TryCreate(ControlID controlId)
			=> LinuxCameraControl.TryCreate(_fd, controlId, _controlLogger);
		
		Brightness = TryCreate(ControlID.Brightness);

		var pan = TryCreate(ControlID.PanAbsolute);
		Pan = pan == null ? null : new AngleControl(pan);
		var tilt = TryCreate(ControlID.TiltAbsolute);
		Tilt = tilt == null ? null : new AngleControl(tilt);
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
