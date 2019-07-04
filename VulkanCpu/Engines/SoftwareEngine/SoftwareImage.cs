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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GlmSharp;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareImage : VkImage
	{
		public static SoftwareImage CreateSwapchainImage(SoftwareDevice device, VkSwapchainCreateInfoKHR swapchainInfo, out VkResult result)
		{
			VkImageCreateInfo createInfo = new VkImageCreateInfo();
			createInfo.extent = VkExtent3D.Create(swapchainInfo.imageExtent.width, swapchainInfo.imageExtent.height, 1);
			createInfo.format = swapchainInfo.imageFormat;
			createInfo.imageType = VkImageType.VK_IMAGE_TYPE_2D;
			createInfo.arrayLayers = swapchainInfo.imageArrayLayers;
			createInfo.sharingMode = swapchainInfo.imageSharingMode;
			createInfo.usage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VkImageUsageFlags.VK_IMAGE_USAGE_INPUT_ATTACHMENT_BIT;
			createInfo.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
			createInfo.mipLevels = 1;
			createInfo.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;

			return new SoftwareImage(device, createInfo, out result);
		}

		public static SoftwareImage CreateImage(SoftwareDevice device, VkImageCreateInfo createInfo, out VkResult result)
		{
			return new SoftwareImage(device, createInfo, out result);
		}

		private SoftwareDevice m_device;
		private VkImageCreateInfo m_createInfo;

		internal VkFormat m_imageFormat;
		internal VkColorSpaceKHR m_imageColorSpace;
		internal VkExtent3D m_imageExtent;

		internal int[] m_imageData;
		internal float[] m_depthData;
		internal int[] m_stencilData;

		internal SoftwareDeviceMemory m_memory;
		internal int m_memoryOffset;

		private SoftwareImage(SoftwareDevice device, VkImageCreateInfo pCreateInfo, out VkResult result)
		{
			this.m_device = device;
			this.m_createInfo = pCreateInfo;

			this.m_imageFormat = pCreateInfo.format;
			this.m_imageExtent = pCreateInfo.extent;
			this.m_imageColorSpace = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR;

			Initialize(out result);
		}

		private SoftwareImage(SoftwareDevice device, VkSwapchainCreateInfoKHR createInfo, out VkResult result)
		{
			this.m_device = device;

			this.m_imageFormat = createInfo.imageFormat;
			this.m_imageExtent = VkExtent3D.Create(createInfo.imageExtent.width, createInfo.imageExtent.height, 1);
			this.m_imageColorSpace = createInfo.imageColorSpace;

			Initialize(out result);
		}

		private SoftwareImage(SoftwareDevice device, VkFormat format, VkExtent3D imageExtent, VkColorSpaceKHR colorSpace, out VkResult result)
		{
			this.m_device = device;
			this.m_imageFormat = format;
			this.m_imageExtent = imageExtent;
			this.m_imageColorSpace = colorSpace;

			Initialize(out result);
		}

		internal float ReadPixel_float(ivec2 pos)
		{
			return 1f;
		}

		internal void WritePixel(ivec2 coord, float depth)
		{
		}

		private void Initialize(out VkResult result)
		{
			if (m_imageColorSpace != VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
			{
				result = VkResult.VK_ERROR_FORMAT_NOT_SUPPORTED;
				return;
			}

			switch (m_imageFormat)
			{
				case VkFormat.VK_FORMAT_B8G8R8A8_UNORM:
					// B8G8R8A8 / 0xAARRGGBB
					InitializeImage_B8G8R8A8_UNORM_SRGB_NONLINEAR();
					break;

				case VkFormat.VK_FORMAT_R8G8B8A8_UNORM:
					// R8G8B8A8 / 0xAABBGGRR
					InitializeImage_B8G8R8A8_UNORM_SRGB_NONLINEAR();
					break;

				case VkFormat.VK_FORMAT_D32_SFLOAT:
					InitializeDepthBuffer();
					break;

				default:
					result = VkResult.VK_ERROR_FORMAT_NOT_SUPPORTED;
					return;
			}

			result = VkResult.VK_SUCCESS;
		}

		private void InitializeDepthBuffer()
		{
			int arraySize = m_imageExtent.height * m_imageExtent.width;
			m_depthData = new float[arraySize];
		}

		private void InitializeStencilBuffer()
		{
			int arraySize = m_imageExtent.height * m_imageExtent.width;
			m_stencilData = new int[arraySize];
		}

		private void InitializeImage_B8G8R8A8_UNORM_SRGB_NONLINEAR()
		{
			int pixelSize = 1;
			int arraySize = m_imageExtent.height * m_imageExtent.width * pixelSize;
			m_imageData = new int[arraySize];

			// TODO: InitializeImage_B8G8R8A8_UNORM_SRGB_NONLINEAR

			FillRandom();
		}

		internal void GetImageMemoryRequirements(out VkMemoryRequirements pMemoryRequirements)
		{
			int pixelSize = 4;
			pMemoryRequirements.alignment = 4;
			pMemoryRequirements.memoryTypeBits = int.MaxValue;
			pMemoryRequirements.size = m_imageExtent.width * m_imageExtent.height * m_imageExtent.depth * pixelSize;
		}

		private void FillTeste()
		{
			// B8G8R8A8 / 0xAARRGGBB
			UInt32[] colors = new UInt32[] { 0xFFFF0000, 0xFF00FF00, 0xFF0000FF };

			int width = m_imageExtent.width;
			int height = Math.Min(64, m_imageExtent.height);
			int barSize = 32;
			int lastX = 0;
			foreach (var color in colors)
			{
				for (int x = 0; x < barSize; x++)
				{
					for (int y = 0; y < height; y++)
					{
						m_imageData[lastX + x + (y * width)] = (int)color;
					}
				}
				lastX += barSize;
			}
		}

		private void FillRandom()
		{
			Random rnd = new Random();
			int width = m_imageExtent.width;
			int height = m_imageExtent.height;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					unchecked
					{
						m_imageData[x + (y * width)] = (int)(rnd.Next() | 0xFF000000);
					}
				}
			}
		}

		internal int GetWidth()
		{
			return m_imageExtent.width;
		}

		internal int GetHeight()
		{
			return m_imageExtent.height;
		}

		internal void ClearColor(VkClearValue clearValue)
		{
			Fill(m_imageData, (int)clearValue.color.uint32_scalar, m_imageExtent.width, m_imageExtent.height, m_imageExtent.width);
		}

		internal void ClearDepth(VkClearValue clearValue)
		{
			Fill(m_depthData, clearValue.depthStencil.depth, m_imageExtent.width, m_imageExtent.height, m_imageExtent.width);
		}

		internal void FillStencil(int stencil)
		{
			Fill(m_stencilData, stencil, m_imageExtent.width, m_imageExtent.height, m_imageExtent.width);
		}

		internal static void Fill<T>(T[] target, T fillValue, int width, int height, int stride) where T : struct
		{
			var pointSize = Marshal.SizeOf(typeof(T));

			for (int x = 0; x < width; x++)
			{
				target[x] = fillValue;
			}

			Array array = target;

			int strideBytes = stride * pointSize;
			int widthBytes = width * pointSize;

			for (int y = 1; y < height; y++)
			{
				Buffer.BlockCopy(array, 0, array, y * strideBytes, widthBytes);
			}
		}

		internal void SetPixel(ivec2 position, uint color)
		{
			var height = m_imageExtent.height;
			var width = m_imageExtent.width;

			if (position.x < 0 || position.y < 0 || position.x >= width || position.y >= height)
				return;

			m_imageData[position.x + (position.y * width)] = (int)color;
		}

		internal uint GetPixel(ivec2 position)
		{
			var height = m_imageExtent.height;
			var width = m_imageExtent.width;

			if (position.x < 0 || position.y < 0 || position.x >= width || position.y >= height)
				return 0XFF997755;

			return (uint)m_imageData[position.x + (position.y * width)];
		}

		public void Destroy()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool OutOfBounds(ivec2 position)
		{
			if (position.x < 0 || position.y < 0 || position.x >= m_imageExtent.width || position.y >= m_imageExtent.height)
				return true;
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal float ReadDepth(ivec2 position)
		{
			if (OutOfBounds(position)) return 1.0f;
			return m_depthData[position.x + (position.y * m_imageExtent.width)];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteDepth(ivec2 position, float depth)
		{
			if (OutOfBounds(position)) return;
			m_depthData[position.x + (position.y * m_imageExtent.width)] = depth;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int ReadStencil(ivec2 position)
		{
			if (OutOfBounds(position)) return 0;

			return m_stencilData[position.x + (position.y * m_imageExtent.width)];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteStencil(ivec2 position, int stencil)
		{
			if (OutOfBounds(position)) return;

			m_stencilData[position.x + (position.y * m_imageExtent.width)] = stencil;
		}

		public override string ToString()
		{
			return string.Format("{0}x{1} format={2}", m_imageExtent.width, m_imageExtent.height, m_imageFormat);
		}

		internal VkResult BindMemory(SoftwareDeviceMemory memory, int memoryOffset)
		{
			// TODO: SoftwareImage.BindMemory()

			this.m_memory = memory;
			this.m_memoryOffset = memoryOffset;
			return VkResult.VK_SUCCESS;
		}

		internal void CopyBufferToImage(VkBuffer srcBuffer, VkImageLayout dstImageLayout, int regionCount, VkBufferImageCopy[] pRegions)
		{
			int dstPixelSize = 4;
			SoftwareBuffer softwareBuffer = (SoftwareBuffer)srcBuffer;
			SoftwareDeviceMemory sdm = softwareBuffer.m_deviceMemory;

			int dstLineStride = m_imageExtent.width * dstPixelSize;
			int dstDepthStride = m_imageExtent.width * m_imageExtent.height * dstPixelSize;

			for (int nRegion = 0; nRegion < regionCount; nRegion++)
			{
				var region = pRegions[nRegion];
				int regLineStride = region.imageExtent.width * dstPixelSize;

				for (int z = 0; z < region.imageExtent.depth; z++)
				{
					int bufferOffset = region.bufferOffset;
					int bufferImageHeight = region.bufferImageHeight;
					int bufferRowLength = region.bufferRowLength;
					if (bufferImageHeight == 0 || bufferRowLength == 0)
					{
						bufferImageHeight = m_imageExtent.height;
						bufferRowLength = m_imageExtent.width * dstPixelSize;
					}

					int srcLineStride = bufferRowLength;
					int srcDepthStride = bufferImageHeight * bufferRowLength;

					int srcOffset = (z * srcDepthStride) + bufferOffset + softwareBuffer.m_memoryOffset;
					int dstOffset = ((region.imageOffset.z + z) * dstDepthStride) + (region.imageOffset.y * dstLineStride) + (region.imageOffset.x * dstPixelSize);

					for (int y = 0; y < region.imageExtent.height; y++)
					{
						int imgOffs = dstOffset + (y * dstLineStride);
						int bufOffs = srcOffset + (y * srcLineStride);
						Buffer.BlockCopy(sdm.m_bytes, bufOffs, m_imageData, imgOffs, regLineStride);
					}
				}
			}
		}
	}
}
