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
	/// <summary>
	/// Vulkan boolean type.
	/// VK_TRUE represents a boolean True(integer 1) value, and VK_FALSE a boolean False(integer 0) value.
	/// All values returned from a Vulkan implementation in a VkBool32 will be either VK_TRUE or VK_FALSE.
	/// Applications must not pass any other values than VK_TRUE or VK_FALSE into a Vulkan implementation
	/// where a VkBool32 is expected.
	/// </summary>
	public enum VkBool32 : uint
	{
		/// <summary>Represents a boolean False(integer 0) value.</summary>
		VK_FALSE = 0,

		/// <summary>Represents a boolean True(integer 1) value.</summary>
		VK_TRUE = 1,
	}
}
