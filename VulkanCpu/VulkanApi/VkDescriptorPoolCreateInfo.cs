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
	/// <summary>Structure specifying parameters of a newly created descriptor pool.</summary>
	public struct VkDescriptorPoolCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkDescriptorPoolCreateFlagBits specifying certain supported
		/// operations on the pool.</summary>
		public VkDescriptorPoolCreateFlagBits flags;

		/// <summary>Is the maximum number of descriptor sets that can be allocated from the pool.</summary>
		public int maxSets;

		/// <summary>Is the number of elements in pPoolSizes.</summary>
		public int poolSizeCount;

		/// <summary>Is a pointer to an array of VkDescriptorPoolSize structures, each containing a
		/// descriptor type and number of descriptors of that type to be allocated in the pool.</summary>
		public VkDescriptorPoolSize[] pPoolSizes;
	}

	/// <summary>Structure specifying descriptor pool size.</summary>
	public struct VkDescriptorPoolSize
	{
		/// <summary>Is the type of descriptor.</summary>
		public VkDescriptorType type;

		/// <summary>Is the number of descriptors of that type to allocate.</summary>
		public int descriptorCount;
	}

	/// <summary>Bitmask specifying certain supported operations on a descriptor
	/// pool (CREATE_FREE_DESCRIPTOR).</summary>
	public enum VkDescriptorPoolCreateFlagBits
	{
		/// <summary>Specifies that descriptor sets can return their individual allocations to
		/// the pool, i.e. all of vkAllocateDescriptorSets, vkFreeDescriptorSets, and
		/// vkResetDescriptorPool are allowed. Otherwise, descriptor sets allocated from the pool
		/// must not be individually freed back to the pool, i.e. only vkAllocateDescriptorSets
		/// and vkResetDescriptorPool are allowed.</summary>
		VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT = 0x00000001,
	}
}
