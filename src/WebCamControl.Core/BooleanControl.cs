namespace WebCamControl.Core;

/// <summary>
/// Wrapper around <see cref="ICameraControl"/> that uses `0` for false and `1` for true.
/// </summary>
public class BooleanControl(ICameraControl<int> control) : ICameraControl<bool>
{
	public string Name => control.Name;
	public bool Minimum => false;
	public bool Maximum => true;
	public bool Step => true;
	public bool IsEnabled => control.IsEnabled;

	public bool Value
	{
		get => control.Value == 1;
		set => control.Value = value ? 1 : 0;
	}

	public string? UserFriendlyValue => null;

	public event EventHandler? Changed
	{
		add => control.Changed += value;
		remove => control.Changed -= value;
	}
}
