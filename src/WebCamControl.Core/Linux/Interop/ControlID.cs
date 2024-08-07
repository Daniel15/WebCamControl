// ReSharper disable InconsistentNaming

namespace WebCamControl.Linux.Interop;

using static Constants;

/// <summary>
/// https://github.com/torvalds/linux/blob/e5b3efbe1ab1793bb49ae07d56d0973267e65112/include/uapi/linux/v4l2-controls.h
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/control.html
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/ext-ctrls-camera.html
/// </summary>
public enum ControlID : uint
{
	/// <summary>
	/// Picture brightness, or more precisely, the black level.
	/// V4L2_CID_BRIGHTNESS
	/// </summary>
	Brightness = V4L2_CID_BASE + 0,
	
	/// <summary>
	/// Picture contrast or luma gain.
	/// V4L2_CID_CONTRAST
	/// </summary>
	Contrast = V4L2_CID_BASE + 1,
	
	/// <summary>
	/// Picture color saturation or chroma gain.
	/// V4L2_CID_SATURATION
	/// </summary>
	Saturation = V4L2_CID_BASE + 2,
	
	/// <summary>
	/// Picture color saturation or chroma gain.
	/// V4L2_CID_HUE
	/// </summary>
	Hue = V4L2_CID_BASE + 3,
	
	/// <summary>
	/// Overall audio volume. Note some drivers also provide an OSS or ALSA mixer interface.
	/// V4L2_CID_AUDIO_VOLUME
	/// </summary>
	AudioVolume = V4L2_CID_BASE + 5,
	
	/// <summary>
	/// Audio stereo balance. Minimum corresponds to all the way left, maximum to right.
	/// V4L2_CID_AUDIO_BALANCE
	/// </summary>
	AudioBalance = V4L2_CID_BASE + 6,
	
	/// <summary>
	/// Audio bass adjustment.
	/// V4L2_CID_AUDIO_BASS
	/// </summary>
	AudioBass = V4L2_CID_BASE + 7,
	
	/// <summary>
	/// Audio treble adjustment.
	/// V4L2_CID_AUDIO_TREBLE
	/// </summary>
	AudioTreble = V4L2_CID_BASE + 8,
	
	/// <summary>
	/// Mute audio, i. e. set the volume to zero, however without affecting V4L2_CID_AUDIO_VOLUME.
	/// Like ALSA drivers, V4L2 drivers must mute at load time to avoid excessive noise. Actually
	/// the entire device should be reset to a low power consumption state.
	/// V4L2_CID_AUDIO_MUTE
	/// </summary>
	AudioMute = V4L2_CID_BASE + 9,
	
	/// <summary>
	/// Audio treble adjustment.
	/// Loudness mode (bass boost).
	/// V4L2_CID_AUDIO_LOUDNESS
	/// </summary>
	AudioLoudness = V4L2_CID_BASE + 10,
	
	/// <summary>
	/// Automatic white balance (cameras).
	/// V4L2_CID_AUTO_WHITE_BALANCE
	/// </summary>
	AutoWhiteBalance = V4L2_CID_BASE + 12,
	
	/// <summary>
	/// This is an action control. When set (the value is ignored), the device will do a white
	/// balance and then hold the current setting. Contrast this with the boolean
	/// V4L2_CID_AUTO_WHITE_BALANCE, which, when activated, keeps adjusting the white balance.
	/// 
	/// V4L2_CID_DO_WHITE_BALANCE
	/// </summary>
	DoWhiteBalance = V4L2_CID_BASE + 13,
	
	/// <summary>
	/// Red chroma balance.
	/// V4L2_CID_RED_BALANCE
	/// </summary>
	RedBalance = V4L2_CID_BASE + 14,
	
	/// <summary>
	/// Blue chroma balance.
	/// V4L2_CID_BLUE_BALANCE
	/// </summary>
	BlueBalance = V4L2_CID_BASE + 15,
	
	/// <summary>
	/// Gamma
	/// V4L2_CID_GAMMA
	/// </summary>
	Gamma = V4L2_CID_BASE + 16,
	
	/// <summary>
	/// Exposure (cameras). [Unit?]
	/// V4L2_CID_EXPOSURE
	/// </summary>
	Exposure = V4L2_CID_BASE + 17,
	
	/// <summary>
	/// Automatic gain/exposure control.
	/// V4L2_CID_AUTOGAIN
	/// </summary>
	AutoGain = V4L2_CID_BASE + 18,
	
	/// <summary>
	/// Gain control.
	/// 
	/// Primarily used to control gain on e.g. TV tuners but also on webcams. Most devices control
	/// only digital gain with this control but on some this could include analogue gain as well.
	/// Devices that recognise the difference between digital and analogue gain use controls
	/// V4L2_CID_DIGITAL_GAIN and V4L2_CID_ANALOGUE_GAIN.
	///
	/// V4L2_CID_GAIN
	/// </summary>
	Gain = V4L2_CID_BASE + 19,
	
	/// <summary>
	/// Mirror the picture horizontally.
	/// V4L2_CID_HFLIP
	/// </summary>
	HorizontalFlip = V4L2_CID_BASE + 20,
	
	/// <summary>
	/// Mirror the picture vertically.
	/// V4L2_CID_VFLIP
	/// </summary>
	VerticalFlip = V4L2_CID_BASE + 21,
	
	/// <summary>
	/// Enables a power line frequency filter to avoid flicker.
	///
	/// V4L2_CID_POWER_LINE_FREQUENCY
	/// </summary>
	PowerLineFrequency = V4L2_CID_BASE + 24,
	
	/// <summary>
	/// Enables automatic hue control by the device. The effect of setting V4L2_CID_HUE while
	/// automatic hue control is enabled is undefined, drivers should ignore such request.
	///
	/// V4L2_CID_HUE_AUTO
	/// </summary>
	HueAuto = V4L2_CID_BASE + 25,
	
	/// <summary>
	/// This control specifies the white balance settings as a color temperature in Kelvin. A
	/// driver should have a minimum of 2800 (incandescent) to 6500 (daylight). For more
	/// information about color temperature see Wikipedia.
	///
	/// V4L2_CID_WHITE_BALANCE_TEMPERATURE
	/// </summary>
	WhiteBalanceTemperature = V4L2_CID_BASE + 26,
	
	/// <summary>
	/// Adjusts the sharpness filters in a camera. The minimum value disables the filters, higher
	/// values give a sharper picture.
	///
	/// V4L2_CID_SHARPNESS
	/// </summary>
	Sharpness = V4L2_CID_BASE + 27,
	
	/// <summary>
	/// Adjusts the backlight compensation in a camera. The minimum value disables backlight
	/// compensation.
	///
	/// V4L2_CID_BACKLIGHT_COMPENSATION
	/// </summary>
	BacklightCompensation = V4L2_CID_BASE + 28,
	
	/// <summary>
	/// Chroma automatic gain control.
	/// V4L2_CID_CHROMA_AGC
	/// </summary>
	ChromaAutomaticGainControl = V4L2_CID_BASE + 29,
	
	/// <summary>
	/// Enable the color killer (i. e. force a black & white image in case of a weak video signal).
	/// V4L2_CID_COLOR_KILLER
	/// </summary>
	ColorKiller = V4L2_CID_BASE + 30,
	
	/// <summary>
	/// Selects a color effect.
	/// V4L2_CID_COLORFX
	/// TODO: Implement v4l2_colorfx enum
	/// </summary>
	ColorEffect = V4L2_CID_BASE + 31,
	
	/// <summary>
	/// Enable Automatic Brightness.
	/// V4L2_CID_AUTOBRIGHTNESS
	/// </summary>
	AutoBrightness = V4L2_CID_BASE + 32,
	
	/// <summary>
	/// Rotates the image by specified angle. Common angles are 90, 270 and 180. Rotating the image
	/// to 90 and 270 will reverse the height and width of the display window. It is necessary to
	/// set the new height and width of the picture using the VIDIOC_S_FMT ioctl according to the
	/// rotation angle selected.
	///
	/// V4L2_CID_ROTATE
	/// </summary>
	Rotate = V4L2_CID_BASE + 34,
	
	/// <summary>
	/// Sets the background color on the current output device. Background color needs to be
	/// specified in the RGB24 format. The supplied 32 bit value is interpreted as bits 0-7 Red
	/// color information, bits 8-15 Green color information, bits 16-23 Blue color information
	/// and bits 24-31 must be zero.
	/// 
	/// V4L2_CID_BG_COLOR
	/// </summary>
	BackgroundColor = V4L2_CID_BASE + 35,
	
	/// <summary>
	/// Adjusts the Chroma gain control (for use when chroma AGC is disabled).
	/// V4L2_CID_CHROMA_GAIN
	/// </summary>
	ChromaGain = V4L2_CID_BASE + 36,
	
	/// <summary>
	/// Switch on or off the illuminator 1 of the device (usually a microscope).
	/// V4L2_CID_ILLUMINATORS_1
	/// </summary>
	Illuminator1 = V4L2_CID_BASE + 37,
	
	/// <summary>
	/// Switch on or off the illuminator 2 of the device (usually a microscope).
	/// V4L2_CID_ILLUMINATORS_2
	/// </summary>
	Illuminator2 = V4L2_CID_BASE + 38,
	
	/// <summary>
	/// This is a read-only control that can be read by the application and used as a hint to
	/// determine the number of CAPTURE buffers to pass to REQBUFS. The value is the minimum number
	/// of CAPTURE buffers that is necessary for hardware to work.
	///
	/// V4L2_CID_MIN_BUFFERS_FOR_CAPTURE
	/// </summary>
	MinBuffersForCapture = V4L2_CID_BASE + 39,
	
	/// <summary>
	/// This is a read-only control that can be read by the application and used as a hint to
	/// determine the number of OUTPUT buffers to pass to REQBUFS. The value is the minimum number
	/// of OUTPUT buffers that is necessary for hardware to work.
	///
	/// V4L2_CID_MIN_BUFFERS_FOR_OUTPUT
	/// </summary>
	MinimumBuffersForOutput = V4L2_CID_BASE + 40,
	
	/// <summary>
	/// Sets the alpha color component. When a capture device (or capture queue of a mem-to-mem
	/// device) produces a frame format that includes an alpha component (e.g. packed RGB image
	/// formats) and the alpha value is not defined by the device or the mem-to-mem input data this
	/// control lets you select the alpha component value of all pixels. When an output device (or
	/// output queue of a mem-to-mem device) consumes a frame format that doesn’t include an alpha
	/// component and the device supports alpha channel processing this control lets you set the
	/// alpha component value of all pixels for further processing in the device.
	///
	/// V4L2_CID_ALPHA_COMPONENT
	/// </summary>
	AlphaComponent = V4L2_CID_BASE + 41,
	
	/// <summary>
	/// Determines the Cb and Cr coefficients for V4L2_COLORFX_SET_CBCR color effect. Bits [7:0] of
	/// the supplied 32 bit value are interpreted as Cr component, bits [15:8] as Cb component and
	/// bits [31:16] must be zero.
	/// 
	/// V4L2_CID_COLORFX_CBCR
	/// </summary>
	ColorEffectCbCr = V4L2_CID_BASE + 42,
	
	/// <summary>
	/// Determines the Red, Green, and Blue coefficients for V4L2_COLORFX_SET_RGB color effect.
	/// Bits [7:0] of the supplied 32 bit value are interpreted as Blue component, bits [15:8] as
	/// Green component, bits [23:16] as Red component, and bits [31:24] must be zero.
	///
	/// V4L2_CID_COLORFX_RGB
	/// </summary>
	ColorEffectRgb = V4L2_CID_BASE + 43, 
	
	/// <summary>
	/// Enables automatic adjustments of the exposure time and/or iris aperture. The effect of
	/// manual changes of the exposure time or iris aperture while these features are enabled is
	/// undefined, drivers should ignore such requests.
	///
	/// V4L2_CID_EXPOSURE_AUTO
	/// </summary>
	ExposureAuto = V4L2_CID_CAMERA_CLASS_BASE + 1,
	
	/// <summary>
	/// Determines the exposure time of the camera sensor. The exposure time is limited by the
	/// frame interval. Drivers should interpret the values as 100 µs units, where the value 1
	/// stands for 1/10000th of a second, 10000 for 1 second and 100000 for 10 seconds.
	///
	/// V4L2_CID_EXPOSURE_ABSOLUTE
	/// </summary>
	ExposureAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 2,
	
	/// <summary>
	/// When V4L2_CID_EXPOSURE_AUTO is set to AUTO or APERTURE_PRIORITY, this control determines if
	/// the device may dynamically vary the frame rate. By default this feature is disabled (0) and
	/// the frame rate must remain constant.
	///
	/// V4L2_CID_EXPOSURE_AUTO_PRIORITY
	/// </summary>
	ExposureAutoPriority = V4L2_CID_CAMERA_CLASS_BASE + 3,
	
	/// <summary>
	/// This control turns the camera horizontally by the specified amount. The unit is undefined.
	/// A positive value moves the camera to the right (clockwise when viewed from above), a
	/// negative value to the left. A value of zero does not cause motion. This is a write-only
	/// control.
	///
	/// V4L2_CID_PAN_RELATIVE
	/// </summary>
	PanRelative = V4L2_CID_CAMERA_CLASS_BASE + 4,
	
	/// <summary>
	/// This control turns the camera vertically by the specified amount. The unit is undefined. A
	/// positive value moves the camera up, a negative value down. A value of zero does not cause
	/// motion. This is a write-only control.
	///
	/// V4L2_CID_PAN_RESET
	/// </summary>
	TiltRelative = V4L2_CID_CAMERA_CLASS_BASE + 5,
	
	/// <summary>
	/// When this control is set, the camera moves horizontally to the default position.
	/// V4L2_CID_PAN_RESET
	/// </summary>
	PanReset = V4L2_CID_CAMERA_CLASS_BASE + 6,
	
	/// <summary>
	/// When this control is set, the camera moves vertically to the default position.
	/// V4L2_CID_TILT_RESET
	/// </summary>
	TiltReset = V4L2_CID_CAMERA_CLASS_BASE + 7,
	
	/// <summary>
	/// This control turns the camera horizontally to the specified position. Positive values move
	/// the camera to the right (clockwise when viewed from above), negative values to the left.
	/// Drivers should interpret the values as arc seconds, with valid values between -180 * 3600
	/// and +180 * 3600 inclusive.
	/// 
	/// V4L2_CID_PAN_ABSOLUTE
	/// </summary>
	PanAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 8,
	
	/// <summary>
	/// This control turns the camera vertically to the specified position. Positive values move the
	/// camera up, negative values down. Drivers should interpret the values as arc seconds, with
	/// valid values between -180 * 3600 and +180 * 3600 inclusive.
	/// 
	/// V4L2_CID_TILT_ABSOLUTE
	/// </summary>
	TiltAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 9,
	
	/// <summary>
	/// This control sets the focal point of the camera to the specified position. The unit is
	/// undefined. Positive values set the focus closer to the camera, negative values towards
	/// infinity.
	/// 
	/// V4L2_CID_FOCUS_ABSOLUTE
	/// </summary>
	FocusAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 10,
	
	/// <summary>
	/// This control moves the focal point of the camera by the specified amount. The unit is
	/// undefined. Positive values move the focus closer to the camera, negative values towards
	/// infinity. This is a write-only control.
	/// 
	/// V4L2_CID_FOCUS_RELATIVE
	/// </summary>
	FocusRelative = V4L2_CID_CAMERA_CLASS_BASE + 11,
	
	/// <summary>
	/// Enables continuous automatic focus adjustments. The effect of manual focus adjustments
	/// while this feature is enabled is undefined, drivers should ignore such requests.
	///
	/// V4L2_CID_FOCUS_AUTO
	/// </summary>
	FocusAuto = V4L2_CID_CAMERA_CLASS_BASE + 12,
	
	/// <summary>
	/// Specify the objective lens focal length as an absolute value. The zoom unit is
	/// driver-specific and its value should be a positive integer.
	///
	/// V4L2_CID_ZOOM_ABSOLUTE
	/// </summary>
	ZoomAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 13,
	
	/// <summary>
	/// Specify the objective lens focal length relatively to the current value. Positive values
	/// move the zoom lens group towards the telephoto direction, negative values towards the
	/// wide-angle direction. The zoom unit is driver-specific. This is a write-only control.
	///
	/// V4L2_CID_ZOOM_RELATIVE
	/// </summary>
	ZoomRelative = V4L2_CID_CAMERA_CLASS_BASE + 14,
	
	/// <summary>
	/// Move the objective lens group at the specified speed until it reaches physical device limits
	/// or until an explicit request to stop the movement. A positive value moves the zoom lens
	/// group towards the telephoto direction. A value of zero stops the zoom lens group movement.
	/// A negative value moves the zoom lens group towards the wide-angle direction. The zoom speed
	/// unit is driver-specific.
	///
	/// V4L2_CID_ZOOM_CONTINUOUS
	/// </summary>
	ZoomContinuous = V4L2_CID_CAMERA_CLASS_BASE + 15,
	
	/// <summary>
	/// Prevent video from being acquired by the camera. When this control is set to TRUE (1), no
	/// image can be captured by the camera. Common means to enforce privacy are mechanical
	/// obturation of the sensor and firmware image processing, but the device is not restricted
	/// to these methods. Devices that implement the privacy control must support read access and
	/// may support write access.
	/// </summary>
	Privacy = V4L2_CID_CAMERA_CLASS_BASE + 16,
	
	/// <summary>
	/// This control sets the camera’s aperture to the specified value. The unit is undefined.
	/// Larger values open the iris wider, smaller values close it.
	/// 
	/// V4L2_CID_IRIS_ABSOLUTE
	/// </summary>
	IrisAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 17,
	
	/// <summary>
	/// This control modifies the camera’s aperture by the specified amount. The unit is undefined.
	/// Positive values open the iris one step further, negative values close it one step further.
	/// This is a write-only control.
	/// </summary>
	IrisRelative = V4L2_CID_CAMERA_CLASS_BASE + 18,
	
	/// <summary>
	/// Determines the automatic exposure compensation, it is effective only when
	/// V4L2_CID_EXPOSURE_AUTO control is set to AUTO, SHUTTER_PRIORITY or APERTURE_PRIORITY. It is
	/// expressed in terms of EV, drivers should interpret the values as 0.001 EV units, where the
	/// value 1000 stands for +1 EV. Increasing the exposure compensation value is equivalent to
	/// decreasing the exposure value (EV) and will increase the amount of light at the image
	/// sensor. The camera performs the exposure compensation by adjusting absolute exposure time
	/// and/or aperture.
	///
	/// V4L2_CID_AUTO_EXPOSURE_BIAS
	/// </summary>
	AutoExposureBias = V4L2_CID_CAMERA_CLASS_BASE + 19,
	
	/// <summary>
	/// Sets white balance to automatic, manual or a preset. The presets determine color
	/// temperature of the light as a hint to the camera for white balance adjustments resulting
	/// in most accurate color representation. The following white balance presets are listed in
	/// order of increasing color temperature.
	/// 
	/// V4L2_CID_AUTO_N_PRESET_WHITE_BALANCE
	/// </summary> 
	AutoPresetWhiteBalance = V4L2_CID_CAMERA_CLASS_BASE + 20,
	
	/// <summary>
	/// Enables or disables the camera’s wide dynamic range feature. This feature allows to obtain
	/// clear images in situations where intensity of the illumination varies significantly
	/// throughout the scene, i.e. there are simultaneously very dark and very bright areas. It is
	/// most commonly realized in cameras by combining two subsequent frames with different
	/// exposure times.
	/// 
	/// V4L2_CID_WIDE_DYNAMIC_RANGE
	/// </summary>
	WideDynamicRange = V4L2_CID_CAMERA_CLASS_BASE + 21,
	
	/// <summary>
	/// Enables or disables image stabilization.
	/// V4L2_CID_IMAGE_STABILIZATION
	/// </summary>
	ImageStabilization = V4L2_CID_CAMERA_CLASS_BASE + 22,
	
	/// <summary>
	/// Determines ISO equivalent of an image sensor indicating the sensor’s sensitivity to light.
	/// The numbers are expressed in arithmetic scale, as per ISO 12232:2006 standard, where
	/// doubling the sensor sensitivity is represented by doubling the numerical ISO value.
	/// Applications should interpret the values as standard ISO values multiplied by 1000, e.g.
	/// control value 800 stands for ISO 0.8. Drivers will usually support only a subset of
	/// standard ISO values. The effect of setting this control while the
	/// V4L2_CID_ISO_SENSITIVITY_AUTO control is set to a value other than
	/// V4L2_CID_ISO_SENSITIVITY_MANUAL is undefined, drivers should ignore such requests.
	/// 
	/// V4L2_CID_ISO_SENSITIVITY
	/// </summary>
	IsoSensitivity = V4L2_CID_CAMERA_CLASS_BASE + 23,
	
	/// <summary>
	/// Enables or disables automatic ISO sensitivity adjustments.
	/// V4L2_CID_ISO_SENSITIVITY_AUTO
	/// </summary>
	IsoSensitivityAuto = V4L2_CID_CAMERA_CLASS_BASE + 24,
	
	/// <summary>
	/// Determines how the camera measures the amount of light available for the frame exposure.
	/// V4L2_CID_EXPOSURE_METERING
	/// </summary>
	ExposureMetering = V4L2_CID_CAMERA_CLASS_BASE + 25,
	
	/// <summary>
	/// This control allows to select scene programs as the camera automatic modes optimized for
	/// common shooting scenes. Within these modes the camera determines best exposure, aperture,
	/// focusing, light metering, white balance and equivalent sensitivity. The controls of those
	/// parameters are influenced by the scene mode control. An exact behavior in each mode is
	/// subject to the camera specification.
	///
	/// When the scene mode feature is not used, this control should be set to V4L2_SCENE_MODE_NONE
	/// to make sure the other possibly related controls are accessible.
	/// 
	/// V4L2_CID_SCENE_MODE
	/// </summary>
	SceneMode = V4L2_CID_CAMERA_CLASS_BASE + 26,
	
	/// <summary>
	/// This control locks or unlocks the automatic focus, exposure and white balance. The automatic
	/// adjustments can be paused independently by setting the corresponding lock bit to 1. The
	/// camera then retains the settings until the lock bit is cleared.
	/// 
	/// When a given algorithm is not enabled, drivers should ignore requests to lock it and should
	/// return no error. An example might be an application setting bit V4L2_LOCK_WHITE_BALANCE when
	/// the V4L2_CID_AUTO_WHITE_BALANCE control is set to FALSE. The value of this control may be
	/// changed by exposure, white balance or focus controls.
	///
	/// V4L2_CID_3A_LOCK
	/// </summary>
	AutoLock = V4L2_CID_CAMERA_CLASS_BASE + 27,
	
	/// <summary>
	/// Starts single auto focus process. The effect of setting this control when
	/// V4L2_CID_FOCUS_AUTO is set to TRUE (1) is undefined, drivers should ignore such requests.
	/// 
	/// V4L2_CID_AUTO_FOCUS_START
	/// </summary>
	AutoFocusStart = V4L2_CID_CAMERA_CLASS_BASE + 28,
	
	/// <summary>
	/// Aborts automatic focusing started with V4L2_CID_AUTO_FOCUS_START control. It is effective
	/// only when the continuous autofocus is disabled, that is when V4L2_CID_FOCUS_AUTO control is
	/// set to FALSE (0).
	/// </summary>
	AutoFocusStop = V4L2_CID_CAMERA_CLASS_BASE + 29, 
	
	/// <summary>
	/// The automatic focus status. This is a read-only control.
	/// 
	/// V4L2_CID_AUTO_FOCUS_STATUS
	/// </summary>
	AutoFocusStatus = V4L2_CID_CAMERA_CLASS_BASE + 30,
	
	/// <summary>
	/// Determines auto focus distance range for which lens may be adjusted.
	/// 
	/// V4L2_CID_AUTO_FOCUS_RANGE
	/// </summary>
	AutoFocusRange = V4L2_CID_CAMERA_CLASS_BASE + 31,
	
	/// <summary>
	/// This control turns the camera horizontally at the specific speed. The unit is undefined. A
	/// positive value moves the camera to the right (clockwise when viewed from above), a negative
	/// value to the left. A value of zero stops the motion if one is in progress and has no effect
	/// otherwise.
	/// 
	/// V4L2_CID_PAN_SPEED
	/// </summary>
	PanSpeed = V4L2_CID_CAMERA_CLASS_BASE + 32,
	
	/// <summary>
	/// This control turns the camera vertically at the specified speed. The unit is undefined. A
	/// positive value moves the camera up, a negative value down. A value of zero stops the motion
	/// if one is in progress and has no effect otherwise.
	/// 
	/// V4L2_CID_TILT_SPEED
	/// </summary>
	TiltSpeed = V4L2_CID_CAMERA_CLASS_BASE + 33,
	
	/// <summary>
	/// This read-only control describes the camera orientation by reporting its mounting position
	/// on the device where the camera is installed. The control value is constant and not
	/// modifiable by software. This control is particularly meaningful for devices which have a
	/// well defined orientation, such as phones, laptops and portable devices since the control is
	/// expressed as a position relative to the device’s intended usage orientation. For example, a
	/// camera installed on the user-facing side of a phone, a tablet or a laptop device is said to
	/// be have V4L2_CAMERA_ORIENTATION_FRONT orientation, while a camera installed on the opposite
	/// side of the front one is said to be have V4L2_CAMERA_ORIENTATION_BACK orientation. Camera
	/// sensors not directly attached to the device, or attached in a way that allows them to move
	/// freely, such as webcams and digital cameras, are said to have the
	/// V4L2_CAMERA_ORIENTATION_EXTERNAL orientation.
	/// 
	/// V4L2_CID_CAMERA_ORIENTATION
	/// </summary>
	CameraOrientation = V4L2_CID_CAMERA_CLASS_BASE + 34,
	
	/// <summary>
	/// This read-only control describes the rotation correction in degrees in the
	/// counter-clockwise direction to be applied to the captured images once captured to memory to
	/// compensate for the camera sensor mounting rotation.
	/// 
	/// For a precise definition of the sensor mounting rotation refer to the extensive description
	/// of the ‘rotation’ properties in the device tree bindings file ‘video-interfaces.txt’.
	/// 
	/// V4L2_CID_CAMERA_SENSOR_ROTATION
	/// </summary>
	SensorRotation = V4L2_CID_CAMERA_CLASS_BASE + 35,
	
	/// <summary>
	/// Change the sensor HDR mode. A HDR picture is obtained by merging two captures of the same
	/// scene using two different exposure periods. HDR mode describes the way these two captures
	/// are merged in the sensor.
	///
	/// As modes differ for each sensor, menu items are not standardized by this control and are
	/// left to the programmer.
	///
	/// V4L2_CID_HDR_SENSOR_MODE
	/// </summary>
	HdrSensorMode = V4L2_CID_CAMERA_CLASS_BASE + 36,
	
}
