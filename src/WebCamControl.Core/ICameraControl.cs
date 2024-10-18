namespace WebCamControl.Core;

/// <summary>
/// Represents a control for a camera (for example, tilt, pan, brightness, etc).
/// </summary>
/// <typeparam name="T">Type of the control</typeparam>
public interface ICameraControl<T>
{
	/// <summary>
	/// User-friendly name of the control
	/// </summary>
	string Name { get; }
	
	/// <summary>
	/// Gets the minimum value this control can be set to.
	/// </summary>
	T Minimum { get; }
	
	/// <summary>
	/// Gets the maximum value this control can be set to.
	/// </summary>
	T Maximum { get; }
	
	/// <summary>
	/// Gets the smallest increment that can be done to this control. The value will be a multiple
	/// of this.
	/// </summary>
	T Step { get; }
	
	/// <summary>
	/// Gets or sets the value for this control.
	/// </summary>
	T Value { get; set; }
	
	/// <summary>
	/// Gets the value in a format suitable for displaying to the user in the UI.
	/// </summary>
	string? UserFriendlyValue { get; }
	
	/// <summary>
	/// Gets whether this control is currently disabled.
	/// </summary>
	bool IsEnabled { get; }
	
	/// <summary>
	/// Fired when the value of this control is changed.
	/// </summary>
	public event EventHandler Changed;
}

/// <summary>
/// Represents a control for a camera, with an integer value. This is the standard data format
/// returned from the V4L2 API.
/// </summary>
public interface ICameraControl : ICameraControl<int> {}
