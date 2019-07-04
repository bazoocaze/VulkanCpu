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
	public class VkDebugReportCallbackEXT
	{
		public VkInstance m_instance;
		public VkDebugReportCallbackCreateInfoEXT m_pCreateInfo;

		public VkDebugReportCallbackEXT(VkInstance instance, VkDebugReportCallbackCreateInfoEXT pCreateInfo)
		{
			this.m_instance = instance;
			this.m_pCreateInfo = pCreateInfo;
		}
	}

	public struct VkDebugReportCallbackCreateInfoEXT
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Indicate which event(s) will cause this callback to be called.
		/// Flags are interpreted as bitmasks and multiple may be set.</summary>
		public VkDebugReportFlagBitsEXT flags;

		/// <summary>Is the application callback function to call.</summary>
		public PFN_vkDebugReportCallbackEXT pfnCallback;

		/// <summary>Is user data to be passed to the callback. </summary>
		public object pUserData;
	}

	public delegate bool PFN_vkDebugReportCallbackEXT(
		VkDebugReportFlagBitsEXT flags,
		VkDebugReportObjectTypeEXT objectType,
		object obj,
		int location,
		int messageCode,
		string pLayerPrefix,
		string pMessage,
		object pUserData);

	public delegate bool PFN_vkDebugReportMessageEXT(
		VkDebugReportFlagBitsEXT flags,
		VkDebugReportObjectTypeEXT objectType,
		object obj,
		int location,
		int messageCode,
		string pLayerPrefix,
		string pMessage);

	[Flags]
	public enum VkDebugReportFlagBitsEXT
	{
		/// <summary>Indicates an informational message such as resource details that may be handy when debugging an application.</summary>
		VK_DEBUG_REPORT_INFORMATION_BIT_EXT = 0x00000001,

		/// <summary>Indicates use of Vulkan that may expose an app bug.
		/// Such cases may not be immediately harmful, such as a fragment shader outputting to a location with no attachment.
		/// Other cases may point to behavior that is almost certainly bad when unintended such as using an image whose memory hasn’t been filled.
		/// In general if you see a warning but you know that the behavior is intended/desired, then simply ignore the warning.</summary>
		VK_DEBUG_REPORT_WARNING_BIT_EXT = 0x00000002,

		/// <summary>Indicates a potentially non-optimal use of Vulkan. E.g. using vkCmdClearColorImage when a RenderPass load_op would have worked.</summary>
		VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT = 0x00000004,

		/// <summary>Indicates an error that may cause undefined results, including an application crash</summary>
		VK_DEBUG_REPORT_ERROR_BIT_EXT = 0x00000008,

		/// <summary>Indicates diagnostic information from the loader and layers.</summary>
		VK_DEBUG_REPORT_DEBUG_BIT_EXT = 0x00000010,
	}

	public enum VkDebugReportObjectTypeEXT
	{
		VK_DEBUG_REPORT_OBJECT_TYPE_UNKNOWN_EXT = 0,
		VK_DEBUG_REPORT_OBJECT_TYPE_INSTANCE_EXT = 1,
		VK_DEBUG_REPORT_OBJECT_TYPE_PHYSICAL_DEVICE_EXT = 2,
		VK_DEBUG_REPORT_OBJECT_TYPE_DEVICE_EXT = 3,
		VK_DEBUG_REPORT_OBJECT_TYPE_QUEUE_EXT = 4,
		VK_DEBUG_REPORT_OBJECT_TYPE_SEMAPHORE_EXT = 5,
		VK_DEBUG_REPORT_OBJECT_TYPE_COMMAND_BUFFER_EXT = 6,
		VK_DEBUG_REPORT_OBJECT_TYPE_FENCE_EXT = 7,
		VK_DEBUG_REPORT_OBJECT_TYPE_DEVICE_MEMORY_EXT = 8,
		VK_DEBUG_REPORT_OBJECT_TYPE_BUFFER_EXT = 9,
		VK_DEBUG_REPORT_OBJECT_TYPE_IMAGE_EXT = 10,
		VK_DEBUG_REPORT_OBJECT_TYPE_EVENT_EXT = 11,
		VK_DEBUG_REPORT_OBJECT_TYPE_QUERY_POOL_EXT = 12,
		VK_DEBUG_REPORT_OBJECT_TYPE_BUFFER_VIEW_EXT = 13,
		VK_DEBUG_REPORT_OBJECT_TYPE_IMAGE_VIEW_EXT = 14,
		VK_DEBUG_REPORT_OBJECT_TYPE_SHADER_MODULE_EXT = 15,
		VK_DEBUG_REPORT_OBJECT_TYPE_PIPELINE_CACHE_EXT = 16,
		VK_DEBUG_REPORT_OBJECT_TYPE_PIPELINE_LAYOUT_EXT = 17,
		VK_DEBUG_REPORT_OBJECT_TYPE_RENDER_PASS_EXT = 18,
		VK_DEBUG_REPORT_OBJECT_TYPE_PIPELINE_EXT = 19,
		VK_DEBUG_REPORT_OBJECT_TYPE_DESCRIPTOR_SET_LAYOUT_EXT = 20,
		VK_DEBUG_REPORT_OBJECT_TYPE_SAMPLER_EXT = 21,
		VK_DEBUG_REPORT_OBJECT_TYPE_DESCRIPTOR_POOL_EXT = 22,
		VK_DEBUG_REPORT_OBJECT_TYPE_DESCRIPTOR_SET_EXT = 23,
		VK_DEBUG_REPORT_OBJECT_TYPE_FRAMEBUFFER_EXT = 24,
		VK_DEBUG_REPORT_OBJECT_TYPE_COMMAND_POOL_EXT = 25,
		VK_DEBUG_REPORT_OBJECT_TYPE_SURFACE_KHR_EXT = 26,
		VK_DEBUG_REPORT_OBJECT_TYPE_SWAPCHAIN_KHR_EXT = 27,
		VK_DEBUG_REPORT_OBJECT_TYPE_DEBUG_REPORT_EXT = 28,
		VK_DEBUG_REPORT_OBJECT_TYPE_DISPLAY_KHR_EXT = 29,
		VK_DEBUG_REPORT_OBJECT_TYPE_DISPLAY_MODE_KHR_EXT = 30,
		VK_DEBUG_REPORT_OBJECT_TYPE_OBJECT_TABLE_NVX_EXT = 31,
		VK_DEBUG_REPORT_OBJECT_TYPE_INDIRECT_COMMANDS_LAYOUT_NVX_EXT = 32,
	}
}
