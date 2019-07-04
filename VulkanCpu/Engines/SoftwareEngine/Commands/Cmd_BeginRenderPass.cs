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

namespace VulkanCpu.Engines.SoftwareEngine.Commands
{
	public class Cmd_BeginRenderPass : SoftwareBufferCommand
	{
		private VkSubpassContents m_contents;
		private VkRenderPassBeginInfo m_renderPassBeginInfo;
		private List<Action> m_ExecuteAction;

		public Cmd_BeginRenderPass(VkRenderPassBeginInfo renderPassBeginInfo, VkSubpassContents contents)
		{
			this.m_renderPassBeginInfo = renderPassBeginInfo;
			this.m_contents = contents;
			this.m_ExecuteAction = new List<Action>();
		}

		public override VkResult Parse(SoftwareExecutionContext context)
		{
			if (context.RenderPassScope == RenderPassScopeEnum.Inside)
			{
				return context.CommandBufferCompilationError("Found BeginRenderPass without finalizing previous RenderPass");
			}

			context.RenderPassScope = RenderPassScopeEnum.Inside;
			context.m_RenderPassBeginInfo = m_renderPassBeginInfo;

			return VkResult.VK_SUCCESS;
		}

		public override void Prepare(SoftwareExecutionContext context)
		{
			context.RenderPassScope = RenderPassScopeEnum.Inside;
			context.m_RenderPassBeginInfo = m_renderPassBeginInfo;
			BeginRenderPass(context);
		}

		public override void Execute(SoftwareExecutionContext context)
		{
			// context.m_GraphicsContext = new SoftwareGraphicsContext(context.m_Device, context.m_CommandBuffer);
			// context.m_GraphicsContext.m_RenderPassBeginInfo = m_renderPassBeginInfo;
			// context.m_GraphicsContext.BeginRenderPass();
			foreach (var item in m_ExecuteAction)
				item.Invoke();
		}

		internal void BeginRenderPass(SoftwareExecutionContext context)
		{
			context.m_SubpassIndex = 0;
			context.m_CurrentRenderPass = ((SoftwareRenderPass)m_renderPassBeginInfo.renderPass).m_createInfo;
			context.m_CurrentSubpass = context.m_CurrentRenderPass.pSubpasses[context.m_SubpassIndex];

			BeginSubpass(context);
		}

		private void BeginSubpass(SoftwareExecutionContext context)
		{
			SoftwareFramebuffer frameBuffer = (SoftwareFramebuffer)m_renderPassBeginInfo.framebuffer;

			for (int i = 0; i < context.m_CurrentSubpass.colorAttachmentCount; i++)
			{
				var attachmentRef = context.m_CurrentSubpass.pColorAttachments[i];
				int attachmentIndex = attachmentRef.attachment;
				var attachmentDescription = context.m_CurrentRenderPass.pAttachments[attachmentIndex];

				if (attachmentDescription.loadOp == VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR)
				{
					SoftwareImageView imageView = frameBuffer.m_createInfo.pAttachments[attachmentIndex] as SoftwareImageView;
					var clearColor = m_renderPassBeginInfo.pClearValues[attachmentIndex];

					m_ExecuteAction.Add(() => imageView.ClearColor(clearColor));
				}
			}

			if (context.m_CurrentSubpass.pDepthStencilAttachment != null)
			{
				int attachmentIndex = context.m_CurrentSubpass.pDepthStencilAttachment[0].attachment;
				var attachmentDescription = context.m_CurrentRenderPass.pAttachments[attachmentIndex];

				if (attachmentDescription.loadOp == VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR)
				{
					SoftwareImageView depthBufferImageView = frameBuffer.m_createInfo.pAttachments[attachmentIndex] as SoftwareImageView;
					var clearColor = m_renderPassBeginInfo.pClearValues[attachmentIndex];

					m_ExecuteAction.Add(() => depthBufferImageView.ClearDepth(clearColor));
				}
			}
		}
	}
}
