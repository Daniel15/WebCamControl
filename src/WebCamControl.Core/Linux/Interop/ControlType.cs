namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_ctrl_type
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-queryctrl.html#c.V4L.v4l2_ctrl_type
/// https://github.com/torvalds/linux/blob/786c8248dbd33a5a7a07f7c6e55a7bfc68d2ca48/include/uapi/linux/videodev2.h#L1907
/// </summary>
public enum ControlType : uint
{
	/// <summary>
	/// An integer-valued control ranging from minimum to maximum inclusive. The step value
	/// indicates the increment between values.
	/// V4L2_CTRL_TYPE_INTEGER
	/// </summary>
	Integer = 1,
	
	/// <summary>
	/// A boolean-valued control. Zero corresponds to “disabled”, and one means “enabled”.
	/// V4L2_CTRL_TYPE_BOOLEAN
	/// </summary>
	Boolean = 2,
	
	/// <summary>
	/// The control has a menu of N choices. The names of the menu items can be enumerated with the
	/// VIDIOC_QUERYMENU ioctl.
	/// V4L2_CTRL_TYPE_MENU
	/// </summary>
	Menu = 3,
	
	/// <summary>
	/// A control which performs an action when set. Drivers must ignore the value passed with
	/// VIDIOC_S_CTRL and return an EACCES error code on a VIDIOC_G_CTRL attempt.
	/// V4L2_CTRL_TYPE_BUTTON
	/// </summary>
	Button = 4,

	/// <summary>
	/// A 64-bit integer valued control. Minimum, maximum and step size cannot be queried using
	/// VIDIOC_QUERYCTRL. Only VIDIOC_QUERY_EXT_CTRL can retrieve the 64-bit min/max/step values,
	/// they should be interpreted as n/a when using VIDIOC_QUERYCTRL.
	/// V4L2_CTRL_TYPE_INTEGER64
	/// </summary>
	Integer64 = 5,
	
	/// <summary>
	/// Not actually a control, but an enum value representing a class of controls.
	/// </summary>
	ControlClass = 6,
	
	/// <summary>
	/// The minimum and maximum string lengths. The step size means that the string must be
	/// (minimum + N * step) characters long for N ≥ 0. These lengths do not include the terminating
	/// zero, so in order to pass a string of length 8 to VIDIOC_S_EXT_CTRLS you need to set the
	/// size field of struct v4l2_ext_control to 9. For VIDIOC_G_EXT_CTRLS you can set the size
	/// field to maximum + 1. Which character encoding is used will depend on the string control
	/// itself and should be part of the control documentation.
	/// V4L2_CTRL_TYPE_STRING
	/// </summary>
	String = 7,
}
