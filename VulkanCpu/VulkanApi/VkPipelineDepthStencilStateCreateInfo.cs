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

using System.Runtime.InteropServices;
using System.Text;

namespace VulkanCpu.VulkanApi
{
	/// <summary>Structure specifying parameters of a newly created pipeline depth stencil state.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VkPipelineDepthStencilStateCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public VkPipelineDepthStencilStateCreateFlags flags;

		/// <summary>Controls whether depth testing is enabled.</summary>
		public VkBool32 depthTestEnable;

		/// <summary>Controls whether depth writes are enabled when depthTestEnable is VK_TRUE.
		/// Depth writes are always disabled when depthTestEnable is VK_FALSE.</summary>
		public VkBool32 depthWriteEnable;

		/// <summary>Is the comparison operator used in the depth test.</summary>
		public VkCompareOp depthCompareOp;

		/// <summary>Controls whether depth bounds testing is enabled.</summary>
		public VkBool32 depthBoundsTestEnable;

		/// <summary>Controls whether stencil testing is enabled.</summary>
		public VkBool32 stencilTestEnable;

		/// <summary>Front and back control the parameters of the stencil test.</summary>
		public VkStencilOpState front;

		/// <summary>front and back control the parameters of the stencil test.</summary>
		public VkStencilOpState back;

		/// <summary>minDepthBounds and maxDepthBounds define the range of values used in the
		/// depth bounds test.</summary>
		public float minDepthBounds;

		/// <summary>minDepthBounds and maxDepthBounds define the range of values used in the
		/// depth bounds test.</summary>
		public float maxDepthBounds;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			if (depthTestEnable == VkBool32.VK_TRUE)
				sb.Append($" depthTest={depthCompareOp}");
			else
				sb.Append(" depthTestEnable=FALSE");

			sb.Append($" depthWriteEnable={depthWriteEnable}");

			return sb.ToString().Trim();
		}
	}

	/// <summary>*RESERVED*</summary>
	public enum VkPipelineDepthStencilStateCreateFlags { }

	/// <summary>Structure specifying stencil operation state</summary>
	public struct VkStencilOpState
	{
		/// <summary>Is a VkStencilOp value specifying the action performed on samples that fail
		/// the stencil test.</summary>
		public VkStencilOp failOp;

		/// <summary>Is a VkStencilOp value specifying the action performed on samples that pass
		/// both the depth and stencil tests.</summary>
		public VkStencilOp passOp;

		/// <summary>Is a VkStencilOp value specifying the action performed on samples that pass
		/// the stencil test and fail the depth test.</summary>
		public VkStencilOp depthFailOp;

		/// <summary>Is a VkCompareOp value specifying the comparison operator used in the stencil
		/// test.</summary>
		public VkCompareOp compareOp;

		/// <summary>Selects the bits of the unsigned integer stencil values participating in the
		/// stencil test.</summary>
		public int compareMask;

		/// <summary>Selects the bits of the unsigned integer stencil values updated by the
		/// stencil test in the stencil framebuffer attachment.</summary>
		public int writeMask;

		/// <summary>Reference is an integer reference value that is used in the unsigned stencil
		/// comparison.</summary>
		public int Reference;
	}

	/// <summary>Stencil comparison function.</summary>
	public enum VkStencilOp
	{
		/// <summary>Keeps the current value.</summary>
		VK_STENCIL_OP_KEEP = 0,

		/// <summary>Sets the value to 0.</summary>
		VK_STENCIL_OP_ZERO = 1,

		/// <summary>Sets the value to reference.</summary>
		VK_STENCIL_OP_REPLACE = 2,

		/// <summary>Increments the current value and clamps to the maximum representable unsigned
		/// value.</summary>
		VK_STENCIL_OP_INCREMENT_AND_CLAMP = 3,

		/// <summary>Decrements the current value and clamps to 0.</summary>
		VK_STENCIL_OP_DECREMENT_AND_CLAMP = 4,

		/// <summary>Bitwise-inverts the current value.</summary>
		VK_STENCIL_OP_INVERT = 5,

		/// <summary>Increments the current value and wraps to 0 when the maximum value would have
		/// been exceeded.</summary>
		VK_STENCIL_OP_INCREMENT_AND_WRAP = 6,

		/// <summary>Decrements the current value and wraps to the maximum possible value when the
		/// value would go below 0.</summary>
		VK_STENCIL_OP_DECREMENT_AND_WRAP = 7,
	}
}
