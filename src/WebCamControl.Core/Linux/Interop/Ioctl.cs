// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

internal static class Ioctl
{
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-querycap.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(IntPtr fd, IoctlCommand command, ref Capability argp);

	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-g-ctrl.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(IntPtr fd, IoctlCommand command, ref Control argp);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-queryctrl.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		ref QueryControl argp
	);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-subscribe-event.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		EventSubscription argp
	);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-dqevent.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		ref Event argp
	);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-fmt.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		ref FormatDescription argp
	);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-framesizes.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		ref FrameSize argp
	);
	
	/// <summary>
	/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-enum-frameintervals.html
	/// </summary>
	[DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
	public static extern int ioctl(
		IntPtr fd,
		IoctlCommand command, 
		ref FrameInterval argp
	);
}
