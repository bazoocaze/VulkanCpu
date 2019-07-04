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
	/// <summary>Structure specifying application info.</summary>
	public struct VkApplicationInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is NULL or is a pointer to a null-terminated UTF-8 string containing the
		/// name of the application.</summary>
		public string pApplicationName;

		/// <summary>Is an unsigned integer variable containing the developer-supplied version number
		/// of the application.</summary>
		public uint applicationVersion;

		/// <summary>Is NULL or is a pointer to a null-terminated UTF-8 string containing the name of
		/// the engine (if any) used to create the application.</summary>
		public string pEngineName;

		/// <summary>Is an unsigned integer variable containing the developer-supplied version number
		/// of the engine used to create the application.</summary>
		public uint engineVersion;

		/// <summary>is the version of the Vulkan API against which the application expects to run,
		/// encoded as described in the API Version Numbers and Semantics section. If apiVersion
		/// is 0 the implementation must ignore it, otherwise if the implementation does not
		/// support the requested apiVersion, or an effective substitute for apiVersion, it must return
		/// VK_ERROR_INCOMPATIBLE_DRIVER. The patch version number specified in apiVersion is ignored
		/// when creating an instance object. Only the major and minor versions of the instance must
		/// match those requested in apiVersion.</summary>
		public VkApiVersion apiVersion;
	}
}


