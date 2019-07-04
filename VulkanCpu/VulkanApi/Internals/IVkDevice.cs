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

namespace VulkanCpu.VulkanApi.Internals
{
	public interface IVkDevice : VkDevice
	{
		VkResult CreateBuffer(VkBufferCreateInfo pCreateInfo, out VkBuffer pBuffer);
		VkResult CreateCommandPool(VkCommandPoolCreateInfo commandPoolCreateInfo, out VkCommandPool commandPool);
		VkResult CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo pCreateInfo, out VkDescriptorSetLayout pSetLayout);
		VkResult CreateDescriptorPool(VkDescriptorPoolCreateInfo pCreateinfo, out VkDescriptorPool pDescriptorPool);
		VkResult CreateFrameBuffer(VkFramebufferCreateInfo frameBufferCreateInfo, out VkFramebuffer frameBuffer);
		VkResult CreateGraphicsPipelines(VkPipelineCache pipelineCache, int createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, VkPipeline[] pPipelines);
		VkResult CreateImage(VkImageCreateInfo pCreateInfo, out VkImage pImage);
		VkResult CreateImageView(VkImageViewCreateInfo createInfo, out VkImageView imageView);
		VkResult CreatePipelineLayout(VkPipelineLayoutCreateInfo pipelineLayoutInfo, out VkPipelineLayout pipelineLayout);
		VkResult CreateRenderPass(VkRenderPassCreateInfo renderPassInfo, out VkRenderPass renderPass);
		VkResult CreateSampler(VkSamplerCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkSampler pSampler);
		VkResult CreateSemaphore(VkSemaphoreCreateInfo semaphoreCreateInfo, out VkSemaphore semaphore);
		VkResult CreateShaderModule(VkShaderModuleCreateInfo createInfo, out VkShaderModule shaderModule);
		VkResult CreateSwapchain(VkSwapchainCreateInfoKHR createInfo, out VkSwapchainKHR swapChain);

		VkResult AllocateCommandBuffer(VkCommandBufferAllocateInfo commandBufferAllocateInfo, out VkCommandBuffer commandBuffer);
		VkResult AllocateCommandBuffers(VkCommandBufferAllocateInfo allocInfo, VkCommandBuffer[] commandBuffers);
		VkResult AllocateMemory(VkMemoryAllocateInfo pAllocateInfo, out VkDeviceMemory pMemory);
		VkResult AllocateDescriptorSets(VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets);
		void FreeCommandBuffers(VkCommandPool commandPool, int commandBufferCount, VkCommandBuffer[] pCommandBuffers);
		void FreeMemory(VkDeviceMemory memory);

		VkResult BindBufferMemory(VkBuffer buffer, VkDeviceMemory memory, int memoryOffset);
		VkResult BindImageMemory(VkImage image, VkDeviceMemory memory, int memoryOffset);

		VkResult MapMemory(VkDeviceMemory memory, int offset, int size, int memoryMapFlags, out byte[] ppData);
		void UnmapMemory(VkDeviceMemory memory);

		void GetQueue(int queueFamilyIndex, int queueIndex, out VkQueue pQueue);
		void GetBufferMemoryRequirements(VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements);
		void UpdateDescriptorSets(int descriptorWriteCount, VkWriteDescriptorSet[] pDescriptorWrites, int descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies); void GetImageMemoryRequirements(VkImage image, out VkMemoryRequirements pMemoryRequirements);

		void Destroy();
		void DestroySwapchainKHR(VkSwapchainKHR swapChain);
		void DestroyPipelineLayout(VkPipelineLayout pipelineLayout);
		void DestroyPipeline(VkPipeline pipeline);
		void DestroyFramebuffer(VkFramebuffer framebuffer);
		void DestroyCommandPool(VkCommandPool commandPool);
		void DestroyRenderPass(VkRenderPass renderPass);
		void DestroySemaphore(VkSemaphore semaphore);
		void DestroyImageView(VkImageView imageView);
		void DestroyShaderModule(VkShaderModule shaderModule);
		void DestroyBuffer(VkBuffer buffer);
		void DestroyDescriptorPool(VkDescriptorPool descriptorPool);
		void DestroyDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout);
		void DestroySampler(VkSampler sampler);
		void DestroyImage(VkImage image);

		void WaitIdle();

		VkResult GetSwapchainImages(VkSwapchainKHR swapChain, ref int imageCount, VkImage[] swapChainImages);
		VkResult AcquireNextImage(VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex);
	}
}

