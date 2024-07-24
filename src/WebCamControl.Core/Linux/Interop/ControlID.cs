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
	/// This control turns the camera horizontally to the specified position. Positive values move
	/// the camera to the right (clockwise when viewed from above), negative values to the left.
	/// Drivers should interpret the values as arc seconds, with valid values between -180 * 3600
	/// and +180 * 3600 inclusive.
	/// V4L2_CID_PAN_ABSOLUTE
	/// </summary>
	PanAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 8,
	
	/// <summary>
	/// This control turns the camera vertically to the specified position. Positive values move the
	/// camera up, negative values down. Drivers should interpret the values as arc seconds, with
	/// valid values between -180 * 3600 and +180 * 3600 inclusive.
	/// V4L2_CID_TILT_ABSOLUTE
	/// </summary>
	TiltAbsolute = V4L2_CID_CAMERA_CLASS_BASE + 9,
}
