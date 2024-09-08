namespace WebCamControl.Core;

/// <summary>
/// Wrapper around <see cref="ICameraControl"/> that treats the value like a percentage.
/// </summary>
public class PercentControl(ICameraControl<int> control) : ICameraControl<int>
{
	public string Name => control.Name;
	public int Minimum => 0;
	public int Maximum => 100;
	public int Step => MapValueToPercentage(control.Step);

	public int Value
	{
		get => MapValueToPercentage(control.Value);
		set => control.Value = MapPercentageToValue(value);
	}
	
	public string? UserFriendlyValue => null;

	private int MapValueToPercentage(int value)
	{
		return (int)((value - control.Minimum) * 100.0 / (control.Maximum - control.Minimum));
	}

	private int MapPercentageToValue(int percentage)
	{
		return (int)(control.Minimum + ((control.Maximum - control.Minimum) / 100.0) * percentage);
	}
	
	public event EventHandler? Changed
	{
		add => control.Changed += value;
		remove => control.Changed -= value;
	}
	
	public static PercentControl? CreateIfNotNull(ICameraControl<int>? control)
	{
		return control == null ? null : new PercentControl(control);
	}
}
