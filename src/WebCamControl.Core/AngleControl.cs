namespace WebCamControl.Core;

/// <summary>
/// Wrapper around <see cref="ICameraControl"/> that treats the value like an angle.
/// </summary>
public class AngleControl : ICameraControl<float>
{
	private const float _arcsecondsInDegree = 3600;
	
	private readonly ICameraControl<int> _control;

	public AngleControl(ICameraControl<int> control)
	{
		_control = control;
	}

	public float Minimum => _control.Minimum / _arcsecondsInDegree;
	public float Maximum => _control.Maximum / _arcsecondsInDegree;
	public float Step => _control.Step / _arcsecondsInDegree;

	public float Value
	{
		get => _control.Value / _arcsecondsInDegree;
		set => _control.Value = (int)(value * _arcsecondsInDegree);
	}
}
