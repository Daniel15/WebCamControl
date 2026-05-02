// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Microsoft.Extensions.Logging;
using WebCamControl.Core.Linux;

namespace WebCamControl.Core;

/// <summary>
/// Detects when any of a camera's controls are out of range.
/// </summary>
public class OutOfRangeDetector(
	ICamera _camera,
	ILogger<OutOfRangeDetector> _logger
)
{
	/// <summary>
	/// Detect out-of-range values for the specified camera.
	/// </summary>
	public IEnumerable<Result> Detect()
	{
		if (_camera is not LinuxCamera linuxCamera)
		{
			yield break;
		}

		foreach (var (id, control) in linuxCamera.RawIntegerControls)
		{
			int value;
			try
			{
				value = control.Value;
			}
			catch (Exception)
			{
				// Ignore exceptions when reading values
				continue;
			}

			if (value >= control.Minimum && value <= control.Maximum)
			{
				continue;
			}
			
			_logger.LogWarning(
				"{ID} is out of range! Min = {Minimum}, Max = {Maximum}, Current = {Value}",
				id,
				control.Minimum,
				control.Maximum,
				value
			);
			yield return new Result(id.ToString(), control);
		}
	}

	/// <summary>
	/// A camera control that's out of range
	/// </summary>
	public record Result(
		string Name,
		ICameraControl Control
	);
}
