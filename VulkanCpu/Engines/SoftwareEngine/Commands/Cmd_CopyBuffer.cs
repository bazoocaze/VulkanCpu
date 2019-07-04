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
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine.Commands
{
	public class Cmd_CopyBuffer : SoftwareBufferCommand
	{
		private SoftwareBuffer srcBuffer;
		private SoftwareBuffer dstBuffer;
		private int regionCount;
		private VkBufferCopy[] pRegions;

		public Cmd_CopyBuffer(SoftwareBuffer srcBuffer, SoftwareBuffer dstBuffer, int regionCount, VkBufferCopy[] pRegions)
		{
			this.srcBuffer = srcBuffer;
			this.dstBuffer = dstBuffer;
			this.regionCount = regionCount;
			this.pRegions = pRegions;
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
			SoftwareDeviceMemory srcMem = srcBuffer.m_deviceMemory;
			SoftwareDeviceMemory dstMem = dstBuffer.m_deviceMemory;
			for (int i = 0; i < regionCount; i++)
			{
				int srcOffset = srcBuffer.m_memoryOffset + pRegions[i].srcOffset;
				int dstOffset = dstBuffer.m_memoryOffset + pRegions[i].dstOffset;
				Buffer.BlockCopy(srcMem.m_bytes, srcOffset, dstMem.m_bytes, dstOffset, pRegions[i].size);
			}
		}
	}
}
