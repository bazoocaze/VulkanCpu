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

namespace VulkanCpu.VulkanApi
{
	/// <summary>Structure specifying the parameters of a newly created pipeline layout 
	/// object</summary>
	public struct VkPipelineLayoutCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is the number of descriptor sets included in the pipeline layout.</summary>
		public int setLayoutCount;

		/// <summary>Is a pointer to an array of VkDescriptorSetLayout objects.</summary>
		public VkDescriptorSetLayout[] pSetLayouts;

		/// <summary>Is the number of push constant ranges included in the pipeline layout.
		/// </summary>
		public int pushConstantRangeCount;

		/// <summary>Is a pointer to an array of VkPushConstantRange structures defining a set
		/// of push constant ranges for use in a single pipeline layout.
		/// In addition to descriptor set layouts, a pipeline layout also describes how many push
		/// constants can be accessed by each stage of the pipeline.</summary>
		public VkPushConstantRange[] pPushConstantRanges;
	}

	/// <summary>Structure specifying a push constant range.</summary>
	public struct VkPushConstantRange
	{
		/// <summary>Set of stage flags describing the shader stages that will access a range of
		/// push constants. If a particular stage is not included in the range, then accessing
		/// members of that range of push constants from the corresponding shader stage will
		/// result in undefined data being read.</summary>
		public VkShaderStageFlagBits stageFlags;

		/// <summary>offset and size are the start offset and size, respectively, consumed by the
		/// range. Both offset and size are in units of bytes and must be a multiple of 4. The
		/// layout of the push constant variables is specified in the shader.</summary>
		public int offset;

		/// <summary>offset and size are the start offset and size, respectively, consumed by the
		/// range. Both offset and size are in units of bytes and must be a multiple of 4. The
		/// layout of the push constant variables is specified in the shader.</summary>
		public int size;
	}
}
