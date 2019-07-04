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
	/// <summary>Structure specifying parameters of a newly created device.</summary>
	public struct VkDeviceCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is the unsigned integer size of the pQueueCreateInfos array. Refer to the
		/// Queue Creation section below for further details.</summary>
		public int queueCreateInfoCount;

		/// <summary>Is a pointer to an array of VkDeviceQueueCreateInfo structures describing the
		/// queues that are requested to be created along with the logical device. Refer to the
		/// Queue Creation section below for further details.</summary>
		public VkDeviceQueueCreateInfo[] pQueueCreateInfos;

		/// <summary>Deprecated and ignored. See Device Layer Deprecation.</summary>
		[Obsolete]
		public int enabledLayerCount;

		/// <summary>Deprecated and ignored. See Device Layer Deprecation.</summary>
		[Obsolete]
		public string[] ppEnabledLayerNames;

		/// <summary>Is the number of device extensions to enable.</summary>
		public int enabledExtensionCount;

		/// <summary>Is a pointer to an array of enabledExtensionCount null-terminated UTF-8
		/// strings containing the names of extensions to enable for the created device. See the
		/// Extensions section for further details.</summary>
		public string[] ppEnabledExtensionNames;

		/// <summary>Is NULL or a pointer to a VkPhysicalDeviceFeatures structure that contains
		/// boolean indicators of all the features to be enabled. Refer to the Features section
		/// for further details.</summary>
		public VkPhysicalDeviceFeatures[] pEnabledFeatures;
	}
}
