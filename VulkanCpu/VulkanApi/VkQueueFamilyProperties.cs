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
	/// <summary>Structure providing information about a queue family.</summary>
	public struct VkQueueFamilyProperties
	{
		/// <summary>is a bitmask of VkQueueFlagBits indicating capabilities of the queues in this
		/// queue family.</summary>
		public VkQueueFlagBits queueFlags;

		/// <summary>Is the unsigned integer count of queues in this queue family.</summary>
		public int queueCount;

		/// <summary>Is the unsigned integer count of meaningful bits in the timestamps written via
		/// vkCmdWriteTimestamp. The valid range for the count is 36..64 bits, or a value of 0,
		/// indicating no support for timestamps. Bits outside the valid range are guaranteed to be
		/// zeros.</summary>
		public uint timestampValidBits;

		/// <summary>Is the minimum granularity supported for image transfer operations on the queues
		/// in this queue family.</summary>
		public VkExtent3D minImageTransferGranularity;

		public static VkQueueFamilyProperties Create(int queueCount, VkQueueFlagBits queueFlags, VkExtent3D minImageTransferGranularity)
		{
			return new VkQueueFamilyProperties() { queueCount = queueCount, queueFlags = queueFlags, minImageTransferGranularity = minImageTransferGranularity };
		}

		public static VkQueueFamilyProperties Create(int queueCount, VkQueueFlagBits queueFlags)
		{
			return new VkQueueFamilyProperties() { queueCount = queueCount, queueFlags = queueFlags, minImageTransferGranularity = VkExtent3D.Create(1, 1, 1) };
		}

		public override string ToString()
		{
			return string.Format("count={0} flags={1}", queueCount, queueFlags);
		}
	}

	/// <summary>Bitmask specifying capabilities of queues in a queue family (GRAPHICS, COMPUTE,
	/// TRANSFER).
	/// <para>Note: All commands that are allowed on a queue that supports transfer operations are
	/// also allowed on a queue that supports either graphics or compute operations.Thus, if the
	/// capabilities of a queue family include VK_QUEUE_GRAPHICS_BIT or VK_QUEUE_COMPUTE_BIT, then
	/// reporting the VK_QUEUE_TRANSFER_BIT capability separately for that queue family is
	/// optional.</para>
	/// </summary>
	[Flags]
	public enum VkQueueFlagBits
	{
		/// <summary>Indicates that queues in this queue family support graphics operations.</summary>
		VK_QUEUE_GRAPHICS_BIT = 0x00000001,

		/// <summary>Indicates that queues in this queue family support compute operations.</summary>
		VK_QUEUE_COMPUTE_BIT = 0x00000002,

		/// <summary>Indicates that queues in this queue family support transfer operations.</summary>
		VK_QUEUE_TRANSFER_BIT = 0x00000004,

		/// <summary>Indicates that queues in this queue family support sparse memory management
		/// operations (see Sparse Resources). If any of the sparse resource features are enabled,
		/// then at least one queue family must support this bit.</summary>
		VK_QUEUE_SPARSE_BINDING_BIT = 0x00000008,
	}

	/// <summary>Structure specifying a queue submit operation.</summary>
	public struct VkSubmitInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is the number of semaphores upon which to wait before executing the command
		/// buffers for the batch.</summary>
		public int waitSemaphoreCount;

		/// <summary>Is a pointer to an array of semaphores upon which to wait before the command
		/// buffers for this batch begin execution. If semaphores to wait on are provided, they
		/// define a semaphore wait operation.</summary>
		public VkSemaphore[] pWaitSemaphores;

		/// <summary>Is a pointer to an array of pipeline stages at which each corresponding
		/// semaphore wait will occur.</summary>
		public VkPipelineStageFlagBits[] pWaitDstStageMask;

		/// <summary>Is the number of command buffers to execute in the batch.</summary>
		public int commandBufferCount;

		/// <summary>Is a pointer to an array of command buffers to execute in the batch.</summary>
		public VkCommandBuffer[] pCommandBuffers;

		/// <summary>Is the number of semaphores to be signaled once the commands specified in
		/// pCommandBuffers have completed execution.</summary>
		public int signalSemaphoreCount;

		/// <summary>Is a pointer to an array of semaphores which will be signaled when the command
		/// buffers for this batch have completed execution. If semaphores to be signaled are
		/// provided, they define a semaphore signal operation.</summary>
		public VkSemaphore[] pSignalSemaphores;
	}
}
