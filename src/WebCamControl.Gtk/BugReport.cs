// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace WebCamControl.Gtk;

public static class BugReport
{
	public static Uri BuildBugReportUri(Exception? ex)
	{
		var version = Assembly.GetEntryAssembly()
			?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			?.InformationalVersion ?? "Unknown";
		
		var query = HttpUtility.ParseQueryString(string.Empty);
		query["title"] = $"Bug: {ex?.Message ?? "[add details here]"}";
		query["body"] = $"""
		                 [Explain your bug report here]
		                 
		                 !!! NOTE: Please attach debug output from WebCamControl. Run it at the command line (`flatpak run com.daniel15.wcc` for Flatpak, `webcamcontrol` otherwise), and copy and paste ALL output below:
		                 ```
		                 !!! Put your debug log here
		                 ```

		                 Version: `{version}`
		                 System: `{RuntimeInformation.OSDescription}`
		                 """;
		if (ex != null)
		{
			query["body"] += $"""

			                  
			                  Exception:
			                  ```
			                  {ex}
			                  ```
			                  """;
		}
		
		var uriBuilder = new UriBuilder("https://github.com/Daniel15/WebCamControl/issues/new")
		{
			Query = query.ToString()
		};
		return uriBuilder.Uri;
	}
}
