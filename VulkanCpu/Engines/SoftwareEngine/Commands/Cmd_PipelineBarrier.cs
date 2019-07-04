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

namespace VulkanCpu.Engines.SoftwareEngine.Commands
{
	public class Cmd_PipelineBarrier : SoftwareBufferCommand
	{
		private int bufferMemoryBarrierCount;
		private int dependencyFlags;
		private VkPipelineStageFlagBits dstStageMask;
		private int imageMemoryBarrierCount;
		private int memoryBarrierCount;
		private VkBufferMemoryBarrier[] pBufferMemoryBarriers;
		private VkImageMemoryBarrier[] pImageMemoryBarriers;
		private VkMemoryBarrier[] pMemoryBarriers;
		private VkPipelineStageFlagBits srcStageMask;

		public Cmd_PipelineBarrier(VkPipelineStageFlagBits srcStageMask, VkPipelineStageFlagBits dstStageMask, int dependencyFlags, int memoryBarrierCount, VkMemoryBarrier[] pMemoryBarriers, int bufferMemoryBarrierCount, VkBufferMemoryBarrier[] pBufferMemoryBarriers, int imageMemoryBarrierCount, VkImageMemoryBarrier[] pImageMemoryBarriers)
		{
			this.srcStageMask = srcStageMask;
			this.dstStageMask = dstStageMask;
			this.dependencyFlags = dependencyFlags;
			this.memoryBarrierCount = memoryBarrierCount;
			this.pMemoryBarriers = pMemoryBarriers;
			this.bufferMemoryBarrierCount = bufferMemoryBarrierCount;
			this.pBufferMemoryBarriers = pBufferMemoryBarriers;
			this.imageMemoryBarrierCount = imageMemoryBarrierCount;
			this.pImageMemoryBarriers = pImageMemoryBarriers;
		}


		public override VkResult Parse(SoftwareExecutionContext context)
		{
			return VkResult.VK_SUCCESS;
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
		}

		public override void Execute(SoftwareExecutionContext context)
		{
			// TODO: Cmd_PipelineBarrier.Execute()
		}
	}
}
