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

using VulkanCpu.Engines.SoftwareEngine.Graphics;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine.Commands
{
	public class Cmd_Draw : SoftwareBufferCommand
	{
		private int firstInstance;
		private int firstVertex;
		private int instanceCount;
		private int vertexCount;

		private SoftwareRasterizer rasterizer;

		public Cmd_Draw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
		{
			this.vertexCount = vertexCount;
			this.instanceCount = instanceCount;
			this.firstVertex = firstVertex;
			this.firstInstance = firstInstance;
		}

		public override VkResult Parse(SoftwareExecutionContext context)
		{
			if (context.RenderPassScope != RenderPassScopeEnum.Inside)
			{
				return context.CommandBufferCompilationError("Found Draw command but missing BeginRenderPass");
			}

			if (context.m_GraphicsPipeline == null)
			{
				return context.CommandBufferCompilationError("Found Draw command but missing BindPipeline");
			}

			return VkResult.VK_SUCCESS;
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
			rasterizer = new SoftwareRasterizer();
			rasterizer.m_context = context;
			rasterizer.Prepare();
		}

		public override void Execute(SoftwareExecutionContext context)
		{
			rasterizer.Draw(vertexCount, instanceCount, firstVertex, firstInstance);
		}
	}
}
