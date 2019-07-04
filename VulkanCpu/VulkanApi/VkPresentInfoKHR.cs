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
	/// <summary>Structure specifying the parameters of the presentation.</summary>
	public struct VkPresentInfoKHR
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is the number of semaphores to wait for before issuing the present request.
		/// The number may be zero.</summary>
		public int waitSemaphoreCount;

		/// <summary>If not NULL, is an array of VkSemaphore objects with waitSemaphoreCount entries,
		/// and specifies the semaphores to wait for before issuing the present request.</summary>
		public VkSemaphore[] pWaitSemaphores;

		/// <summary>Is the number of swapchains being presented to by this command.</summary>
		public int swapchainCount;

		/// <summary>Is an array of VkSwapchainKHR objects with swapchainCount entries. A given
		/// swapchain must not appear in this list more than once.</summary>
		public VkSwapchainKHR[] pSwapchains;

		/// <summary>Is an array of indices into the array of each swapchain’s presentable images,
		/// with swapchainCount entries. Each entry in this array identifies the image to present
		/// on the corresponding entry in the pSwapchains array.</summary>
		public int[] pImageIndices;

		/// <summary>Is an array of VkResult typed elements with swapchainCount entries. Applications
		/// that do not need per-swapchain results can use NULL for pResults. If non-NULL, each entry
		/// in pResults will be set to the VkResult for presenting the swapchain corresponding to the
		/// same index in pSwapchains.</summary>
		public VkResult[] pResults;
	}

	/// <summary>Supported presentation modes.</summary>
	public enum VkPresentModeKHR
	{
		/// <summary>The presentation engine does not wait for a vertical blanking period to update
		/// the current image, meaning this mode may result in visible tearing. No internal queuing
		/// of presentation requests is needed, as the requests are applied immediately.</summary>
		VK_PRESENT_MODE_IMMEDIATE_KHR = 0,

		/// <summary>The presentation engine waits for the next vertical blanking period to update
		/// the current image. Tearing cannot be observed. An internal single-entry queue is used to
		/// hold pending presentation requests. If the queue is full when a new presentation request
		/// is received, the new request replaces the existing entry, and any images associated with
		/// the prior entry become available for re-use by the application. One request is removed
		/// from the queue and processed during each vertical blanking period in which the queue is
		/// non-empty.</summary>
		VK_PRESENT_MODE_MAILBOX_KHR = 1,

		/// <summary>The presentation engine waits for the next vertical blanking period to update
		/// the current image. Tearing cannot be observed. An internal queue is used to hold pending
		/// presentation requests. New requests are appended to the end of the queue, and one request
		/// is removed from the beginning of the queue and processed during each vertical blanking
		/// period in which the queue is non-empty. This is the only value of presentMode that is
		/// required to be supported.</summary>
		VK_PRESENT_MODE_FIFO_KHR = 2,

		/// <summary>The presentation engine generally waits for the next vertical blanking period
		/// to update the current image. If a vertical blanking period has already passed since the
		/// last update of the current image then the presentation engine does not wait for another
		/// vertical blanking period for the update, meaning this mode may result in visible tearing
		/// in this case. This mode is useful for reducing visual stutter with an application that
		/// will mostly present a new image before the next vertical blanking period, but may
		/// occasionally be late, and present a new image just after the next vertical blanking
		/// period. An internal queue is used to hold pending presentation requests. New requests
		/// are appended to the end of the queue, and one request is removed from the beginning of
		/// the queue and processed during or after each vertical blanking period in which the queue
		/// is non-empty.</summary>
		VK_PRESENT_MODE_FIFO_RELAXED_KHR = 3,

		/// <summary>Indicates that the presentation engine and application have concurrent access
		/// to a single image, which is referred to as a shared presentable image. The presentation
		/// engine is only required to update the current image after a new presentation request is
		/// received. Therefore the application must make a presentation request whenever an update
		/// is required. However, the presentation engine may update the current image at any point,
		/// meaning this mode may result in visible tearing.</summary>
		VK_PRESENT_MODE_SHARED_DEMAND_REFRESH_KHR = 1000111000,

		/// <summary>Indicates that the presentation engine and application have concurrent access
		/// to a single image, which is referred to as a shared presentable image. The presentation
		/// engine periodically updates the current image on its regular refresh cycle. The
		/// application is only required to make one initial presentation request, after which the
		/// presentation engine must update the current image without any need for further
		/// presentation requests. The application can indicate the image contents have been updated
		/// by making a presentation request, but this does not guarantee the timing of when it will
		/// be updated. This mode may result in visible tearing if rendering to the image is not
		/// timed correctly.</summary>
		VK_PRESENT_MODE_SHARED_CONTINUOUS_REFRESH_KHR = 1000111001,
	}
}
