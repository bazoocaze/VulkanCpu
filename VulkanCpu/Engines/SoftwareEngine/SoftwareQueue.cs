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

using VulkanCpu.VulkanApi;
using VulkanCpu.VulkanApi.Internals;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareQueue : IVkQueue
	{
		public SoftwareDevice m_device;
		public int m_queueFamiliIndex;

		public SoftwareQueue(SoftwareDevice device, int queueFamilyIndex)
		{
			this.m_device = device;
			this.m_queueFamiliIndex = queueFamilyIndex;
		}

		public VkResult Present(VkPresentInfoKHR pPresentInfo)
		{
			if (pPresentInfo.waitSemaphoreCount > 0)
				SoftwareSemaphore.WaitAll(pPresentInfo.waitSemaphoreCount, pPresentInfo.pWaitSemaphores);

			if (pPresentInfo.pResults == null)
				pPresentInfo.pResults = new VkResult[] { };

			while (pPresentInfo.pResults.Length < pPresentInfo.swapchainCount)
				pPresentInfo.pResults = new VkResult[] { VkResult.VK_SUCCESS };

			VkResult result = VkResult.VK_SUCCESS;

			for (int i = 0; i < pPresentInfo.swapchainCount; i++)
			{
				int imageIndex = pPresentInfo.pImageIndices[i];
				var swapChain = (SoftwareSwapchain)pPresentInfo.pSwapchains[i];
				VkResult subResult = swapChain.PresentImage(imageIndex);
				pPresentInfo.pResults[i] = subResult;
				if (subResult != VkResult.VK_SUCCESS)
					result = subResult;
			}

			return result;
		}

		public VkResult Submit(int submitCount, VkSubmitInfo[] pSubmits, VkFence fence)
		{
			for (int i = 0; i < submitCount; i++)
			{
				var submit = pSubmits[i];

				if (submit.waitSemaphoreCount > 0)
					SoftwareSemaphore.WaitAll(submit.waitSemaphoreCount, submit.pWaitSemaphores);

				for (int cmdIndex = 0; cmdIndex < submit.commandBufferCount; cmdIndex++)
				{
					var cmdBuffer = (SoftwareCommandBuffer)submit.pCommandBuffers[cmdIndex];
					cmdBuffer.Execute();
				}

				if (submit.signalSemaphoreCount > 0)
					SoftwareSemaphore.SignalAll(submit.signalSemaphoreCount, submit.pSignalSemaphores);
			}

			((SoftwareFence)fence)?.Signal();

			return VkResult.VK_SUCCESS;
		}

		public VkResult WaitIdle()
		{
			return VkResult.VK_SUCCESS;
		}
	}
}
