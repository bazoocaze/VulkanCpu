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
	/// <summary>Structure specifying the parameters of a newly created image object</summary>
	public struct VkImageCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is a bitmask of VkImageCreateFlagBits describing additional parameters of the
		/// image.</summary>
		public VkImageCreateFlagBits flags;

		/// <summary>Is a VkImageType value specifying the basic dimensionality of the image.
		/// Layers in array textures do not count as a dimension for the purposes of the image
		/// type.</summary>
		public VkImageType imageType;

		/// <summary>Is a VkFormat describing the format and type of the data elements that will
		/// be contained in the image.</summary>
		public VkFormat format;

		/// <summary>Is a VkExtent3D describing the number of data elements in each dimension of
		/// the base level.</summary>
		public VkExtent3D extent;

		/// <summary>Describes the number of levels of detail available for minified sampling of
		/// the image.</summary>
		public int mipLevels;

		/// <summary>Is the number of layers in the image.</summary>
		public int arrayLayers;

		/// <summary>Is the number of sub-data element samples in the image as defined in
		/// VkSampleCountFlagBits. See Multisampling.</summary>
		public VkSampleCountFlagBits samples;

		/// <summary>Is a VkImageTiling value specifying the tiling arrangement of the data
		/// elements in memory.</summary>
		public VkImageTiling tiling;

		/// <summary>Is a bitmask of VkImageUsageFlagBits describing the intended usage of the
		/// image.</summary>
		public VkImageUsageFlags usage;

		/// <summary>Is a VkSharingMode value specifying the sharing mode of the image when it
		/// will be accessed by multiple queue families.</summary>
		public VkSharingMode sharingMode;

		/// <summary>Is the number of entries in the pQueueFamilyIndices array.</summary>
		public int queueFamilyIndexCount;

		/// <summary>Is a list of queue families that will access this image (ignored if
		/// sharingMode is not VK_SHARING_MODE_CONCURRENT).</summary>
		public int[] pQueueFamilyIndices;

		/// <summary>Is a VkImageLayout value specifying the initial VkImageLayout of all image
		/// subresources of the image. See Image Layouts.</summary>
		public VkImageLayout initialLayout;
	}

	/// <summary>Bitmask specifying additional parameters of an image (Sparse, mutable,
	/// aliased)</summary>
	[Flags]
	public enum VkImageCreateFlagBits
	{
		VK_IMAGE_CREATE_SPARSE_BINDING_BIT = 0x00000001,
		VK_IMAGE_CREATE_SPARSE_RESIDENCY_BIT = 0x00000002,
		VK_IMAGE_CREATE_SPARSE_ALIASED_BIT = 0x00000004,
		VK_IMAGE_CREATE_MUTABLE_FORMAT_BIT = 0x00000008,
		VK_IMAGE_CREATE_CUBE_COMPATIBLE_BIT = 0x00000010,
	}

	/// <summary>Specifies the type of an image object (1D, 2D, 3D)</summary>
	public enum VkImageType
	{
		VK_IMAGE_TYPE_1D = 0,
		VK_IMAGE_TYPE_2D = 1,
		VK_IMAGE_TYPE_3D = 2,
	}

	/// <summary>Image tiling (optimal, linear)</summary>
	public enum VkImageTiling
	{
		/// <summary>specifies optimal tiling (texels are laid out in an implementation-dependent arrangement, for more optimal memory access).</summary>
		VK_IMAGE_TILING_OPTIMAL = 0,

		/// <summary>specifies linear tiling (texels are laid out in memory in row-major order, possibly with some padding on each row).</summary>
		VK_IMAGE_TILING_LINEAR = 1,
	}

	/// <summary>Structure specifying a image subresource layers</summary>
	public struct VkImageSubresourceLayers
	{
		/// <summary>Is a combination of VkImageAspectFlagBits, selecting the color, depth and/or
		/// stencil aspects to be copied.</summary>
		public VkImageAspectFlagBits aspectMask;

		/// <summary>Is the mipmap level to copy from.</summary>
		public int mipLevel;

		/// <summary>baseArrayLayer and layerCount are the starting layer and number of layers to
		/// copy.</summary>
		public int baseArrayLayer;

		/// <summary>baseArrayLayer and layerCount are the starting layer and number of layers to
		/// copy.</summary>
		public int layerCount;

		public override string ToString()
		{
			return string.Format("aspectMask={0} mipLevel={1} baseArrayLayer={2} layerCount={3}", aspectMask, mipLevel, baseArrayLayer, layerCount);
		}
	}

	/// <summary>Image usage (transfer, sampled, color, depth, stencil)</summary>
	[Flags]
	public enum VkImageUsageFlags
	{
		/// <summary>indicates that the image can be used as the source of a transfer command.
		/// </summary>
		VK_IMAGE_USAGE_TRANSFER_SRC_BIT = 0x00000001,

		/// <summary>indicates that the image can be used as the destination of a transfer command.
		/// </summary>
		VK_IMAGE_USAGE_TRANSFER_DST_BIT = 0x00000002,

		/// <summary>indicates that the image can be used to create a VkImageView suitable for
		/// occupying a VkDescriptorSet slot either of type VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE or
		/// VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, and be sampled by a shader. </summary>
		VK_IMAGE_USAGE_SAMPLED_BIT = 0x00000004,

		/// <summary>indicates that the image can be used to create a VkImageView suitable for
		/// occupying a VkDescriptorSet slot of type VK_DESCRIPTOR_TYPE_STORAGE_IMAGE. </summary>
		VK_IMAGE_USAGE_STORAGE_BIT = 0x00000008,

		/// <summary>indicates that the image can be used to create a VkImageView suitable for
		/// use as a color or resolve attachment in a VkFramebuffer. </summary>
		VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT = 0x00000010,

		/// <summary>indicates that the image can be used to create a VkImageView suitable for
		/// use as a depth/stencil attachment in a VkFramebuffer. </summary>
		VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT = 0x00000020,

		/// <summary>indicates that the memory bound to this image will have been allocated with
		/// the VK_MEMORY_PROPERTY_LAZILY_ALLOCATED_BIT (see Chapter 10, Memory Allocation for
		/// more detail). This bit can be set for any image that can be used to create a
		/// VkImageView suitable for use as a color, resolve, depth/stencil, or input attachment.
		/// </summary>
		VK_IMAGE_USAGE_TRANSIENT_ATTACHMENT_BIT = 0x00000040,

		/// <summary>indicates that the image can be used to create a VkImageView suitable for
		/// occupying VkDescriptorSet slot of type VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT; be read
		/// from a shader as an input attachment; and be used as an input attachment in a
		/// framebuffer. </summary>
		VK_IMAGE_USAGE_INPUT_ATTACHMENT_BIT = 0x00000080,
	}

	/// <summary>Buffer and image sharing modes (EXCLUSIVE, CONCURRENT).</summary>
	public enum VkSharingMode
	{
		/// <summary>Specifies that access to any range or image subresource of the object will be
		/// exclusive to a single queue family at a time.</summary>
		VK_SHARING_MODE_EXCLUSIVE = 0,

		/// <summary>Specifies that concurrent access to any range or image subresource of the
		/// object from multiple queue families is supported.</summary>
		VK_SHARING_MODE_CONCURRENT = 1,
	}
}
