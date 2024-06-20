using Gtk;
using Timer = System.Timers.Timer;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A button that emits events while it is being held in.
/// </summary>
public class PressAndHoldButton : Button
{
	private readonly Timer _timer = new(TimeSpan.FromMilliseconds(100));
	
	protected internal PressAndHoldButton(IntPtr ptr, bool ownedRef)
		: base(ptr, ownedRef)
	{
		AttachEvents();
	}

	public event EventHandler OnHeld;

	private void AttachEvents()
	{
		_timer.Elapsed += (_, _) => OnHeld(this, EventArgs.Empty);
		
		var gesture = GestureClick.New();
		gesture.PropagationPhase = PropagationPhase.Capture;
		gesture.OnPressed += (_, _) =>
		{
			OnHeld(this, EventArgs.Empty);
			_timer.Start();
		};
		gesture.OnReleased += (_, _) => _timer.Stop();
		AddController(gesture);
	}
}
