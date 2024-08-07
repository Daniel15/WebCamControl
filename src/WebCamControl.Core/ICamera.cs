namespace WebCamControl.Core;

/// <summary>
/// Represents a webcam.
/// </summary>
public interface ICamera
{
	// Gets whether this device is supported.
	public bool IsSupported { get; }
	
	/// <summary>
	/// Raw device name of the webcam (e.g. "video0").
	/// </summary>
	public string RawName { get; }
	
	/// <summary>
	/// User-friendly name of the webcam (e.g. "Insta360 Link")
	/// </summary>
	public string Name { get; }
	
	/// <summary>
	/// Gets or sets the brightness
	/// </summary>
	public PercentControl? Brightness { get; }
	
	/// <summary>
	/// Gets or sets the pan. This is a number of degrees between -180 and 180.
	/// </summary>
	public AngleControl? Pan { get; }
	
	/// <summary>
	/// Gets or sets the tilt. This is a number of degrees between -180 and 180.
	/// </summary>
	public AngleControl? Tilt { get; }
}
