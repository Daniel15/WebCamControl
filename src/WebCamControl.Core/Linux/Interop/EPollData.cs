// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// epoll_data
/// https://man7.org/linux/man-pages/man3/epoll_data.3type.html
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct EPollData
{
	[FieldOffset(0)]
	public IntPtr ptr;
	
	[FieldOffset(0)]
	public int fd;
	
	[FieldOffset(0)]
	public uint u32;
	
	[FieldOffset(0)]
	public ulong u64;
}
