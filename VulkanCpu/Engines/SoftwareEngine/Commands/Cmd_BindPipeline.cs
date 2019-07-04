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
	public class Cmd_BindPipeline : SoftwareBufferCommand
	{
		private VkPipeline m_pipeline;
		private VkPipelineBindPoint m_pipelineBindPoint;

		public Cmd_BindPipeline(VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)
		{
			this.m_pipelineBindPoint = pipelineBindPoint;
			this.m_pipeline = pipeline;
		}

		public override VkResult Parse(SoftwareExecutionContext context)
		{
			if (m_pipelineBindPoint == VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS)
			{
				SoftwareGraphicsPipeline pipeline = m_pipeline as SoftwareGraphicsPipeline;

				if (pipeline == null)
				{
					return context.CommandBufferCompilationError("Informed VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS without a graphical pipeline");
				}

				context.m_GraphicsPipeline = pipeline;

				return VkResult.VK_SUCCESS;
			}
			else if (m_pipelineBindPoint == VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE)
			{
				context.m_ComputePipeline = m_pipeline;

				return VkResult.VK_SUCCESS;
			}
			throw new NotImplementedException();
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
			if (m_pipelineBindPoint == VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS)
			{
				context.m_GraphicsPipeline = (SoftwareGraphicsPipeline)m_pipeline;
			}
			else if (m_pipelineBindPoint == VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_COMPUTE)
			{
				context.m_ComputePipeline = m_pipeline;
			}
		}

		public override void Execute(SoftwareExecutionContext context)
		{
		}
	}
}
