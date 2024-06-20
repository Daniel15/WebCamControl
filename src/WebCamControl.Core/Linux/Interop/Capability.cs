using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-querycap.html
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/videodev2.h#L451
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Capability
{
	/// <summary>
	/// Name of the driver module (e.g. "bttv").
	/// </summary>
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
	public string Driver;

	/// <summary>
	/// Name of the card (e.g. "Hauppauge WinTV").
	/// </summary>
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string Device;

	/// <summary>
	/// Name of the bus (e.g. "PCI:" + pci_name(pci_dev) ).
	/// </summary>
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string BusInfo;

	/// <summary>
	/// KERNEL_VERSION.
	/// </summary>
	public uint Version;

	/// <summary>
	/// Capabilities of the physical device as a whole.
	/// TODO replace with DeviceCaps
	/// </summary>
	public Capabilities Capabilities;

	/// <summary>
	/// Capabilities accessed via this particular device (node).
	/// </summary>
	public Capabilities DeviceCapabilities;
	
	[MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
	public uint[] Reserved;
}
