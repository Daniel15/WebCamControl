// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmivaltypes 
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-frameintervals.html?highlight=vidioc_enum_frameintervals#c.V4L.v4l2_frmivaltypes
/// </summary>
public enum FrameIntervalType : uint
{
	/// <summary>
	/// V4L2_FRMIVAL_TYPE_DISCRETE
	/// </summary>
	Discrete = 1,
	
	/// <summary>
	/// V4L2_FRMIVAL_TYPE_CONTINUOUS
	/// </summary>
	Continuous = 2,
	
	/// <summary>
	/// V4L2_FRMIVAL_TYPE_STEPWISE
	/// </summary>
	Stepwise = 3,
}
