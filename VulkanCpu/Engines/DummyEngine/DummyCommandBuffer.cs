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

namespace VulkanCpu.Engines.DummyEngine
{
	internal class DummyCommandBuffer : VkCommandBuffer
	{
		private DummyDevice m_device;
		private VkCommandBufferAllocateInfo m_commandBufferAllocateInfo;

		public DummyCommandBuffer(DummyDevice device, VkCommandBufferAllocateInfo commandBufferAllocateInfo)
		{
			this.m_device = device;
			this.m_commandBufferAllocateInfo = commandBufferAllocateInfo;
		}

		internal VkResult Begin(VkCommandBufferBeginInfo pBeginInfo)
		{
			return VkResult.VK_SUCCESS;
		}

		internal void CmdBeginRenderPass(VkRenderPassBeginInfo renderPassBeginInfo, VkSubpassContents contents)
		{
		}

		internal void CmdBindPipeline(VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)
		{
		}

		internal void CmdDraw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
		{
		}

		internal void CmdDrawIndexed(int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance)
		{
		}

		internal void CmdEndRenderPass()
		{
		}

		internal void CmdBindVertexBuffers(int firstBinding, int bindingCount, VkBuffer[] pBuffers, int[] pOffsets)
		{
		}

		internal void CmdBindIndexBuffer(VkBuffer buffer, int offset, VkIndexType indexType)
		{
		}

		internal void CmdCopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, int regionCount, VkBufferCopy[] pRegions)
		{
		}

		internal void CmdCopyBufferToImage(VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, int regionCount, VkBufferImageCopy[] pRegions)
		{
		}

		internal void CmdPipelineBarrier(VkPipelineStageFlagBits srcStageMask, VkPipelineStageFlagBits dstStageMask, int dependencyFlags, int memoryBarrierCount, VkMemoryBarrier[] pMemoryBarriers, int bufferMemoryBarrierCount, VkBufferMemoryBarrier[] pBufferMemoryBarriers, int imageMemoryBarrierCount, VkImageMemoryBarrier[] pImageMemoryBarriers)
		{
		}

		internal void CmdBindDescriptorSets(VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, int firstSet, int descriptorSetCount, VkDescriptorSet[] pDescriptorSets, int dynamicOffsetCount, int[] pDynamicOffsets)
		{
		}

		internal VkResult End()
		{
			return VkResult.VK_SUCCESS;
		}

		internal void Execute()
		{
		}
	}
}
