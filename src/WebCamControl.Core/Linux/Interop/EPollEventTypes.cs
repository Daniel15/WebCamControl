// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// EPOLL* constants.
/// https://github.com/torvalds/linux/blob/da3ea35007d0af457a0afc87e84fddaebc4e0b63/include/uapi/linux/eventpoll.h#L31
/// </summary>
[Flags]
public enum EPollEventTypes : uint
{
	/// <summary>
	/// EPOLLIN
	/// </summary>
	In = 0x00000001,
	
	/// <summary>
	/// EPOLLPRI
	/// </summary>
	Priority = 0x00000002,
	
	/// <summary>
	/// EPOLLOUT
	/// </summary>
	Out = 0x00000004,
	
	/// <summary>
	/// EPOLLERR
	/// </summary>
	Error = 0x00000008,
	
	/// <summary>
	/// EPOLLHUP
	/// </summary>
	HangUp = 0x00000010,
	
	/// <summary>
	/// EPOLLNVAL
	/// </summary>
	Invalid = 0x00000020,
	
	/// <summary>
	/// EPOLLET
	/// </summary>
	EdgeTriggered = 0x80000000,
}
