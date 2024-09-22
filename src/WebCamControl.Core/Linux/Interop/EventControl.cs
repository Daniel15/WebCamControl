// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_event_ctrl
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-dqevent.html#c.V4L.v4l2_event_ctrl
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct EventControl
{
	/// <summary>
	/// A bitmask that tells what has changed.
	/// </summary>
	public ControlChanges Changes;

	/// <summary>
	/// The type of the control. See enum v4l2_ctrl_type.
	/// </summary>
	public ControlType Type;

	public ValueUnion Value;

	/// <summary>
	/// The control flags
	/// </summary>
	public ControlFlags Flags;

	public int Minimum;
	public int Maximum;
	public int Step;
	public int DefaultValue;

	[StructLayout(LayoutKind.Explicit)]
	public struct ValueUnion
	{
		/// <summary>
		/// The 32-bit value of the control for 32-bit control types. This is 0 for string controls
		/// since the value of a string cannot be passed using ioctl VIDIOC_DQEVENT.
		/// </summary>
		[FieldOffset(0)]
		public int Value;

		/// <summary>
		/// The 64-bit value of the control for 64-bit control types.
		/// </summary>
		[FieldOffset(0)]
		public long Value64;
	}
}
