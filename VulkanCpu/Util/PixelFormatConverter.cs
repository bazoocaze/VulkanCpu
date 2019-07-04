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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GlmSharp;

namespace VulkanCpu.Util
{
	public static class PixelFormatConverter
	{
		public const float P_BYTE_TO_FLOAT = 1f / 255f;

		public const int P_BYTE_OFFSET_ALPHA = 3;
		public const int P_BYTE_OFFSET_RED = 2;
		public const int P_BYTE_OFFSET_GREEN = 1;
		public const int P_BYTE_OFFSET_BLUE = 0;

		public const int P_BIT_SHIFT_RED = (8 * P_BYTE_OFFSET_RED);
		public const int P_BIT_SHIFT_GREEN = (8 * P_BYTE_OFFSET_GREEN);
		public const int P_BIT_SHIFT_BLUE = (8 * P_BYTE_OFFSET_BLUE);
		public const int P_BIT_SHIFT_ALPHA = (8 * P_BYTE_OFFSET_ALPHA);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(RGB565 input)
		{
			return ConvertToUint(input.color);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(RGBA8888 input)
		{
			return ConvertToUint(input.r, input.g, input.b, input.a);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(ushort Value)
		{
			byte r, g, b;

			r = (byte)((Value >> 11) & (ushort)0x01F);
			g = (byte)((Value >> 5) & (ushort)0x03F);
			b = (byte)((Value >> 0) & (ushort)0x01F);

			return ConvertToUint((byte)(r << 3), (byte)(g << 2), (byte)(b << 3), 255);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(ref vec4 input)
		{
			return ConvertToUint(
				(byte)(input.r * 255),
				(byte)(input.g * 255),
				(byte)(input.b * 255),
				(byte)(input.a * 255)
				);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(byte r, byte g, byte b, byte a)
		{
			return (uint)(
				(r << P_BIT_SHIFT_RED) |
				(g << P_BIT_SHIFT_GREEN) |
				(b << P_BIT_SHIFT_BLUE) |
				(a << P_BIT_SHIFT_ALPHA)
				);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ConvertToUint(byte[] inputData, int offsetIndex)
		{
			return ConvertToUint(
				inputData[offsetIndex + P_BYTE_OFFSET_RED],
				inputData[offsetIndex + P_BYTE_OFFSET_GREEN],
				inputData[offsetIndex + P_BYTE_OFFSET_BLUE],
				inputData[offsetIndex + P_BYTE_OFFSET_ALPHA]
				);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToBytes(RGBA8888 color, byte[] outputData, int offsetIndex)
		{
			ConvertToBytes(color.r, color.g, color.b, color.a, outputData, offsetIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToBytes(uint color, byte[] output, int offsetIndex)
		{
			ConvertToBytes(new RGBA8888() { color = color }, output, offsetIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToBytes(byte r, byte g, byte b, byte a, byte[] output, int offsetIndex)
		{
			output[offsetIndex + P_BYTE_OFFSET_RED] = r;
			output[offsetIndex + P_BYTE_OFFSET_GREEN] = g;
			output[offsetIndex + P_BYTE_OFFSET_BLUE] = b;
			output[offsetIndex + P_BYTE_OFFSET_ALPHA] = a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToBytes(float value, byte[] outputData, int offsetIndex)
		{
			ConvertToBytes(new RGBA8888() { depth = value }, outputData, offsetIndex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGB565 ConvertToRGB565(byte r, byte g, byte b)
		{
			return new RGB565() { color = ConvertToUshort(r, g, b) };
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGB565 ConvertToRGB565(RGBA8888 color)
		{
			return ConvertToRGB565(color.r, color.g, color.b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGB565 ConvertToRGB565(uint color)
		{
			RGBA8888 ucolor = new RGBA8888() { color = color };
			return ConvertToRGB565(ucolor.r, ucolor.g, ucolor.b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToVec4(uint color, ref vec4 output)
		{
			var rgba8888 = new RGBA8888() { color = color };
			ConvertToVec4(rgba8888.r, rgba8888.g, rgba8888.b, rgba8888.a, ref output);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToVec4(byte r, byte g, byte b, byte a, ref vec4 output)
		{
			output.r = r * P_BYTE_TO_FLOAT;
			output.g = g * P_BYTE_TO_FLOAT;
			output.b = b * P_BYTE_TO_FLOAT;
			output.a = a * P_BYTE_TO_FLOAT;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToVec4(RGBA8888 color, ref vec4 output)
		{
			ConvertToVec4(color.r, color.g, color.b, color.a, ref output);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ConvertToVec4(byte[] data, int offsetIndex, ref vec4 output)
		{
			output.r = data[offsetIndex + P_BYTE_OFFSET_RED] * P_BYTE_TO_FLOAT;
			output.g = data[offsetIndex + P_BYTE_OFFSET_GREEN] * P_BYTE_TO_FLOAT;
			output.b = data[offsetIndex + P_BYTE_OFFSET_BLUE] * P_BYTE_TO_FLOAT;
			output.a = data[offsetIndex + P_BYTE_OFFSET_ALPHA] * P_BYTE_TO_FLOAT;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ConvertToUshort(byte r, byte g, byte b)
		{
			return (ushort)(
				((r & 0xF8) << 8) |
				((g & 0xFC) << 3) |
				((b & 0xF8) >> 3));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ConvertToUshort(uint color)
		{
			RGBA8888 ucolor = new RGBA8888() { color = color };
			return ConvertToUshort(ucolor.r, ucolor.g, ucolor.b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort ConvertToUshort(RGBA8888 color)
		{
			return ConvertToUshort(color.r, color.g, color.b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGBA8888 ConvertToRGBA8888(uint color)
		{
			return new RGBA8888() { color = color };
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGBA8888 ConvertToRGBA8888(ref vec4 color)
		{
			return new RGBA8888()
			{
				r = (byte)(color.r * 255f),
				g = (byte)(color.g * 255f),
				b = (byte)(color.b * 255f),
				a = (byte)(color.a * 255f),
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RGBA8888 ConvertToRGBA8888(byte[] data, int offsetIndex)
		{
			return new RGBA8888()
			{
				r = data[offsetIndex + P_BYTE_OFFSET_RED],
				g = data[offsetIndex + P_BYTE_OFFSET_GREEN],
				b = data[offsetIndex + P_BYTE_OFFSET_BLUE],
				a = data[offsetIndex + P_BYTE_OFFSET_ALPHA],
			};
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ConvertToFloat(byte[] data, int offsetIndex)
		{
			var point = new RGBA8888()
			{
				r = data[offsetIndex + P_BYTE_OFFSET_RED],
				g = data[offsetIndex + P_BYTE_OFFSET_GREEN],
				b = data[offsetIndex + P_BYTE_OFFSET_BLUE],
				a = data[offsetIndex + P_BYTE_OFFSET_ALPHA],
			};
			return point.depth;
		}
	}

	/// <summary>
	/// Represents a uint32 pixel in the format B8G8R8A8 / 0xAARRGGBB.
	/// Compatible with PixelFormat.Format32bppRgb and 
	/// VkFormat.VK_FORMAT_B8G8R8A8_UNORM.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct RGBA8888
	{
		[FieldOffset(0)]
		public uint color;

		[FieldOffset(0)]
		public float depth;

		[FieldOffset(PixelFormatConverter.P_BYTE_OFFSET_RED)]
		public byte r;

		[FieldOffset(PixelFormatConverter.P_BYTE_OFFSET_GREEN)]
		public byte g;

		[FieldOffset(PixelFormatConverter.P_BYTE_OFFSET_BLUE)]
		public byte b;

		[FieldOffset(PixelFormatConverter.P_BYTE_OFFSET_ALPHA)]
		public byte a;

		public static RGBA8888 Create(byte r, byte g, byte b)
		{
			return new RGBA8888() { r = r, g = g, b = b, a = 255 };
		}

		public override string ToString()
		{
			return string.Format("r={0} g={1} b={2} a={3} value=0x{4:X}",
				r, g, b, a, color);
		}
	}

	/// <summary>
	/// Represents a uint16 (ushort) pixel in the format R5GG6B5.
	/// Red: bits 11 to 15. Green: bits 5 to 10. Blue: bits 0 to 4.
	/// HI-BYTE=RRRRRGGG LO-BYTE=GGGBBBBB
	/// </summary>
	public struct RGB565
	{
		public ushort color;

		public override string ToString()
		{
			RGBA8888 rgba = new RGBA8888()
			{
				color = (PixelFormatConverter.ConvertToUint(this))
			};
			return string.Format("r={0} g={1} b={2} value=0x{3:X}",
				rgba.r, rgba.g, rgba.b, color);
		}
	}
}
