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
	/// <summary>Structure specifying parameters of a newly created pipeline color blend state.
	/// </summary>
	public struct VkPipelineColorBlendStateCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Controls whether to apply Logical Operations.</summary>
		public VkBool32 logicOpEnable;

		/// <summary>Selects which logical operation to apply.</summary>
		public VkLogicOp logicOp;

		/// <summary>The number of VkPipelineColorBlendAttachmentState elements in pAttachments.
		/// This value must equal the colorAttachmentCount for the subpass in which this pipeline
		/// is used.</summary>
		public int attachmentCount;

		/// <summary>Pointer to array of per target attachment states.</summary>
		public VkPipelineColorBlendAttachmentState[] pAttachments;

		/// <summary>Array of four values used as the R, G, B, and A components of the blend
		/// constant that are used in blending, depending on the blend factor.</summary>
		public float[] blendConstants;

		public static VkPipelineColorBlendStateCreateInfo Create()
		{
			VkPipelineColorBlendStateCreateInfo ret = new VkPipelineColorBlendStateCreateInfo();
			ret.blendConstants = new float[4];
			return ret;
		}
	}

	/// <summary>Framebuffer logical operations.
	/// <para>Logical operations are controlled by the logicOpEnable and logicOp members of
	/// VkPipelineColorBlendStateCreateInfo. If logicOpEnable is VK_TRUE, then a logical operation
	/// selected by logicOp is applied between each color attachment and the fragment’s
	/// corresponding output value, and blending of all attachments is treated as if it were
	/// disabled. Any attachments using color formats for which logical operations are not
	/// supported simply pass through the color values unmodified. The logical operation is
	/// applied independently for each of the red, green, blue, and alpha components.</para>
	/// </summary>
	public enum VkLogicOp
	{
		/// <summary>result = 0</summary>
		VK_LOGIC_OP_CLEAR = 0,

		/// <summary>result = src & dst</summary>
		VK_LOGIC_OP_AND = 1,

		/// <summary>result = src & !dst</summary>
		VK_LOGIC_OP_AND_REVERSE = 2,

		/// <summary>result = src</summary>
		VK_LOGIC_OP_COPY = 3,

		/// <summary>result = !src & dst</summary>
		VK_LOGIC_OP_AND_INVERTED = 4,

		/// <summary>result = dst</summary>
		VK_LOGIC_OP_NO_OP = 5,

		/// <summary>result = src XOR dst</summary>
		VK_LOGIC_OP_XOR = 6,

		/// <summary>result = src | dst</summary>
		VK_LOGIC_OP_OR = 7,

		/// <summary>result = !(src | dst) </summary>
		VK_LOGIC_OP_NOR = 8,

		/// <summary>result = !(src XOR dst)</summary>
		VK_LOGIC_OP_EQUIVALENT = 9,

		/// <summary>result = !dst</summary>
		VK_LOGIC_OP_INVERT = 10,

		/// <summary>result = src | !dst</summary>
		VK_LOGIC_OP_OR_REVERSE = 11,

		/// <summary>result = !src</summary>
		VK_LOGIC_OP_COPY_INVERTED = 12,

		/// <summary>result = !src | dst</summary>
		VK_LOGIC_OP_OR_INVERTED = 13,

		/// <summary>result = !(src & dst)</summary>
		VK_LOGIC_OP_NAND = 14,

		/// <summary>result = all 1s</summary>
		VK_LOGIC_OP_SET = 15,
	}
}
