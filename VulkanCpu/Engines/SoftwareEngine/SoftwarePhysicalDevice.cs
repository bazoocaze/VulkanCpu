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
using VulkanCpu.VulkanApi;
using VulkanCpu.VulkanApi.Internals;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwarePhysicalDevice : BaseSoftwarePhysicalDevice
	{
		public SoftwarePhysicalDevice(SoftwareInstance instance)
		{
			m_Instance = instance;

			m_QueueFamilyProperties.Add(VkQueueFamilyProperties.Create(1, VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT));
			m_QueueFamilyProperties.Add(VkQueueFamilyProperties.Create(1, VkQueueFlagBits.VK_QUEUE_COMPUTE_BIT));

			m_ExtensionProperties.Add(VkExtensionProperties.Create(VkExtensionNames.VK_KHR_SWAPCHAIN_EXTENSION_NAME, 1));

			m_PhysicalDeviceMemoryProperties = new VkPhysicalDeviceMemoryProperties();
			m_PhysicalDeviceMemoryProperties.memoryTypeCount = 1;
			m_PhysicalDeviceMemoryProperties.memoryTypes = new VkMemoryType[]
			{
				new VkMemoryType() {
					heapIndex = 0,
					propertyFlags = VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT
				}
			};
			m_PhysicalDeviceMemoryProperties.memoryHeapCount = 1;
			m_PhysicalDeviceMemoryProperties.memoryHeaps = new VkMemoryHeap[]
			{
				new VkMemoryHeap() { flags = VkMemoryHeapFlagBits.VK_MEMORY_HEAP_DEVICE_LOCAL_BIT, size = 64 * 1024 * 1024 }
			};
		}

		public override VkResult CreateDevice(VkDeviceCreateInfo createInfo, out VkDevice device)
		{
			return SoftwareDevice.Create(this, createInfo, out device);
		}

		public override VkResult GetSurfaceSupport(int queueFamilyIndex, VkSurfaceKHR surface, out bool pSupported)
		{
			// TODO: SoftwarePhysicalDevice.GetSurfaceSupport
			pSupported = true;
			return VkResult.VK_SUCCESS;
		}

		public override void GetFormatProperties(VkFormat format, out VkFormatProperties pFormatProperties)
		{
			switch (format)
			{
				case VkFormat.VK_FORMAT_D32_SFLOAT_S8_UINT: /* FALL_THROUGH */
				case VkFormat.VK_FORMAT_D32_SFLOAT:
					pFormatProperties = VkFormatProperties.Create(VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT, VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT, 0);
					return;

				/* UNSUPPORTED */
				case VkFormat.VK_FORMAT_D24_UNORM_S8_UINT:
					pFormatProperties = new VkFormatProperties();
					return;

				default: throw new NotImplementedException();
			}
		}

		public override void GetPhysicalDeviceProperties(out VkPhysicalDeviceProperties pProperties)
		{
			throw new NotImplementedException();
		}
	}
}
