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
	public struct VkSurfaceCapabilitiesKHR
	{
		/// <summary>Is the minimum number of images the specified device supports for a
		/// swapchain created for the surface, and will be at least one.</summary>
		public int minImageCount;

		/// <summary>Is the maximum number of images the specified device supports for a swapchain
		/// created for the surface, and will be either 0, or greater than or equal to
		/// minImageCount. A value of 0 means that there is no limit on the number of images,
		/// though there may be limits related to the total amount of memory used by presentable
		/// images.</summary>
		public int maxImageCount;

		/// <summary>Is the current width and height of the surface, or the special value
		/// (0xFFFFFFFF, 0xFFFFFFFF) indicating that the surface size will be determined by
		/// the extent of a swapchain targeting the surface.</summary>
		public VkExtent2D currentExtent;

		/// <summary>Contains the smallest valid swapchain extent for the surface on the specified
		/// device. The width and height of the extent will each be less than or equal to the
		/// corresponding width and height of currentExtent, unless currentExtent has the special
		/// value described above.</summary>
		public VkExtent2D minImageExtent;

		/// <summary>Contains the largest valid swapchain extent for the surface on the specified
		/// device. The width and height of the extent will each be greater than or equal to the
		/// corresponding width and height of minImageExtent. The width and height of the extent
		/// will each be greater than or equal to the corresponding width and height of
		/// currentExtent, unless currentExtent has the special value described above.</summary>
		public VkExtent2D maxImageExtent;

		/// <summary>Is the maximum number of layers presentable images can have for a swapchain
		/// created for this device and surface, and will be at least one.</summary>
		public int maxImageArrayLayers;

		/// <summary>Is a bitmask of VkSurfaceTransformFlagBitsKHR indicating the presentation
		/// transforms supported for the surface on the specified device. At least one bit will
		/// be set.</summary>
		public VkSurfaceTransformFlagBitsKHR supportedTransforms;

		/// <summary>Is VkSurfaceTransformFlagBitsKHR value indicating the surface’s current
		/// transform relative to the presentation engine’s natural orientation.</summary>
		public VkSurfaceTransformFlagBitsKHR currentTransform;

		/// <summary>Bitmask of VkCompositeAlphaFlagBitsKHR, representing the alpha compositing
		/// modes supported by the presentation engine for the surface on the specified device,
		/// and at least one bit will be set. Opaque composition can be achieved in any alpha
		/// compositing mode by either using an image format that has no alpha component, or by
		/// ensuring that all pixels in the presentable images have an alpha value of 1.0.
		/// </summary>
		public VkCompositeAlphaFlagBitsKHR supportedCompositeAlpha;

		/// <summary>Bitmask of VkImageUsageFlagBits representing the ways the application can
		/// use the presentable images of a swapchain created with VkPresentModeKHR set to
		/// VK_PRESENT_MODE_IMMEDIATE_KHR, VK_PRESENT_MODE_MAILBOX_KHR,
		/// VK_PRESENT_MODE_FIFO_KHR or VK_PRESENT_MODE_FIFO_RELAXED_KHR for the surface on the 
		/// specified device. VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT must be included in the set
		/// but implementations may support additional usages.</summary>
		public VkImageUsageFlags supportedUsageFlags;
	}

	[Flags]
	public enum VkSurfaceTransformFlagBitsKHR
	{
		/// <summary>The image content is presented without being transformed. </summary>
		VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR = 0x00000001,

		/// <summary>The image content is rotated 90 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_ROTATE_90_BIT_KHR = 0x00000002,

		/// <summary>The image content is rotated 180 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_ROTATE_180_BIT_KHR = 0x00000004,

		/// <summary>The image content is rotated 270 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_ROTATE_270_BIT_KHR = 0x00000008,

		/// <summary>The image content is mirrored horizontally. </summary>
		VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_BIT_KHR = 0x00000010,

		/// <summary>The image content is mirrored horizontally, then rotated 90 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_90_BIT_KHR = 0x00000020,

		/// <summary>The image content is mirrored horizontally, then rotated 180 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_180_BIT_KHR = 0x00000040,

		/// <summary>The image content is mirrored horizontally, then rotated 270 degrees clockwise. </summary>
		VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_270_BIT_KHR = 0x00000080,

		/// <summary>The presentation transform is not specified, and is instead determined by platform-specific considerations and mechanisms outside Vulkan. </summary>
		VK_SURFACE_TRANSFORM_INHERIT_BIT_KHR = 0x00000100,
	}

	[Flags]
	public enum VkCompositeAlphaFlagBitsKHR
	{
		/// <summary>The alpha channel, if it exists, of the images is ignored in the compositing process. Instead, the image is treated as if it has a constant alpha of 1.0.</summary>
		VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR = 0x00000001,

		/// <summary>The alpha channel, if it exists, of the images is respected in the compositing process. The non-alpha channels of the image are expected to already be multiplied by the alpha channel by the application.</summary>
		VK_COMPOSITE_ALPHA_PRE_MULTIPLIED_BIT_KHR = 0x00000002,

		/// <summary>The alpha channel, if it exists, of the images is respected in the compositing process. The non-alpha channels of the image are not expected to already be multiplied by the alpha channel by the application; instead, the compositor will multiply the non-alpha channels of the image by the alpha channel during compositing.</summary>
		VK_COMPOSITE_ALPHA_POST_MULTIPLIED_BIT_KHR = 0x00000004,

		/// <summary>The way in which the presentation engine treats the alpha channel in the images is unknown to the Vulkan API. Instead, the application is responsible for setting the composite alpha blending mode using native window system commands. If the application does not set the blending mode using native window system commands, then a platform-specific default will be used.</summary>
		VK_COMPOSITE_ALPHA_INHERIT_BIT_KHR = 0x00000008,
	}
}
