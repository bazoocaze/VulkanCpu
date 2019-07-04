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
	public class Cmd_BindVertexBuffers : SoftwareBufferCommand
	{
		private int firstBinding;
		private int bindingCount;
		private SoftwareBuffer[] pBuffers;
		private int[] pOffsets;

		public Cmd_BindVertexBuffers(int firstBinding, int bindingCount, SoftwareBuffer[] pBuffers, int[] pOffsets)
		{
			this.firstBinding = firstBinding;
			this.bindingCount = bindingCount;
			this.pBuffers = pBuffers;
			this.pOffsets = pOffsets;
		}

		public override VkResult Parse(SoftwareExecutionContext context)
		{
			for (int i = 0; i < bindingCount; i++)
			{
				context.m_VertexBuffers[i + firstBinding] = pBuffers[i];
				context.m_VertexBuffersOffsets[i + firstBinding] = pOffsets[i];
			}

			return VkResult.VK_SUCCESS;
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
			for (int i = 0; i < bindingCount; i++)
			{
				context.m_VertexBuffers[i + firstBinding] = pBuffers[i];
				context.m_VertexBuffersOffsets[i + firstBinding] = pOffsets[i];
			}
		}

		public override void Execute(SoftwareExecutionContext context)
		{
		}
	}
}
