// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// Handles subscribing to events using epoll
/// https://man7.org/linux/man-pages/man7/epoll.7.html
/// https://copyconstruct.medium.com/the-method-to-epolls-madness-d9d2d6378642
/// </summary>
internal static class EPoll
{
	/// <summary>
	/// https://man7.org/linux/man-pages/man2/epoll_create.2.html
	/// </summary>
	[DllImport("libc", SetLastError = true)]
	public static extern int epoll_create1(int size);

	/// <summary>
	/// https://man7.org/linux/man-pages/man2/epoll_ctl.2.html
	/// </summary>
	[DllImport("libc", SetLastError = true)]
	public static extern int epoll_ctl(
		int epfd,
		EPollOperation op,
		IntPtr fd,
		ref EPollEvent evt
	);
	
	/// <summary>
	/// https://man7.org/linux/man-pages/man2/epoll_wait.2.html
	/// </summary>
	[DllImport("libc", SetLastError = true)]
	public static extern int epoll_wait(
		int epfd,
		ref EPollEvent events,
		int maxEvents,
		int timeout
	);
}
