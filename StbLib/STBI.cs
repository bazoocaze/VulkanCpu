/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

* This is a simple emulation of stbi_load() from https://github.com/nothings/stb/blob/master/stb_image.h by Jose Ferreira (Bazoocaze)
*/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using VulkanCpu.Util;

namespace StbLib
{
	public class STBI
	{
		public static byte[] stbi_load(string fileName, out int width, out int height, out int channels, PixelFormat pixelFormat)
		{
			using (var img = Image.FromFile(fileName))
			{
				using (var bmp = new Bitmap(img))
				{
					return stbi_load(bmp, out width, out height, out channels, pixelFormat);
				}
			}
		}

		public static byte[] stbi_load(Bitmap input, out int width, out int height, out int channels, PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Format32bppRgb:
				/* FALL-THROUGH */
				case PixelFormat.Format32bppArgb:
					break;

				default:
					throw new NotImplementedException();
			}

			width = input.Width;
			height = input.Height;
			channels = 4;
			return ImageLoader.LoadImage(input);
		}

		public static void stbi_image_free(byte[] data)
		{
		}
	}
}
