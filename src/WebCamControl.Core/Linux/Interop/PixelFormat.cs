// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;

namespace WebCamControl.Linux.Interop;

/// <summary>
/// v4l2_pix_format
/// https://www.kernel.org/doc/html/v6.9/userspace-api/media/v4l/pixfmt-v4l2.html
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct PixelFormat
{
	/// <summary>
	/// Image width in pixels.
	/// </summary>
	public uint Width;

	/// <summary>
	/// Image height in pixels.
	/// </summary>
	public uint Height;

	/// <summary>
	/// The pixel format or type of compression, set by the application. This is a little endian
	/// four character code.
	/// </summary>
	public uint PixelFormatField;

	/// <summary>
	/// Field order, from enum v4l2_field.
	/// </summary>
	public uint Field;

	/// <summary>
	/// Distance in bytes between the leftmost pixels in two adjacent lines.
	/// </summary>
	public uint BytesPerLine;

	/// <summary>
	/// Size in bytes of the buffer to hold a complete image, set by the driver.
	/// </summary>
	public uint SizeImage;

	/// <summary>
	/// Image colorspace, from enum v4l2_colorspace.
	/// </summary>
	public uint Colorspace;

	/// <summary>
	/// Private data, depends on pixelformat. When set to V4L2_PIX_FMT_PRIV_MAGIC it indicates that
	/// the extended fields have been correctly initialized.
	/// </summary>
	public uint Priv;

	/// <summary>
	/// Flags set by the application or driver.
	/// </summary>
	public uint Flags;

	/// <summary>
	/// Y'CbCr encoding or HSV encoding.
	/// </summary>
	public EncodingUnion Encoding;

	/// <summary>
	/// Quantization range, from enum v4l2_quantization.
	/// </summary>
	public uint Quantization;

	/// <summary>
	/// Transfer function, from enum v4l2_xfer_func.
	/// </summary>
	public uint XferFunc;

	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct EncodingUnion
	{
		/// <summary>
		/// Y'CbCr encoding, from enum v4l2_ycbcr_encoding.
		/// </summary>
		[FieldOffset(0)]
		public uint YcbcrEnc;

		/// <summary>
		/// HSV encoding, from enum v4l2_hsv_encoding.
		/// </summary>
		[FieldOffset(0)]
		public uint HsvEnc;
	}
}
