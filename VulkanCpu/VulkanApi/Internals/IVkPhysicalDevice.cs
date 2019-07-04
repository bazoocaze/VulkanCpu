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

namespace VulkanCpu.VulkanApi.Internals
{
	public interface IVkPhysicalDevice : VkPhysicalDevice
	{
		VkResult CreateDevice(VkDeviceCreateInfo createInfo, out VkDevice device);
		VkResult GetSurfaceSupport(int queueFamilyIndex, VkSurfaceKHR surface, out bool pSupported);
		VkResult GetSurfaceCapabilities(VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR capabilities);
		VkResult GetSurfaceFormats(VkSurfaceKHR surface, ref int formatCount, VkSurfaceFormatKHR[] formats);
		VkResult GetSurfacePresentModes(VkSurfaceKHR surface, ref int presentModeCount, VkPresentModeKHR[] presentModes);
		VkResult EnumerateExtensionProperties(ref int extensionCount, VkExtensionProperties[] availableExtensions);

		void GetMemoryProperties(out VkPhysicalDeviceMemoryProperties pMemoryProperties);
		void GetFormatProperties(VkFormat format, out VkFormatProperties pFormatProperties);
		void GetPhysicalDeviceProperties(out VkPhysicalDeviceProperties pProperties);
		void GetQueueFamilyProperties(ref int queueFamilyCount, VkQueueFamilyProperties[] queueFamilies);
	}

	public interface IVkPhysicalDeviceFactory
	{
		IEnumerable<VkPhysicalDevice> EnumeratePhysicalDevices01(VkInstance instance);
		IEnumerable<IVkPhysicalDevice> EnumeratePhysicalDevices02(VkInstance instance);
	}
}
