// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Core.Exceptions;

/// <summary>
/// Represents an exception that happened in P/Invoke code.
/// </summary>
public class InteropException(string message) : Exception(message)
{
	/// <summary>
	/// Throws an exception if the last P/Invoke call threw an error (set errno).
	/// </summary>
	public static void ThrowIfError()
	{
		var errno = Marshal.GetLastPInvokeError();
		if (errno != 0)
		{
			throw new InteropException(Marshal.GetPInvokeErrorMessage(errno));
		}
	}

	/// <summary>
	/// Throws an exception if result is -1 or the last P/Invoke call threw an error (set errno).
	/// </summary>
	public static void ThrowIfError(int result)
	{
		ThrowIfError();
		if (result == -1)
		{
			throw new InteropException("Unknown error (returned -1)");
		}
	}
	
	/// <summary>
	/// Throws an exception if result is NULL or the last P/Invoke call threw an error (set errno).
	/// </summary>
	public static void ThrowIfError(IntPtr result)
	{
		ThrowIfError();
		if (result == IntPtr.Zero)
		{
			throw new InteropException("Unknown error (returned NULL)");
		}
	}
}
