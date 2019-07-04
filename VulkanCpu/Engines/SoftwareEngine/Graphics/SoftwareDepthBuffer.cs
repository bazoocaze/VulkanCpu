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
using System.Runtime.CompilerServices;
using GlmSharp;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class SoftwareDepthBuffer : ISoftwareDepthBuffer
	{
		private delegate bool CompareOpDelegate(float value, ivec2 coord);

		private VkPipelineDepthStencilStateCreateInfo m_State;
		private CompareOpDelegate m_CompareOp;
		private SoftwareImageView m_depthBufferImageView;

		internal SoftwareDepthBuffer(VkPipelineDepthStencilStateCreateInfo state, SoftwareImageView depthBufferImageView)
		{
			m_State = state;
			if (m_State.depthTestEnable == VkBool32.VK_TRUE)
				m_CompareOp = GetCompareOp(state.depthCompareOp);
			else
				m_CompareOp = GetCompareOp(VkCompareOp.VK_COMPARE_OP_ALWAYS);
			m_depthBufferImageView = depthBufferImageView;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsDepthTestEnabled()
		{
			return (m_State.depthTestEnable == VkBool32.VK_TRUE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float ReadDepth(ivec2 coord)
		{
			return m_depthBufferImageView.ReadDepth(coord);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteDepth(ivec2 coord, float depth)
		{
			if (m_State.depthWriteEnable == VkBool32.VK_TRUE)
			{
				m_depthBufferImageView.WriteDepth(coord, depth);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReadStencil(ivec2 coord)
		{
			return m_depthBufferImageView.ReadStencil(coord);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteStencil(ivec2 coord, int stencil)
		{
			throw new NotImplementedException();
		}

		public bool TestDepth(ivec2 coord, float value)
		{
			return m_CompareOp(value, coord);
		}

		private CompareOpDelegate GetCompareOp(VkCompareOp compareOp)
		{
			switch (compareOp)
			{
				case VkCompareOp.VK_COMPARE_OP_ALWAYS: return (float v, ivec2 coord) => true;
				case VkCompareOp.VK_COMPARE_OP_NEVER: return (float v, ivec2 coord) => false;
			}

			switch (compareOp)
			{
				case VkCompareOp.VK_COMPARE_OP_EQUAL:
					return (float v, ivec2 coord) => (v == ReadDepth(coord));

				case VkCompareOp.VK_COMPARE_OP_NOT_EQUAL:
					return (float v, ivec2 coord) => (v != ReadDepth(coord));

				case VkCompareOp.VK_COMPARE_OP_GREATER:
					return (float v, ivec2 coord) => (v > ReadDepth(coord));

				case VkCompareOp.VK_COMPARE_OP_GREATER_OR_EQUAL:
					return (float v, ivec2 coord) => (v >= ReadDepth(coord));

				case VkCompareOp.VK_COMPARE_OP_LESS:
					return (float v, ivec2 coord) => (v < ReadDepth(coord));

				case VkCompareOp.VK_COMPARE_OP_LESS_OR_EQUAL:
					return (float v, ivec2 coord) => (v <= ReadDepth(coord));

				default: throw new NotImplementedException();
			}
		}

		internal static ISoftwareDepthBuffer Create(SoftwareExecutionContext context)
		{
			VkPipelineDepthStencilStateCreateInfo pDepthStencilState = context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pDepthStencilState;

			if (pDepthStencilState.sType != VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO)
			{
				return new DisabledSoftwareDepthBuffer();
			}

			var frameBufferObj = (SoftwareFramebuffer)context.m_RenderPassBeginInfo.framebuffer;
			var frameBuffer = frameBufferObj.m_createInfo;
			int attachmentIndex = -1;
			var subpass = context.m_CurrentSubpass;
			if (subpass.pDepthStencilAttachment == null)
			{
				context.CommandBufferCompilationError($"WARNING: depth buffer not configured for subpass {context.m_SubpassIndex}");
				return new DisabledSoftwareDepthBuffer();
			}

			attachmentIndex = subpass.pDepthStencilAttachment[0].attachment;

			if (attachmentIndex < 0 || attachmentIndex >= frameBuffer.attachmentCount)
			{
				context.CommandBufferCompilationError("ERROR: missing or invalid depth buffer attachment index");
				return new DisabledSoftwareDepthBuffer();
			}

			SoftwareImageView depthBufferImageView = (SoftwareImageView)frameBuffer.pAttachments[attachmentIndex];
			if (depthBufferImageView == null)
			{
				context.CommandBufferCompilationError($"ERROR: missing image view for depth buffer for attachment {attachmentIndex}");
				return new DisabledSoftwareDepthBuffer();
			}
			return new SoftwareDepthBuffer(pDepthStencilState, depthBufferImageView);
		}
	}

	public interface ISoftwareDepthBuffer
	{
		bool IsDepthTestEnabled();
		bool TestDepth(ivec2 coord, float value);

		float ReadDepth(ivec2 coord);
		void WriteDepth(ivec2 coord, float depth);

		int ReadStencil(ivec2 coord);
		void WriteStencil(ivec2 coord, int stencil);
	}

	public class DisabledSoftwareDepthBuffer : ISoftwareDepthBuffer
	{
		public bool IsDepthTestEnabled() { return false; }
		public bool TestDepth(ivec2 coord, float value) { return true; }

		public float ReadDepth(ivec2 coord) { return 0f; }
		public void WriteDepth(ivec2 coord, float depth) { }

		public int ReadStencil(ivec2 coord) { return 0; }
		public void WriteStencil(ivec2 coord, int stencil) { }
	}
}
