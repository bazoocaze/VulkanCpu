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
	/// <summary>Structure specifying a two-dimensional extent.</summary>
	public struct VkExtent2D
	{
		/// <summary>Width of the extent.</summary>
		public int width;

		/// <summary>Height of the extent.</summary>
		public int height;

		public VkExtent2D(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public static VkExtent2D Create(int width, int height)
		{
			return new VkExtent2D() { width = width, height = height };
		}

		public override string ToString()
		{
			return string.Format("width={0} height={1}", width, height);
		}
	}

	/// <summary>Structure specifying a two-dimensional offset.</summary>
	public struct VkOffset2D
	{
		/// <summary>The x offset.</summary>
		public int x;

		/// <summary>The y offset.</summary>
		public int y;

		public static VkOffset2D Create(int x, int y)
		{
			return new VkOffset2D() { x = x, y = y };
		}

		public override string ToString()
		{
			return string.Format("x={0} y={1}", x, y);
		}
	}

	/// <summary>Structure specifying a two-dimensional subregion.</summary>
	public struct VkRect2D
	{
		/// <summary>Is a VkOffset2D specifying the rectangle offset.</summary>
		public VkOffset2D offset;

		/// <summary>Is a VkExtent2D specifying the rectangle extent.</summary>
		public VkExtent2D extent;

		public override string ToString()
		{
			return string.Format("left={0} top={1} width={2} height={3}", offset.x, offset.y, extent.width, extent.height);
		}
	}

	/// <summary>Structure specifying a three-dimensional extent.</summary>
	public struct VkExtent3D
	{
		/// <summary>Is the width of the extent.</summary>
		public int width;

		/// <summary>Is the height of the extent.</summary>
		public int height;

		/// <summary>Is the depth of the extent.</summary>
		public int depth;

		public static VkExtent3D Create(int width, int height, int depth)
		{
			return new VkExtent3D() { width = width, height = height, depth = depth };
		}

		public override string ToString()
		{
			return string.Format("width={0} height={1} depth={2}", width, height, depth);
		}
	}

	/// <summary>Structure specifying a three-dimensional offset.</summary>
	public struct VkOffset3D
	{
		/// <summary>The x offset.</summary>
		public int x;

		/// <summary>The x offset.</summary>
		public int y;

		/// <summary>The x offset.</summary>
		public int z;

		public static VkOffset3D Create(int x, int y, int z)
		{
			return new VkOffset3D() { x = x, y = y, z = z };
		}

		public override string ToString()
		{
			return string.Format("x={0} y={1} z={2}", x, y, z);
		}
	}
}
