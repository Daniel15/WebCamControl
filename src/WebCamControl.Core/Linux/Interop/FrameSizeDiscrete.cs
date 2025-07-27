// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmsize_discrete
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-framesizes.html#c.V4L.v4l2_frmsize_discrete
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FrameSizeDiscrete
{
	/// <summary>
	/// Width of the frame [pixel].
	/// </summary>
	public uint Width;
	
	/// <summary>
	/// Height of the frame [pixel].
	/// </summary>
	public uint Height;
}
