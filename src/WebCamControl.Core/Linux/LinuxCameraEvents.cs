// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WebCamControl.Core.Exceptions;
using WebCamControl.Linux.Interop;
using static WebCamControl.Linux.Interop.EPoll;
using static WebCamControl.Linux.Interop.Ioctl;

namespace WebCamControl.Core.Linux;

/// <summary>
/// Handles subscribing to V4L2 events using epoll.
/// </summary>
public class LinuxCameraEvents(
	ILogger<LinuxCameraEvents> _logger,
	IntPtr _fd
)
{
	private readonly Dictionary<ControlID, IList<Action<EventControl>>> _subscriptions = new();
	
	/// <summary>
	/// Starts an async job for listening to epoll events.
	/// </summary>
	public void StartAsync()
	{
		Task.Factory.StartNew(() =>
		{
			try
			{
				Poll();
			}
			catch (Exception ex)
			{
				_logger.LogWarning(
					"{fd}: Could not poll for events: {Error}",
					_fd,
					ex.Message
				);
			}
		});
	}

	/// <summary>
	/// Poll for epoll events
	/// </summary>
	private void Poll()
	{
		_logger.LogInformation("Starting to poll for events");
		
		var epollFd = epoll_create1(0);
		InteropException.ThrowIfError(epollFd);

		var epollEvent = new EPollEvent
		{
			Events = EPollEventTypes.In | EPollEventTypes.Priority | EPollEventTypes.EdgeTriggered,
			Data = new EPollData
			{
				// TODO: This is always a 32-bit number so it's likely that everywhere that has it
				// typed as IntPtr is incorrect :/
				fd = _fd.ToInt32()
			}
		};

		var result = epoll_ctl(epollFd, EPollOperation.Add, _fd, ref epollEvent);
		InteropException.ThrowIfError(result);
		
		// Discard first epoll result. It seems to always be in error status even though subsequent
		// calls work fine?
		var discardedEvt = new EPollEvent();
		epoll_wait(epollFd, ref discardedEvt, maxEvents: 1, timeout: -1);

		// TODO: Stop when disposed
		while (true)
		{
			_logger.LogInformation("Waiting for event");
			var polledEvt = new EPollEvent();
			result = epoll_wait(epollFd, ref polledEvt, maxEvents: 1, timeout: -1);
			InteropException.ThrowIfError(result);

			var videoEvent = new Event();
			result = ioctl(_fd, IoctlCommand.DequeueEvent, ref videoEvent);
			InteropException.ThrowIfError(result);

			if (videoEvent.Type != EventType.Control)
			{
				_logger.LogInformation("Ignoring {Type} event", videoEvent.Type);
				continue;
			}

			var controlID = (ControlID)videoEvent.ID;
			var controlData = videoEvent.Data.ControlData;
			_logger.LogInformation(
				"Got control event: ID = {ID}, Changes = {Changes}, Type = {Type}",
				controlID,
				controlData.Changes,
				controlData.Type
			);
			var handlers = _subscriptions.GetValueOrDefault(controlID);
			if (handlers == null || handlers.Count == 0)
			{
				_logger.LogWarning(
					"Received event for {ControlID}, but no handlers exist!", 
					controlID
				);
				continue;
			}

			foreach (var handler in handlers)
			{
				handler(controlData);
			}
		}
	}

	public void Subscribe(LinuxCameraControl control, Action<EventControl> handler)
	{
		// TODO: How do we handle volatile controls?

		_logger.LogInformation("Subscribing to events for {ID}", control.ID);
		ioctl(_fd, IoctlCommand.SubscribeEvent, BuildSubscription(control));
		var errno = Marshal.GetLastPInvokeError();
		if (errno != 0)
		{
			_logger.LogWarning(
				"Could not subscribe to updates to {ControlID}: {ErrorMessage} ({ErrNo})",
				control.ID,
				Marshal.GetPInvokeErrorMessage(errno),
				errno
			);
		}

		var controlSubs = _subscriptions.GetValueOrDefault(control.ID);
		if (controlSubs == null)
		{
			controlSubs = new List<Action<EventControl>>();
			_subscriptions[control.ID] = controlSubs;
		}
		controlSubs.Add(handler);
	}

	public void Unsubscribe(LinuxCameraControl control)
	{
		if (!_subscriptions.ContainsKey(control.ID))
		{
			return;
		}
		
		ioctl(_fd, IoctlCommand.UnsubscribeEvent, BuildSubscription(control));
		InteropException.ThrowIfError();
		_subscriptions.Remove(control.ID);
	}

	private static EventSubscription BuildSubscription(LinuxCameraControl control) =>
		new()
		{
			ID = (uint)control.ID,
			Type = EventType.Control
		};
}
