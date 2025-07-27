// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_frmsizetypes 
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-framesizes.html#c.V4L.v4l2_frmsizetypes
/// </summary>
public enum FrameSizeType : uint
{
	/// <summary>
	/// V4L2_FRMSIZE_TYPE_DISCRETE
	/// </summary>
	Discrete = 1,
	
	/// <summary>
	/// V4L2_FRMSIZE_TYPE_CONTINUOUS
	/// </summary>
	Continuous = 2,
	
	/// <summary>
	/// V4L2_FRMSIZE_TYPE_STEPWISE
	/// </summary>
	Stepwise = 3,
}
