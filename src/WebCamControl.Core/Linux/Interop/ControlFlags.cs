namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/videodev2.h#L1983
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-queryctrl.html#control-flags
/// </summary>
[Flags]
public enum ControlFlags : uint
{
	/// <summary>
	/// This control is permanently disabled and should be ignored by the application. Any attempt
	/// to change the control will result in an <c>EINVAL</c> error code.
	/// V4L2_CTRL_FLAG_DISABLED
	/// </summary>
	Disabled = 0x0001,
	
	/// <summary>
	/// This control is temporarily unchangeable, for example because another application took over
	/// control of the respective resource. Such controls may be displayed specially in a user
	/// interface. Attempts to change the control may result in an <c>EBUSY</c> error code.
	/// V4L2_CTRL_FLAG_GRABBED
	/// </summary>
	Grabbed = 0x0002,
	
	/// <summary>
	/// This control is permanently readable only. Any attempt to change the control will result in
	/// an <c>EINVAL</c> error code.
	/// V4L2_CTRL_FLAG_READ_ONLY
	/// </summary>
	ReadOnly = 0x0004,
	
	/// <summary>
	/// A hint that changing this control may affect the value of other controls within the same
	/// control class. Applications should update their user interface accordingly.
	/// V4L2_CTRL_FLAG_UPDATE
	/// </summary>
	Update = 0x0008,
	
	/// <summary>
	/// This control is not applicable to the current configuration and should be displayed
	/// accordingly in a user interface. For example the flag may be set on a MPEG audio level 2
	/// bitrate control when MPEG audio encoding level 1 was selected with another control.
	/// V4L2_CTRL_FLAG_INACTIVE
	/// </summary>
	Inactive = 0x0010,
	
	/// <summary>
	/// A hint that this control is best represented as a slider-like element in a user interface.
	/// V4L2_CTRL_FLAG_SLIDER
	/// </summary>
	Slider = 0x0020,
	
	/// <summary>
	/// This control is permanently writable only. Any attempt to read the control will result in an
	/// <c>EACCES</c> error code. This flag is typically present for relative controls or action
	/// controls where writing a value will cause the device to carry out a given action (e.g.
	/// motor control) but no meaningful value can be returned.
	/// V4L2_CTRL_FLAG_WRITE_ONLY
	/// </summary>
	WriteOnly = 0x0040,
	
	/// <summary>
	/// This control is volatile, which means that the value of the control changes continuously. A
	/// typical example would be the current gain value if the device is in auto-gain mode. In such
	/// a case the hardware calculates the gain value based on the lighting conditions which can
	/// change over time.
	/// V4L2_CTRL_FLAG_VOLATILE
	/// </summary>
	Volatile = 0x0080,
	
	/// <summary>
	/// This control has a pointer type, so its value has to be accessed using one of the pointer
	/// fields of struct v4l2_ext_control. This flag is set for controls that are an array, string,
	/// or have a compound type. In all cases you have to set a pointer to memory containing the
	/// payload of the control.
	/// V4L2_CTRL_FLAG_HAS_PAYLOAD
	/// </summary>
	HasPayload = 0x0100,
	
	/// <summary>
	/// The value provided to the control will be propagated to the driver even if it remains
	/// constant. This is required when the control represents an action on the hardware. For
	/// example: clearing an error flag or triggering the flash. All the controls of the type
	/// V4L2_CTRL_TYPE_BUTTON have this flag set.
	/// V4L2_CTRL_FLAG_EXECUTE_ON_WRITE
	/// </summary>
	ExecuteOnWrite = 0x0200,
	
	/// <summary>
	/// Changing this control value may modify the layout of the buffer (for video devices) or the media bus format (for sub-devices).
	/// A typical example would be the V4L2_CID_ROTATE control.
	/// Note that typically controls with this flag will also set the V4L2_CTRL_FLAG_GRABBED flag
	/// when buffers are allocated or streaming is in progress since most drivers do not support
	/// changing the format in that case.
	/// </summary>
	ModifyLayout = 0x0400,
	
	/// <summary>
	/// This control is a dynamically sized 1-dimensional array. It behaves the same as a regular
	/// array, except that the number of elements as reported by the elems field is between 1 and
	/// dims[0]. So setting the control with a differently sized array will change the elems field
	/// when the control is queried afterwards.
	/// </summary>
	DynamicArray = 0x0800,
}
