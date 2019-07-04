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
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareDevice : BaseSoftwareDevice
	{
		SoftwarePhysicalDevice m_softwarePhysicalDevice;
		VkDeviceCreateInfo m_createInfo;

		private List<SoftwareDeviceMemory> m_DeviceMemory = new List<SoftwareDeviceMemory>();

		private SoftwareDevice(SoftwarePhysicalDevice physicalDevice, VkDeviceCreateInfo createInfo)
			: base(physicalDevice.m_Instance)
		{
			this.m_softwarePhysicalDevice = physicalDevice;
			this.m_createInfo = createInfo;

			foreach (var item in createInfo.pQueueCreateInfos)
			{
				List<VkQueue> list = new List<VkQueue>();

				for (int i = 0; i < item.queueCount; i++)
					list.Add(new SoftwareQueue(this, item.queueFamilyIndex));

				m_Queue.Add(item.queueFamilyIndex, list);
			}
		}

		internal static VkResult Create(SoftwarePhysicalDevice softwarePhysicalDevice, VkDeviceCreateInfo createInfo, out VkDevice device)
		{
			device = new SoftwareDevice(softwarePhysicalDevice, createInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult AllocateCommandBuffer(VkCommandBufferAllocateInfo commandBufferAllocateInfo, out VkCommandBuffer commandBuffer)
		{
			return SoftwareCommandBuffer.Create(this, commandBufferAllocateInfo, out commandBuffer);
		}

		public override VkResult CreateGraphicsPipelines(VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo, out VkPipeline pipeline)
		{
			return SoftwareGraphicsPipeline.Create(this, graphicsPipelineCreateInfo, out pipeline);
		}

		public override VkResult CreateFrameBuffer(VkFramebufferCreateInfo frameBufferCreateInfo, out VkFramebuffer frameBuffer)
		{
			frameBuffer = new SoftwareFramebuffer(this, frameBufferCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateImageView(VkImageViewCreateInfo createInfo, out VkImageView imageView)
		{
			imageView = new SoftwareImageView(this, createInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateSemaphore(VkSemaphoreCreateInfo semaphoreCreateInfo, out VkSemaphore semaphore)
		{
			return SoftwareSemaphore.Create(this, semaphoreCreateInfo, out semaphore);
		}

		public override VkResult CreateSwapchain(VkSwapchainCreateInfoKHR createInfo, out VkSwapchainKHR swapChain)
		{
			return SoftwareSwapchain.Create(this, createInfo, out swapChain);
		}

		public override void GetBufferMemoryRequirements(VkBuffer buffer, out VkMemoryRequirements pMemoryRequirements)
		{
			pMemoryRequirements = new VkMemoryRequirements();
			pMemoryRequirements.memoryTypeBits = 1;
			pMemoryRequirements.size = ((SoftwareBuffer)buffer).createInfo.size;
			pMemoryRequirements.alignment = 4;
		}

		public override VkResult AllocateMemory(VkMemoryAllocateInfo pAllocateInfo, out VkDeviceMemory pMemory)
		{
			VkPreconditions.CheckRange(pAllocateInfo.allocationSize, 1, int.MaxValue, nameof(pAllocateInfo.allocationSize));
			VkPreconditions.CheckRange(pAllocateInfo.memoryTypeIndex != 0, nameof(pAllocateInfo.memoryTypeIndex));

			var ret = new SoftwareDeviceMemory(this, pAllocateInfo);
			m_DeviceMemory.Add(ret);
			pMemory = ret;
			return VkResult.VK_SUCCESS;
		}

		public override VkResult BindBufferMemory(VkBuffer buffer, VkDeviceMemory memory, int memoryOffset)
		{
			((SoftwareBuffer)buffer).BindMemory((SoftwareDeviceMemory)memory, memoryOffset);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult MapMemory(VkDeviceMemory memory, int offset, int size, int memoryMapFlags, out byte[] ppData)
		{
			SoftwareDeviceMemory mem = (SoftwareDeviceMemory)memory;
			return mem.MapMemory(offset, size, memoryMapFlags, out ppData);
		}

		public override void UnmapMemory(VkDeviceMemory memory)
		{
			SoftwareDeviceMemory mem = (SoftwareDeviceMemory)memory;
			mem.UnmapMemory();
		}

		public override VkResult BindImageMemory(VkImage image, VkDeviceMemory memory, int memoryOffset)
		{
			var softImage = (SoftwareImage)image;
			return softImage.BindMemory((SoftwareDeviceMemory)memory, memoryOffset);
		}

		public override VkResult CreateImage(VkImageCreateInfo pCreateInfo, out VkImage pImage)
		{
			VkResult result;
			pImage = SoftwareImage.CreateImage(this, pCreateInfo, out result);
			return result;
		}

		public override void GetImageMemoryRequirements(VkImage image, out VkMemoryRequirements pMemoryRequirements)
		{
			((SoftwareImage)image).GetImageMemoryRequirements(out pMemoryRequirements);
		}

		public override VkResult CreateSampler(VkSamplerCreateInfo pCreateInfo, VkAllocationCallbacks pAllocator, out VkSampler pSampler)
		{
			SoftwareSampler sampler = new SoftwareSampler(this, pCreateInfo);
			pSampler = sampler;
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateBuffer(VkBufferCreateInfo pCreateInfo, out VkBuffer pBuffer)
		{
			pBuffer = new SoftwareBuffer(this, pCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateCommandPool(VkCommandPoolCreateInfo commandPoolCreateInfo, out VkCommandPool commandPool)
		{
			commandPool = new SoftwareCommandPool(this, commandPoolCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override void DestroyPipelineLayout(VkPipelineLayout pipelineLayout)
		{
			((SoftwarePipelineLayout)pipelineLayout).Destroy();
		}

		public override void DestroyPipeline(VkPipeline pipeline)
		{
			if (pipeline != null)
			{
				((SoftwarePipeline)pipeline).Destroy();
			}
		}

		public override void DestroyFramebuffer(VkFramebuffer framebuffer)
		{
			if (framebuffer != null)
			{
				((SoftwareFramebuffer)framebuffer).Destroy();
			}
		}

		public override void DestroyCommandPool(VkCommandPool commandPool)
		{
			((SoftwareCommandPool)commandPool).Destroy();
		}

		public override void DestroyRenderPass(VkRenderPass renderPass)
		{
			((SoftwareRenderPass)renderPass).Destroy();
		}

		public override void FreeCommandBuffers(VkCommandPool commandPool, int commandBufferCount, VkCommandBuffer[] pCommandBuffers)
		{
		}

		public override void DestroySemaphore(VkSemaphore semaphore)
		{
			((SoftwareSemaphore)semaphore).Destroy();
		}

		public override void DestroyImageView(VkImageView imageView)
		{
			((SoftwareImageView)imageView).Destroy();
		}

		public override void DestroyShaderModule(VkShaderModule shaderModule)
		{
			if (shaderModule != null)
				((SoftwareShaderModule)shaderModule).Destroy();
		}

		public override void DestroyBuffer(VkBuffer buffer)
		{
			if (buffer != null)
				((SoftwareBuffer)buffer).Destroy();
		}

		public override void DestroyDescriptorPool(VkDescriptorPool descriptorPool)
		{
			((SoftwareDescriptorPool)descriptorPool).Destroy();
		}

		public override void DestroyDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout)
		{

		}

		public override void DestroySampler(VkSampler sampler)
		{
			((SoftwareSampler)sampler).Destroy();
		}

		public override void DestroyImage(VkImage image)
		{
			((SoftwareImage)image).Destroy();
		}

		public override void FreeMemory(VkDeviceMemory memory)
		{
			SoftwareDeviceMemory mem = (SoftwareDeviceMemory)memory;
			if (mem != null)
			{
				m_DeviceMemory.Remove(mem);
				mem.Destroy();
			}
		}

		public override void WaitIdle()
		{
		}

		public override VkResult GetSwapchainImages(VkSwapchainKHR swapchain, ref int imageCount, VkImage[] swapChainImages)
		{
			return ((SoftwareSwapchain)swapchain).GetSwapchainImages(ref imageCount, swapChainImages);
		}

		public override VkResult CreateShaderModule(VkShaderModuleCreateInfo createInfo, out VkShaderModule shaderModule)
		{
			shaderModule = new SoftwareShaderModule(this, createInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult AcquireNextImage(VkSwapchainKHR swapchain, long timeout, VkSemaphore semaphore, VkFence fence, out int pImageIndex)
		{
			return ((SoftwareSwapchain)swapchain).AcquireNextImage(timeout, (SoftwareSemaphore)semaphore, (SoftwareFence)fence, out pImageIndex);
		}

		public override void UpdateDescriptorSets(int descriptorWriteCount, VkWriteDescriptorSet[] pDescriptorWrites, int descriptorCopyCount, VkCopyDescriptorSet[] pDescriptorCopies)
		{
			for (int i = 0; i < descriptorWriteCount; i++)
			{
				var descriptorType = pDescriptorWrites[i].descriptorType;
				int descriptorCount = pDescriptorWrites[i].descriptorCount;
				var dstSet = (SoftwareDescriptorSet)pDescriptorWrites[i].dstSet;
				int binding = pDescriptorWrites[i].dstBinding;
				int dstArrayElement = pDescriptorWrites[i].dstArrayElement;

				if (descriptorType == VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER)
				{
					for (int iInfo = 0; iInfo < descriptorCount; iInfo++)
					{
						dstSet.m_BufferInfo[binding + iInfo] = pDescriptorWrites[i].pBufferInfo[dstArrayElement + iInfo];
					}
				}
				else if (descriptorType == VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER)
				{
					for (int iInfo = 0; iInfo < descriptorCount; iInfo++)
					{
						dstSet.m_ImageInfo[binding + iInfo] = pDescriptorWrites[i].pImageInfo[dstArrayElement + iInfo];
					}
				}
				else
				{
					throw new NotImplementedException();
				}
			}

			if (descriptorCopyCount > 0)
				throw new NotImplementedException();
		}

		public override VkResult AllocateCommandBuffers(VkCommandBufferAllocateInfo allocInfo, VkCommandBuffer[] commandBuffers)
		{
			VkResult result;
			for (int i = 0; i < allocInfo.commandBufferCount; i++)
			{
				VkCommandBuffer commandBuffer = null;
				result = AllocateCommandBuffer(allocInfo, out commandBuffer);
				if (result != VkResult.VK_SUCCESS)
					return result;
				commandBuffers[i] = commandBuffer;
			}
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateGraphicsPipelines(VkPipelineCache pipelineCache, int createInfoCount, VkGraphicsPipelineCreateInfo[] pCreateInfos, VkPipeline[] pPipelines)
		{
			VkResult result;
			for (int i = 0; i < createInfoCount; i++)
			{
				VkPipeline pipeline = null;
				result = CreateGraphicsPipelines(pCreateInfos[i], out pipeline);
				if (result != VkResult.VK_SUCCESS)
					return result;
				pPipelines[i] = pipeline;
			}
			return VkResult.VK_SUCCESS;
		}

		public override VkResult AllocateDescriptorSets(VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets)
		{
			var descriptorPool = (SoftwareDescriptorPool)pAllocateInfo.descriptorPool;
			return descriptorPool.AllocateDescriptorSets(pAllocateInfo, pDescriptorSets);
		}

		public override VkResult CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo pCreateInfo, out VkDescriptorSetLayout pSetLayout)
		{
			pSetLayout = new SoftwareDescriptorSetLayout(this, pCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateDescriptorPool(VkDescriptorPoolCreateInfo pCreateinfo, out VkDescriptorPool pDescriptorPool)
		{
			pDescriptorPool = new SoftwareDescriptorPool(this, pCreateinfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreatePipelineLayout(VkPipelineLayoutCreateInfo pipelineLayoutInfo, out VkPipelineLayout pipelineLayout)
		{
			pipelineLayout = new SoftwarePipelineLayout(this, pipelineLayoutInfo);
			return VkResult.VK_SUCCESS;
		}

		public override VkResult CreateRenderPass(VkRenderPassCreateInfo renderPassInfo, out VkRenderPass renderPass)
		{
			renderPass = new SoftwareRenderPass(this, renderPassInfo);
			return VkResult.VK_SUCCESS;
		}

		public override void DestroySwapchainKHR(VkSwapchainKHR swapChain)
		{
			((SoftwareSwapchain)swapChain).Destroy();
		}
	}
}
