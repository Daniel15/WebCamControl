using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WebCamControl.Core.Exceptions;
using WebCamControl.Linux.Interop;
using static WebCamControl.Linux.Interop.Ioctl;
using static WebCamControl.Linux.Interop.Constants;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Implementation of <see cref="ICameraControl"/> that uses V4L2.
/// </summary>
public class LinuxCameraControl : ICameraControl
{
	private readonly IntPtr _fd;
	private readonly QueryControl _controlData;
	private readonly ILogger<LinuxCameraControl> _logger;
	private readonly ControlID _id;

	public LinuxCameraControl(
		IntPtr fd,
		QueryControl controlData,
		ILogger<LinuxCameraControl> logger
	)
	{
		_fd = fd;
		_controlData = controlData;
		_logger = logger;
		_id = controlData.ID;
	}
	public string Name => _controlData.Name;
	public int Minimum => _controlData.Minimum;
	public int Maximum => _controlData.Maximum;
	public int Step => _controlData.Step;

	public int Value
	{
		get
		{
			var control = new Control
			{
				ID = _id,
			};
			ioctl(_fd, IoctlCommand.GetControl, ref control);
			InteropException.ThrowIfError();
			_logger.LogInformation("GetControl({id}) = {value}", _id, control.Value);
			return control.Value;
		}

		set
		{
			var control = new Control
			{
				ID = _id,
				Value = value,
			};
			ioctl(_fd, IoctlCommand.SetControl, ref control);
			InteropException.ThrowIfError();
			_logger.LogInformation("SetControl({id}, {value})", _id, value);
		}
	}

	public static LinuxCameraControl? TryCreate(
		IntPtr fd,
		ControlID controlId,
		ILogger<LinuxCameraControl> logger
	)
	{
		try
		{
			var queryControl = new QueryControl
			{
				ID = controlId
			};
			ioctl(fd, IoctlCommand.QueryControl, ref queryControl);

			var err = Marshal.GetLastPInvokeError();
			if (err == InappropriateIoctlForDevice)
			{
				logger.LogInformation(" => Does not support {ControlID}", controlId);
				return null;
			}
			if (err != 0)
			{
				throw new InteropException(Marshal.GetPInvokeErrorMessage(err));
			}

			logger.LogInformation("=> Supports {ControlID}", controlId);
			return new LinuxCameraControl(fd, queryControl, logger);
		}
		catch (Exception ex)
		{
			logger.LogWarning(" => ERROR for {ControlID}: {Error}", controlId, ex.Message);
			return null;
		}
	}
}
