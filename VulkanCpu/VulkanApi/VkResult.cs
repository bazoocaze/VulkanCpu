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
	/// <summary>Vulkan command return codes.
	/// <para>Successful completion codes are returned when a command needs to communicate success or
	/// status information. All successful completion codes are non-negative values.</para>
	/// <para>Run time error codes are returned when a command needs to communicate a failure that
	/// could only be detected at run time. All run time error codes are negative values.</para>
	/// </summary>
	public enum VkResult
	{
		/// <summary>Command successfully completed </summary>
		VK_SUCCESS = 0,

		/// <summary>A fence or query has not yet completed </summary>
		VK_NOT_READY = 1,

		/// <summary>A wait operation has not completed in the specified time </summary>
		VK_TIMEOUT = 2,

		/// <summary>An event is signaled </summary>
		VK_EVENT_SET = 3,

		/// <summary>An event is unsignaled </summary>
		VK_EVENT_RESET = 4,

		/// <summary>A return array was too small for the result </summary>
		VK_INCOMPLETE = 5,

		/// <summary>A host memory allocation has failed.</summary>
		VK_ERROR_OUT_OF_HOST_MEMORY = -1,

		/// <summary>A device memory allocation has failed.</summary>
		VK_ERROR_OUT_OF_DEVICE_MEMORY = -2,

		/// <summary>Initialization of an object could not be completed for implementation-specific
		/// reasons. </summary>
		VK_ERROR_INITIALIZATION_FAILED = -3,

		/// <summary>The logical or physical device has been lost. See Lost Device.</summary>
		VK_ERROR_DEVICE_LOST = -4,

		/// <summary>Mapping of a memory object has failed.</summary>
		VK_ERROR_MEMORY_MAP_FAILED = -5,

		/// <summary>A requested layer is not present or could not be loaded.</summary>
		VK_ERROR_LAYER_NOT_PRESENT = -6,

		/// <summary>A requested extension is not supported.</summary>
		VK_ERROR_EXTENSION_NOT_PRESENT = -7,

		/// <summary>A requested feature is not supported.</summary>
		VK_ERROR_FEATURE_NOT_PRESENT = -8,

		/// <summary>The requested version of Vulkan is not supported by the driver or is otherwise
		/// incompatible for implementation-specific reasons. </summary>
		VK_ERROR_INCOMPATIBLE_DRIVER = -9,

		/// <summary>Too many objects of the type have already been created.</summary>
		VK_ERROR_TOO_MANY_OBJECTS = -10,

		/// <summary>A requested format is not supported on this device.</summary>
		VK_ERROR_FORMAT_NOT_SUPPORTED = -11,

		/// <summary>A requested pool allocation has failed due to fragmentation of the pool’s 
		/// memory. </summary>
		VK_ERROR_FRAGMENTED_POOL = -12,

		/// <summary>A surface is no longer available.</summary>
		VK_ERROR_SURFACE_LOST_KHR = -1000000000,

		/// <summary>The requested window is already connected to a VkSurfaceKHR, or to some other
		/// non-Vulkan API. </summary>
		VK_ERROR_NATIVE_WINDOW_IN_USE_KHR = -1000000001,

		/// <summary>A swapchain no longer matches the surface properties exactly, but can still be
		/// used to present to the surface successfully.</summary>
		VK_SUBOPTIMAL_KHR = 1000001003,

		/// <summary>A surface has changed in such a way that it is no longer compatible with the
		/// swapchain, and further presentation requests using the swapchain will fail. Applications
		/// must query the new surface properties and recreate their swapchain if they wish to
		/// continue presenting to the surface. </summary>
		VK_ERROR_OUT_OF_DATE_KHR = -1000001004,

		/// <summary>The display used by a swapchain does not use the same presentable image layout,
		/// or is incompatible in a way that prevents sharing an image. </summary>
		VK_ERROR_INCOMPATIBLE_DISPLAY_KHR = -1000003001,

		VK_ERROR_VALIDATION_FAILED_EXT = -1000011001,

		/* ----- EXCLUSIVE FOR THIS WRAPPER ----- */
		VK_ERROR_BUFFER_COMPILATION = -1000,
	}
}
