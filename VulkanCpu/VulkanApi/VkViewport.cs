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
	/// <summary>Structure specifying a viewport.</summary>
	public struct VkViewport
	{
		/// <summary>x and y are the viewport’s upper left corner (x,y).</summary>
		public float x;

		/// <summary>x and y are the viewport’s upper left corner (x,y).</summary>
		public float y;

		/// <summary>width and height are the viewport’s width and height, respectively.</summary>
		public float width;

		/// <summary>width and height are the viewport’s width and height, respectively.</summary>
		public float height;

		/// <summary>minDepth and maxDepth are the depth range for the viewport. It is valid for
		/// minDepth to be greater than or equal to maxDepth.</summary>
		public float minDepth;

		/// <summary>minDepth and maxDepth are the depth range for the viewport. It is valid for
		/// minDepth to be greater than or equal to maxDepth.</summary>
		public float maxDepth;
	}
}
