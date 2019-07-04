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
	/// <summary>Structure specifying parameters of a newly created device queue.</summary>
	public struct VkDeviceQueueCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Is an unsigned integer indicating the index of the queue family to create on
		/// this device. This index corresponds to the index of an element of the
		/// pQueueFamilyProperties array that was returned by
		/// vkGetPhysicalDeviceQueueFamilyProperties.</summary>
		public int queueFamilyIndex;

		/// <summary>Is an unsigned integer specifying the number of queues to create in the queue
		/// family indicated by queueFamilyIndex.</summary>
		public int queueCount;

		/// <summary>Is an array of queueCount normalized floating point values, specifying
		/// priorities of work that will be submitted to each created queue. See Queue Priority
		/// for more information.</summary>
		public float[] pQueuePriorities;

		public override string ToString()
		{
			return string.Format("familyIndex={0} count={1}", queueFamilyIndex, queueCount);
		}
	}
}
