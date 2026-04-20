// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_format
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-g-fmt.html
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Format
{
	/// <summary>
	/// Type of the data stream, see v4l2_buf_type.
	/// </summary>
	[FieldOffset(0)]
	public BufferType Type;

	/// <summary>
	/// Union containing format-specific data. Starts at offset 8 due to 8-byte alignment of the
	/// union in the kernel struct (v4l2_window contains a pointer).
	/// </summary>
	[FieldOffset(8)]
	public FormatUnion Data;

	[StructLayout(LayoutKind.Explicit, Size = 200)]
	public struct FormatUnion
	{
		/// <summary>
		/// Definition of an image format, used by video capture and output devices.
		/// </summary>
		[FieldOffset(0)]
		public PixelFormat PixelFormat;
	}
}
