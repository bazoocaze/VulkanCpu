﻿/*
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
	/// <summary>Structure specifying parameters of a newly created instance.</summary>
	public struct VkInstanceCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is NULL or a pointer to an instance of VkApplicationInfo. If not NULL, this
		/// information helps implementations recognize behavior inherent to classes of
		/// applications. VkApplicationInfo is defined in detail below.</summary>
		public VkApplicationInfo pApplicationInfo;

		/// <summary>Is the number of global layers to enable.</summary>
		public int enabledLayerCount;

		/// <summary>Pointer to an array of enabledLayerCount null-terminated UTF-8 strings
		/// containing the names of layers to enable for the created instance. See the Layers
		/// section for further details.</summary>
		public string[] ppEnabledLayerNames;

		/// <summary>The number of global extensions to enable.</summary>
		public int enabledExtensionCount;

		/// <summary>Pointer to an array of enabledExtensionCount null-terminated UTF-8 strings
		/// containing the names of extensions to enable.</summary>
		public string[] ppEnabledExtensionNames;
	}
}
