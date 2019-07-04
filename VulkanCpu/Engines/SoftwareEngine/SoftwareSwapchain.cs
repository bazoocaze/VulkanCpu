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
using VulkanCpu.VulkanApi;
using VulkanCpu.VulkanApi.Internals;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareSwapchain : VkSwapchainKHR
	{
		public readonly SoftwareDevice m_device;
		public readonly BaseSoftwareSurface m_surface;
		public readonly VkSwapchainCreateInfoKHR m_createInfo;
		public readonly List<SoftwareImage> m_Images;

		public int m_NextImageIndex;

		public SoftwareSwapchain(SoftwareDevice device, VkSwapchainCreateInfoKHR createInfo)
		{
			this.m_device = device;
			this.m_surface = (BaseSoftwareSurface)createInfo.surface;
			this.m_createInfo = createInfo;
			this.m_Images = new List<SoftwareImage>();
		}

		public static VkResult Create(SoftwareDevice device, VkSwapchainCreateInfoKHR createInfo, out VkSwapchainKHR swapChain)
		{
			SoftwareSwapchain retVal = new SoftwareSwapchain(device, createInfo);
			retVal.m_Images.Clear();

			while (retVal.m_Images.Count < createInfo.minImageCount)
			{
				VkResult resultCode;
				SoftwareImage image = SoftwareImage.CreateSwapchainImage(device, createInfo, out resultCode);
				if (resultCode != VkResult.VK_SUCCESS)
				{
					swapChain = default(VkSwapchainKHR);
					return resultCode;
				}
				retVal.m_Images.Add(image);
			}

			swapChain = retVal;
			return VkResult.VK_SUCCESS;
		}

		public VkResult AcquireNextImage(long timeout, SoftwareSemaphore semaphore, SoftwareFence fence, out int pImageIndex)
		{
			// TODO: SoftwareSwapchain.AcquireNextImage()

			pImageIndex = m_NextImageIndex;
			m_NextImageIndex = (m_NextImageIndex + 1) % m_Images.Count;

			semaphore?.Signal();
			fence?.Signal();

			return VkResult.VK_SUCCESS;
		}

		public VkResult PresentImage(int imageIndex)
		{
			// TODO: SoftwareSwapchain.PresentImage()

			VkExtent2D imageExtent = new VkExtent2D(
				m_Images[imageIndex].m_imageExtent.width,
				m_Images[imageIndex].m_imageExtent.height);
			return ((BaseSoftwareSurface)m_createInfo.surface).PresentImage(m_Images[imageIndex], imageExtent);
		}

		public VkResult GetSwapchainImages(ref int imageCount, VkImage[] swapChainImages)
		{
			return VkArrayUtil.CopyToList(m_Images, swapChainImages, ref imageCount);
		}

		public void Destroy()
		{
		}
	}
}
