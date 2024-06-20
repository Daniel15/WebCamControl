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
}
