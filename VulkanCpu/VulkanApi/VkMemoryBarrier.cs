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
	/// <summary>Structure specifying a global memory barrier.</summary>
	public struct VkMemoryBarrier
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a source access mask.</summary>
		public VkAccessFlagBits srcAccessMask;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a destination access mask.
		/// </summary>
		public VkAccessFlagBits dstAccessMask;
	}

	/// <summary>Structure specifying a buffer memory barrier.</summary>
	public struct VkBufferMemoryBarrier
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a source access mask.</summary>
		public VkAccessFlagBits srcAccessMask;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a destination access mask.
		/// </summary>
		public VkAccessFlagBits dstAccessMask;

		/// <summary>Is the source queue family for a queue family ownership transfer.</summary>
		public int srcQueueFamilyIndex;

		/// <summary>Is the destination queue family for a queue family ownership transfer.
		/// </summary>
		public int dstQueueFamilyIndex;

		/// <summary>Is a handle to the buffer whose backing memory is affected by the barrier.
		/// </summary>
		public VkBuffer buffer;

		/// <summary>Is an offset in bytes into the backing memory for buffer; this is relative
		/// to the base offset as bound to the buffer (see vkBindBufferMemory).</summary>
		public int offset;

		/// <summary>Is a size in bytes of the affected area of backing memory for buffer, or
		/// VK_WHOLE_SIZE to use the range from offset to the end of the buffer.</summary>
		public int size;
	}

	/// <summary>Structure specifying the parameters of an image memory barrier.</summary>
	public struct VkImageMemoryBarrier
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a source access mask.</summary>
		public VkAccessFlagBits srcAccessMask;

		/// <summary>Is a bitmask of VkAccessFlagBits specifying a destination access mask.
		/// </summary>
		public VkAccessFlagBits dstAccessMask;

		/// <summary>Is the old layout in an image layout transition.</summary>
		public VkImageLayout oldLayout;

		/// <summary>Is the new layout in an image layout transition.</summary>
		public VkImageLayout newLayout;

		/// <summary>Is the source queue family for a queue family ownership transfer.</summary>
		public int srcQueueFamilyIndex;

		/// <summary>Is the destination queue family for a queue family ownership transfer.
		/// </summary>
		public int dstQueueFamilyIndex;

		/// <summary>Is a handle to the image affected by this barrier.</summary>
		public VkImage image;

		/// <summary>Describes the image subresource range within image that is affected by
		/// this barrier.</summary>
		public VkImageSubresourceRange subresourceRange;
	}
}
