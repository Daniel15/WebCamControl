// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmsizeenum
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-framesizes.html#c.V4L.v4l2_frmsizeenum
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FrameSize
{
	/// <summary>
	/// IN: Index of the given frame size in the enumeration.
	/// </summary>
	public uint Index;

	/// <summary>
	/// IN: Pixel format for which the frame sizes are enumerated.
	/// </summary>
	public uint PixelFormat;

	/// <summary>
	/// OUT: Frame size type the device supports.
	/// </summary>
	public FrameSizeType Type;

	/// <summary>
	/// Frame size
	/// </summary>
	public FrameSizeUnion Size;

	/// <summary>
	/// Reserved space for future use. Must be zeroed by drivers and applications.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public byte[] Reserved;

	[StructLayout(LayoutKind.Explicit)]
	public struct FrameSizeUnion
	{
		[FieldOffset(0)]
		public FrameSizeDiscrete Discrete;

		[FieldOffset(0)]
		public FrameSizeStepwise Stepwise;
	}
}
