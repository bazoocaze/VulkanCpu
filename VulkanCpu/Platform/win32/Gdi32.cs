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
using System.Runtime.InteropServices;

namespace VulkanCpu.Platform.win32
{
	public static class Gdi32
	{
		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern int SetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines, IntPtr lpvBits, [In] ref BITMAPINFO lpbmi, uint fuColorUse);

		[DllImport("gdi32.dll")]
		public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, out BITMAP lpvObject);

		public enum TernaryRasterOperations : uint
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062,
			CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
		}

		public enum BitmapCompressionMode : uint
		{
			BI_RGB = 0,
			BI_RLE8 = 1,
			BI_RLE4 = 2,
			BI_BITFIELDS = 3,
			BI_JPEG = 4,
			BI_PNG = 5
		}

		public enum DibColorMode : uint
		{
			DIB_RGB_COLORS = 0,
			DIB_PAL_COLORS = 1
		}

		/// <summary>The BITMAPINFOHEADER structure contains information about the dimensions and color format of a DIB.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFOHEADER
		{
			public uint biSize;
			public int biWidth;
			public int biHeight;
			public ushort biPlanes;
			public ushort biBitCount;
			public BitmapCompressionMode biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public DibColorMode biClrUsed;
			public uint biClrImportant;
		}

		/// <summary>The BITMAPINFO structure defines the dimensions and color information for a DIB.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFO
		{
			/// <summary>A BITMAPINFOHEADER structure that contains information about the dimensions of color format.</summary>        
			public BITMAPINFOHEADER bmiHeader;

			/// <summary>Color table</summary>
			public IntPtr bmiColors;

			public static BITMAPINFO Create()
			{
				BITMAPINFO ret = new BITMAPINFO();
				ret.bmiHeader.biSize = (uint)Marshal.SizeOf(ret.bmiHeader);
				return ret;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAP
		{
			public uint bmType;
			public uint bmWidth;
			public uint bmHeight;
			public uint bmWidthBytes;
			public ushort bmPlanes;
			public ushort bmBitsPixel;
			public IntPtr bmBits;
		}

		public static void DrawImage(Graphics dest, int xDest, int yDest, Bitmap bmp)
		{
			IntPtr pTarget = dest.GetHdc();
			IntPtr pSource = CreateCompatibleDC(pTarget);
			IntPtr hBitmap = bmp.GetHbitmap();
			IntPtr pOrig = SelectObject(pSource, hBitmap);
			BitBlt(pTarget, xDest, yDest, bmp.Width, bmp.Height, pSource, 0, 0, TernaryRasterOperations.SRCCOPY);
			DeleteObject(hBitmap);
			DeleteDC(pSource);
			dest.ReleaseHdc(pTarget);
		}
	}
}

