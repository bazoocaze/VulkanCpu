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
	/// <summary>Structure specifying parameters of a newly created framebuffer.</summary>
	public struct VkFramebufferCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is a render pass that defines what render passes the framebuffer will be
		/// compatible with. See Render Pass Compatibility for details.</summary>
		public VkRenderPass renderPass;

		/// <summary>Is the number of attachments.</summary>
		public int attachmentCount;

		/// <summary>Is an array of VkImageView handles, each of which will be used as the
		/// corresponding attachment in a render pass instance.</summary>
		public VkImageView[] pAttachments;

		/// <summary>width, height and layers define the dimensions of the framebuffer.</summary>
		public int width;

		/// <summary>width, height and layers define the dimensions of the framebuffer.</summary>
		public int height;

		/// <summary>width, height and layers define the dimensions of the framebuffer.</summary>
		public int layers;
	}
}
