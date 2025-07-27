// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_fract
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enumstd.html?highlight=v4l2_fract#c.V4L.v4l2_fract
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Fraction
{
	public uint Numerator;
	public uint Denominator;
}
