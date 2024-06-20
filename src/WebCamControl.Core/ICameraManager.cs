using System.Collections.Immutable;

namespace WebCamControl.Core;

/// <summary>
/// Handles enumerating all cameras attached to the system.
/// </summary>
public interface ICameraManager
{
	/// <summary>
	/// Enumerates the cameras attached to the system.
	/// </summary>
	/// <returns>The cameras</returns>
	public IReadOnlyList<ICamera> GetCameras();
	
	/// <summary>
	/// Determine the best camera to use
	/// </summary>
	/// <returns>The camera</returns>
	public ICamera GetDefaultCamera();
}
