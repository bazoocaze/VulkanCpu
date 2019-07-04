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
using VulkanCpu.Engines.SoftwareEngine.Commands;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;
using VulkanCpu.VulkanApi.Internals;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareCommandBuffer : IVkCommandBuffer, VkCommandBuffer
	{
		public readonly SoftwareDevice m_device;
		public readonly VkCommandBufferAllocateInfo m_allocInfo;
		public readonly List<SoftwareBufferCommand> m_Commands;

		public CommandBufferState m_State;
		public VkCommandBufferBeginInfo m_CmdBeginInfo;
		public SoftwareExecutionContext m_context;

		private SoftwareCommandBuffer(SoftwareDevice device, VkCommandBufferAllocateInfo allocInfo)
		{
			this.m_device = device;
			this.m_allocInfo = allocInfo;
			this.m_Commands = new List<SoftwareBufferCommand>();
			this.m_State = CommandBufferState.Initial;
		}

		public static VkResult Create(SoftwareDevice device, VkCommandBufferAllocateInfo commandBufferAllocateInfo, out VkCommandBuffer commandBuffer)
		{
			commandBuffer = new SoftwareCommandBuffer(device, commandBufferAllocateInfo);
			return VkResult.VK_SUCCESS;
		}

		public VkResult Begin(VkCommandBufferBeginInfo pBeginInfo)
		{
			if (m_State == CommandBufferState.Recording)
			{
				return VkResult.VK_ERROR_VALIDATION_FAILED_EXT;
			}

			m_State = CommandBufferState.Recording;
			m_Commands.Clear();
			m_CmdBeginInfo = pBeginInfo;
			return VkResult.VK_SUCCESS;
		}

		public VkResult End()
		{
			if (m_State != CommandBufferState.Recording)
			{
				return VkResult.VK_ERROR_VALIDATION_FAILED_EXT;
			}

			m_Commands.Add(new Cmd_End());
			return CompileBuffer();
		}

		public void CmdBeginRenderPass(VkRenderPassBeginInfo renderPassBeginInfo, VkSubpassContents contents)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_BeginRenderPass(renderPassBeginInfo, contents));
		}



		public void CmdEndRenderPass()
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_EndRenderPass());
		}

		public void CmdBindPipeline(VkPipelineBindPoint pipelineBindPoint, VkPipeline pipeline)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_BindPipeline(pipelineBindPoint, pipeline));
		}

		public void CmdBindVertexBuffers(int firstBinding, int bindingCount, VkBuffer[] pBuffers, int[] pOffsets)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			SoftwareBuffer[] softwareBuffers = ArrayUtil.ConvertArray<VkBuffer, SoftwareBuffer>(pBuffers);
			m_Commands.Add(new Cmd_BindVertexBuffers(firstBinding, bindingCount, softwareBuffers, pOffsets));
		}

		public void CmdBindIndexBuffer(VkBuffer buffer, int offset, VkIndexType indexType)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_BindIndexBuffer((SoftwareBuffer)buffer, offset, indexType));
		}

		public void CmdBindDescriptorSets(VkPipelineBindPoint pipelineBindPoint, VkPipelineLayout layout, int firstSet, int descriptorSetCount, VkDescriptorSet[] pDescriptorSets, int dynamicOffsetCount, int[] pDynamicOffsets)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_BindDescriptorSets(pipelineBindPoint, layout, firstSet, descriptorSetCount, pDescriptorSets, dynamicOffsetCount, pDynamicOffsets));
		}

		public void CmdDraw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_Draw(vertexCount, instanceCount, firstVertex, firstInstance));
		}

		public void CmdDrawIndexed(int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_DrawIndexed(indexCount, instanceCount, firstIndex, vertexOffset, firstInstance));
		}

		public void CmdCopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, int regionCount, VkBufferCopy[] pRegions)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_CopyBuffer((SoftwareBuffer)srcBuffer, (SoftwareBuffer)dstBuffer, regionCount, pRegions));
		}

		public void CmdCopyBufferToImage(VkBuffer srcBuffer, VkImage dstImage, VkImageLayout dstImageLayout, int regionCount, VkBufferImageCopy[] pRegions)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_CopyBufferToImage((SoftwareBuffer)srcBuffer, (SoftwareImage)dstImage, dstImageLayout, regionCount, pRegions));
		}

		public void CmdPipelineBarrier(VkPipelineStageFlagBits srcStageMask, VkPipelineStageFlagBits dstStageMask, int dependencyFlags, int memoryBarrierCount, VkMemoryBarrier[] pMemoryBarriers, int bufferMemoryBarrierCount, VkBufferMemoryBarrier[] pBufferMemoryBarriers, int imageMemoryBarrierCount, VkImageMemoryBarrier[] pImageMemoryBarriers)
		{
			if (m_State != CommandBufferState.Recording)
				return;

			m_Commands.Add(new Cmd_PipelineBarrier(srcStageMask, dstStageMask, dependencyFlags, memoryBarrierCount, pMemoryBarriers, bufferMemoryBarrierCount, pBufferMemoryBarriers, imageMemoryBarrierCount, pImageMemoryBarriers));
		}

		private VkResult CompileBuffer()
		{
			VkResult result;

			var tmpContext = new SoftwareExecutionContext(m_device, this);
			foreach (var item in m_Commands)
			{
				result = item.Parse(tmpContext);
				if (result != VkResult.VK_SUCCESS)
				{
					DebugReportMessage(VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT, string.Format("Falha de compilação do CommandBuffer", m_State, CommandBufferState.Executable));
					m_State = CommandBufferState.Invalid;
					return result;
				}
			}

			m_context = new SoftwareExecutionContext(m_device, this);
			foreach (var item in m_Commands)
			{
				item.Prepare(m_context);
			}

			m_State = CommandBufferState.Executable;
			return VkResult.VK_SUCCESS;
		}

		public void Execute()
		{
			if (!(m_State == CommandBufferState.Executable || m_State == CommandBufferState.Pending))
			{
				DebugReportMessage(VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT, string.Format("Estado inválido para CommandBuffer: atual={0}, esperado={1}", m_State, CommandBufferState.Executable));
				return;
			}

			if (m_State == CommandBufferState.Pending && !m_CmdBeginInfo.flags.HasFlag(VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT))
			{
				DebugReportMessage(VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT, string.Format("CommandBuffer pendente mas não foi informado VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT"));
				return;
			}

			m_State = CommandBufferState.Pending;
			foreach (var item in m_Commands)
			{
				item.Execute(m_context);
			}

			if (m_CmdBeginInfo.flags.HasFlag(VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT))
			{
				m_State = CommandBufferState.Invalid;
			}
			else
			{
				m_State = CommandBufferState.Executable;
			}
		}

		public void DebugReportMessage(VkDebugReportFlagBitsEXT flags, string message)
		{
			m_device.DebugReportMessage(flags, VkDebugReportObjectTypeEXT.VK_DEBUG_REPORT_OBJECT_TYPE_COMMAND_BUFFER_EXT, this, 0, 0, "", message);
		}

		public VkResult CommandBufferCompilationError(string message)
		{
			m_device.DebugReportMessage(VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT, VkDebugReportObjectTypeEXT.VK_DEBUG_REPORT_OBJECT_TYPE_COMMAND_BUFFER_EXT, this, 0, 0, "", message);
			return VkResult.VK_ERROR_BUFFER_COMPILATION;
		}

		public void CmdNextSubpass(VkSubpassContents contents)
		{
			throw new NotImplementedException();
		}
	}

	public enum CommandBufferState
	{
		Initial,
		Recording,
		Executable,
		Invalid,
		Pending
	}
}
