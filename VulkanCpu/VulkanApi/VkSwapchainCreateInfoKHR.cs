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
	/// <summary>Structure specifying parameters of a newly created VkSwapchainKHR.</summary>
	public struct VkSwapchainCreateInfoKHR
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is the surface that the swapchain will present images to. </summary>
		public VkSurfaceKHR surface;

		/// <summary>The minimum number of presentable images that the application needs. The
		/// platform will either create the swapchain with at least that many images, or will
		/// fail to create the swapchain. </summary>
		public int minImageCount;

		/// <summary>Is a VkFormat that is valid for swapchains on the specified surface.
		/// </summary>
		public VkFormat imageFormat;

		/// <summary>Is a VkColorSpaceKHR that is valid for swapchains on the specified surface.
		/// </summary>
		public VkColorSpaceKHR imageColorSpace;

		/// <summary>The size (in pixels) of the swapchain. Behavior is platform-dependent when
		/// the image extent does not match the surface’s currentExtent as returned by
		/// vkGetPhysicalDeviceSurfaceCapabilitiesKHR.</summary>
		public VkExtent2D imageExtent;

		/// <summary>Is the number of views in a multiview/stereo surface. For non-stereoscopic-3D
		/// applications, this value is 1.</summary>
		public int imageArrayLayers;

		/// <summary>Is a bitfield of VkImageUsageFlagBits, indicating how the application will
		/// use the swapchain’s presentable images.</summary>
		public VkImageUsageFlags imageUsage;

		/// <summary>The sharing mode used for the images of the swapchain.</summary>
		public VkSharingMode imageSharingMode;

		/// <summary>The number of queue families having access to the images of the swapchain in
		/// case imageSharingMode is VK_SHARING_MODE_CONCURRENT.</summary>
		public int queueFamilyIndexCount;

		/// <summary>Array of queue family indices having access to the images of the swapchain in
		/// case imageSharingMode is VK_SHARING_MODE_CONCURRENT.</summary>
		public int[] pQueueFamilyIndices;

		/// <summary>Is a bitfield of VkSurfaceTransformFlagBitsKHR, describing the transform,
		/// relative to the presentation engine’s natural orientation, applied to the image
		/// content prior to presentation. If it does not match the currentTransform value
		/// returned by vkGetPhysicalDeviceSurfaceCapabilitiesKHR, the presentation engine will
		/// transform the image content as part of the presentation operation.</summary>
		public VkSurfaceTransformFlagBitsKHR preTransform;

		/// <summary>Is a bitfield of VkCompositeAlphaFlagBitsKHR, indicating the alpha
		/// compositing mode to use when this surface is composited together with other surfaces
		/// on certain window systems.</summary>
		public VkCompositeAlphaFlagBitsKHR compositeAlpha;

		/// <summary>Is the presentation mode the swapchain will use. A swapchain’s present mode
		/// determines how incoming present requests will be processed and queued internally.
		/// </summary>
		public VkPresentModeKHR presentMode;

		/// <summary>Indicates whether the Vulkan implementation is allowed to discard rendering
		/// operations that affect regions of the surface which aren’t visible.
		/// <para>If set to VK_TRUE, the presentable images associated with the swapchain may not
		/// own all of their pixels. Pixels in the presentable images that correspond to regions
		/// of the target surface obscured by another window on the desktop or subject to some
		/// other clipping mechanism will have undefined content when read back. Pixel shaders
		/// may not execute for these pixels, and thus any side affects they would have had will
		/// not occur.</para>
		/// <para>If set to VK_FALSE, presentable images associated with the swapchain will own
		/// all the pixels they contain.</para>
		/// Setting this value to VK_TRUE does not guarantee any clipping will occur, but allows
		/// more optimal presentation methods to be used on some platforms. 
		///	<para>Note: Applications should set this value to VK_TRUE if they do not expect to
		///	read back the content of presentable images before presenting them or after
		///	reacquiring them and if their pixel shaders do not have any side effects that require
		///	them to run for all pixels in the presentable image.</para></summary>
		public VkBool32 clipped;

		/// <summary>If non-NULL, specifies the swapchain that will be replaced by the new
		/// swapchain being created. Upon calling vkCreateSwapchainKHR with a non-NULL
		/// oldSwapchain, any images not acquired by the application may be freed by the
		/// implementation, which may occur even if creation of the new swapchain fails. The
		/// application must destroy the old swapchain to free all memory associated with the
		/// old swapchain. The application must wait for the completion of any outstanding
		/// rendering to images it currently has acquired at the time the swapchain is destroyed.
		/// The application can continue to present any images it acquired and has not yet
		/// presented using the old swapchain, as long as it has not entered a state that causes
		/// it to return VK_ERROR_OUT_OF_DATE_KHR. However, the application cannot acquire any
		/// more images from the old swapchain regardless of whether or not creation of the new
		/// swapchain succeeds.</summary>
		public VkSwapchainKHR oldSwapchain;
	}
}
