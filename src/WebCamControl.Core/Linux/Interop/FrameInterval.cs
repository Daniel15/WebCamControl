// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmivalenum
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-frameintervals.html?highlight=vidioc_enum_frameintervals#c.V4L.v4l2_frmivalenum
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FrameInterval
{
	/// <summary>
	/// IN: Index of the given frame interval in the enumeration.
	/// </summary>
	public uint Index;

	/// <summary>
	/// IN: Pixel format for which the frame intervals are enumerated.
	/// </summary>
	public uint PixelFormat;

	/// <summary>
	/// IN: Frame width for which the frame intervals are enumerated.
	/// </summary>
	public uint Width;

	/// <summary>
	/// IN: Frame height for which the frame intervals are enumerated.
	/// </summary>
	public uint Height;

	/// <summary>
	/// OUT: Frame interval type the device supports.
	/// </summary>
	public FrameIntervalType Type;

	/// <summary>
	/// OUT: Frame interval with the given index.
	/// </summary>
	public FrameIntervalUnion Interval;

	[StructLayout(LayoutKind.Explicit)]
	public struct FrameIntervalUnion
	{
		[FieldOffset(0)]
		public Fraction Discrete;
		
		[FieldOffset(0)]
		public FrameIntervalStepwise Stepwise;
	}

	/// <summary>
	/// Reserved space for future use. Must be zeroed by drivers and applications.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public byte[] Reserved;
}
