// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_event_subscription
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-subscribe-event.html#c.V4L.v4l2_event_subscription
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public class EventSubscription
{
	/// <summary>
	/// Type of the event
	/// </summary>
	public EventType Type;
	
	/// <summary>
	/// ID of the event source. If there is no ID associated with the event source, then set this
	/// to 0. Whether an event needs an ID depends on the event type.
	/// </summary>
	public uint ID;
	
	/// <summary>
	/// Event flags
	/// </summary>
	public uint Flags;
	
	/// <summary>
	/// Reserved for future extensions. Drivers and applications must set the array to zero.
	/// </summary>
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	public uint[] Reserved = new uint[5];
}
