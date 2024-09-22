// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_event
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-dqevent.html#vidioc-dqevent
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Event
{
	/// <summary>
	/// Type of the event
	/// </summary>
	public EventType Type;
	
	/// <summary>
	/// Event data
	/// </summary>
	public EventDataUnion Data;
	
	/// <summary>
	/// Number of pending events excluding this one.
	/// </summary>
	public uint Pending;
	
	/// <summary>
	/// Event sequence number. The sequence number is incremented for every subscribed event that
	/// takes place. If sequence numbers are not contiguous it means that events have been lost.
	/// </summary>
	public uint Sequence;
	
	/// <summary>
	/// Event timestamp. The timestamp has been taken from the CLOCK_MONOTONIC clock. To access the
	/// same clock outside V4L2, use clock_gettime().
	/// </summary>
	public TimeSpec Timestamp;

	/// <summary>
	/// The ID associated with the event source. If the event does not have an associated ID (this
	/// depends on the event type), then this is 0.
	/// </summary>
	public uint ID;

	/// <summary>
	/// Reserved for future extensions. Drivers must set the array to zero.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public uint[] Reserved;
	
	[StructLayout(LayoutKind.Explicit, Size = 64)] 
	public struct EventDataUnion
	{
		/// <summary>
		/// Event data for event V4L2_EVENT_CTRL.
		/// </summary>
		[FieldOffset(0)]
		public EventControl ControlData;
		
		// /// <summary>
		// /// Event data. Defined by the event type. The union should be used to define easily
		// /// accessible type for events.
		// /// </summary>
		// [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		// [FieldOffset(0)]
		// public byte[] RawData;
	}
}
