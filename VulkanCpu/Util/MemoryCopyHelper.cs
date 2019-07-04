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
using System.Runtime.InteropServices;
using VulkanCpu.Platform.win32;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.Util
{
	public static class MemoryCopyHelper
	{
		public static void CopyMemory(IntPtr input, IntPtr output, uint byteCount)
		{
			if (byteCount == 0)
				return;

			VkPreconditions.CheckNull(input == IntPtr.Zero, nameof(input));
			VkPreconditions.CheckNull(output == IntPtr.Zero, nameof(output));

			Msvcrt.memcpy(output, input, (UIntPtr)byteCount);
		}

		public static T ReadStructure<T>(byte[] input, int inputOffset) where T : struct
		{
			VkPreconditions.CheckNull(input, nameof(input));
			VkPreconditions.CheckRange(inputOffset, 0, input.Length - 1, nameof(inputOffset));

			GCHandle pinned = GCHandle.Alloc(input, GCHandleType.Pinned);
			try
			{
				IntPtr addr = pinned.AddrOfPinnedObject();
				return (T)Marshal.PtrToStructure(addr + inputOffset, typeof(T));
			}
			finally
			{
				pinned.Free();
			}
		}

		public static void ReadStructure<T>(byte[] input, int inputOffset, ref T output) where T : struct
		{
			VkPreconditions.CheckNull(input, nameof(input));
			VkPreconditions.CheckRange(inputOffset, 0, input.Length - 1, nameof(inputOffset));

			GCHandle pinned = GCHandle.Alloc(input, GCHandleType.Pinned);
			try
			{
				IntPtr addr = pinned.AddrOfPinnedObject();
				output = (T)Marshal.PtrToStructure(addr + inputOffset, typeof(T));
			}
			finally
			{
				pinned.Free();
			}
		}

		public static void Copy(object input, byte[] output, int outputOffset, int length)
		{
			if (length == 0)
				return;

			VkPreconditions.CheckNull(input, nameof(input));
			VkPreconditions.CheckNull(output, nameof(output));
			VkPreconditions.CheckRange(outputOffset, 0, int.MaxValue, nameof(outputOffset));
			VkPreconditions.CheckRange(length, 0, int.MaxValue, nameof(length));
			VkPreconditions.CheckRange(outputOffset + length > output.Length, nameof(length));

			GCHandle pinned = GCHandle.Alloc(input, GCHandleType.Pinned);
			try
			{
				IntPtr addr = pinned.AddrOfPinnedObject();
				Marshal.Copy(addr, output, outputOffset, length);
			}
			finally
			{
				pinned.Free();
			}
		}

		public static void Copy<T>(byte[] input, int inputOffset, ref T output, int length)
		{
			if (length == 0)
				return;

			VkPreconditions.CheckNull(input, nameof(input));
			VkPreconditions.CheckRange(inputOffset, 0, int.MaxValue, nameof(inputOffset));
			VkPreconditions.CheckRange(length, 0, int.MaxValue, nameof(length));
			VkPreconditions.CheckRange(inputOffset + length > input.Length, nameof(length));

			GCHandle pinned = GCHandle.Alloc(output, GCHandleType.Pinned);
			try
			{
				IntPtr addr = pinned.AddrOfPinnedObject();
				Marshal.Copy(input, inputOffset, addr, length);
			}
			finally
			{
				pinned.Free();
			}
		}
	}
}
