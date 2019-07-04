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
	/// <summary>Structure specifying parameters of a newly created graphics pipeline.</summary>
	public struct VkGraphicsPipelineCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkPipelineCreateFlagBits specifying how the pipeline will be
		/// generated.</summary>
		public VkPipelineCreateFlagBits flags;

		/// <summary>Is the number of entries in the pStages array.</summary>
		public int stageCount;

		/// <summary>Is an array of size stageCount structures of type
		/// VkPipelineShaderStageCreateInfo describing the set of the shader stages to be included
		/// in the graphics pipeline.</summary>
		public VkPipelineShaderStageCreateInfo[] pStages;

		/// <summary>Is a pointer to an instance of the VkPipelineVertexInputStateCreateInfo
		/// structure.</summary>
		public VkPipelineVertexInputStateCreateInfo pVertexInputState;

		/// <summary>Is a pointer to an instance of the VkPipelineInputAssemblyStateCreateInfo
		/// structure which determines input assembly behavior, as described in Drawing
		/// Commands.</summary>
		public VkPipelineInputAssemblyStateCreateInfo pInputAssemblyState;

		/// <summary>Is a pointer to an instance of the VkPipelineTessellationStateCreateInfo
		/// structure, and is ignored if the pipeline does not include a tessellation control
		/// shader stage and tessellation evaluation shader stage.</summary>
		public object pTessellationState;

		/// <summary>Is a pointer to an instance of the VkPipelineViewportStateCreateInfo
		/// structure, and is ignored if the pipeline has rasterization disabled.</summary>
		public VkPipelineViewportStateCreateInfo pViewportState;

		/// <summary>Is a pointer to an instance of the VkPipelineRasterizationStateCreateInfo
		/// structure.</summary>
		public VkPipelineRasterizationStateCreateInfo pRasterizationState;

		/// <summary>Is a pointer to an instance of the VkPipelineMultisampleStateCreateInfo,
		/// and is ignored if the pipeline has rasterization disabled.</summary>
		public VkPipelineMultisampleStateCreateInfo pMultisampleState;

		/// <summary>Is a pointer to an instance of the VkPipelineDepthStencilStateCreateInfo
		/// structure, and is ignored if the pipeline has rasterization disabled or if the subpass
		/// of the render pass the pipeline is created against does not use a depth/stencil
		/// attachment.</summary>
		public VkPipelineDepthStencilStateCreateInfo pDepthStencilState;

		/// <summary>Is a pointer to an instance of the VkPipelineColorBlendStateCreateInfo
		/// structure, and is ignored if the pipeline has rasterization disabled or if the
		/// subpass of the render pass the pipeline is created against does not use any color
		/// attachments.</summary>
		public VkPipelineColorBlendStateCreateInfo pColorBlendState;

		/// <summary>Is a pointer to VkPipelineDynamicStateCreateInfo and is used to indicate
		/// which properties of the pipeline state object are dynamic and can be changed
		/// independently of the pipeline state. This can be NULL, which means no state in the
		/// pipeline is considered dynamic.</summary>
		public object pDynamicState;

		/// <summary>Is the description of binding locations used by both the pipeline and
		/// descriptor sets used with the pipeline.</summary>
		public VkPipelineLayout layout;

		/// <summary>Is a handle to a render pass object describing the environment in which the
		/// pipeline will be used; the pipeline must only be used with an instance of any render
		/// pass compatible with the one provided. See Render Pass Compatibility for more
		/// information.</summary>
		public VkRenderPass renderPass;

		/// <summary>Is the index of the subpass in the render pass where this pipeline will be
		/// used.</summary>
		public int subpass;

		/// <summary>Is a pipeline to derive from.</summary>
		public VkPipeline basePipelineHandle;

		/// <summary>Is an index into the pCreateInfos parameter to use as a pipeline to derive from.</summary>
		public int basePipelineIndex;
	}

	/// <summary>Bitmask controlling how a pipeline is created.</summary>
	[Flags]
	public enum VkPipelineCreateFlagBits
	{
		/// <summary>Specifies that the created pipeline will not be optimized. Using this flag
		/// may reduce the time taken to create the pipeline.</summary>
		VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT = 0x00000001,

		/// <summary>Specifies that the pipeline to be created is allowed to be the parent of a
		/// pipeline that will be created in a subsequent call to vkCreateGraphicsPipelines or
		/// vkCreateComputePipelines.</summary>
		VK_PIPELINE_CREATE_ALLOW_DERIVATIVES_BIT = 0x00000002,

		/// <summary>Specifies that the pipeline to be created will be a child of a previously
		/// created parent pipeline.</summary>
		VK_PIPELINE_CREATE_DERIVATIVE_BIT = 0x00000004,
	}
}
