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

namespace VulkanCpu.VulkanApi
{
	/// <summary>Structure specifying the allocation parameters for command buffer object.</summary>
	public struct VkCommandBufferAllocateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is the command pool from which the command buffers are allocated.</summary>
		public VkCommandPool commandPool;

		/// <summary>Is an VkCommandBufferLevel value specifying the command buffer level.</summary>
		public VkCommandBufferLevel level;

		/// <summary>Is the number of command buffers to allocate from the pool.</summary>
		public int commandBufferCount;
	}

	/// <summary>Structure specifying a command buffer begin operation.</summary>
	public struct VkCommandBufferBeginInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkCommandBufferUsageFlagBits specifying usage behavior for
		/// the command buffer.</summary>
		public VkCommandBufferUsageFlagBits flags;

		/// <summary>Is a pointer to a VkCommandBufferInheritanceInfo structure, which is used if
		/// commandBuffer is a secondary command buffer. If this is a primary command buffer, then
		/// this value is ignored.</summary>
		public VkCommandBufferInheritanceInfo pInheritanceInfo;
	}

	/// <summary>Structure specifying command buffer inheritance info.</summary>
	public struct VkCommandBufferInheritanceInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a VkRenderPass object defining which render passes the VkCommandBuffer will be
		/// compatible with and can be executed within. If the VkCommandBuffer will not be executed
		/// within a render pass instance, renderPass is ignored.</summary>
		public VkRenderPass renderPass;

		/// <summary>Is the index of the subpass within the render pass instance that the
		/// VkCommandBuffer will be executed within. If the VkCommandBuffer will not be executed within
		/// a render pass instance, subpass is ignored.</summary>
		public int subpass;

		/// <summary>Optionally refers to the VkFramebuffer object that the VkCommandBuffer will be
		/// rendering to if it is executed within a render pass instance. It can be VK_NULL_HANDLE
		/// if the framebuffer is not known, or if the VkCommandBuffer will not be executed within
		/// a render pass instance.
		/// <para>
		/// Note: Specifying the exact framebuffer that the secondary command buffer will be
		/// executed with may result in better performance at command buffer execution time.</para>
		/// </summary>
		public VkFramebuffer framebuffer;

		/// <summary>Indicates whether the command buffer can be executed while an occlusion query
		/// is active in the primary command buffer. If this is VK_TRUE, then this command buffer
		/// can be executed whether the primary command buffer has an occlusion query active or
		/// not. If this is VK_FALSE, then the primary command buffer must not have an occlusion
		/// query active.</summary>
		public VkBool32 occlusionQueryEnable;

		/// <summary>Indicates the query flags that can be used by an active occlusion query in
		/// the primary command buffer when this secondary command buffer is executed. If this
		/// value includes the VK_QUERY_CONTROL_PRECISE_BIT bit, then the active query can return
		/// boolean results or actual sample counts. If this bit is not set, then the active query
		/// must not use the VK_QUERY_CONTROL_PRECISE_BIT bit.</summary>
		public VkQueryControlFlagBits queryFlags;

		/// <summary>is a bitmask of VkQueryPipelineStatisticFlagBits specifying the set of
		/// pipeline statistics that can be counted by an active query in the primary command
		/// buffer when this secondary command buffer is executed. If this value includes a given
		/// bit, then this command buffer can be executed whether the primary command buffer has a
		/// pipeline statistics query active that includes this bit or not. If this value excludes
		/// a given bit, then the active pipeline statistics query must not be from a query pool
		/// that counts that statistic.</summary>
		public VkQueryPipelineStatisticFlagBits pipelineStatistics;
	}

	/// <summary>
	/// Enumeration specifying a command buffer level (PRIMARY, SECONDARY).
	/// </summary>
	public enum VkCommandBufferLevel
	{
		VK_COMMAND_BUFFER_LEVEL_PRIMARY = 0,
		VK_COMMAND_BUFFER_LEVEL_SECONDARY = 1,
	}

	/// <summary>
	/// Bitmask specifying usage behavior for command buffer (ON_TIME_SUBMIT, RENDER_PASS_CONTINUE,
	/// SIMULTANEOUS_USE_BI).
	/// </summary>
	[Flags]
	public enum VkCommandBufferUsageFlagBits
	{
		/// <summary>Specifies that each recording of the command buffer will only be submitted
		/// once, and the command buffer will be reset and recorded again between each submission.
		/// </summary>
		VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT = 0x00000001,

		/// <summary>Specifies that a secondary command buffer is considered to be entirely inside a
		/// render pass. If this is a primary command buffer, then this bit is ignored.
		/// </summary>
		VK_COMMAND_BUFFER_USAGE_RENDER_PASS_CONTINUE_BIT = 0x00000002,

		/// <summary>Specifies that a command buffer can be resubmitted to a queue while it is in the
		/// pending state, and recorded into multiple primary command buffers.</summary>
		VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT = 0x00000004,
	}

	/// <summary>
	/// Specify how commands in the first subpass of a render pass are provided (INLINE or
	/// SECONDARY_COMMAND_BUFFERS).
	/// </summary>
	public enum VkSubpassContents
	{
		/// <summary>specifies that the contents of the subpass will be recorded inline in the
		/// primary command buffer, and secondary command buffers must not be executed within
		/// the subpass.</summary>
		VK_SUBPASS_CONTENTS_INLINE = 0,

		/// <summary>specifies that the contents are recorded in secondary command buffers that
		/// will be called from the primary command buffer, and vkCmdExecuteCommands is the only
		/// valid command on the command buffer until vkCmdNextSubpass or vkCmdEndRenderPass.
		/// </summary>
		VK_SUBPASS_CONTENTS_SECONDARY_COMMAND_BUFFERS = 1,
	}

	/// <summary>Bitmask specifying constraints on a query</summary>
	[Flags]
	public enum VkQueryControlFlagBits
	{
		/// <summary>Specifies the precision of occlusion queries.</summary>
		VK_QUERY_CONTROL_PRECISE_BIT = 0x00000001,
	}

	/// <summary>Bitmask specifying queried pipeline statistics.</summary>
	[Flags]
	public enum VkQueryPipelineStatisticFlagBits
	{
		VK_QUERY_PIPELINE_STATISTIC_INPUT_ASSEMBLY_VERTICES_BIT = 0x00000001,
		VK_QUERY_PIPELINE_STATISTIC_INPUT_ASSEMBLY_PRIMITIVES_BIT = 0x00000002,
		VK_QUERY_PIPELINE_STATISTIC_VERTEX_SHADER_INVOCATIONS_BIT = 0x00000004,
		VK_QUERY_PIPELINE_STATISTIC_GEOMETRY_SHADER_INVOCATIONS_BIT = 0x00000008,
		VK_QUERY_PIPELINE_STATISTIC_GEOMETRY_SHADER_PRIMITIVES_BIT = 0x00000010,
		VK_QUERY_PIPELINE_STATISTIC_CLIPPING_INVOCATIONS_BIT = 0x00000020,
		VK_QUERY_PIPELINE_STATISTIC_CLIPPING_PRIMITIVES_BIT = 0x00000040,
		VK_QUERY_PIPELINE_STATISTIC_FRAGMENT_SHADER_INVOCATIONS_BIT = 0x00000080,
		VK_QUERY_PIPELINE_STATISTIC_TESSELLATION_CONTROL_SHADER_PATCHES_BIT = 0x00000100,
		VK_QUERY_PIPELINE_STATISTIC_TESSELLATION_EVALUATION_SHADER_INVOCATIONS_BIT = 0x00000200,
		VK_QUERY_PIPELINE_STATISTIC_COMPUTE_SHADER_INVOCATIONS_BIT = 0x00000400,
	}
}
