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
	/// <summary>Structure specifying parameters of a newly created pipeline input assembly state.
	/// <para>Each draw is made up of zero or more vertices and zero or more instances, which are
	/// processed by the device and result in the assembly of primitives. Primitives are assembled
	/// according to the pInputAssemblyState member of the VkGraphicsPipelineCreateInfo structure,
	/// which is of type VkPipelineInputAssemblyStateCreateInfo</para>
	/// </summary>
	public struct VkPipelineInputAssemblyStateCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>is a VkPrimitiveTopology defining the primitive topology, as described
		/// below.</summary>
		public VkPrimitiveTopology topology;

		/// <summary>
		/// Controls whether a special vertex index value is treated as restarting the assembly of
		/// primitives. This enable only applies to indexed draws (vkCmdDrawIndexed and
		/// vkCmdDrawIndexedIndirect), and the special index value is either 0xFFFFFFFF when the
		/// indexType parameter of vkCmdBindIndexBuffer is equal to VK_INDEX_TYPE_UINT32, or
		/// 0xFFFF when indexType is equal to VK_INDEX_TYPE_UINT16. Primitive restart is not
		/// allowed for “list” topologies.
		/// </summary>
		public VkBool32 primitiveRestartEnable;
	}

	/// <summary>Supported primitive topologies.</summary>
	public enum VkPrimitiveTopology
	{
		VK_PRIMITIVE_TOPOLOGY_POINT_LIST = 0,
		VK_PRIMITIVE_TOPOLOGY_LINE_LIST = 1,
		VK_PRIMITIVE_TOPOLOGY_LINE_STRIP = 2,
		VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST = 3,
		VK_PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP = 4,
		VK_PRIMITIVE_TOPOLOGY_TRIANGLE_FAN = 5,
		VK_PRIMITIVE_TOPOLOGY_LINE_LIST_WITH_ADJACENCY = 6,
		VK_PRIMITIVE_TOPOLOGY_LINE_STRIP_WITH_ADJACENCY = 7,
		VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST_WITH_ADJACENCY = 8,
		VK_PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP_WITH_ADJACENCY = 9,
		VK_PRIMITIVE_TOPOLOGY_PATCH_LIST = 10,
	}
}
