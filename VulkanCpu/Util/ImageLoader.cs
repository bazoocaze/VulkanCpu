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
using GlmSharp;

namespace VulkanCpu.Util
{
	public class ImageLoader
	{
		public static byte[] LoadImage(Bitmap bmp)
		{
			byte[] dstPixelData;
			int imageWidth = bmp.Width;
			int imageHeight = bmp.Height;
			int pixelWidth = 4;
			PixelFormat pixelFormat = PixelFormat.Format32bppRgb;
			Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
			BitmapData srcData = bmp.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
			try
			{
				dstPixelData = new byte[imageWidth * imageHeight * pixelWidth];
				CopyImage(imageWidth, imageHeight, pixelWidth, srcData.Scan0, 0, srcData.Stride, dstPixelData, 0, srcData.Stride);
				return dstPixelData;
			}
			finally
			{
				bmp.UnlockBits(srcData);
			}
		}

		public static void LoadImage(Bitmap bmp, byte[] dstPixelData, int dstByteOffset)
		{
			int imageWidth = bmp.Width;
			int imageHeight = bmp.Height;
			int pixelWidth = 4;
			PixelFormat pixelFormat = PixelFormat.Format32bppRgb;
			Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
			BitmapData srcData = bmp.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
			try
			{
				dstPixelData = new byte[imageWidth * imageHeight * pixelWidth];
				CopyImage(imageWidth, imageHeight, pixelWidth, srcData.Scan0, 0, srcData.Stride, dstPixelData, dstByteOffset, srcData.Stride);
			}
			finally
			{
				bmp.UnlockBits(srcData);
			}
		}

		public static void LoadImage(Bitmap bmp, int[] dstPixelData, int dstByteOffset)
		{
			int imageWidth = bmp.Width;
			int imageHeight = bmp.Height;
			int pixelWidth = 4;
			PixelFormat pixelFormat = PixelFormat.Format32bppRgb;
			Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
			BitmapData srcData = bmp.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
			try
			{
				dstPixelData = new int[imageWidth * imageHeight];
				CopyImage(imageWidth, imageHeight, pixelWidth, srcData.Scan0, 0, srcData.Stride, dstPixelData, dstByteOffset, srcData.Stride);
			}
			finally
			{
				bmp.UnlockBits(srcData);
			}
		}

		public static void CopyImage<TSrc, TDst>(
				int width, int height, int pointSize,
				TSrc[] srcImage, int srcIndexOffset, int srcStride,
				TDst[] dstImage, int dstIndexOffset, int dstStride)
			where TSrc : struct where TDst : struct
		{
			int srcItemSize = Marshal.SizeOf(typeof(TSrc));
			int dstItemSize = Marshal.SizeOf(typeof(TDst));
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				Buffer.BlockCopy(
					srcImage, srcIndexOffset * srcItemSize,
					dstImage, dstIndexOffset * dstItemSize,
					srcStride * height);
			}
			else
			{
				int srcPos = srcIndexOffset * srcItemSize;
				int dstPos = dstIndexOffset + dstItemSize;

				for (int y = 0; y < height; y++)
				{
					Buffer.BlockCopy(srcImage, srcPos, dstImage, dstPos, lineSize);
					srcPos += srcStride;
					dstPos += dstStride;
				}
			}
		}

		public static void CopyImage(
				int width, int height, int pointSize,
				IntPtr srcImage, int srcOffset, int srcStride,
				IntPtr dstImage, int dstOffset, int dstStride)
		{
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				MemoryCopyHelper.CopyMemory(
					srcImage + srcOffset,
					dstImage + dstOffset,
					(uint)(srcStride * height));
			}
			else
			{
				int srcPos = srcOffset;
				int dstPos = dstOffset;

				for (int y = 0; y < height; y++)
				{
					MemoryCopyHelper.CopyMemory(
						srcImage + srcPos,
						dstImage + dstPos,
						(uint)lineSize);
					srcPos += srcStride;
					dstPos += dstStride;
				}
			}
		}

		public static void CopyImage(
				int width, int height, int pointSize,
				IntPtr srcImage, int srcOffset, int srcStride,
				byte[] dstImage, int dstOffset, int dstStride)
		{
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				Marshal.Copy(
					srcImage + srcOffset,
					dstImage, dstOffset,
					srcStride * height);
			}
			else
			{
				int srcPos = srcOffset;
				int dstPos = dstOffset;

				for (int y = 0; y < height; y++)
				{
					Marshal.Copy(
						srcImage + srcPos,
						dstImage, dstPos,
						lineSize);
					srcPos += srcStride;
					dstPos += dstStride;
				}
			}
		}

		public static void CopyImage(
				int width, int height, int pointSize,
				byte[] srcImage, int srcOffset, int srcStride,
				IntPtr dstImage, int dstOffset, int dstStride)
		{
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				Marshal.Copy(
					srcImage, srcOffset,
					dstImage + dstOffset,
					srcStride * height);
			}
			else
			{
				int srcPos = srcOffset;
				int dstPos = dstOffset;

				for (int y = 0; y < height; y++)
				{
					Marshal.Copy(
						srcImage, srcPos,
						dstImage + dstPos,
						lineSize);
					srcPos += srcStride;
					dstPos += dstStride;
				}
			}
		}

		public static void CopyImage(
				int width, int height, int pointSize,
				IntPtr srcImage, int srcOffset, int srcStride,
				int[] dstImage, int dstOffset, int dstStride)
		{
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				Marshal.Copy(
					srcImage + srcOffset,
					dstImage, dstOffset,
					srcStride * height / 4);
			}
			else
			{
				int srcPos = srcOffset;
				int dstPos = dstOffset / 4;
				int dstPosStep = dstStride / 4;
				int copySize = width * pointSize / 4;

				for (int y = 0; y < height; y++)
				{
					Marshal.Copy(
						srcImage + srcPos,
						dstImage, dstPos,
						copySize);
					srcPos += srcStride;
					dstPos += dstPosStep;
				}
			}
		}

		public static void CopyImage(
		int width, int height, int pointSize,
		int[] srcImage, int srcOffset, int srcStride,
		IntPtr dstImage, int dstOffset, int dstStride)
		{
			int lineSize = width * pointSize;

			if ((srcStride == dstStride) && (lineSize == srcStride))
			{
				Marshal.Copy(
					srcImage, srcOffset,
					dstImage + dstOffset,
					srcStride * height / 4);
			}
			else
			{
				int srcPos = srcOffset / 4;
				int dstPos = dstOffset;
				int srcPosStep = srcStride / 4;
				int copySize = width * pointSize / 4;

				for (int y = 0; y < height; y++)
				{
					Marshal.Copy(
						srcImage, srcPos,
						dstImage + dstPos,
						copySize);
					srcPos += srcPosStep;
					dstPos += dstStride;
				}
			}
		}

		public static void ConvertBlock<TSrc, TDst>(
			ivec2 blockSize,
			TSrc[] srcImage, ivec2 srcOffset, ivec2 srcDimension,
			TDst[] dstImage, ivec2 dstOffset, ivec2 dstDimension,
			Func<TSrc, TDst> convert) where TSrc : struct where TDst : struct
		{
			for (int posY = 0; posY < blockSize.y; posY++)
			{
				int srcPos = ((srcOffset.y + posY) * srcDimension.x) + srcOffset.x;
				int dstPos = ((dstOffset.y + posY) * dstDimension.x) + dstOffset.x;

				for (int posX = 0; posX < blockSize.x; posX++)
				{
					dstImage[dstPos] = convert.Invoke(srcImage[srcPos]);

					srcPos++;
					dstPos++;
				}
			}
		}

		public static void ConvertBlock<TSrc, TDst>(
			ivec3 blockSize,
			TSrc[] srcImage, ivec3 srcOffset, ivec3 srcDimension,
			TDst[] dstImage, ivec3 dstOffset, ivec3 dstDimension,
			Func<TSrc, TDst> convert) where TSrc : struct where TDst : struct
		{
			for (int posZ = 0; posZ < blockSize.z; posZ++)
			{
				int srcDepthOffset = (srcOffset.z + posZ) * (srcDimension.y * srcDimension.x);
				int dstDepthOffset = (dstOffset.z + posZ) * (dstDimension.y * dstDimension.x);

				for (int posY = 0; posY < blockSize.y; posY++)
				{
					int srcPos = srcDepthOffset + ((srcOffset.y + posY) * srcDimension.x) + srcOffset.x;
					int dstPos = dstDepthOffset + ((dstOffset.y + posY) * dstDimension.x) + dstOffset.x;

					for (int posX = 0; posX < blockSize.x; posX++)
					{
						dstImage[dstPos] = convert.Invoke(srcImage[srcPos]);

						srcPos++;
						dstPos++;
					}
				}
			}
		}

		public static void CreateImage<TSrc, TDst>(
			TDst[] dstImage, int dstOffset, ivec3 dstDimension,
			Func<ivec3, TDst> convert) where TSrc : struct where TDst : struct
		{
			ivec3 pos = new ivec3();

			for (int z = 0; z < dstDimension.z; z++)
			{
				int dstDepthOffset = dstOffset + z * (dstDimension.y * dstDimension.x);

				for (int y = 0; y < dstDimension.y; y++)
				{
					int dstPos = dstDepthOffset + (y * dstDimension.x);

					for (int x = 0; x < dstDimension.x; x++)
					{
						pos.x = x;
						pos.y = y;
						pos.z = z;

						dstImage[dstPos] = convert.Invoke(pos);
						dstPos++;
					}
				}
			}
		}

		public static void CreateImage<TSrc, TDst>(
			TDst[] dstImage, int dstOffset, ivec2 dstDimension,
			Func<ivec2, TDst> convert) where TSrc : struct where TDst : struct
		{
			ivec2 pos = new ivec2();

			for (int y = 0; y < dstDimension.y; y++)
			{
				int dstPos = dstOffset + (y * dstDimension.x);

				for (int x = 0; x < dstDimension.x; x++)
				{
					pos.x = x;
					pos.y = y;

					dstImage[dstPos] = convert.Invoke(pos);
					dstPos++;
				}
			}
		}

	}
}
