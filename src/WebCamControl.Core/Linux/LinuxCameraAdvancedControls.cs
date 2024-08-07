using System.Collections.Immutable;
using WebCamControl.Linux.Interop;

namespace WebCamControl.Core.Linux;

public class LinuxCameraAdvancedControls
{
	/// <summary>
	/// Boolean controls
	/// </summary>
	public IImmutableDictionary<ControlID, BooleanControl> Booleans { get; init; } =
		ImmutableDictionary<ControlID, BooleanControl>.Empty;
	
	/// <summary>
	/// Integer controls
	/// </summary>
	public IImmutableDictionary<ControlID, LinuxCameraControl> Integers { get; init; } =
		ImmutableDictionary<ControlID, LinuxCameraControl>.Empty;
}
