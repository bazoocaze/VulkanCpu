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
	/// <summary>Structure specifying a buffer image copy operation.</summary>
	public struct VkBufferImageCopy
	{
		/// <summary>Is the offset in bytes from the start of the buffer object where the image
		/// data is copied from or to.</summary>
		public int bufferOffset;

		/// <summary>bufferRowLength and bufferImageHeight specify the data in buffer memory as a
		/// subregion of a larger two- or three-dimensional image, and control the addressing
		/// calculations of data in buffer memory.
		/// If either of these values is zero, that aspect of the buffer memory is considered to
		/// be tightly packed according to the imageExtent.</summary>
		public int bufferRowLength;

		/// <summary>bufferRowLength and bufferImageHeight specify the data in buffer memory as a
		/// subregion of a larger two- or three-dimensional image, and control the addressing
		/// calculations of data in buffer memory.
		/// If either of these values is zero, that aspect of the buffer memory is considered to be
		/// tightly packed according to the imageExtent.</summary>
		public int bufferImageHeight;

		/// <summary>Is a VkImageSubresourceLayers used to specify the specific image subresources
		/// of the image used for the source or destination image data.</summary>
		public VkImageSubresourceLayers imageSubresource;

		/// <summary>Selects the initial x, y, z offsets in texels of the sub-region of the source or
		/// destination image data.</summary>
		public VkOffset3D imageOffset;

		/// <summary>Is the size in texels of the image to copy in width, height and depth.</summary>
		public VkExtent3D imageExtent;

		public override string ToString()
		{
			return string.Format("{0} {1} bufferOffset={2} bufferRowLength={3} bufferImageHeight={4} imageSubresource=[{5}]",
				imageOffset, imageExtent,
				bufferOffset, bufferRowLength, bufferImageHeight, imageSubresource);
		}
	}
}
