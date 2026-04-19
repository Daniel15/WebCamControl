using GObject;
using Gtk;
using Timer = System.Timers.Timer;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A <see cref="Button"/> that emits events while it is being held in.
/// TODO: Use qualifiedName once available.
/// </summary>
[Subclass<Button>]
public partial class PressAndHoldButton
{
	private readonly Timer _timer = new(TimeSpan.FromMilliseconds(100));

	public event EventHandler? OnHeld;

	partial void Initialize()
	{
		_timer.Elapsed += (_, _) => OnHeld?.Invoke(this, EventArgs.Empty);

		var gesture = GestureClick.New();
		gesture.PropagationPhase = PropagationPhase.Capture;
		gesture.OnPressed += (_, _) =>
		{
			OnHeld?.Invoke(this, EventArgs.Empty);
			_timer.Start();
		};
		gesture.OnReleased += (_, _) => _timer.Stop();
		AddController(gesture);
	}
}
