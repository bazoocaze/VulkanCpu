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
	/// <summary>Structure specifying parameters of a newly created image view.</summary>
	public struct VkImageViewCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is a VkImage on which the view will be created.</summary>
		public VkImage image;

		/// <summary>Is an VkImageViewType value specifying the type of the image view.</summary>
		public VkImageViewType viewType;

		/// <summary>Is a VkFormat describing the format and type used to interpret data elements
		/// in the image.</summary>
		public VkFormat format;

		/// <summary>Is a VkComponentMapping specifies a remapping of color components (or of
		/// depth or stencil components after they have been converted into color components).
		/// </summary>
		public VkComponentMapping components;

		/// <summary>is a VkImageSubresourceRange selecting the set of mipmap levels and array
		/// layers to be accessible to the view.</summary>
		public VkImageSubresourceRange subresourceRange;
	}

	/// <summary>Image view types.
	/// <para>The exact image view type is partially implicit, based on the image’s type and
	/// sample count, as well as the view creation parameters as described in the image view
	/// compatibility table for vkCreateImageView. This table also shows which SPIR-V OpTypeImage
	/// Dim and Arrayed parameters correspond to each image view type.</para></summary>
	public enum VkImageViewType
	{
		VK_IMAGE_VIEW_TYPE_1D = 0,
		VK_IMAGE_VIEW_TYPE_2D = 1,
		VK_IMAGE_VIEW_TYPE_3D = 2,
		VK_IMAGE_VIEW_TYPE_CUBE = 3,
		VK_IMAGE_VIEW_TYPE_1D_ARRAY = 4,
		VK_IMAGE_VIEW_TYPE_2D_ARRAY = 5,
		VK_IMAGE_VIEW_TYPE_CUBE_ARRAY = 6,
	}

	/// <summary>Structure specifying a color component mapping.</summary>
	public struct VkComponentMapping
	{
		public VkComponentSwizzle r;
		public VkComponentSwizzle g;
		public VkComponentSwizzle b;
		public VkComponentSwizzle a;
	}

	/// <summary>Specify how a component is swizzled (IDENTITY, ZERO, ONE, R, G, B, A).</summary>
	public enum VkComponentSwizzle
	{
		VK_COMPONENT_SWIZZLE_IDENTITY = 0,
		VK_COMPONENT_SWIZZLE_ZERO = 1,
		VK_COMPONENT_SWIZZLE_ONE = 2,
		VK_COMPONENT_SWIZZLE_R = 3,
		VK_COMPONENT_SWIZZLE_G = 4,
		VK_COMPONENT_SWIZZLE_B = 5,
		VK_COMPONENT_SWIZZLE_A = 6,
	}

	/// <summary>Structure specifying a image subresource range.</summary>
	public struct VkImageSubresourceRange
	{
		/// <summary>Is a bitmask of VkImageAspectFlagBits specifying which aspect(s) of the
		/// image are included in the view.</summary>
		public VkImageAspectFlagBits aspectMask;

		/// <summary>Is the first mipmap level accessible to the view.</summary>
		public int baseMipLevel;

		/// <summary>Is the number of mipmap levels (starting from baseMipLevel) accessible to the
		/// view.</summary>
		public int levelCount;

		/// <summary>Is the first array layer accessible to the view.</summary>
		public int baseArrayLayer;

		/// <summary>Is the number of array layers (starting from baseArrayLayer) accessible to
		/// the view.</summary>
		public int layerCount;
	}

	/// <summary>Bitmask specifying which aspects of an image are included in a view.</summary>
	[Flags]
	public enum VkImageAspectFlagBits
	{
		VK_IMAGE_ASPECT_COLOR_BIT = 0x00000001,
		VK_IMAGE_ASPECT_DEPTH_BIT = 0x00000002,
		VK_IMAGE_ASPECT_STENCIL_BIT = 0x00000004,
		VK_IMAGE_ASPECT_METADATA_BIT = 0x00000008,
	}
}
