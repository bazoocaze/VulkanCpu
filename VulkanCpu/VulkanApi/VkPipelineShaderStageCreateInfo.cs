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
	/// <summary>Structure specifying parameters of a newly created pipeline shader stage.</summary>
	public struct VkPipelineShaderStageCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is a VkShaderStageFlagBits value specifying a single pipeline stage.
		/// </summary>
		public VkShaderStageFlagBits stage;

		/// <summary>Is a VkShaderModule object that contains the shader for this stage.</summary>
		public VkShaderModule module;

		/// <summary>Is a pointer to a null-terminated UTF-8 string specifying the entry point name
		/// of the shader for this stage.</summary>
		public string pName;

		/// <summary>Is a pointer to VkSpecializationInfo, as described in Specialization Constants,
		/// and can be NULL.</summary>
		public VkSpecializationInfo[] pSpecializationInfo;
	}

	/// <summary>Structure specifying specialization info.</summary>
	public struct VkSpecializationInfo
	{
		/// <summary>Is the number of entries in the pMapEntries array.</summary>
		public uint mapEntryCount;

		/// <summary>Is a pointer to an array of VkSpecializationMapEntry which maps constant
		/// IDs to offsets in pData.</summary>
		public VkSpecializationMapEntry[] pMapEntries;

		/// <summary>Is the byte size of the pData buffer.</summary>
		public uint dataSize;

		/// <summary>Contains the actual constant values to specialize with.</summary>
		public byte[] pData;
	}

	/// <summary>Structure specifying a specialization map entry</summary>
	public struct VkSpecializationMapEntry
	{
		/// <summary>Is the ID of the specialization constant in SPIR-V.</summary>
		public uint constantID;

		/// <summary>Is the byte offset of the specialization constant value within the supplied
		/// data buffer.</summary>
		public uint offset;

		/// <summary>Is the byte size of the specialization constant value within the supplied data
		/// buffer.</summary>
		public uint size;
	}

	/// <summary>Bitmask specifying a pipeline stage.</summary>
	[Flags]
	public enum VkShaderStageFlagBits
	{
		/// <summary>Specifies the vertex stage.</summary>
		VK_SHADER_STAGE_VERTEX_BIT = 0x00000001,

		/// <summary>Specifies the tessellation control stage.</summary>
		VK_SHADER_STAGE_TESSELLATION_CONTROL_BIT = 0x00000002,

		/// <summary>Specifies the tessellation evaluation stage.</summary>
		VK_SHADER_STAGE_TESSELLATION_EVALUATION_BIT = 0x00000004,

		/// <summary>Specifies the geometry stage.</summary>
		VK_SHADER_STAGE_GEOMETRY_BIT = 0x00000008,

		/// <summary>Specifies the fragment stage.</summary>
		VK_SHADER_STAGE_FRAGMENT_BIT = 0x00000010,

		/// <summary>Specifies the compute stage.</summary>
		VK_SHADER_STAGE_COMPUTE_BIT = 0x00000020,

		/// <summary>Is a combination of bits used as shorthand to specify all graphics stages
		/// defined above (excluding the compute stage).</summary>
		VK_SHADER_STAGE_ALL_GRAPHICS = 0x0000001F,

		/// <summary>Is a combination of bits used as shorthand to specify all shader stages
		/// supported by the device, including all additional stages which are introduced by
		/// extensions.</summary>
		VK_SHADER_STAGE_ALL = 0x7FFFFFFF,
	}
}
