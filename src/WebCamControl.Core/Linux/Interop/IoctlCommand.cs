// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

// ReSharper disable InconsistentNaming
namespace WebCamControl.Linux.Interop;

/// <summary>
/// ioctl commands for V4L2
/// </summary>
public enum IoctlCommand : uint
{
	/// <summary>
	/// VIDIOC_QUERYCAP
	/// </summary>
	QueryCapabilities = 0x80685600,
	
	/// <summary>
	/// VIDIOC_G_CTRL
	/// </summary>
	GetControl = 0xc008561b,
	
	/// <summary>
	/// VIDIOC_S_CTRL
	/// </summary>
	SetControl = 0xc008561c,
	
	/// <summary>
	/// VIDIOC_QUERYCTRL
	/// </summary>
	QueryControl = 0xc0445624,
	
	/// <summary>
	/// VIDIOC_SUBSCRIBE_EVENT
	/// </summary>
	SubscribeEvent = 0x4020565a,
	
	/// <summary>
	/// VIDIOC_UNSUBSCRIBE_EVENT
	/// </summary>
	UnsubscribeEvent = 0x4020565b,
	
	/// <summary>
	/// VIDIOC_DQEVENT
	/// </summary>
	DequeueEvent = 0x80885659,
}
