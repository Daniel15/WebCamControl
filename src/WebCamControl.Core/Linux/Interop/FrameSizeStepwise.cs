// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmsize_stepwise
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-framesizes.html#c.V4L.v4l2_frmsize_stepwise
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FrameSizeStepwise
{
	/// <summary>
	/// Minimum frame width [pixel].
	/// </summary>
	public uint MinWidth;
	
	/// <summary>
	/// Maximum frame width [pixel].
	/// </summary>
	public uint MaxWidth;
	
	/// <summary>
	/// Frame width step size [pixel].
	/// </summary>
	public uint StepWidth;
	
	/// <summary>
	/// Minimum frame height [pixel].
	/// </summary>
	public uint MinHeight;
	
	/// <summary>
	/// Maximum frame height [pixel].
	/// </summary>
	public uint MaxHeight;

	/// <summary>
	/// Frame height step size [pixel].
	/// </summary>
	public uint StepHeight;
}
