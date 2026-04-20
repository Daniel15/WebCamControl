// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_streamparm
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-g-parm.html
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct StreamParam
{
	/// <summary>
	/// Type of the data stream, see v4l2_buf_type.
	/// </summary>
	[FieldOffset(0)]
	public BufferType Type;

	/// <summary>
	/// Stream type-dependent parameters.
	/// </summary>
	[FieldOffset(4)]
	public StreamParamUnion Data;

	[StructLayout(LayoutKind.Explicit, Size = 200)]
	public struct StreamParamUnion
	{
		/// <summary>
		/// Parameters for capture devices.
		/// </summary>
		[FieldOffset(0)]
		public CaptureParam Capture;
	}
}
