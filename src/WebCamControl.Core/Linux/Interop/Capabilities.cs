namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/videodev2.h#L473
/// </summary>
[Flags]
public enum Capabilities : uint
{
	/// <summary>
	/// Is a video capture device
	/// V4L2_CAP_VIDEO_CAPTURE
	/// </summary>
	VideoCapture = 0x00000001
}
