// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Linux.Interop;

/// <summary>
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/vidioc-dqevent.html#id2
/// https://github.com/torvalds/linux/blob/da3ea35007d0af457a0afc87e84fddaebc4e0b63/include/uapi/linux/videodev2.h#L2509
/// </summary>
public enum EventType : uint
{
	/// <summary>
	/// All events. V4L2_EVENT_ALL is valid only for VIDIOC_UNSUBSCRIBE_EVENT for unsubscribing all
	/// events at once.
	/// V4L2_EVENT_ALL
	/// </summary>
	All = 0,
	
	/// <summary>
	/// This event is triggered on the vertical sync. This event has a struct v4l2_event_vsync
	/// associated with it.
	/// V4L2_EVENT_VSYNC
	/// </summary>
	VSync = 1,
	
	/// <summary>
	/// This event is triggered when the end of a stream is reached. This is typically used with
	/// MPEG decoders to report to the application when the last of the MPEG stream has been
	/// decoded.
	/// V4L2_EVENT_EOS
	/// </summary>
	EndOfStream = 2,
	
	/// <summary>
	/// This event requires that the id matches the control ID from which you want to receive
	/// events. This event is triggered if the control’s value changes, if a button control is
	/// pressed or if the control’s flags change. This event has a struct v4l2_event_ctrl
	/// associated with it. This struct contains much of the same information as struct
	/// v4l2_queryctrl and struct v4l2_control.
	///
	/// If the event is generated due to a call to VIDIOC_S_CTRL or VIDIOC_S_EXT_CTRLS, then the
	/// event will not be sent to the file handle that called the ioctl function. This prevents
	/// nasty feedback loops. If you do want to get the event, then set the
	/// V4L2_EVENT_SUB_FL_ALLOW_FEEDBACK flag.
	///
	/// This event type will ensure that no information is lost when more events are raised than
	/// there is room internally. In that case the struct v4l2_event_ctrl of the second-oldest
	/// event is kept, but the changes field of the second-oldest event is ORed with the changes
	/// field of the oldest event.
	/// 
	/// V4L2_EVENT_CTRL
	/// </summary>
	Control = 3,
	
	/// <summary>
	/// Triggered immediately when the reception of a frame has begun. This event has a struct
	/// v4l2_event_frame_sync associated with it.
	///
	/// If the hardware needs to be stopped in the case of a buffer underrun it might not be able
	/// to generate this event. In such cases the frame_sequence field in struct
	/// v4l2_event_frame_sync will not be incremented. This causes two consecutive frame sequence
	/// numbers to have n times frame interval in between them.
	/// 
	/// V4L2_EVENT_FRAME_SYNC
	/// </summary>
	FrameSync = 4,
	
	/// <summary>
	/// This event is triggered when a source parameter change is detected during runtime by the
	/// video device. It can be a runtime resolution change triggered by a video decoder or the
	/// format change happening on an input connector. This event requires that the id matches the
	/// input index (when used with a video device node) or the pad index (when used with a
	/// subdevice node) from which you want to receive events.
	/// 
	/// This event has a struct v4l2_event_src_change associated with it. The changes bitfield
	/// denotes what has changed for the subscribed pad. If multiple events occurred before
	/// application could dequeue them, then the changes will have the ORed value of all the
	/// events generated.
	/// 
	/// V4L2_EVENT_SOURCE_CHANGE
	/// </summary>
	SourceChange = 5,
	
	/// <summary>
	/// Triggered whenever the motion detection state for one or more of the regions changes. This
	/// event has a struct v4l2_event_motion_det associated with it.
	/// 
	/// V4L2_EVENT_MOTION_DET
	/// </summary>
	MotionDetected = 6,
	
}
