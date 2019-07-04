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
	/// <summary>Structure specifying parameters of a newly created pipeline multisample
	/// state.</summary>
	public struct VkPipelineMultisampleStateCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is a VkSampleCountFlagBits specifying the number of samples per pixel used in
		/// rasterization.</summary>
		public VkSampleCountFlagBits rasterizationSamples;

		/// <summary>Can be used to enable Sample Shading.</summary>
		public VkBool32 sampleShadingEnable;

		/// <summary>Specifies a minimum fraction of sample shading if sampleShadingEnable is set
		/// to VK_TRUE.</summary>
		public float minSampleShading;

		/// <summary>Is a bitmask of static coverage information that is ANDed with the coverage
		/// information generated during rasterization, as described in Sample Mask.</summary>
		public uint pSampleMask;

		/// <summary>Controls whether a temporary coverage value is generated based on the alpha
		/// component of the fragment’s first color output as specified in the Multisample Coverage
		/// section.</summary>
		public VkBool32 alphaToCoverageEnable;

		/// <summary>Controls whether the alpha component of the fragment’s first color output is
		/// replaced with one as described in Multisample Coverage.</summary>
		public VkBool32 alphaToOneEnable;
	}

	/// <summary>Bitmask specifying sample counts supported for an image used for storage
	/// operations.</summary>
	[Flags]
	public enum VkSampleCountFlagBits
	{
		/// <summary>Specifies an image with one sample per pixel.</summary>
		VK_SAMPLE_COUNT_1_BIT = 0x00000001,

		/// <summary>Specifies an image with 2 samples per pixel.</summary>
		VK_SAMPLE_COUNT_2_BIT = 0x00000002,

		/// <summary>Specifies an image with 4 samples per pixel.</summary>
		VK_SAMPLE_COUNT_4_BIT = 0x00000004,

		/// <summary>Specifies an image with 8 samples per pixel.</summary>
		VK_SAMPLE_COUNT_8_BIT = 0x00000008,

		/// <summary>Specifies an image with 16 samples per pixel.</summary>
		VK_SAMPLE_COUNT_16_BIT = 0x00000010,

		/// <summary>Specifies an image with 32 samples per pixel.</summary>
		VK_SAMPLE_COUNT_32_BIT = 0x00000020,

		/// <summary>Specifies an image with 64 samples per pixel.</summary>
		VK_SAMPLE_COUNT_64_BIT = 0x00000040,
	}
}
