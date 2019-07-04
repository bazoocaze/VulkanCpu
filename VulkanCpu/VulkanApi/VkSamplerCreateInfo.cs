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
	/// <summary>Structure specifying parameters of a newly created sampler.</summary>
	public struct VkSamplerCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is a VkFilter value specifying the magnification filter to apply to lookups.
		/// </summary>
		public VkFilter magFilter;

		/// <summary>Is a VkFilter value specifying the minification filter to apply to lookups.
		/// </summary>
		public VkFilter minFilter;

		/// <summary>Is a VkSamplerMipmapMode value specifying the mipmap filter to apply to
		/// lookups.</summary>
		public VkSamplerMipmapMode mipmapMode;

		/// <summary>Is a VkSamplerAddressMode value specifying the addressing mode for
		/// outside [0..1] range for U coordinate.</summary>
		public VkSamplerAddressMode addressModeU;

		/// <summary>Is a VkSamplerAddressMode value specifying the addressing mode for
		/// outside [0..1] range for V coordinate.</summary>
		public VkSamplerAddressMode addressModeV;

		/// <summary>Is a VkSamplerAddressMode value specifying the addressing mode for
		/// outside [0..1] range for W coordinate.</summary>
		public VkSamplerAddressMode addressModeW;

		/// <summary>Is the bias to be added to mipmap LOD (level-of-detail) calculation and
		/// bias provided by image sampling functions in SPIR-V, as described in the
		/// Level-of-Detail Operation section.</summary>
		public float mipLodBias;

		/// <summary>Is VK_TRUE to enable anisotropic filtering, as described in the Texel
		/// Anisotropic Filtering section, or VK_FALSE otherwise.</summary>
		public VkBool32 anisotropyEnable;

		/// <summary>Is the anisotropy value clamp used by the sampler when anisotropyEnable
		/// is VK_TRUE. If anisotropyEnable is VK_FALSE, maxAnisotropy is ignored.</summary>
		public float maxAnisotropy;

		/// <summary>Is VK_TRUE to enable comparison against a reference value during lookups, or
		/// VK_FALSE otherwise. 
		/// <para>Note: Some implementations will default to shader state if this member does not
		/// match.</para></summary>
		public VkBool32 compareEnable;

		/// <summary>Is a VkCompareOp value specifying the comparison function to apply to fetched
		/// data before filtering as described in the Depth Compare Operation section.</summary>
		public VkCompareOp compareOp;

		/// <summary>minLod and maxLod are the values used to clamp the computed LOD value, as
		/// described in the Level-of-Detail Operation section. maxLod must be greater than or
		/// equal to minLod.</summary>
		public float minLod;

		/// <summary>minLod and maxLod are the values used to clamp the computed LOD value, as
		/// described in the Level-of-Detail Operation section. maxLod must be greater than or
		/// equal to minLod.</summary>
		public float maxLod;

		/// <summary>Is a VkBorderColor value specifying the predefined border color to use.
		/// </summary>
		public VkBorderColor borderColor;

		/// <summary>Controls whether to use unnormalized or normalized texel coordinates to
		/// address texels of the image. When set to VK_TRUE, the range of the image coordinates
		/// used to lookup the texel is in the range of zero to the image dimensions for x, y
		/// and z. When set to VK_FALSE the range of image coordinates is zero to one.</summary>
		public VkBool32 unnormalizedCoordinates;
	}

	/// <summary>Stencil comparison function</summary>
	public enum VkCompareOp
	{
		/// <summary>Specifies that the test never passes.</summary>
		VK_COMPARE_OP_NEVER = 0,

		/// <summary>Specifies that the test passes when Val &lt; Stored</summary>
		VK_COMPARE_OP_LESS = 1,

		/// <summary>Specifies that the test passes when Val == Stored</summary>
		VK_COMPARE_OP_EQUAL = 2,

		/// <summary>Specifies that the test passes when Val &lt;= Stored</summary>
		VK_COMPARE_OP_LESS_OR_EQUAL = 3,

		/// <summary>Specifies that the test passes when Val &gt;= Stored</summary>
		VK_COMPARE_OP_GREATER = 4,

		/// <summary>Specifies that the test passes when Val != Stored</summary>
		VK_COMPARE_OP_NOT_EQUAL = 5,

		/// <summary>Specifies that the test passes when Val &gt;= Stored</summary>
		VK_COMPARE_OP_GREATER_OR_EQUAL = 6,

		/// <summary>Specifies that the test always passes</summary>
		VK_COMPARE_OP_ALWAYS = 7,
	}

	/// <summary>Specify border color used for texture lookups</summary>
	public enum VkBorderColor
	{
		VK_BORDER_COLOR_FLOAT_TRANSPARENT_BLACK = 0,
		VK_BORDER_COLOR_INT_TRANSPARENT_BLACK = 1,
		VK_BORDER_COLOR_FLOAT_OPAQUE_BLACK = 2,
		VK_BORDER_COLOR_INT_OPAQUE_BLACK = 3,
		VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE = 4,
		VK_BORDER_COLOR_INT_OPAQUE_WHITE = 5,
	}

	/// <summary>Specify filters used for texture lookups</summary>
	public enum VkFilter
	{
		VK_FILTER_NEAREST = 0,
		VK_FILTER_LINEAR = 1,
	}

	/// <summary>Specify mipmap mode used for texture lookups</summary>
	public enum VkSamplerMipmapMode
	{
		VK_SAMPLER_MIPMAP_MODE_NEAREST = 0,
		VK_SAMPLER_MIPMAP_MODE_LINEAR = 1,
	}

	/// <summary>Possible values of the VkSamplerCreateInfo::addressMode* parameters, specifying the behavior of sampling with coordinates outside the range [0,1] for the respective u, v, or w coordinate as defined in the Wrapping Operation section, are:</summary>
	public enum VkSamplerAddressMode
	{
		/// <summary>specifies that the repeat wrap mode will be used.</summary>
		VK_SAMPLER_ADDRESS_MODE_REPEAT = 0,

		/// <summary>specifies that the mirrored repeat wrap mode will be used.</summary>
		VK_SAMPLER_ADDRESS_MODE_MIRRORED_REPEAT = 1,

		/// <summary> specifies that the clamp to edge wrap mode will be used.</summary>
		VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE = 2,

		/// <summary>specifies that the clamp to border wrap mode will be used.</summary>
		VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_BORDER = 3,

		/// <summary>specifies that the mirror clamp to edge wrap mode will be used. This is only valida if the VK_KHR_sampler_mirror_clamp_to_edge extension is enabled.</summary>
		VK_SAMPLER_ADDRESS_MODE_MIRROR_CLAMP_TO_EDGE = 4,
	}
}
