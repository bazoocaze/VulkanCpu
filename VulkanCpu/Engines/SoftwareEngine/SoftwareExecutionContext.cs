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

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareExecutionContext
	{
		private const int P_MAX_VERTEX_BUFFERS = 32;
		private const int P_MAX_DESCRIPTOR_SETS = 64;

		public SoftwareDevice m_Device;
		public SoftwareCommandBuffer m_CommandBuffer;

		public RenderPassScopeEnum RenderPassScope;

		// COMMON
		public SoftwareDescriptorSet[] m_DescriptorSets = new SoftwareDescriptorSet[P_MAX_DESCRIPTOR_SETS];

		// COMPUTE
		public VkPipeline m_ComputePipeline;

		// GRAPHICS
		public SoftwareGraphicsPipeline m_GraphicsPipeline;
		public VkRenderPassBeginInfo m_RenderPassBeginInfo;

		public SoftwareBuffer[] m_VertexBuffers = new SoftwareBuffer[P_MAX_VERTEX_BUFFERS];
		public int[] m_VertexBuffersOffsets = new int[P_MAX_VERTEX_BUFFERS];

		public SoftwareBuffer m_IndexBuffer;
		public int m_IndexBufferOffset;
		public VkIndexType m_IndexBufferType;
		public int m_SubpassIndex;
		public VkRenderPassCreateInfo m_CurrentRenderPass;
		public VkSubpassDescription m_CurrentSubpass;

		public SoftwareExecutionContext(SoftwareDevice device, SoftwareCommandBuffer commandBuffer)
		{
			this.m_Device = device;
			this.m_CommandBuffer = commandBuffer;

			this.m_VertexBuffers = new SoftwareBuffer[P_MAX_VERTEX_BUFFERS];
			this.m_VertexBuffersOffsets = new int[P_MAX_VERTEX_BUFFERS];

			this.m_DescriptorSets = new SoftwareDescriptorSet[P_MAX_DESCRIPTOR_SETS];

			this.RenderPassScope = RenderPassScopeEnum.Outside;
		}

		internal VkResult CommandBufferCompilationError(string message)
		{
			m_Device.DebugReportMessage(VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT, VkDebugReportObjectTypeEXT.VK_DEBUG_REPORT_OBJECT_TYPE_COMMAND_BUFFER_EXT, m_CommandBuffer, 0, 0, "", message);
			return VkResult.VK_ERROR_BUFFER_COMPILATION;
		}
	}

	public enum RenderPassScopeEnum
	{
		None = 0,
		Inside = 1,
		Outside = 2,
	}
}
