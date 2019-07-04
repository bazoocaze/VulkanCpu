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
	/// <summary>Structure specifying the parameters of a newly created buffer object.</summary>
	public struct VkBufferCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>flags is a bitmask of VkBufferCreateFlagBits specifying additional parameters
		/// of the buffer.</summary>
		public VkBufferCreateFlagBits flags;

		/// <summary>Is the size in bytes of the buffer to be created.</summary>
		public int size;

		/// <summary>Is a bitmask of VkBufferUsageFlagBits specifying allowed usages of the
		/// buffer.</summary>
		public VkBufferUsageFlagBits usage;

		/// <summary>Is a VkSharingMode value specifying the sharing mode of the buffer when it will be
		/// accessed by multiple queue families.</summary>
		public VkSharingMode sharingMode;

		/// <summary>Is the number of entries in the pQueueFamilyIndices array.</summary>
		public int queueFamilyIndexCount;

		/// <summary>Is a list of queue families that will access this buffer (ignored if sharingMode
		/// is not VK_SHARING_MODE_CONCURRENT).</summary>
		public int[] pQueueFamilyIndices;
	}

	/// <summary>Bitmask specifying additional parameters of a buffer (SPARSE).</summary>
	[Flags]
	public enum VkBufferCreateFlagBits
	{
		/// <summary>Specifies that the buffer will be backed using sparse memory binding.</summary>
		VK_BUFFER_CREATE_SPARSE_BINDING_BIT = 0x00000001,

		/// <summary>Specifies that the buffer can be partially backed using sparse memory
		/// binding. Buffers created with this flag must also be created with the
		/// VK_BUFFER_CREATE_SPARSE_BINDING_BIT flag.</summary>
		VK_BUFFER_CREATE_SPARSE_RESIDENCY_BIT = 0x00000002,

		/// <summary>Specifies that the buffer will be backed using sparse memory binding with memory
		/// ranges that might also simultaneously be backing another buffer (or another portion
		/// of the same buffer). Buffers created with this flag must also be created with the
		/// VK_BUFFER_CREATE_SPARSE_BINDING_BIT flag.</summary>
		VK_BUFFER_CREATE_SPARSE_ALIASED_BIT = 0x00000004,
	}

	/// <summary>Bitmask specifying allowed usage of a buffer (TRANSFER, UNIFORM, STORAGE, INDEX,
	/// VERTEX).</summary>
	[Flags]
	public enum VkBufferUsageFlagBits
	{
		/// <summary>Specifies that the buffer can be used as the source of a transfer command
		/// (see the definition of VK_PIPELINE_STAGE_TRANSFER_BIT).</summary>
		VK_BUFFER_USAGE_TRANSFER_SRC_BIT = 0x00000001,

		/// <summary>Specifies that the buffer can be used as the destination of a transfer
		/// command.</summary>
		VK_BUFFER_USAGE_TRANSFER_DST_BIT = 0x00000002,

		/// <summary>specifies that the buffer can be used to create a VkBufferView suitable for
		/// occupying a VkDescriptorSet slot of type VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER.</summary>
		VK_BUFFER_USAGE_UNIFORM_TEXEL_BUFFER_BIT = 0x00000004,

		/// <summary>Specifies that the buffer can be used to create a VkBufferView suitable for
		/// occupying a VkDescriptorSet slot of type VK_DESCRIPTOR_TYPE_STORAGE_TEXEL_BUFFER.</summary>
		VK_BUFFER_USAGE_STORAGE_TEXEL_BUFFER_BIT = 0x00000008,

		/// <summary>Specifies that the buffer can be used in a VkDescriptorBufferInfo suitable for
		/// occupying a VkDescriptorSet slot either of type VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER or VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC.</summary>
		VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT = 0x00000010,

		/// <summary>Specifies that the buffer can be used in a VkDescriptorBufferInfo suitable
		/// for occupying a VkDescriptorSet slot either of type VK_DESCRIPTOR_TYPE_STORAGE_BUFFER or VK_DESCRIPTOR_TYPE_STORAGE_BUFFER_DYNAMIC.</summary>
		VK_BUFFER_USAGE_STORAGE_BUFFER_BIT = 0x00000020,

		/// <summary>Specifies that the buffer is suitable for passing as the buffer parameter to
		/// vkCmdBindIndexBuffer.</summary>
		VK_BUFFER_USAGE_INDEX_BUFFER_BIT = 0x00000040,

		/// <summary>specifies that the buffer is suitable for passing as an element of the
		/// pBuffers array to vkCmdBindVertexBuffers.</summary>
		VK_BUFFER_USAGE_VERTEX_BUFFER_BIT = 0x00000080,

		/// <summary>Specifies that the buffer is suitable for passing as the buffer parameter to
		/// vkCmdDrawIndirect, vkCmdDrawIndexedIndirect, or vkCmdDispatchIndirect.</summary>
		VK_BUFFER_USAGE_INDIRECT_BUFFER_BIT = 0x00000100,
	}

	/// <summary>Structure specifying memory requirements.</summary>
	public struct VkMemoryRequirements
	{
		/// <summary>size is the size, in bytes, of the memory allocation required for the
		/// resource.</summary>
		public int size;

		/// <summary>alignment is the alignment, in bytes, of the offset within the allocation
		/// required for the resource.</summary>
		public int alignment;

		/// <summary>memoryTypeBits is a bitmask and contains one bit set for every supported
		/// memory type for the resource. Bit i is set if and only if the memory type i in the
		/// VkPhysicalDeviceMemoryProperties structure for the physical device is supported for
		/// the resource.</summary>
		public int memoryTypeBits;
	}

	/// <summary>Structure containing parameters of a memory allocation</summary>
	public struct VkMemoryAllocateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>allocationSize is the size of the allocation in bytes</summary>
		public int allocationSize;

		/// <summary>memoryTypeIndex is an index identifying a memory type from the memoryTypes
		/// array of the VkPhysicalDeviceMemoryProperties structure.</summary>
		public int memoryTypeIndex;
	}

	/// <summary>Bitmask specifying properties for a memory type (DEVICE_LOCAL, HOST_VISIBLE, COHERENT, CACHED).</summary>
	[Flags]
	public enum VkMemoryPropertyFlagBits
	{
		/// <summary>bit indicates that memory allocated with this type is the most efficient for
		/// device access. This property will be set if and only if the memory type belongs to a
		/// heap with the VK_MEMORY_HEAP_DEVICE_LOCAL_BIT set.</summary>
		VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT = 0x00000001,

		/// <summary>bit indicates that memory allocated with this type can be mapped for host access
		/// using vkMapMemory.</summary>
		VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT = 0x00000002,

		/// <summary>bit indicates that the host cache management commands vkFlushMappedMemoryRanges
		/// and vkInvalidateMappedMemoryRanges are not needed to flush host writes to the device or
		/// make device writes visible to the host, respectively.</summary>
		VK_MEMORY_PROPERTY_HOST_COHERENT_BIT = 0x00000004,

		/// <summary>bit indicates that memory allocated with this type is cached on the host.
		/// Host memory accesses to uncached memory are slower than to cached memory, however
		/// uncached memory is always host coherent.</summary>
		VK_MEMORY_PROPERTY_HOST_CACHED_BIT = 0x00000008,

		/// <summary>bit indicates that the memory type only allows device access to the memory.
		/// Memory types must not have both VK_MEMORY_PROPERTY_LAZILY_ALLOCATED_BIT and
		/// VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT set. Additionally, the object’s backing memory
		/// may be provided by the implementation lazily as specified in Lazily Allocated
		/// Memory.</summary>
		VK_MEMORY_PROPERTY_LAZILY_ALLOCATED_BIT = 0x00000010,
	}

	/// <summary>Structure specifying physical device memory propertie.</summary>
	public struct VkPhysicalDeviceMemoryProperties
	{
		/// <summary>Is the number of valid elements in the memoryTypes array.</summary>
		public int memoryTypeCount;

		/// <summary>Is an array of VkMemoryType structures describing the memory types that can
		/// be used to access memory allocated from the heaps specified by memoryHeaps.</summary>
		public VkMemoryType[] memoryTypes;

		/// <summary>Is the number of valid elements in the memoryHeaps array.</summary>
		public int memoryHeapCount;

		/// <summary>Is an array of VkMemoryHeap structures describing the memory heaps from which
		/// memory can be allocated.</summary>
		public VkMemoryHeap[] memoryHeaps;
	}

	/// <summary>Structure specifying memory type.</summary>
	public struct VkMemoryType
	{
		/// <summary>propertyFlags is a bitmask of VkMemoryPropertyFlagBits of properties for this
		/// memory type.</summary>
		public VkMemoryPropertyFlagBits propertyFlags;

		/// <summary>heapIndex describes which memory heap this memory type corresponds to, and must be
		/// less than memoryHeapCount from the VkPhysicalDeviceMemoryProperties structure.</summary>
		public int heapIndex;
	}

	/// <summary>Structure specifying a memory heap</summary>
	public struct VkMemoryHeap
	{
		/// <summary>Is the total memory size in bytes in the heap.</summary>
		public int size;

		/// <summary>Is a bitmask of VkMemoryHeapFlagBits specifying attribute flags for the
		/// heap.</summary>
		public VkMemoryHeapFlagBits flags;
	}

	/// <summary>Bitmask specifying attribute flags for a heap (DEVICE_LOCAL).</summary>
	[Flags]
	public enum VkMemoryHeapFlagBits
	{
		/// <summary>Indicates that the heap corresponds to device local memory. Device local
		/// memory may have different performance characteristics than host local memory, and may
		/// support different memory property flags.</summary>
		VK_MEMORY_HEAP_DEVICE_LOCAL_BIT = 0x00000001,
	}

	/// <summary>Structure specifying a buffer copy operation.</summary>
	public struct VkBufferCopy
	{
		/// <summary>Is the starting offset in bytes from the start of srcBuffer.</summary>
		public int srcOffset;

		/// <summary>Is the starting offset in bytes from the start of dstBuffer.</summary>
		public int dstOffset;

		/// <summary>Is the number of bytes to copy.</summary>
		public int size;
	}

	/// <summary>Type of index buffer indices (UINT16, UINT32).</summary>
	public enum VkIndexType
	{
		VK_INDEX_TYPE_UINT16 = 0,
		VK_INDEX_TYPE_UINT32 = 1,
	}
}
