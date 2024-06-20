namespace WebCamControl.Linux.Interop;

/// <summary>
/// Constants from v4l2-controls.h.
/// </summary>
internal static class Constants
{
	public const int InappropriateIoctlForDevice = 25;
	
	/// <summary>
	/// Old-style 'user' controls
	/// </summary>
	private const uint V4L2_CTRL_CLASS_USER = 0x00980000;
	public const uint V4L2_CID_BASE = V4L2_CTRL_CLASS_USER | 0x900;
	
	/// <summary>
	/// Camera class controls
	/// </summary>
	private const uint V4L2_CTRL_CLASS_CAMERA = 0x009a0000;
	public const uint V4L2_CID_CAMERA_CLASS_BASE = V4L2_CTRL_CLASS_CAMERA | 0x900;
}
