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
	/// <summary>Structure specifying parameters of a newly created descriptor set layout</summary>
	public struct VkDescriptorSetLayoutCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is the number of elements in pBindings.</summary>
		public int bindingCount;

		/// <summary>Is a pointer to an array of VkDescriptorSetLayoutBinding structures.</summary>
		public VkDescriptorSetLayoutBinding[] pBindings;
	}

	/// <summary>Structure specifying a descriptor set layout binding</summary>
	public struct VkDescriptorSetLayoutBinding
	{
		/// <summary>Is the binding number of this entry and corresponds to a resource of the same binding number in the shader stages.</summary>
		public int binding;

		/// <summary>Is a VkDescriptorType specifying which type of resource descriptors are used for this binding.</summary>
		public VkDescriptorType descriptorType;

		/// <summary>Is the number of descriptors contained in the binding, accessed in a shader as an array.
		/// If descriptorCount is zero this binding entry is reserved and the resource must not be accessed from any stage via this binding within any pipeline using the set layout.</summary>
		public int descriptorCount;

		/// <summary>Is a bitmask of VkShaderStageFlagBits specifying which pipeline shader stages can access a resource for this binding.
		/// VK_SHADER_STAGE_ALL is a shorthand specifying that all defined shader stages, including any additional stages defined by extensions, can access the resource.</summary>
		public VkShaderStageFlagBits stageFlags;

		/// <summary>Affects initialization of samplers.
		/// If descriptorType specifies a VK_DESCRIPTOR_TYPE_SAMPLER or VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER type descriptor, then pImmutableSamplers can be used to initialize a set of immutable samplers.
		/// Immutable samplers are permanently bound into the set layout; later binding a sampler into an immutable sampler slot in a descriptor set is not allowed.
		/// If pImmutableSamplers is not NULL, then it is considered to be a pointer to an array of sampler handles that will be consumed by the set layout and used for the corresponding binding.
		/// If pImmutableSamplers is NULL, then the sampler slots are dynamic and sampler handles must be bound into descriptor sets using this layout. If descriptorType is not one of these descriptor types, then pImmutableSamplers is ignored.</summary>
		public VkSampler[] pImmutableSamplers;
	}

	/// <summary>Structure specifying the allocation parameters for descriptor sets</summary>
	public struct VkDescriptorSetAllocateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is the pool which the sets will be allocated from.</summary>
		public VkDescriptorPool descriptorPool;

		/// <summary>Determines the number of descriptor sets to be allocated from the pool.</summary>
		public int descriptorSetCount;

		/// <summary>Is an array of descriptor set layouts, with each member specifying how the
		/// corresponding descriptor set is allocated.</summary>
		public VkDescriptorSetLayout[] pSetLayouts;
	}

	/// <summary>Structure specifying descriptor buffer info</summary>
	public struct VkDescriptorBufferInfo
	{
		/// <summary>Is the buffer resource.</summary>
		public VkBuffer buffer;

		/// <summary>Is the offset in bytes from the start of buffer. Access to buffer memory via
		/// this descriptor uses addressing that is relative to this starting offset.</summary>
		public int offset;

		/// <summary>Range is the size in bytes that is used for this descriptor update, or
		/// VK_WHOLE_SIZE to use the range from offset to the end of the buffer.</summary>
		public int range;
	}

	/// <summary>Structure specifying the parameters of a descriptor set write operation</summary>
	public struct VkWriteDescriptorSet
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>Is the destination descriptor set to update.</summary>
		public VkDescriptorSet dstSet;

		/// <summary>Is the descriptor binding within that set.</summary>
		public int dstBinding;

		/// <summary>Is the starting element in that array.</summary>
		public int dstArrayElement;

		/// <summary>Is the number of descriptors to update (the number of elements in
		/// pImageInfo, pBufferInfo, or pTexelBufferView).</summary>
		public int descriptorCount;

		/// <summary>Is a VkDescriptorType specifying the type of each descriptor in pImageInfo,
		/// pBufferInfo, or pTexelBufferView, as described below. It must be the same type as
		/// that specified in VkDescriptorSetLayoutBinding for dstSet at dstBinding. The type of
		/// the descriptor also controls which array the descriptors are taken from.</summary>
		public VkDescriptorType descriptorType;

		/// <summary>Points to an array of VkDescriptorImageInfo structures or is ignored, as
		/// described below.</summary>
		public VkDescriptorImageInfo[] pImageInfo;

		/// <summary>Points to an array of VkDescriptorBufferInfo structures or is ignored, as
		/// described below.</summary>
		public VkDescriptorBufferInfo[] pBufferInfo;

		/// <summary>Points to an array of VkBufferView handles as described in the Buffer Views
		/// section or is ignored, as described below.</summary>
		public VkBufferView[] pTexelBufferView;
	}

	/// <summary>Structure specifying a copy descriptor set operation.</summary>
	public struct VkCopyDescriptorSet
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>srcSet, srcBinding, and srcArrayElement are the source set, binding, and
		/// array element, respectively.</summary>
		public VkDescriptorSet srcSet;

		/// <summary>srcSet, srcBinding, and srcArrayElement are the source set, binding, and
		/// array element, respectively.</summary>
		public int srcBinding;

		/// <summary>srcSet, srcBinding, and srcArrayElement are the source set, binding, and
		/// array element, respectively.</summary>
		public int srcArrayElement;

		/// <summary>dstSet, dstBinding, and dstArrayElement are the destination set, binding,
		/// and array element, respectively.</summary>
		public VkDescriptorSet dstSet;

		/// <summary>dstSet, dstBinding, and dstArrayElement are the destination set, binding,
		/// and array element, respectively.</summary>
		public int dstBinding;

		/// <summary>dstSet, dstBinding, and dstArrayElement are the destination set, binding,
		/// and array element, respectively.</summary>
		public int dstArrayElement;

		/// <summary>Is the number of descriptors to copy from the source to destination. If
		/// descriptorCount is greater than the number of remaining array elements in the source
		/// or destination binding, those affect consecutive bindings in a manner similar to
		/// VkWriteDescriptorSet above.</summary>
		public int descriptorCount;
	}

	/// <summary>Structure specifying descriptor image info.</summary>
	public struct VkDescriptorImageInfo
	{
		/// <summary>Is a sampler handle, and is used in descriptor updates for types
		/// VK_DESCRIPTOR_TYPE_SAMPLER and VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER if the
		/// binding being updated does not use immutable samplers.</summary>
		public VkSampler sampler;

		/// <summary>Is an image view handle, and is used in descriptor updates for types
		/// VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE, VK_DESCRIPTOR_TYPE_STORAGE_IMAGE,
		/// VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, and VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT.
		/// </summary>
		public VkImageView imageView;

		/// <summary>Is the layout that the image subresources accessible from imageView will be
		/// in at the time this descriptor is accessed. imageLayout is used in descriptor updates
		/// for types VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE, VK_DESCRIPTOR_TYPE_STORAGE_IMAGE,
		/// VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, and VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT.
		/// </summary>
		public VkImageLayout imageLayout;
	}

	/// <summary>Specifies the type of a descriptor in a descriptor set (UNIFORM_BUFFER,
	/// SAMPLER, COMBINED_IMAGE_SAMPLER, INPUT_ATTACHMENT).</summary>
	public enum VkDescriptorType
	{
		/// <summary>
		/// A sampler represents a set of parameters which control address calculations,
		/// filtering behavior, and other properties, that can be used to perform filtered loads
		/// from sampled images (see Sampled Image).
		/// <para>GLSL example: layout (set=m, binding=n) uniform sampler mySampler;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_SAMPLER = 0,

		/// <summary>
		/// A combined image sampler represents a sampled image along with a set of sampling
		/// parameters. It is logically considered a sampled image and a sampler bound together.
		/// <para>GLSL example: layout (set=m, binding=n) uniform sampler2D myCombinedImageSampler;
		/// </para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER = 1,

		/// <summary>
		/// A sampled image can be used (usually in conjunction with a sampler) to retrieve sampled
		/// image data. Shaders use a sampled image handle and a sampler handle to sample data,
		/// where the image handle generally defines the shape and format of the memory and the
		/// sampler generally defines how coordinate addressing is performed. The same sampler can
		/// be used to sample from multiple images, and it is possible to sample from the same
		/// sampled image with multiple samplers, each containing a different set of sampling
		/// parameters.
		/// <para>GLSL example: layout (set=m, binding=n) uniform texture2D mySampledImage;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE = 2,

		/// <summary>
		/// A storage image is a descriptor type that is used for load, store, and atomic
		/// operations on image memory from within shaders bound to pipelines.
		/// <para>GLSL example: layout (set=m, binding=n, r32f) uniform image2D myStorageImage;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_STORAGE_IMAGE = 3,

		/// <summary>
		/// A uniform texel buffer represents a tightly packed array of homogeneous formatted
		/// data that is stored in a buffer and is made accessible to shaders. Uniform texel
		/// buffers are read-only.
		/// <para>GLSL example: layout (set=m, binding=n) uniform samplerBuffer myUniformTexelBuffer;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER = 4,

		/// <summary>
		/// A storage texel buffer represents a tightly packed array of homogeneous formatted data
		/// that is stored in a buffer and is made accessible to shaders. Storage texel buffers
		/// differ from uniform texel buffers in that they support stores and atomic operations in
		/// shaders, may support a different maximum length, and may have different performance
		/// characteristics.
		/// <para>GLSL example: layout (set=m, binding=n, r32f) uniform imageBuffer myStorageTexelBuffer;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_STORAGE_TEXEL_BUFFER = 5,

		/// <summary>
		/// A uniform buffer is a region of structured storage that is made accessible for read-only
		/// access to shaders. It is typically used to store medium sized arrays of constants such
		/// as shader parameters, matrices and other related data.
		/// <para>GLSL example: layout (set=m, binding=n) uniform myUniformBuffer { vec4
		/// myElement[32];	};
		/// </para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER = 6,

		/// <summary>
		/// A storage buffer is a region of structured storage that supports both read and write
		/// access for shaders. In addition to general read and write operations, some members of
		/// storage buffers can be used as the target of atomic operations. In general, atomic
		/// operations are only supported on members that have unsigned integer formats.
		/// <para>GLSL example: layout (set=m, binding=n) buffer myStorageBuffer { vec4 myElement[]; };
		/// </para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_STORAGE_BUFFER = 7,

		/// <summary>
		/// A dynamic uniform buffer differs from a uniform
		/// buffer only in how its address and length are specified. Uniform buffers bind a buffer
		/// address and length that is specified in the descriptor set update by a buffer handle,
		/// offset and range (see Descriptor Set Updates). With dynamic uniform buffers the buffer
		/// handle, offset and range specified in the descriptor set define the base address and
		/// length. The dynamic offset which is relative to this base address is taken from the
		/// pDynamicOffsets parameter to vkCmdBindDescriptorSets (see Descriptor Set Binding).
		/// The address used for a dynamic uniform buffer is the sum of the buffer base address
		/// and the relative offset. The length is unmodified and remains the range as specified
		/// in the descriptor update. The shader syntax is identical for uniform buffers and
		/// dynamic uniform buffers.
		/// </summary>
		VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC = 8,

		/// <summary>
		/// A dynamic storage buffer differs from a storage buffer only in how its address and
		/// length are specified. The difference is identical to the difference between uniform
		/// buffers and dynamic uniform buffers (see Dynamic Uniform Buffer). The shader syntax
		/// is identical for storage buffers and dynamic storage buffers.
		/// </summary>
		VK_DESCRIPTOR_TYPE_STORAGE_BUFFER_DYNAMIC = 9,

		/// <summary>
		/// An input attachment is an image view that can be used for pixel local load operations
		/// from within fragment shaders bound to pipelines. Loads from input attachments are
		/// unfiltered. All image formats that are supported for color attachments
		/// (VK_FORMAT_FEATURE_COLOR_ATTACHMENT_BIT) or depth/stencil attachments
		/// (VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT) for a given image tiling mode are
		/// also supported for input attachments.
		/// <para>GLSL example: layout (input_attachment_index=i, set=m, binding=n) uniform
		/// subpassInput myInputAttachment;</para>
		/// </summary>
		VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT = 10,
	}
}
