// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-dqevent.html#ctrl-changes-flags
/// </summary>
[Flags]
public enum ControlChanges : uint
{
	/// <summary>
	/// This control event was triggered because the value of the control changed. Special cases:
	/// Volatile controls do not generate this event; If a control has the
	/// V4L2_CTRL_FLAG_EXECUTE_ON_WRITE flag set, then this event is sent as well, regardless its
	/// value.
	/// </summary>
	Value = 0x0001,
	
	/// <summary>
	/// This control event was triggered because the control flags changed.
	/// </summary>
	Flags = 0x0002,
	
	/// <summary>
	/// This control event was triggered because the minimum, maximum, step or the default value of
	/// the control changed.
	/// </summary>
	Range = 0x0004,
	
	/// <summary>
	/// This control event was triggered because the dimensions of the control changed. Note that
	/// the number of dimensions remains the same.
	/// </summary>
	Dimensions = 0x0008,
}
