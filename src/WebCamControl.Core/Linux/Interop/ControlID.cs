// ReSharper disable InconsistentNaming

namespace WebCamControl.Linux.Interop;

using static Constants;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/v4l2-controls.h
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/control.html
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/ext-ctrls-camera.html
/// </summary>
public enum ControlID : uint
{
	/// <summary>
	/// Picture brightness, or more precisely, the black level.
	/// V4L2_CID_BRIGHTNESS
	/// </summary>
	Brightness = V4L2_CID_BASE + 0,
	
	/// <summary>
	/// This control turns the camera horizontally to the specified position. Positive values move
	/// the camera to the right (clockwise when viewed from above), negative values to the left.
	/// Drivers should interpret the values as arc seconds, with valid values between -180 * 3600
	/// and +180 * 3600 inclusive.
	/// V4L2_CID_PAN_ABSOLUTE
	/// </summary>
	PanAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 8,
	
	/// <summary>
	/// This control turns the camera vertically to the specified position. Positive values move the
	/// camera up, negative values down. Drivers should interpret the values as arc seconds, with
	/// valid values between -180 * 3600 and +180 * 3600 inclusive.
	/// V4L2_CID_TILT_ABSOLUTE
	/// </summary>
	TiltAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 9,
}
