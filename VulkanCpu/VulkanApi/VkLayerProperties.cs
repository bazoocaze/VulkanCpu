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
	/// <summary>Structure specifying layer properties.</summary>
	public struct VkLayerProperties
	{
		/// <summary>null-terminated UTF-8 string specifying the name of the layer. Use this name
		/// in the ppEnabledLayerNames array passed in the VkInstanceCreateInfo structure to
		/// enable this layer for an instance.</summary>
		public string layerName;

		/// <summary>The Vulkan version the layer was written to, encoded as described in the API
		/// Version Numbers and Semantics section.</summary>
		public uint specVersion;

		/// <summary>The version of this layer. It is an integer, increasing with backward
		/// compatible changes.</summary>
		public uint implementationVersion;

		/// <summary>null-terminated UTF-8 string providing additional details that can be used
		/// by the application to identify the layer.</summary>
		public string description;

		public static VkLayerProperties Create(string layerName, string description, uint specVersion, uint implementationVersion)
		{
			VkLayerProperties ret = new VkLayerProperties()
			{
				layerName = layerName,
				description = description,
				specVersion = specVersion,
				implementationVersion = implementationVersion
			};
			return ret;
		}
	}
}
