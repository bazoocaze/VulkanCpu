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
using System.Diagnostics;
using VulkanCpu.VulkanApi.Internals;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.VulkanApi
{
	public class Vulkan
	{
		public const int VK_QUEUE_FAMILY_IGNORED = int.MaxValue;
		public const int VK_ATTACHMENT_UNUSED = int.MaxValue;
		public const int VK_SUBPASS_EXTERNAL = int.MaxValue;
		public static readonly uint VK_API_VERSION_1_0 = VK_MAKE_VERSION(1, 0, 0);

		public static uint VK_MAKE_VERSION(uint major, uint minor, uint patch) => ((major << 22) | (minor << 12) | patch);
		public static uint VK_VERSION_MAJOR(uint version) => (version >> 22);
		public static uint VK_VERSION_MINOR(uint version) => ((version >> 12) & 0x3ff);
		public static uint VK_VERSION_PATCH(uint version) => (version & 0xfff);

		private static List<VkLayerProperties> m_GlobalLayerProperties;
		private static List<VkExtensionProperties> m_GlobalExtensionProperties;

		static Vulkan()
		{
			m_GlobalLayerProperties = new List<VkLayerProperties>();
			m_GlobalLayerProperties.Add(VkLayerProperties.Create("VK_LAYER_LUNARG_standard_validation", "VK_LAYER_LUNARG_standard_validation", 1, 2));

			m_GlobalExtensionProperties = new List<VkExtensionProperties>();
		}

		[DebuggerStepThrough]
		private static IVkInstance GetInstance(VkInstance instance)
		{
			return (IVkInstance)instance;
		}

		[DebuggerStepThrough]
		private static IVkPhysicalDevice GetPhysicalDevice(VkPhysicalDevice physicalDevice)
		{
			return (IVkPhysicalDevice)physicalDevice;
		}

		[DebuggerStepThrough]
		private static IVkDevice GetDevice(VkDevice device)
		{
			return (IVkDevice)device;
		}

		[DebuggerStepThrough]
		private static IVkQueue GetQueue(VkQueue queue)
		{
			return (IVkQueue)queue;
		}

		[DebuggerStepThrough]
		private static IVkCommandBuffer GetCommandBuffer(VkCommandBuffer commandBuffer)
		{
			return (IVkCommandBuffer)commandBuffer;
		}

		public static VkResult vkEnumerateInstanceLayerProperties(ref int pPropertyCount, VkLayerProperties[] pProperties)
		{
			return VkArrayUtil.CopyToList(m_GlobalLayerProperties, pProperties, ref pPropertyCount);
		}

		public static VkResult vkEnumerateInstanceExtensionProperties(string pLayerName, ref int pPropertyCount, VkExtensionProperties[] pProperties)
		{
			if (pLayerName == null)
			{
				return VkArrayUtil.CopyToList(m_GlobalExtensionProperties, pProperties, ref pPropertyCount);
			}
			return VkResult.VK_SUCCESS;
		}

		public static VkResult vkEnumeratePhysicalDevices(VkInstance instance, ref int pPhysicalDeviceCount, VkPhysicalDevice[] pPhysicalDevices)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));

			return GetInstance(instance).EnumeratePhysicalDevices(ref pPhysicalDeviceCount, pPhysicalDevices);
		}

		public static void vkGetPhysicalDeviceProperties(VkPhysicalDevice pPhysicalDevice, out VkPhysicalDeviceProperties pProperties)
		{
			VkPreconditions.CheckNull(pPhysicalDevice, nameof(pPhysicalDevice));
			GetPhysicalDevice(pPhysicalDevice).GetPhysicalDeviceProperties(out pProperties);
		}

		public static VkResult vkCreateInstance(VkInstanceCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkInstance pInstance)
		{
			pInstance = new SoftwareInstance(pCreateInfo);

			return VkResult.VK_SUCCESS;
		}

		public static VkResult vkCreateDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackCreateInfoEXT pCreateInfo, VkAllocationCallbacks pAllocator, out VkDebugReportCallbackEXT pCallback)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));

			return GetInstance(instance).CreateDebugReportCallback(pCreateInfo, out pCallback);
		}

		public static void vkDestroyInstance(VkInstance instance, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));

			GetInstance(instance).Destroy();
		}

		public static void vkDestroyPipelineLayout(VkDevice device, VkPipelineLayout pipelineLayout, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyPipelineLayout(pipelineLayout);
		}

		public static void vkDestroyPipeline(VkDevice device, VkPipeline pipeline, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyPipeline(pipeline);
		}

		public static void vkDestroyFramebuffer(VkDevice device, VkFramebuffer framebuffer, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyFramebuffer(framebuffer);
		}

		public static void vkDestroyCommandPool(VkDevice device, VkCommandPool commandPool, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyCommandPool(commandPool);
		}

		public static void vkDestroyRenderPass(VkDevice device, VkRenderPass renderPass, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyRenderPass(renderPass);
		}

		public static void vkFreeCommandBuffers(VkDevice device, VkCommandPool commandPool, int commandBufferCount, VkCommandBuffer[] pCommandBuffers)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(commandPool, nameof(commandPool));
			VkPreconditions.CheckNull(pCommandBuffers, nameof(pCommandBuffers));
			VkPreconditions.CheckRange(commandBufferCount, 0, pCommandBuffers.Length, nameof(commandBufferCount));

			GetDevice(device).FreeCommandBuffers(commandPool, commandBufferCount, pCommandBuffers);
		}

		public static void vkDestroySemaphore(VkDevice device, VkSemaphore semaphore, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroySemaphore(semaphore);
		}

		public static void vkDestroyImageView(VkDevice device, VkImageView imageView, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyImageView(imageView);
		}

		public static void vkDestroySurfaceKHR(VkInstance instance, VkSurfaceKHR surface, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));

			GetInstance(instance).DestroySurfaceKHR(surface);
		}

		public static void vkDestroyDevice(VkDevice device, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).Destroy();
		}

		public static void vkDestroyDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackEXT callback, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));

			if (callback != null)
			{
				GetInstance(instance).DestroyDebugReportCallback(callback);
			}
		}

		public static void vkDestroySwapchainKHR(VkDevice device, VkSwapchainKHR swapChain, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			if (swapChain != null)
			{
				GetDevice(device).DestroySwapchainKHR(swapChain);
			}
		}

		public static void vkDestroyShaderModule(VkDevice device, VkShaderModule shaderModule, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyShaderModule(shaderModule);
		}

		public static void vkDestroyBuffer(VkDevice device, VkBuffer buffer, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyBuffer(buffer);
		}

		public static void vkDestroyDescriptorPool(VkDevice device, VkDescriptorPool descriptorPool, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyDescriptorPool(descriptorPool);
		}

		public static void vkDestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout descriptorSetLayout, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyDescriptorSetLayout(descriptorSetLayout);
		}

		public static void vkDestroySampler(VkDevice device, VkSampler sampler, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroySampler(sampler);
		}

		public static void vkDestroyImage(VkDevice device, VkImage image, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).DestroyImage(image);
		}

		public static void vkFreeMemory(VkDevice device, VkDeviceMemory memory, VkAllocationCallbacks pAllocator)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).FreeMemory(memory);
		}

		public static void vkDeviceWaitIdle(VkDevice device)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).WaitIdle();
		}

		public static void vkGetPhysicalDeviceQueueFamilyProperties(VkPhysicalDevice device, ref int queueFamilyCount, VkQueueFamilyProperties[] queueFamilies)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetPhysicalDevice(device).GetQueueFamilyProperties(ref queueFamilyCount, queueFamilies);
		}

		public static VkResult vkCreateDevice(VkPhysicalDevice physicalDevice, VkDeviceCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkDevice pDevice)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));

			return GetPhysicalDevice(physicalDevice).CreateDevice(pCreateInfo, out pDevice);
		}

		public static void vkGetDeviceQueue(VkDevice device, int queueFamilyIndex, int queueIndex, out VkQueue pQueue)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckRange(queueFamilyIndex, 0, int.MaxValue, nameof(queueFamilyIndex));
			VkPreconditions.CheckRange(queueIndex, 0, int.MaxValue, nameof(queueIndex));

			GetDevice(device).GetQueue(queueFamilyIndex, queueIndex, out pQueue);
		}

		public static VkResult vkGetPhysicalDeviceSurfaceSupportKHR(VkPhysicalDevice physicalDevice, int queueFamilyIndex, VkSurfaceKHR surface, out bool pSupported)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));
			VkPreconditions.CheckNull(surface, nameof(surface));
			VkPreconditions.CheckRange(queueFamilyIndex, 0, int.MaxValue, nameof(queueFamilyIndex));

			return GetPhysicalDevice(physicalDevice).GetSurfaceSupport(queueFamilyIndex, surface, out pSupported);
		}

		public static VkResult vkCreateSwapchainKHR(VkDevice device, VkSwapchainCreateInfoKHR pCreateInfo, VkAllocationCallbacks pAllocator, out VkSwapchainKHR pSwapchain)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateSwapchain(pCreateInfo, out pSwapchain);
		}

		public static VkResult vkGetSwapchainImagesKHR(VkDevice device, VkSwapchainKHR swapChain, ref int imageCount, VkImage[] swapChainImages)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(swapChain, nameof(swapChain));

			return GetDevice(device).GetSwapchainImages(swapChain, ref imageCount, swapChainImages);
		}

		public static VkResult vkGetPhysicalDeviceSurfaceCapabilitiesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceCapabilitiesKHR capabilities)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));
			VkPreconditions.CheckNull(surface, nameof(surface));

			return GetPhysicalDevice(physicalDevice).GetSurfaceCapabilities(surface, out capabilities);
		}

		public static VkResult vkGetPhysicalDeviceSurfaceFormatsKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref int formatCount, VkSurfaceFormatKHR[] formats)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));
			VkPreconditions.CheckNull(surface, nameof(surface));

			return GetPhysicalDevice(physicalDevice).GetSurfaceFormats(surface, ref formatCount, formats);
		}

		public static VkResult vkGetPhysicalDeviceSurfacePresentModesKHR(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, ref int presentModeCount, VkPresentModeKHR[] presentModes)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));
			VkPreconditions.CheckNull(surface, nameof(surface));

			return GetPhysicalDevice(physicalDevice).GetSurfacePresentModes(surface, ref presentModeCount, presentModes);
		}

		public static VkResult vkEnumerateDeviceExtensionProperties(VkPhysicalDevice physicalDevice, VkAllocationCallbacks pAllocator, ref int extensionCount, VkExtensionProperties[] availableExtensions)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));

			return GetPhysicalDevice(physicalDevice).EnumerateExtensionProperties(ref extensionCount, availableExtensions);
		}

		public static VkResult vkCreateImageView(VkDevice device, VkImageViewCreateInfo createInfo, VkAllocationCallbacks pAllocator, out VkImageView imageView)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateImageView(createInfo, out imageView);
		}

		public static VkResult vkCreateShaderModule(VkDevice device, VkShaderModuleCreateInfo createInfo, VkAllocationCallbacks pAllocator, out VkShaderModule shaderModule)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateShaderModule(createInfo, out shaderModule);
		}

		public static VkResult vkCreatePipelineLayout(VkDevice device, VkPipelineLayoutCreateInfo pipelineLayoutInfo, VkAllocationCallbacks pAllocator, out VkPipelineLayout pipelineLayout)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreatePipelineLayout(pipelineLayoutInfo, out pipelineLayout);
		}

		public static VkResult vkCreateRenderPass(VkDevice device, VkRenderPassCreateInfo renderPassInfo, VkAllocationCallbacks pAllocator, out VkRenderPass renderPass)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateRenderPass(renderPassInfo, out renderPass);
		}

		public static VkResult vkCreateGraphicsPipelines(VkDevice device, VkPipelineCache pipelineCache, int createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, VkAllocationCallbacks pAllocator, VkPipeline[] pPipelines)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(device, nameof(pCreateInfos));
			VkPreconditions.CheckNull(device, nameof(pPipelines));

			VkPreconditions.CheckRange(createInfoCount, 1, int.MaxValue, nameof(createInfoCount));
			VkPreconditions.CheckRange(pCreateInfos.Length < createInfoCount, nameof(pCreateInfos.Length));

			return GetDevice(device).CreateGraphicsPipelines(pipelineCache, createInfoCount, pCreateInfos, pPipelines);
		}

		public static VkResult vkCreateFramebuffer(VkDevice device, VkFramebufferCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkFramebuffer pFrameBuffer)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateFrameBuffer(pCreateInfo, out pFrameBuffer);
		}

		public static VkResult vkCreateCommandPool(VkDevice device, VkCommandPoolCreateInfo poolCreateInfo, VkAllocationCallbacks pAllocator, out VkCommandPool commandPool)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateCommandPool(poolCreateInfo, out commandPool);
		}

		public static VkResult vkAllocateCommandBuffers(VkDevice device, VkCommandBufferAllocateInfo allocInfo, VkCommandBuffer[] commandBuffers)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(commandBuffers, nameof(commandBuffers));

			return GetDevice(device).AllocateCommandBuffers(allocInfo, commandBuffers);
		}

		public static VkResult vkBeginCommandBuffer(VkCommandBuffer commandBuffer, VkCommandBufferBeginInfo pBeginInfo)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			return GetCommandBuffer(commandBuffer).Begin(pBeginInfo);
		}

		public static void vkCmdBeginRenderPass(VkCommandBuffer commandBuffer, VkRenderPassBeginInfo renderPassBeginInfo, VkSubpassContents contents)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			GetCommandBuffer(commandBuffer).CmdBeginRenderPass(renderPassBeginInfo, contents);
		}

		public static void vkCmdBindPipeline(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckNull(pipeline, nameof(pipeline));

			GetCommandBuffer(commandBuffer).CmdBindPipeline(pipelineBindPoint, pipeline);
		}

		public static void vkCmdBindVertexBuffers(VkCommandBuffer commandBuffer, int firstBinding, int bindingCount, VkBuffer[] pBuffers, int[] pOffsets)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			GetCommandBuffer(commandBuffer).CmdBindVertexBuffers(firstBinding, bindingCount, pBuffers, pOffsets);
		}

		public static void vkCmdBindIndexBuffer(VkCommandBuffer commandBuffer, VkBuffer buffer, int offset, VkIndexType indexType)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckNull(buffer, nameof(buffer));
			VkPreconditions.CheckRange(offset, 0, int.MaxValue, nameof(offset));

			GetCommandBuffer(commandBuffer).CmdBindIndexBuffer(buffer, offset, indexType);
		}

		public static void vkCmdBindDescriptorSets(VkCommandBuffer commandBuffer, VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, int firstSet, int descriptorSetCount, VkDescriptorSet[] pDescriptorSets, int dynamicOffsetCount, int[] pDynamicOffsets)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			GetCommandBuffer(commandBuffer).CmdBindDescriptorSets(pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, dynamicOffsetCount, pDynamicOffsets);
		}

		public static void vkCmdDraw(VkCommandBuffer commandBuffer, int vertexCount, int instanceCount, int firstVertex, int firstInstance)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckRange(vertexCount, 1, int.MaxValue, nameof(vertexCount));
			VkPreconditions.CheckRange(instanceCount, 1, int.MaxValue, nameof(instanceCount));
			VkPreconditions.CheckRange(firstVertex, 0, int.MaxValue, nameof(firstVertex));
			VkPreconditions.CheckRange(firstInstance, 0, int.MaxValue, nameof(firstInstance));

			GetCommandBuffer(commandBuffer).CmdDraw(vertexCount, instanceCount, firstVertex, firstInstance);
		}

		public static void vkCmdDrawIndexed(VkCommandBuffer commandBuffer, int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckRange(indexCount, 1, int.MaxValue, nameof(indexCount));
			VkPreconditions.CheckRange(instanceCount, 1, int.MaxValue, nameof(instanceCount));
			VkPreconditions.CheckRange(firstIndex, 0, int.MaxValue, nameof(firstIndex));
			VkPreconditions.CheckRange(firstInstance, 0, int.MaxValue, nameof(firstInstance));
			VkPreconditions.CheckRange(vertexOffset, 0, int.MaxValue, nameof(vertexOffset));

			GetCommandBuffer(commandBuffer).CmdDrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance);
		}

		public static void vkCmdEndRenderPass(VkCommandBuffer commandBuffer)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			GetCommandBuffer(commandBuffer).CmdEndRenderPass();
		}

		public static void vkCmdCopyBuffer(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkBuffer dstBuffer, int regionCount, VkBufferCopy[] pRegions)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckNull(srcBuffer, nameof(srcBuffer));
			VkPreconditions.CheckNull(dstBuffer, nameof(dstBuffer));
			VkPreconditions.CheckNull(pRegions, nameof(pRegions));
			VkPreconditions.CheckRange(regionCount, 1, pRegions.Length, nameof(regionCount));

			GetCommandBuffer(commandBuffer).CmdCopyBuffer(srcBuffer, dstBuffer, regionCount, pRegions);
		}

		public static void vkCmdCopyBufferToImage(VkCommandBuffer commandBuffer, VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, int regionCount, VkBufferImageCopy[] pRegions)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			VkPreconditions.CheckNull(srcBuffer, nameof(srcBuffer));
			VkPreconditions.CheckNull(dstImage, nameof(dstImage));
			VkPreconditions.CheckNull(pRegions, nameof(pRegions));
			VkPreconditions.CheckRange(regionCount, 1, pRegions.Length, nameof(regionCount));

			GetCommandBuffer(commandBuffer).CmdCopyBufferToImage(srcBuffer, dstImage, dstImageLayout, regionCount, pRegions);
		}

		public static void vkCmdPipelineBarrier(VkCommandBuffer commandBuffer, VkPipelineStageFlagBits srcStageMask, VkPipelineStageFlagBits dstStageMask, int dependencyFlags, int memoryBarrierCount, VkMemoryBarrier[] pMemoryBarriers, int bufferMemoryBarrierCount, VkBufferMemoryBarrier[] pBufferMemoryBarriers, int imageMemoryBarrierCount, VkImageMemoryBarrier[] pImageMemoryBarriers)
		{
			bool hasBarrier = false;
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));
			if (memoryBarrierCount != 0)
			{
				hasBarrier = true;
				VkPreconditions.CheckNull(pMemoryBarriers, nameof(pMemoryBarriers));
				VkPreconditions.CheckRange(memoryBarrierCount, 1, pMemoryBarriers.Length, nameof(memoryBarrierCount));
			}
			if (bufferMemoryBarrierCount != 0)
			{
				hasBarrier = true;
				VkPreconditions.CheckNull(pBufferMemoryBarriers, nameof(pBufferMemoryBarriers));
				VkPreconditions.CheckRange(bufferMemoryBarrierCount, 1, pBufferMemoryBarriers.Length, nameof(bufferMemoryBarrierCount));
			}
			if (imageMemoryBarrierCount != 0)
			{
				hasBarrier = true;
				VkPreconditions.CheckNull(pImageMemoryBarriers, nameof(pImageMemoryBarriers));
				VkPreconditions.CheckRange(imageMemoryBarrierCount, 1, pImageMemoryBarriers.Length, nameof(imageMemoryBarrierCount));
			}
			VkPreconditions.CheckOperation(!hasBarrier, ("At least one type of barrier must be informed on the command"));

			GetCommandBuffer(commandBuffer).CmdPipelineBarrier(srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount, pImageMemoryBarriers);
		}

		public static VkResult vkEndCommandBuffer(VkCommandBuffer commandBuffer)
		{
			VkPreconditions.CheckNull(commandBuffer, nameof(commandBuffer));

			return GetCommandBuffer(commandBuffer).End();
		}

		public static VkResult vkCreateSemaphore(VkDevice device, VkSemaphoreCreateInfo semaphoreCreateInfo, VkAllocationCallbacks pAllocator, out VkSemaphore semaphore)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateSemaphore(semaphoreCreateInfo, out semaphore);
		}

		public static VkResult vkAcquireNextImageKHR(VkDevice device, VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(swapchain, nameof(swapchain));
			VkPreconditions.CheckRange(timeout, 1, long.MaxValue, nameof(timeout));

			return GetDevice(device).AcquireNextImage(swapchain, timeout, semaphore, fence, out pImageIndex);
		}

		public static VkResult vkQueueSubmit(VkQueue queue, int submitCount, VkSubmitInfo[] pSubmits, VkFence fence)
		{
			VkPreconditions.CheckNull(queue, nameof(queue));
			VkPreconditions.CheckNull(pSubmits, nameof(pSubmits));

			VkPreconditions.CheckRange(submitCount, 1, int.MaxValue, nameof(submitCount));
			VkPreconditions.CheckRange(pSubmits.Length < submitCount, nameof(pSubmits.Length));

			return GetQueue(queue).Submit(submitCount, pSubmits, fence);
		}

		public static VkResult vkQueuePresentKHR(VkQueue queue, VkPresentInfoKHR pPresentInfo)
		{
			VkPreconditions.CheckNull(queue, nameof(queue));

			return GetQueue(queue).Present(pPresentInfo);
		}

		public static VkResult vkQueueWaitIdle(VkQueue queue)
		{
			VkPreconditions.CheckNull(queue, nameof(queue));

			return GetQueue(queue).WaitIdle();
		}

		public static void vkDebugReportMessageEXT(
			VkInstance instance,
			VkDebugReportFlagBitsEXT flags,
			VkDebugReportObjectTypeEXT objectType,
			object obj,
			int location,
			int messageCode,
			string pLayerPrefix,
			string pMessage)
		{
			VkPreconditions.CheckNull(instance, nameof(instance));
			VkPreconditions.CheckString(pMessage, nameof(pMessage));

			GetInstance(instance).DebugReportMessage(flags, objectType, obj, location, messageCode, pLayerPrefix, pMessage);
		}

		public static VkResult vkCreateBuffer(VkDevice device, VkBufferCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkBuffer pBuffer)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateBuffer(pCreateInfo, out pBuffer);
		}

		public static void vkGetBufferMemoryRequirements(VkDevice device, VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(buffer, nameof(buffer));

			GetDevice(device).GetBufferMemoryRequirements(buffer, out pMemoryRequirements);
		}

		public static VkResult vkCreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayoutCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkDescriptorSetLayout pSetLayout)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateDescriptorSetLayout(pCreateInfo, out pSetLayout);
		}

		public static VkResult vkAllocateMemory(VkDevice device, VkMemoryAllocateInfo pAllocateInfo, VkAllocationCallbacks pAllocator, out VkDeviceMemory pMemory)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).AllocateMemory(pAllocateInfo, out pMemory);
		}

		public static VkResult vkBindBufferMemory(VkDevice device, VkBuffer buffer, VkDeviceMemory memory, int memoryOffset)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(buffer, nameof(buffer));
			VkPreconditions.CheckNull(memory, nameof(memory));
			VkPreconditions.CheckRange(memoryOffset, 0, int.MaxValue, nameof(memoryOffset));

			return GetDevice(device).BindBufferMemory(buffer, memory, memoryOffset);
		}

		public static void vkGetPhysicalDeviceMemoryProperties(VkPhysicalDevice physicalDevice, out VkPhysicalDeviceMemoryProperties pMemoryProperties)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));

			GetPhysicalDevice(physicalDevice).GetMemoryProperties(out pMemoryProperties);
		}

		public static VkResult vkMapMemory(VkDevice device, VkDeviceMemory memory, int offset, int size, int memoryMapFlags, out byte[] ppData)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(memory, nameof(memory));

			return GetDevice(device).MapMemory(memory, offset, size, memoryMapFlags, out ppData);
		}

		public static void vkUnmapMemory(VkDevice device, VkDeviceMemory memory)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(memory, nameof(memory));

			GetDevice(device).UnmapMemory(memory);
		}

		public static VkResult vkCreateDescriptorPool(VkDevice device, VkDescriptorPoolCreateInfo pCreateinfo, VkAllocationCallbacks pAllocator, out VkDescriptorPool pDescriptorPool)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateDescriptorPool(pCreateinfo, out pDescriptorPool);
		}

		public static void vkUpdateDescriptorSets(VkDevice device, int descriptorWriteCount, VkWriteDescriptorSet[] pDescriptorWrites, int descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			GetDevice(device).UpdateDescriptorSets(descriptorWriteCount, pDescriptorWrites, descriptorCopyCount, pDescriptorCopies);
		}

		public static VkResult vkAllocateDescriptorSets(VkDevice device, VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).AllocateDescriptorSets(pAllocateInfo, pDescriptorSets);
		}

		public static VkResult vkCreateSampler(VkDevice device, VkSamplerCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkSampler pSampler)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateSampler(pCreateInfo, pAllocator, out pSampler);
		}

		public static VkResult vkCreateImage(VkDevice device, VkImageCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkImage pImage)
		{
			VkPreconditions.CheckNull(device, nameof(device));

			return GetDevice(device).CreateImage(pCreateInfo, out pImage);
		}

		public static void vkGetImageMemoryRequirements(VkDevice device, VkImage image, out VkMemoryRequirements pMemoryRequirements)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(image, nameof(image));

			GetDevice(device).GetImageMemoryRequirements(image, out pMemoryRequirements);
		}

		public static VkResult vkBindImageMemory(VkDevice device, VkImage image, VkDeviceMemory memory, int memoryOffset)
		{
			VkPreconditions.CheckNull(device, nameof(device));
			VkPreconditions.CheckNull(image, nameof(image));
			VkPreconditions.CheckNull(memory, nameof(memory));

			return GetDevice(device).BindImageMemory(image, memory, memoryOffset);
		}

		/// <summary>
		/// Lists physical device's format capabilities
		/// </summary>
		/// <param name="physicalDevice">Is the physical device from which to query the format
		/// properties.</param>
		/// <param name="format">Is the format whose properties are queried.</param>
		/// <param name="pFormatProperties">Is a pointer to a VkFormatProperties structure in which
		/// physical device properties for format are returned.</param>
		public static void vkGetPhysicalDeviceFormatProperties(VkPhysicalDevice physicalDevice, VkFormat format, out VkFormatProperties pFormatProperties)
		{
			VkPreconditions.CheckNull(physicalDevice, nameof(physicalDevice));

			GetPhysicalDevice(physicalDevice).GetFormatProperties(format, out pFormatProperties);
		}

		public static VkResult vkCreateWin32SurfaceKHR(VkInstance instance, VkWin32SurfaceCreateInfoKHR pCreateInfo, VkAllocationCallbacks pAllocator, out VkSurfaceKHR pSurface)
		{
			return GetInstance(instance).CreateWin32SurfaceKHR(pCreateInfo, out pSurface);
		}
	}
}
