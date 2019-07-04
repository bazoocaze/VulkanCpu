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
	public abstract class BaseSoftwareDevice : IVkDevice
	{
		public readonly SoftwareInstance m_instance;
		public Dictionary<int, List<VkQueue>> m_Queue = new Dictionary<int, List<VkQueue>>();
		public readonly PFN_vkDebugReportMessageEXT DebugReportMessage;
		public readonly List<VkDescriptorPool> m_DescriptorPools;

		public abstract VkResult CreateCommandPool(VkCommandPoolCreateInfo commandPoolCreateInfo, out VkCommandPool commandPool);
		public abstract VkResult CreateFrameBuffer(VkFramebufferCreateInfo frameBufferCreateInfo, out VkFramebuffer frameBuffer);
		public abstract VkResult CreateSemaphore(VkSemaphoreCreateInfo semaphoreCreateInfo, out VkSemaphore semaphore);
		public abstract VkResult CreateImageView(VkImageViewCreateInfo createInfo, out VkImageView imageView);
		public abstract VkResult CreateSwapchain(VkSwapchainCreateInfoKHR createInfo, out VkSwapchainKHR swapChain);
		public abstract VkResult CreateBuffer(VkBufferCreateInfo pCreateInfo, out VkBuffer pBuffer);
		public abstract VkResult CreateGraphicsPipelines(VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo, out VkPipeline pipeline);
		public abstract VkResult CreateImage(VkImageCreateInfo pCreateInfo, out VkImage pImage);
		public abstract VkResult CreateSampler(VkSamplerCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkSampler pSampler);

		public abstract VkResult AllocateCommandBuffer(VkCommandBufferAllocateInfo commandBufferAllocateInfo, out VkCommandBuffer commandBuffer);
		public abstract VkResult AllocateMemory(VkMemoryAllocateInfo pAllocateInfo, out VkDeviceMemory pMemory);
		public abstract VkResult BindBufferMemory(VkBuffer buffer, VkDeviceMemory memory, int memoryOffset);
		public abstract VkResult BindImageMemory(VkImage image, VkDeviceMemory memory, int memoryOffset);
		public abstract VkResult MapMemory(VkDeviceMemory memory, int offset, int size, int memoryMapFlags, out byte[] ppData);
		public abstract void UnmapMemory(VkDeviceMemory memory);
		public abstract void GetBufferMemoryRequirements(VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements);
		public abstract void GetImageMemoryRequirements(VkImage image, out VkMemoryRequirements pMemoryRequirements);

		public abstract void DestroyPipelineLayout(VkPipelineLayout pipelineLayout);
		public abstract void DestroyPipeline(VkPipeline pipeline);
		public abstract void DestroyFramebuffer(VkFramebuffer framebuffer);
		public abstract void DestroyCommandPool(VkCommandPool commandPool);
		public abstract void DestroyRenderPass(VkRenderPass renderPass);
		public abstract void DestroySemaphore(VkSemaphore semaphore);
		public abstract void DestroyImageView(VkImageView imageView);
		public abstract void DestroyShaderModule(VkShaderModule shaderModule);
		public abstract void DestroyBuffer(VkBuffer buffer);
		public abstract void DestroyDescriptorPool(VkDescriptorPool descriptorPool);
		public abstract void DestroyDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout);
		public abstract void DestroySampler(VkSampler sampler);
		public abstract void DestroyImage(VkImage image);

		public abstract void FreeCommandBuffers(VkCommandPool commandPool, int commandBufferCount, VkCommandBuffer[] pCommandBuffers);
		public abstract void FreeMemory(VkDeviceMemory memory);

		public abstract void WaitIdle();
		public abstract VkResult CreateShaderModule(VkShaderModuleCreateInfo createInfo, out VkShaderModule shaderModule);
		public abstract VkResult GetSwapchainImages(VkSwapchainKHR swapchain, ref int imageCount, VkImage[] swapChainImages);
		public abstract VkResult AcquireNextImage(VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex);

		public abstract VkResult CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo pCreateInfo, out VkDescriptorSetLayout pSetLayout);
		public abstract VkResult CreateDescriptorPool(VkDescriptorPoolCreateInfo pCreateinfo, out VkDescriptorPool pDescriptorPool);
		public abstract VkResult CreatePipelineLayout(VkPipelineLayoutCreateInfo pipelineLayoutInfo, out VkPipelineLayout pipelineLayout);
		public abstract VkResult CreateRenderPass(VkRenderPassCreateInfo renderPassInfo, out VkRenderPass renderPass);
		public abstract void UpdateDescriptorSets(int descriptorWriteCount, VkWriteDescriptorSet[] pDescriptorWrites, int descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies);
		public abstract VkResult CreateGraphicsPipelines(VkPipelineCache pipelineCache, int createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, VkPipeline[] pPipelines);
		public abstract VkResult AllocateCommandBuffers(VkCommandBufferAllocateInfo allocInfo, VkCommandBuffer[] commandBuffers);
		public abstract VkResult AllocateDescriptorSets(VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets);
		public abstract void DestroySwapchainKHR(VkSwapchainKHR swapChain);

		protected BaseSoftwareDevice(SoftwareInstance instance)
		{
			this.m_instance = instance;
			this.DebugReportMessage = m_instance.DebugReportMessage;
			this.m_DescriptorPools = new List<VkDescriptorPool>();
		}

		public virtual void Destroy()
		{
		}

		public virtual void GetQueue(int queueFamilyIndex, int queueIndex, out VkQueue pQueue)
		{
			pQueue = m_Queue[queueFamilyIndex][queueIndex];
		}
	}
}

