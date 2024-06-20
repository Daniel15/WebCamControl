using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Enumerates all the cameras attached to the system using V4L2
/// </summary>
public class LinuxCameraManager : ICameraManager
{
	private readonly ILogger<LinuxCamera> _cameraLogger;
	private readonly ILogger<LinuxCameraControl> _controlLogger;

	private const string _v4lDeviceDir = "/sys/class/video4linux";
	private const string _preferredDeviceName = "Insta360 Link";

	public LinuxCameraManager(
		ILogger<LinuxCamera> cameraLogger,
		ILogger<LinuxCameraControl> controlLogger
	)
	{
		_cameraLogger = cameraLogger;
		_controlLogger = controlLogger;
	}
	
	public IReadOnlyList<ICamera> GetCameras()
	{
		if (!Directory.Exists(_v4lDeviceDir))
		{
			// If the directory is missing, no V4L devices are present.
			return ReadOnlyCollection<ICamera>.Empty;
		}
		
		return Directory.GetDirectories(_v4lDeviceDir)
			.Select(dir => new LinuxCamera(Path.GetFileName(dir), _cameraLogger, _controlLogger))
			.Where(cam => cam.IsSupported)
			.ToImmutableArray();
	}

	public ICamera GetDefaultCamera()
	{
		var cameras = GetCameras();
		if (cameras.Count == 0)
		{
			throw new Exception("No cameras were found");
		}
		
		// 1. Prefer Insta360 cams
		var preferredCam = cameras.FirstOrDefault(x => x.Name == _preferredDeviceName);
		if (preferredCam != null)
		{
			return preferredCam;
		}
		
		// 2. TODO: maybe pick camera with most controls, or highest resolution?
		
		// 3. Pick the first one
		return cameras[0];
	}
}
