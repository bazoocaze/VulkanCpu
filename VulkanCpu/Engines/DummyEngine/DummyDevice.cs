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
using VulkanCpu.VulkanApi.Internals;

namespace VulkanCpu.Engines.DummyEngine
{
	public class DummyDevice : BaseSoftwareDevice
	{
		private VkDeviceCreateInfo m_createInfo;
		private DummyPhysicalDevice m_PhysicalDevice;

		public DummyDevice(DummyPhysicalDevice physicalDevice, VkDeviceCreateInfo createInfo)
			: base(physicalDevice.m_Instance)
		{
			this.m_PhysicalDevice = physicalDevice;
			this.m_createInfo = createInfo;

			foreach (var item in createInfo.pQueueCreateInfos)
			{
				List<VkQueue> list = new List<VkQueue>();

				for (int i = 0; i < item.queueCount; i++)
					list.Add(new DummyQueue(this, item.queueFamilyIndex));

				m_Queue.Add(item.queueFamilyIndex, list);
			}
		}

		public override VkResult AllocateCommandBuffer(VkCommandBufferAllocateInfo commandBufferAllocateInfo, out VkCommandBuffer commandBuffer)
		{
			commandBuffer = new DummyCommandBuffer(this, commandBufferAllocateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateGraphicsPipelines(VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo, out VkPipeline pipeline)
		{
			pipeline = new DummyGraphicsPipeline(this, graphicsPipelineCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateFrameBuffer(VkFramebufferCreateInfo frameBufferCreateInfo, out VkFramebuffer frameBuffer)
		{
			frameBuffer = new DummyFramebuffer(this, frameBufferCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateImageView(VkImageViewCreateInfo createInfo, out VkImageView imageView)
		{
			imageView = new DummyImageView();
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateSemaphore(VkSemaphoreCreateInfo semaphoreCreateInfo, out VkSemaphore semaphore)
		{
			semaphore = new DummySemaphore(this, semaphoreCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateSwapchain(VkSwapchainCreateInfoKHR createInfo, out VkSwapchainKHR swapChain)
		{
			swapChain = new DummySwapchain(this, createInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateBuffer(VkBufferCreateInfo pCreateInfo, out VkBuffer pBuffer)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateCommandPool(VkCommandPoolCreateInfo commandPoolCreateInfo, out VkCommandPool commandPool)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateImage(VkImageCreateInfo pCreateInfo, out VkImage pImage)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateSampler(VkSamplerCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkSampler pSampler)
		{
			throw new NotImplementedException();
		}

		public override VkResult AllocateMemory(VkMemoryAllocateInfo pAllocateInfo, out VkDeviceMemory pMemory)
		{
			throw new NotImplementedException();
		}

		public override VkResult BindBufferMemory(VkBuffer buffer, VkDeviceMemory memory, int memoryOffset)
		{
			throw new NotImplementedException();
		}

		public override VkResult BindImageMemory(VkImage image, VkDeviceMemory memory, int memoryOffset)
		{
			throw new NotImplementedException();
		}

		public override VkResult MapMemory(VkDeviceMemory memory, int offset, int size, int memoryMapFlags, out byte[] ppData)
		{
			throw new NotImplementedException();
		}

		public override void UnmapMemory(VkDeviceMemory memory)
		{
			throw new NotImplementedException();
		}

		public override void GetBufferMemoryRequirements(VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements)
		{
			throw new NotImplementedException();
		}

		public override void GetImageMemoryRequirements(VkImage image, out VkMemoryRequirements pMemoryRequirements)
		{
			throw new NotImplementedException();
		}

		public override void DestroyPipelineLayout(VkPipelineLayout pipelineLayout)
		{
			throw new NotImplementedException();
		}

		public override void DestroyPipeline(VkPipeline pipeline)
		{
			throw new NotImplementedException();
		}

		public override void DestroyFramebuffer(VkFramebuffer framebuffer)
		{
			throw new NotImplementedException();
		}

		public override void DestroyCommandPool(VkCommandPool commandPool)
		{
			throw new NotImplementedException();
		}

		public override void DestroyRenderPass(VkRenderPass renderPass)
		{
			throw new NotImplementedException();
		}

		public override void FreeCommandBuffers(VkCommandPool commandPool, int commandBufferCount, VkCommandBuffer[] pCommandBuffers)
		{
			throw new NotImplementedException();
		}

		public override void DestroySemaphore(VkSemaphore semaphore)
		{
			throw new NotImplementedException();
		}

		public override void DestroyImageView(VkImageView imageView)
		{
			throw new NotImplementedException();
		}

		public override void DestroyShaderModule(VkShaderModule shaderModule)
		{
			throw new NotImplementedException();
		}

		public override void DestroyBuffer(VkBuffer buffer)
		{
			throw new NotImplementedException();
		}

		public override void DestroyDescriptorPool(VkDescriptorPool descriptorPool)
		{
			throw new NotImplementedException();
		}

		public override void DestroyDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout)
		{
			throw new NotImplementedException();
		}

		public override void DestroySampler(VkSampler sampler)
		{
			throw new NotImplementedException();
		}

		public override void DestroyImage(VkImage image)
		{
			throw new NotImplementedException();
		}

		public override void FreeMemory(VkDeviceMemory memory)
		{
			throw new NotImplementedException();
		}

		public override void WaitIdle()
		{
			throw new NotImplementedException();
		}

		public override VkResult GetSwapchainImages(VkSwapchainKHR swapChain, ref int imageCount, VkImage[] swapChainImages)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateShaderModule(VkShaderModuleCreateInfo createInfo, out VkShaderModule shaderModule)
		{
			throw new NotImplementedException();
		}

		public override VkResult AcquireNextImage(VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo pCreateInfo, out VkDescriptorSetLayout pSetLayout)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateDescriptorPool(VkDescriptorPoolCreateInfo pCreateinfo, out VkDescriptorPool pDescriptorPool)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreatePipelineLayout(VkPipelineLayoutCreateInfo pipelineLayoutInfo, out VkPipelineLayout pipelineLayout)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateRenderPass(VkRenderPassCreateInfo renderPassInfo, out VkRenderPass renderPass)
		{
			throw new NotImplementedException();
		}

		public override void UpdateDescriptorSets(int descriptorWriteCount, VkWriteDescriptorSet[] pDescriptorWrites, int descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies)
		{
			throw new NotImplementedException();
		}

		public override VkResult CreateGraphicsPipelines(VkPipelineCache pipelineCache, int createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, VkPipeline[] pPipelines)
		{
			throw new NotImplementedException();
		}

		public override VkResult AllocateCommandBuffers(VkCommandBufferAllocateInfo allocInfo, VkCommandBuffer[] commandBuffers)
		{
			throw new NotImplementedException();
		}

		public override VkResult AllocateDescriptorSets(VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets)
		{
			throw new NotImplementedException();
		}

		public override void DestroySwapchainKHR(VkSwapchainKHR swapChain)
		{
			throw new NotImplementedException();
		}
	}

	public class DummyImageView : VkImageView { }
}
