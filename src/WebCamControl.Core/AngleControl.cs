namespace WebCamControl.Core;

/// <summary>
/// Wrapper around <see cref="ICameraControl"/> that treats the value like an angle.
/// </summary>
public class AngleControl(ICameraControl<int> control) : ICameraControl<float>
{
	private const float _arcsecondsInDegree = 3600;

	public string Name => control.Name;
	public float Minimum => control.Minimum / _arcsecondsInDegree;
	public float Maximum => control.Maximum / _arcsecondsInDegree;
	public float Step => control.Step / _arcsecondsInDegree;

	public float Value
	{
		get => control.Value / _arcsecondsInDegree;
		set => control.Value = (int)(value * _arcsecondsInDegree);
	}

	public static AngleControl? CreateIfNotNull(ICameraControl<int>? control)
	{
		return control == null ? null : new AngleControl(control);
	}
}
