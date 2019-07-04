/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static VulkanCpu.Platform.win32.Gdi32;

namespace VulkanCpu.Platform.win32
{
	public class DibBitmap : IDisposable
	{
		private const int MAX_WIDTH = 2048;
		private const int MAX_HEIGHT = 2048;

		private readonly int m_Width;
		private readonly int m_Height;
		private IntPtr m_hBitmap;
		private BITMAPINFO m_BmpInfo;

		public DibBitmap(int width, int height)
		{
			if (width <= 0 || width > MAX_WIDTH)
				throw new ArgumentOutOfRangeException(nameof(width));

			if (height <= 0 || height > MAX_HEIGHT)
				throw new ArgumentOutOfRangeException(nameof(width));

			using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb))
			{
				m_hBitmap = bmp.GetHbitmap();
			}

			m_Width = width;
			m_Height = height;

			PrepareHeader();
		}

		private void PrepareHeader()
		{
			m_BmpInfo = BITMAPINFO.Create();
			m_BmpInfo.bmiHeader.biWidth = m_Width;
			m_BmpInfo.bmiHeader.biHeight = -m_Height; // top down DIB
			m_BmpInfo.bmiHeader.biPlanes = 1;
			m_BmpInfo.bmiHeader.biBitCount = 32; // 0xXXRRGGBB
			m_BmpInfo.bmiHeader.biCompression = BitmapCompressionMode.BI_RGB;
			m_BmpInfo.bmiHeader.biSizeImage = 0;
			m_BmpInfo.bmiHeader.biXPelsPerMeter = 10;
			m_BmpInfo.bmiHeader.biYPelsPerMeter = 10;
			m_BmpInfo.bmiHeader.biClrUsed = (uint)DibColorMode.DIB_RGB_COLORS;
		}

		public void WritePixels(byte[] pixels, int byteOffset = 0)
		{
			if (m_hBitmap == IntPtr.Zero)
				throw new ObjectDisposedException(nameof(DibBitmap));
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));
			if (byteOffset < 0)
				throw new ArgumentOutOfRangeException(nameof(byteOffset));

			var pinned = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try
			{
				InternalWritePixels(pinned.AddrOfPinnedObject(), byteOffset);
			}
			finally
			{
				pinned.Free();
			}
		}

		public void WritePixels(int[] pixels, int byteOffset = 0)
		{
			if (m_hBitmap == IntPtr.Zero)
				throw new ObjectDisposedException(nameof(DibBitmap));
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));
			if (byteOffset < 0)
				throw new ArgumentOutOfRangeException(nameof(byteOffset));

			var pinned = GCHandle.Alloc(pixels, GCHandleType.Pinned);
			try
			{
				InternalWritePixels(pinned.AddrOfPinnedObject(), byteOffset);
			}
			finally
			{
				pinned.Free();
			}
		}

		public void WritePixels(IntPtr pixels, int byteOffset = 0)
		{
			if (pixels == IntPtr.Zero)
				throw new ArgumentNullException(nameof(pixels));
			if (byteOffset < 0)
				throw new ArgumentOutOfRangeException(nameof(byteOffset));

			InternalWritePixels(pixels, byteOffset);
		}

		private void InternalWritePixels(IntPtr pixels, int byteOffset)
		{
			SetDIBits(IntPtr.Zero, m_hBitmap, 0, (uint)(m_Height), pixels + byteOffset, ref m_BmpInfo, 0);
		}

		public void WritePixelsDirect(int[] pixels, int byteOffset = 0)
		{
			if (m_hBitmap == IntPtr.Zero)
				throw new ObjectDisposedException(nameof(DibBitmap));
			if (pixels == null)
				throw new ArgumentNullException(nameof(pixels));
			if (byteOffset < 0)
				throw new ArgumentOutOfRangeException(nameof(byteOffset));

			BITMAP bitmap = new BITMAP();
			int size = Marshal.SizeOf(bitmap);
			GetObject(m_hBitmap, size, out bitmap);
			if (bitmap.bmBits != IntPtr.Zero)
			{
				// invert y axis
				for (int y = 0; y < m_Height; y++)
				{
					int srcPos = (byteOffset / 4) + (y * m_Width);
					int dstPos = ((m_Height - y) * m_Width * 4);
					// dstPos = 0;
					Marshal.Copy(pixels, srcPos, bitmap.bmBits + dstPos, m_Width);
				}

				// Marshal.Copy(pixels, byteOffset / 4, bitmap.bmBits, m_Width * m_Height);
			}
		}

		public void DrawTo(Graphics target)
		{
			if (m_hBitmap == IntPtr.Zero)
				throw new ObjectDisposedException(nameof(DibBitmap));

			if (target == null)
				throw new ArgumentNullException(nameof(target));

			IntPtr hdcTarget = target.GetHdc();
			IntPtr memHdcTarget = Gdi32.CreateCompatibleDC(hdcTarget);
			IntPtr objPrevious = Gdi32.SelectObject(memHdcTarget, m_hBitmap);
			bool ret = Gdi32.BitBlt(hdcTarget, 0, 0, m_Width, m_Height, memHdcTarget, 0, 0, Gdi32.TernaryRasterOperations.SRCCOPY);
			Gdi32.SelectObject(memHdcTarget, objPrevious);
			Gdi32.DeleteDC(memHdcTarget);
			target.ReleaseHdc(hdcTarget);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (m_hBitmap != IntPtr.Zero)
			{
				Gdi32.DeleteObject(m_hBitmap);
				m_hBitmap = IntPtr.Zero;
			}
		}
	}
}
