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

using System.Collections.Generic;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.VulkanApi.Internals
{
	public abstract class BaseSoftwarePhysicalDevice : IVkPhysicalDevice
	{
		public SoftwareInstance m_Instance;
		public List<VkQueueFamilyProperties> m_QueueFamilyProperties = new List<VkQueueFamilyProperties>();
		public List<VkDevice> m_Devices = new List<VkDevice>();
		public List<VkExtensionProperties> m_ExtensionProperties = new List<VkExtensionProperties>();
		public List<VkSurfaceFormatKHR> m_SurfaceFormats = new List<VkSurfaceFormatKHR>();
		public List<VkPresentModeKHR> m_PresentModes = new List<VkPresentModeKHR>();
		public VkPhysicalDeviceMemoryProperties m_PhysicalDeviceMemoryProperties;

		public abstract void GetFormatProperties(VkFormat format, out VkFormatProperties pFormatProperties);
		public abstract void GetPhysicalDeviceProperties(out VkPhysicalDeviceProperties pProperties);
		public abstract VkResult CreateDevice(VkDeviceCreateInfo createInfo, out VkDevice device);
		public abstract VkResult GetSurfaceSupport(int queueFamilyIndex, VkSurfaceKHR surface, out bool pSupported);

		private IVkSurface GetSurface(VkSurfaceKHR surface)
		{
			return (IVkSurface)surface;
		}

		public virtual VkResult GetSurfaceCapabilities(VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR capabilities)
		{
			return GetSurface(surface).GetCapabilities(out capabilities);
		}

		public virtual VkResult GetSurfaceFormats(VkSurfaceKHR surface, ref int formatCount, VkSurfaceFormatKHR[] formats)
		{
			return GetSurface(surface).GetFormats(ref formatCount, formats);
		}

		public virtual VkResult GetSurfacePresentModes(VkSurfaceKHR surface, ref int presentModeCount, VkPresentModeKHR[] presentModes)
		{
			return GetSurface(surface).GetPresentModes(ref presentModeCount, presentModes);
		}

		public virtual VkResult EnumerateExtensionProperties(ref int extensionCount, VkExtensionProperties[] availableExtensions)
		{
			return VkArrayUtil.CopyToList(m_ExtensionProperties, availableExtensions, ref extensionCount);
		}

		public virtual void GetMemoryProperties(out VkPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			pMemoryProperties = m_PhysicalDeviceMemoryProperties;
		}

		public void GetQueueFamilyProperties(ref int queueFamilyCount, VkQueueFamilyProperties[] queueFamilies)
		{
			VkArrayUtil.CopyToList(m_QueueFamilyProperties, queueFamilies, ref queueFamilyCount);
		}
	}
}
