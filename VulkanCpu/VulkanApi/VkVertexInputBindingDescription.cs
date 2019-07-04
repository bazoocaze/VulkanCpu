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
	/// <summary>Structure specifying vertex input binding description.</summary>
	public struct VkVertexInputBindingDescription
	{
		/// <summary>Binding is the binding number that this structure describes.</summary>
		public int binding;

		/// <summary>Stride is the distance in bytes between two consecutive elements within the buffer.</summary>
		public int stride;

		/// <summary>InputRate is a VkVertexInputRate value specifying whether vertex attribute addressing is a function of the vertex index or of the instance index.</summary>
		public VkVertexInputRate inputRate;
	}

	/// <summary>Structure specifying vertex input attribute description</summary>
	public struct VkVertexInputAttributeDescription
	{
		/// <summary>Location is the shader binding location number for this attribute.</summary>
		public int location;

		/// <summary>Binding is the binding number which this attribute takes its data from.</summary>
		public int binding;

		/// <summary>Format is the size and type of the vertex attribute data.</summary>
		public VkFormat format;

		/// <summary>Offset is a byte offset of this attribute relative to the start of an element in the vertex input binding.</summary>
		public int offset;

		public override string ToString()
		{
			return string.Format("location={0} binding={1} format={2} offset={3}", location, binding, format, offset);
		}
	}

	/// <summary>Specify rate at which vertex attributes are pulled from buffers</summary>
	public enum VkVertexInputRate
	{
		/// <summary>Specifies that vertex attribute addressing is a function of the vertex index.</summary>
		VK_VERTEX_INPUT_RATE_VERTEX = 0,

		/// <summary>Specifies that vertex attribute addressing is a function of the instance index.</summary>
		VK_VERTEX_INPUT_RATE_INSTANCE = 1,
	}
}
