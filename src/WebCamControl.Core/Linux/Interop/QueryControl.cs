using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/videodev2.h#L1942
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/control.html#example-enumerating-all-controls
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-queryctrl.html
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct QueryControl
{
	/// <summary>
	/// Identifies the control, set by the application.
	/// </summary>
	public ControlID ID;
	
	/// <summary>
	/// Type of control
	/// TODO: v4l2_ctrl_type
	/// </summary>
	public uint Type;
	
	/// <summary>
	/// Name of the control, a NUL-terminated ASCII string. This information is intended for the
	/// user.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string Name;

	/// <summary>
	/// Minimum value, inclusive. This field gives a lower bound for the control.
	/// </summary>
	public int Minimum;

	/// <summary>
	/// Maximum value, inclusive. This field gives an upper bound for the control.
	/// </summary>
	public int Maximum;

	/// <summary>
	/// This field gives the smallest change of an integer control actually affecting hardware.
	/// Often the information is needed when the user can change controls by keyboard or GUI
	/// buttons, rather than a slider. When for example a hardware register accepts values 0-511
	/// and the driver reports 0-65535, step should be 128.
	///
	/// Note that although signed, the step value is supposed to be always positive.
	/// </summary>
	public int Step;

	/// <summary>
	/// The default value of a V4L2_CTRL_TYPE_INTEGER, _BOOLEAN, _BITMASK, _MENU or _INTEGER_MENU
	/// control. Not valid for other types of controls.
	/// </summary>
	public int DefaultValue;

	/// <summary>
	/// Control flags
	/// </summary>
	public uint Flags;

	/// <summary>
	/// Reserved for future extensions.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public uint[] Reserved;
}
