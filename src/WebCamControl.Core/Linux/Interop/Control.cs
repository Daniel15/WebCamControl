using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/videodev2.h#L1818
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Control
{
	public ControlID ID;
	public int Value;
}
