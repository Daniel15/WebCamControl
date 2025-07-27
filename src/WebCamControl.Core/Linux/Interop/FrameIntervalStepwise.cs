// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmival_stepwise
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-frameintervals.html?highlight=vidioc_enum_frameintervals#c.V4L.v4l2_frmival_stepwise
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FrameIntervalStepwise
{
	/// <summary>
	/// Minimum frame interval [s].
	/// </summary>
	public Fraction Minimum;

	/// <summary>
	/// Maximum frame interval [s].
	/// </summary>
	public Fraction Maximum;

	/// <summary>
	/// Frame interval step size [s].
	/// </summary>
	public Fraction Step;
}
