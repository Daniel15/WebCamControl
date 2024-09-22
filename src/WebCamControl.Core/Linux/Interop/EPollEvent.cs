// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// epoll_event
/// https://man7.org/linux/man-pages/man3/epoll_event.3type.html
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct EPollEvent
{
	public EPollEventTypes Events;

	public EPollData? Data;
}
