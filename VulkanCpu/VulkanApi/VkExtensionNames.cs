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
	/// <summary>Know extension names.</summary>
	public static class VkExtensionNames
	{
		public const string VK_EXT_DEBUG_REPORT_EXTENSION_NAME = "VK_EXT_DEBUG_REPORT_EXTENSION";
		public const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_SWAPCHAIN_EXTENSION";
	}

	/// <summary>Structure specifying a extension properties.</summary>
	public struct VkExtensionProperties
	{
		/// <summary>Is a null-terminated string specifying the name of the extension.</summary>
		public string extensionName;

		/// <summary>Is the version of this extension. It is an integer, incremented with backward
		/// compatible changes.</summary>
		public uint specVersion;

		public static VkExtensionProperties Create(string extensionName, uint specVersion)
		{
			return new VkExtensionProperties() { extensionName = extensionName, specVersion = specVersion };
		}

		public override string ToString()
		{
			return extensionName ?? "?";
		}
	}
}
