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
using System.Collections.Generic;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.DummyEngine
{
	public class DummySwapchain : VkSwapchainKHR
	{
		public readonly DummyDevice m_device;
		public int m_NextImageIndex;
		public VkSwapchainCreateInfoKHR m_CreateInfo;
		public readonly List<VkImage> m_Images;

		public DummySwapchain(DummyDevice m_device, VkSwapchainCreateInfoKHR createInfo)
		{
			this.m_device = m_device;
			this.m_CreateInfo = createInfo;
			this.m_Images = new List<VkImage>();

			while (m_Images.Count < Math.Max(1, createInfo.minImageCount))
			{
				m_Images.Add(new DummyImage(m_device, createInfo));
			}
		}

		public VkResult AcquireNextImage(VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex)
		{
			pImageIndex = m_NextImageIndex;
			m_NextImageIndex = (m_NextImageIndex + 1) % m_Images.Count;
			// semaphore?.Signal();
			// fence?.Signal();
			return VkResult.VK_SUCCESS;
		}

		public VkResult PresentImage(int imageIndex)
		{
			return VkResult.VK_SUCCESS;
		}
	}
}