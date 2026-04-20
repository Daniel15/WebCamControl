// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_captureparm
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-g-parm.html#c.V4L.v4l2_captureparm
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct CaptureParam
{
	/// <summary>
	/// Supported modes.
	/// </summary>
	public uint Capability;

	/// <summary>
	/// Current mode.
	/// </summary>
	public uint CaptureMode;

	/// <summary>
	/// Time per frame in seconds.
	/// </summary>
	public Fraction TimePerFrame;

	/// <summary>
	/// Driver-specific extensions.
	/// </summary>
	public uint ExtendedMode;

	/// <summary>
	/// Number of buffers for read.
	/// </summary>
	public uint ReadBuffers;
}
