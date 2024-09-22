// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// struct timespec
/// https://www.gnu.org/software/libc/manual/html_node/Time-Types.html#index-struct-timespec
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct TimeSpec
{
	/// <summary>
	/// time_t tv_sec
	/// </summary>
	public long Seconds;

	/// <summary>
	/// long int tv_nsec
	/// </summary>
	public long Nanoseconds;
}
