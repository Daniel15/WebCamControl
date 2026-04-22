using System.Reflection;
using System.Text;
using Gtk;

namespace WebCamControl.Gtk;

public static class BuilderUtils
{
	/// <summary>
	/// Gets a Gtk builder template (.ui file) from an assembly resource.
	/// </summary>
	/// <remarks>
	/// Unfortunately, the only public constructor for <c>Builder</c> takes a resource
	/// name and uses `Assembly.GetCallingAssembly()`, which doesn't always work
	/// as expected in release mode.
	/// </remarks>
	public static Builder CreateFromAssembly(string templateFile)
	{
		var assembly = Assembly.GetEntryAssembly()!;
		using var stream = assembly.GetManifestResourceStream(templateFile);
		if (stream is null)
		{
			throw new Exception($"Cannot get resource file '{templateFile}' from assembly {assembly}");
		}
		
		using var streamReader = new StreamReader(stream);
		var template = streamReader.ReadToEnd();
		return Builder.NewFromString(template, Encoding.UTF8.GetByteCount(template));
	}
}
