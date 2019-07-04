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
	/// <summary>Structure containing callback function pointers for memory allocation
	/// (NOT IMPLEMENTED).</summary>
	public interface VkAllocationCallbacks { }

	/// <summary>There is no global state in Vulkan and all per-application state is stored in a
	/// VkInstance object. Creating a VkInstance object initializes the Vulkan library and allows
	/// the application to pass information about itself to the implementation.</summary>
	public interface VkInstance { }

	/// <summary>Vulkan separates the concept of physical and logical devices. A physical device
	/// usually represents a single device in a system (perhaps made up of several individual
	/// hardware devices working together), of which there are a finite number. A logical device
	/// represents an application’s view of the device.</summary>
	public interface VkPhysicalDevice { }

	/// <summary>Opaque handle to a device object.</summary>
	public interface VkDevice { }

	/// <summary>Buffers represent linear arrays of data which are used for various purposes by
	/// binding them to a graphics or compute pipeline via descriptor sets or via certain commands,
	/// or by directly specifying them as parameters to certain commands.</summary>
	public interface VkBuffer { }

	/// <summary>A buffer view represents a contiguous range of a buffer and a specific format to
	/// be used to interpret the data. Buffer views are used to enable shaders to access buffer
	/// contents interpreted as formatted data. In order to create a valid buffer view, the buffer
	/// must have been created with at least one of the following usage flags:
	/// VK_BUFFER_USAGE_UNIFORM_TEXEL_BUFFER_BIT or VK_BUFFER_USAGE_STORAGE_TEXEL_BUFFER_BIT
	/// </summary>
	public interface VkBufferView { }

	/// <summary>A Vulkan device operates on data in device memory via memory objects that are
	/// represented in the API by a VkDeviceMemory handle.</summary>
	public interface VkDeviceMemory { }

	/// <summary>
	/// Command pools are opaque objects that command buffer memory is allocated from, and
	/// which allow the implementation to amortize the cost of resource creation across multiple
	/// command buffers. Command pools are externally synchronized, meaning that a command pool
	/// must not be used concurrently in multiple threads. That includes use via recording
	/// commands on any command buffers allocated from the pool, as well as operations that
	/// allocate, free, and reset command buffers or the pool itself.</summary>
	public interface VkCommandPool { }

	/// <summary>Command buffers are objects used to record commands which can be subsequently
	/// submitted to a device queue for execution. There are two levels of command buffers - primary
	/// command buffers, which can execute secondary command buffers, and which are submitted to
	/// queues, and secondary command buffers, which can be executed by primary command buffers,
	/// and which are not directly submitted to queues.</summary>
	public interface VkCommandBuffer { }

	/// <summary>Images represent multidimensional - up to 3 - arrays of data which can be used
	/// for various purposes (e.g. attachments, textures), by binding them to a graphics or
	/// compute pipeline via descriptor sets, or by directly specifying them as parameters to
	/// certain commands.</summary>
	public interface VkImage { }

	/// <summary>Image objects are not directly accessed by pipeline shaders for reading or writing
	/// image data. Instead, image views representing contiguous ranges of the image subresources
	/// and containing additional metadata are used for that purpose. Views must be created on
	/// images of compatible types, and must represent a valid subset of image subresources.
	/// </summary>
	public interface VkImageView { }

	/// <summary>Compute and graphics pipelines are each represented by VkPipeline handles.</summary>
	public interface VkPipeline { }

	/// <summary>Pipeline cache objects allow the result of pipeline construction to be reused
	/// between pipelines and between runs of an application. Reuse between pipelines is achieved
	/// by passing the same pipeline cache object when creating multiple related pipelines. Reuse
	/// across runs of an application is achieved by retrieving pipeline cache contents in one
	/// run of an application, saving the contents, and using them to preinitialize a pipeline
	/// cache on a subsequent run. The contents of the pipeline cache objects are managed by the
	/// implementation. Applications can manage the host memory consumed by a pipeline cache
	/// object and control the amount of data retrieved from a pipeline cache object.</summary>
	public interface VkPipelineCache { }

	/// <summary>VkSampler objects represent the state of an image sampler which is used by the
	/// implementation to read image data and apply filtering and other transformations for the
	/// shader.</summary>
	public interface VkSampler { }

	public interface VkDescriptorPool { };
	public interface VkDescriptorSet { };
	public interface VkDescriptorSetLayout { };
	public interface VkFramebuffer { };
	public interface VkPipelineLayout { };
	public interface VkQueue { };
	public interface VkRenderPass { };
	public interface VkSemaphore { };
	public interface VkFence { };
	public interface VkShaderModule { };

	/// <summary>Represents an abstract type of surface to present rendered images to.
	/// A VkSurfaceKHR object abstracts a native platform surface or window object for use with
	/// Vulkan.</summary>
	public interface VkSurfaceKHR { };


	public interface VkSwapchainKHR { };
}
