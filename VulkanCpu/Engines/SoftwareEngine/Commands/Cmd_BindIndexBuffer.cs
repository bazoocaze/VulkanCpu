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
	public class Cmd_BindIndexBuffer : SoftwareBufferCommand
	{
		private SoftwareBuffer buffer;
		private VkIndexType indexType;
		private int offset;

		public Cmd_BindIndexBuffer(SoftwareBuffer buffer, int offset, VkIndexType indexType)
		{
			this.buffer = buffer;
			this.offset = offset;
			this.indexType = indexType;
		}

		public override VkResult Parse(SoftwareExecutionContext context)
		{
			context.m_IndexBuffer = buffer;
			context.m_IndexBufferOffset = offset;
			context.m_IndexBufferType = indexType;

			return VkResult.VK_SUCCESS;
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
			context.m_IndexBuffer = buffer;
			context.m_IndexBufferOffset = offset;
			context.m_IndexBufferType = indexType;
		}

		public override void Execute(SoftwareExecutionContext context)
		{
		}
	}
}
