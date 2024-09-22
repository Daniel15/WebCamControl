// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// EPOLL_CTL_* constants
/// https://man7.org/linux/man-pages/man2/epoll_ctl.2.html
/// </summary>
internal enum EPollOperation
{
	/// <summary>
	/// EPOLL_CTL_ADD
	/// </summary>
	Add = 1,
	
	/// <summary>
	/// EPOLL_CTL_DEL
	/// </summary>
	Delete = 2
}
