using System.Runtime.InteropServices;

namespace WebCamControl.Core.Exceptions;

/// <summary>
/// Represents an exception that happened in P/Invoke code.
/// </summary>
public class InteropException(string message) : Exception(message)
{
	public static void ThrowIfError()
	{
		var errno = Marshal.GetLastPInvokeError();
		if (errno != 0)
		{
			throw new InteropException(Marshal.GetPInvokeErrorMessage(errno));
		}
	}
}
