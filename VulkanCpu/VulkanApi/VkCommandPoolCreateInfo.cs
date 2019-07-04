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
	/// <summary>Structure specifying parameters of a newly created command pool.</summary>
	public struct VkCommandPoolCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkCommandPoolCreateFlagBits indicating usage behavior for
		/// the pool and command buffers allocated from it.</summary>
		public VkCommandPoolCreateFlagBits flags;

		/// <summary>Designates a queue family as described in section Queue Family Properties.
		/// All command buffers allocated from this command pool must be submitted on queues from
		/// the same queue family.</summary>
		public int queueFamilyIndex;
	}

	/// <summary>Bitmask specifying usage behavior for a command pool (TRANSIENT, RESET).</summary>
	public enum VkCommandPoolCreateFlagBits
	{
		/// <summary>Indicates that command buffers allocated from the pool will be short-lived,
		/// meaning that they will be reset or freed in a relatively short timeframe. This flag
		/// may be used by the implementation to control memory allocation behavior within the
		/// pool.</summary>
		VK_COMMAND_POOL_CREATE_TRANSIENT_BIT = 0x00000001,

		/// <summary>Allows any command buffer allocated from a pool to be individually reset to
		/// the initial state; either by calling vkResetCommandBuffer, or via the implicit reset
		/// when calling vkBeginCommandBuffer. If this flag is not set on a pool, then
		/// vkResetCommandBuffer must not be called for any command buffer allocated from that
		/// pool.</summary>
		VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT = 0x00000002,
	}
}
