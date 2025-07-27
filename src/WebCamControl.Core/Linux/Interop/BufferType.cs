// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_buf_type
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/buffer.html#c.V4L.v4l2_buf_type
/// </summary>
public enum BufferType : uint
{
	/// <summary>
	/// V4L2_BUF_TYPE_VIDEO_CAPTURE
	/// </summary>
	VideoCapture = 1,
}
