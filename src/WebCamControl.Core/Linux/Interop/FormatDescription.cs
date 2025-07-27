// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_fmtdesc
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-fmt.html
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FormatDescription
{
	/// <summary>
	/// Number of the format in the enumeration, set by the application. This is in no way related
	/// to the pixelformat field.
	/// </summary>
	public uint Index;

	/// <summary>
	/// Type of the data stream, set by the application. 
	/// </summary>
	public BufferType Type;

	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-fmt.html#fmtdesc-flags
	/// </summary>
	public uint Flags;

	/// <summary>
	/// Description of the format, a NUL-terminated ASCII string. This information is intended for
	/// the user, for example: "YUV 4:2:2".
	/// </summary>
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string Description;

	/// <summary>
	/// The image format identifier. This is a four character code as computed by the v4l2_fourcc()
	/// macro.
	/// </summary>
	public uint PixelFormat;

	/// <summary>
	/// Media bus code restricting the enumerated formats, set by the application. Only applicable
	/// to drivers that advertise the V4L2_CAP_IO_MC capability, shall be 0 otherwise.
	/// </summary>
	public uint MediaBusCode;

	/// <summary>
	/// Reserved for future extensions. Drivers must set the array to zero.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public uint[] Reserved;
}
