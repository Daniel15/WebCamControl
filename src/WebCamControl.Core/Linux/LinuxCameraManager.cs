// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Enumerates all the cameras attached to the system using V4L2
/// </summary>
public class LinuxCameraManager : ICameraManager, IDisposable
{
	private readonly ILogger<LinuxCamera> _cameraLogger;
	private readonly ILogger<LinuxCameraControl> _controlLogger;
	private readonly ILogger<LinuxCameraEvents> _eventsLogger;
	private readonly Lazy<IReadOnlyList<ICamera>> _cameras;

	private const string _v4lDeviceDir = "/sys/class/video4linux";
	private const string _preferredDeviceName = "Insta360 Link";

	public LinuxCameraManager(
		ILogger<LinuxCamera> cameraLogger,
		ILogger<LinuxCameraControl> controlLogger,
		ILogger<LinuxCameraEvents> eventsLogger
	)
	{
		_cameraLogger = cameraLogger;
		_controlLogger = controlLogger;
		_eventsLogger = eventsLogger;
		_cameras = new Lazy<IReadOnlyList<ICamera>>(GetCameras);
	}
	
	private IReadOnlyList<ICamera> GetCameras()
	{
		// TODO: We need to handle the case when a device is plugged in after startup
		
		if (!Directory.Exists(_v4lDeviceDir))
		{
			// If the directory is missing, no V4L devices are present.
			return ReadOnlyCollection<ICamera>.Empty;
		}
		
		return Directory.GetDirectories(_v4lDeviceDir)
			.Select(dir => new LinuxCamera(
				Path.GetFileName(dir),
				_cameraLogger,
				_controlLogger,
				_eventsLogger
			))
			.Where(cam => cam.IsSupported)
			.ToImmutableArray();
	}

	public ICamera DefaultCamera
	{
		get
		{
			var cameras = _cameras.Value;
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

	public void Dispose()
	{
		if (_cameras.IsValueCreated)
		{
			foreach (var camera in _cameras.Value)
			{
				camera.Dispose();
			}
		}
	}
}
