// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCamControl.Core.Configuration;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Enumerates all the cameras attached to the system using V4L2
/// </summary>
public class LinuxCameraManager : ICameraManager, IDisposable
{
	private readonly IOptionsMonitor<Config> _config;
	private readonly IConfigManager _configManager;
	private readonly ILogger<LinuxCamera> _cameraLogger;
	private readonly ILogger<LinuxCameraControl> _controlLogger;
	private readonly ILogger<LinuxCameraEvents> _eventsLogger;
	private readonly Lazy<IReadOnlyList<ICamera>> _cameras;

	private const string _v4lDeviceDir = "/sys/class/video4linux";
	private const string _preferredDeviceName = "Insta360 Link";

	public LinuxCameraManager(
		IOptionsMonitor<Config> config,
		IConfigManager configManager,
		ILogger<LinuxCamera> cameraLogger,
		ILogger<LinuxCameraControl> controlLogger,
		ILogger<LinuxCameraEvents> eventsLogger
	)
	{
		_config = config;
		_configManager = configManager;
		_cameraLogger = cameraLogger;
		_controlLogger = controlLogger;
		_eventsLogger = eventsLogger;
		_cameras = new Lazy<IReadOnlyList<ICamera>>(GetCameras);
	}
	
	private IReadOnlyList<LinuxCamera> GetCameras()
	{
		// TODO: We need to handle the case when a device is plugged in after startup
		
		if (!Directory.Exists(_v4lDeviceDir))
		{
			// If the directory is missing, no V4L devices are present.
			return ReadOnlyCollection<LinuxCamera>.Empty;
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

	public ICamera SelectedCamera
	{
		get
		{
			var cameras = _cameras.Value;
			if (cameras.Count == 0)
			{
				throw new Exception(_("No cameras were found"));
			}
			
			// 1. See if the user has explicitly selected a camera, and that camera is available.
			var selectedCameraName = _config.CurrentValue.SelectedCamera;
			if (!string.IsNullOrEmpty(selectedCameraName))
			{
				var selectedCamera = cameras.FirstOrDefault(x => x.Name == selectedCameraName);
				if (selectedCamera != null)
				{
					return selectedCamera;
				}
				_cameraLogger.LogError(
					"Selected camera '{CameraName}' not found! Reverting to default.",
					selectedCameraName
				);
			}
			
			// 2. Prefer Insta360 cams
			var preferredCam = cameras.FirstOrDefault(x => x.Name == _preferredDeviceName);
			if (preferredCam != null)
			{
				return preferredCam;
			}
			
			// 3. Pick the camera with the highest resolution. (if multiple cameras have the same 
			//    max resolution, it'll be arbitrary).
			return cameras
				.OrderByDescending(x => x.VideoModes.Max(y => y.Width * y.Height))
				.First();
		}
		set
		{
			_config.CurrentValue.SelectedCamera = value.Name;
			_configManager.Save();
		}
	}
	
	public IReadOnlyList<ICamera> Cameras => _cameras.Value;

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		if (_cameras.IsValueCreated)
		{
			foreach (var camera in _cameras.Value)
			{
				camera.Dispose();
			}
		}
	}
}
