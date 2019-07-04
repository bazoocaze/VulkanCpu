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
	/// <summary>Structure specifying parameters of a newly created render pass.</summary>
	public struct VkRenderPassCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>*RESERVED*</summary>
		public int flags;

		/// <summary>Is the number of attachments used by this render pass, or zero indicating no
		/// attachments. Attachments are referred to by zero-based indices in the range
		/// [0,attachmentCount).</summary>
		public int attachmentCount;

		/// <summary>Points to an array of attachmentCount number of VkAttachmentDescription
		/// structures describing properties of the attachments, or NULL if attachmentCount is
		/// zero.</summary>
		public VkAttachmentDescription[] pAttachments;

		/// <summary>Is the number of subpasses to create for this render pass. Subpasses are
		/// referred to by zero-based indices in the range [0,subpassCount). A render pass must
		/// have at least one subpass.</summary>
		public int subpassCount;

		/// <summary>Points to an array of subpassCount number of VkSubpassDescription structures
		/// describing properties of the subpasses.</summary>
		public VkSubpassDescription[] pSubpasses;

		/// <summary>Is the number of dependencies between pairs of subpasses, or zero indicating
		/// no dependencies.</summary>
		public int dependencyCount;

		/// <summary>Points to an array of dependencyCount number of VkSubpassDependency structures
		/// describing dependencies between pairs of subpasses, or NULL if dependencyCount is
		/// zero.</summary>
		public VkSubpassDependency[] pDependencies;
	}

	/// <summary>Structure specifying an attachment description.</summary>
	public struct VkAttachmentDescription
	{
		/// <summary>Is a bitmask of VkAttachmentDescriptionFlagBits specifying additional
		/// properties of the attachment.</summary>
		public VkAttachmentDescriptionFlagBits flags;

		/// <summary>Is a VkFormat value specifying the format of the image that will be used for
		/// the attachment.</summary>
		public VkFormat format;

		/// <summary>Is the number of samples of the image as defined in VkSampleCountFlagBits.
		/// </summary>
		public VkSampleCountFlagBits samples;

		/// <summary>Is a VkAttachmentLoadOp value specifying how the contents of color and depth
		/// components of the attachment are treated at the beginning of the subpass where it is
		/// first used.</summary>
		public VkAttachmentLoadOp loadOp;

		/// <summary>Is a VkAttachmentStoreOp value specifying how the contents of color and depth
		/// components of the attachment are treated at the end of the subpass where it is last
		/// used.</summary>
		public VkAttachmentStoreOp storeOp;

		/// <summary>Is a VkAttachmentLoadOp value specifying how the contents of stencil
		/// components of the attachment are treated at the beginning of the subpass where it is
		/// first used.</summary>
		public VkAttachmentLoadOp stencilLoadOp;

		/// <summary>Is a VkAttachmentStoreOp value specifying how the contents of stencil
		/// components of the attachment are treated at the end of the last subpass where it is
		/// used.</summary>
		public VkAttachmentStoreOp stencilStoreOp;

		/// <summary>Is the layout the attachment image subresource will be in when a render pass
		/// instance begins.</summary>
		public VkImageLayout initialLayout;

		/// <summary>Is the layout the attachment image subresource will be transitioned to when
		/// a render pass instance ends. During a render pass instance, an attachment can use a
		/// different layout in each subpass, if desired.</summary>
		public VkImageLayout finalLayout;
	}

	/// <summary>Structure specifying an attachment reference.</summary>
	public struct VkAttachmentReference
	{
		/// <summary>Is the index of the attachment of the render pass, and corresponds to the
		/// index of the corresponding element in the pAttachments array of the
		/// VkRenderPassCreateInfo structure. If any color or depth/stencil attachments are
		/// VK_ATTACHMENT_UNUSED, then no writes occur for those attachments.</summary>
		public int attachment;

		/// <summary>Is a VkImageLayout value specifying the layout the attachment uses during
		/// the subpass.</summary>
		public VkImageLayout layout;
	}

	/// <summary>Structure specifying a subpass description.</summary>
	public struct VkSubpassDescription
	{
		/// <summary>Is a bitmask of VkSubpassDescriptionFlagBits specifying usage of the
		/// subpass.</summary>
		public int flags;

		/// <summary>Is a VkPipelineBindPoint value specifying whether this is a compute or
		/// graphics subpass. Currently, only graphics subpasses are supported.</summary>
		public VkPipelineBindPoint pipelineBindPoint;

		/// <summary>Is the number of input attachments.</summary>
		public int inputAttachmentCount;

		/// <summary>Is an array of VkAttachmentReference structures (defined below) that lists
		/// which of the render pass’s attachments can be read in the fragment shader stage
		/// during the subpass, and what layout each attachment will be in during the subpass.
		/// Each element of the array corresponds to an input attachment unit number in the
		/// shader, i.e. if the shader declares an input variable
		/// layout(input_attachment_index=X, set=Y, binding=Z) then it uses the attachment
		/// provided in pInputAttachments[X]. Input attachments must also be bound to the
		/// pipeline with a descriptor set, with the input attachment descriptor written in the
		/// location (set=Y, binding=Z). Fragment shaders can use subpass input variables to
		/// access the contents of an input attachment at the fragment’s (x, y, layer)
		/// framebuffer coordinates.</summary>
		public VkAttachmentReference[] pInputAttachments;

		/// <summary>Is the number of color attachments.</summary>
		public int colorAttachmentCount;

		/// <summary>Is an array of colorAttachmentCount VkAttachmentReference structures that
		/// lists which of the render pass’s attachments will be used as color attachments in
		/// the subpass, and what layout each attachment will be in during the subpass. Each
		/// element of the array corresponds to a fragment shader output location, i.e. if the
		/// shader declared an output variable layout(location=X) then it uses the attachment
		/// provided in pColorAttachments[X].</summary>
		public VkAttachmentReference[] pColorAttachments;

		/// <summary>Is NULL or an array of colorAttachmentCount VkAttachmentReference
		/// structures that lists which of the render pass’s attachments are resolved to at the
		/// end of the subpass, and what layout each attachment will be in during the multisample
		/// resolve operation. If pResolveAttachments is not NULL, each of its elements
		/// corresponds to a color attachment (the element in pColorAttachments at the same
		/// index), and a multisample resolve operation is defined for each attachment. At the
		/// end of each subpass, multisample resolve operations read the subpass’s color
		/// attachments, and resolve the samples for each pixel to the same pixel location in
		/// the corresponding resolve attachments, unless the resolve attachment index is
		/// VK_ATTACHMENT_UNUSED. If the first use of an attachment in a render pass is as a
		/// resolve attachment, then the loadOp is effectively ignored as the resolve is
		/// guaranteed to overwrite all pixels in the render area.</summary>
		public VkAttachmentReference[] pResolveAttachments;

		/// <summary>Is a pointer to a VkAttachmentReference specifying which attachment will
		/// be used for depth/stencil data and the layout it will be in during the subpass.
		/// Setting the attachment index to VK_ATTACHMENT_UNUSED or leaving this pointer as NULL
		/// indicates that no depth/stencil attachment will be used in the subpass.</summary>
		public VkAttachmentReference[] pDepthStencilAttachment;

		/// <summary>Is the number of preserved attachments.</summary>
		public int preserveAttachmentCount;

		/// <summary>Is an array of preserveAttachmentCount render pass attachment indices
		/// describing the attachments that are not used by a subpass, but whose contents must
		/// be preserved throughout the subpass.</summary>
		public int[] pPreserveAttachments;
	}

	/// <summary>Structure specifying a subpass dependency</summary>
	public struct VkSubpassDependency
	{
		/// <summary>srcSubpass is the subpass index of the first subpass in the dependency, or VK_SUBPASS_EXTERNAL.</summary>
		public int srcSubpass;

		/// <summary>dstSubpass is the subpass index of the second subpass in the dependency, or VK_SUBPASS_EXTERNAL.</summary>
		public int dstSubpass;

		/// <summary>srcStageMask is a bitmask of VkPipelineStageFlagBits specifying the source stage mask.</summary>
		public VkPipelineStageFlagBits srcStageMask;

		/// <summary>dstStageMask is a bitmask of VkPipelineStageFlagBits specifying the destination stage mask.</summary>
		public VkPipelineStageFlagBits dstStageMask;

		/// <summary>srcAccessMask is a bitmask of VkAccessFlagBits specifying a source access mask.</summary>
		public VkAccessFlagBits srcAccessMask;

		/// <summary>dstAccessMask is a bitmask of VkAccessFlagBits specifying a destination access mask.</summary>
		public VkAccessFlagBits dstAccessMask;

		/// <summary>dependencyFlags is a bitmask of VkDependencyFlagBits.</summary>
		public VkDependencyFlagBits dependencyFlags;
	}

	/// <summary>Structure specifying a clear value.</summary>
	public struct VkClearValue
	{
		/// <summary>Specifies the color image clear values to use when clearing a color image or
		/// attachment.</summary>
		public VkClearColorValue color;

		/// <summary>Specifies the depth and stencil clear values to use when clearing a
		/// depth/stencil image or attachment.</summary>
		public VkClearDepthStencilValue depthStencil;

		public static VkClearValue Create(float r, float g, float b, float a)
		{
			VkClearValue ret = new VkClearValue();
			ret.color = VkClearColorValue.Create(r, g, b, a);
			return ret;
		}

		public static VkClearValue Create(float depth, int stencil)
		{
			VkClearValue ret = new VkClearValue();
			ret.depthStencil = VkClearDepthStencilValue.Create(depth, stencil);
			return ret;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", color, depthStencil);
		}
	}

	/// <summary>Structure specifying a clear color value.</summary>
	public struct VkClearColorValue
	{
		/// <summary>Are the color clear values when the format of the image or attachment is one of
		/// the formats in the Interpretation of Numeric Format table other than signed integer (SINT)
		/// or unsigned integer (UINT). Floating point values are automatically converted to the
		/// format of the image, with the clear value being treated as linear if the image is sRGB.
		/// </summary>
		public float[] float32;

		/// <summary>Are the color clear values when the format of the image or attachment is signed
		/// integer (SINT). Signed integer values are converted to the format of the image by casting
		/// to the smaller type (with negative 32-bit values mapping to negative values in the smaller
		/// type). If the integer clear value is not representable in the target type (e.g. would
		/// overflow in conversion to that type), the clear value is undefined.</summary>
		public int[] int32;

		/// <summary>Are the color clear values when the format of the image or attachment is
		/// unsigned integer (UINT). Unsigned integer values are converted to the format of the image
		/// by casting to the integer type with fewer bits.</summary>
		public uint[] uint32;

		/// <summary>BBGGRRAA / 0xAARRGGBB</summary>
		public uint uint32_scalar;

		public static VkClearColorValue Create(float r, float g, float b, float a)
		{
			VkClearColorValue ret = new VkClearColorValue();

			SetFloat32(ref ret, r, g, b, a);
			SetInt32(ref ret, (int)(r * 255), (int)(g * 255), (int)(b * 255), (int)(a * 255));

			return ret;
		}

		private static void SetFloat32(ref VkClearColorValue ret, float r, float g, float b, float a)
		{
			ret.float32 = new float[4];
			ret.float32[0] = r;
			ret.float32[1] = g;
			ret.float32[2] = b;
			ret.float32[3] = a;
		}

		private static void SetInt32(ref VkClearColorValue ret, int r, int g, int b, int a)
		{
			ret.int32 = new int[4];
			ret.int32[0] = r;
			ret.int32[1] = g;
			ret.int32[2] = b;
			ret.int32[3] = a;

			ret.uint32 = new UInt32[4];
			ret.uint32[0] = (UInt32)r;
			ret.uint32[1] = (UInt32)g;
			ret.uint32[2] = (UInt32)b;
			ret.uint32[3] = (UInt32)a;

			ret.uint32_scalar = (uint)((a << 24) | (r << 16) | (g << 8) | b);
		}

		public override string ToString()
		{
			return string.Format("color=0x{0:X8}", uint32_scalar);
		}
	}

	/// <summary>Structure specifying a clear depth stencil value.</summary>
	public struct VkClearDepthStencilValue
	{
		/// <summary>Is the clear value for the depth aspect of the depth/stencil attachment. It is
		/// a floating-point value which is automatically converted to the attachment’s format.
		/// Depth must be between 0.0f and 1.0f, inclusive
		/// </summary>
		public float depth;

		/// <summary>Is the clear value for the stencil aspect of the depth/stencil attachment. It is
		/// a 32-bit integer value which is converted to the attachment’s format by taking the
		/// appropriate number of LSBs.</summary>
		public int stencil;

		public static VkClearDepthStencilValue Create(float depth, int stencil)
		{
			return new VkClearDepthStencilValue() { depth = depth, stencil = stencil };
		}

		public override string ToString()
		{
			return string.Format("depth={0} stencil={1}", depth, stencil);
		}
	}

	/// <summary>Layout of image and image subresources</summary>
	public enum VkImageLayout
	{
		/// <summary>Does not support device access. This layout must only be used as the initialLayout member of VkImageCreateInfo or VkAttachmentDescription, or as the oldLayout in an image transition. When transitioning out of this layout, the contents of the memory are not guaranteed to be preserved.</summary>
		VK_IMAGE_LAYOUT_UNDEFINED = 0,

		/// <summary>Supports all types of device access.</summary>
		VK_IMAGE_LAYOUT_GENERAL = 1,

		/// <summary>Must only be used as a color or resolve attachment in a VkFramebuffer. This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT usage bit enabled.</summary>
		VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL = 2,

		/// <summary>Must only be used as a depth/stencil attachment in a VkFramebuffer. This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT usage bit enabled.</summary>
		VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL = 3,

		/// <summary>Must only be used as a read-only depth/stencil attachment in a VkFramebuffer and/or as a read-only image in a shader (which can be read as a sampled image, combined image/sampler and/or input attachment). This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT usage bit enabled. Only image subresources of images created with VK_IMAGE_USAGE_SAMPLED_BIT can be used as a sampled image or combined image/sampler in a shader. Similarly, only image subresources of images created with VK_IMAGE_USAGE_INPUT_ATTACHMENT_BIT can be used as input attachments.</summary>
		VK_IMAGE_LAYOUT_DEPTH_STENCIL_READ_ONLY_OPTIMAL = 4,

		/// <summary>Must only be used as a read-only image in a shader (which can be read as a sampled image, combined image/sampler and/or input attachment). This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_SAMPLED_BIT or VK_IMAGE_USAGE_INPUT_ATTACHMENT_BIT usage bit enabled.</summary>
		VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL = 5,

		/// <summary>Must only be used as a source image of a transfer command (see the definition of VK_PIPELINE_STAGE_TRANSFER_BIT). This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_TRANSFER_SRC_BIT usage bit enabled.</summary>
		VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL = 6,

		/// <summary>Must only be used as a destination image of a transfer command. This layout is valid only for image subresources of images created with the VK_IMAGE_USAGE_TRANSFER_DST_BIT usage bit enabled.</summary>
		VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL = 7,

		/// <summary>Does not support device access. This layout must only be used as the initialLayout member of VkImageCreateInfo or VkAttachmentDescription, or as the oldLayout in an image transition. When transitioning out of this layout, the contents of the memory are preserved. This layout is intended to be used as the initial layout for an image whose contents are written by the host, and hence the data can be written to memory immediately, without first executing a layout transition. Currently, VK_IMAGE_LAYOUT_PREINITIALIZED is only useful with VK_IMAGE_TILING_LINEAR images because there is not a standard layout defined for VK_IMAGE_TILING_OPTIMAL images.</summary>
		VK_IMAGE_LAYOUT_PREINITIALIZED = 8,

		/// <summary>Must only be used for presenting a swapchain image for display. A swapchain’s image must be transitioned to this layout before calling vkQueuePresentKHR, and must be transitioned away from this layout after calling vkAcquireNextImageKHR.</summary>
		VK_IMAGE_LAYOUT_PRESENT_SRC_KHR = 1000001002,
	}

	/// <summary>Bitmask specifying additional properties of an attachment.</summary>
	[Flags]
	public enum VkAttachmentDescriptionFlagBits
	{
		/// <summary>specifies that the attachment aliases the same device memory as other
		/// attachments.</summary>
		VK_ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT = 0x00000001,
	}

	/// <summary>Specify how contents of an attachment are treated at the beginning of a
	/// subpass.</summary>
	public enum VkAttachmentLoadOp
	{
		/// <summary>Specifies that the previous contents of the image within the render area
		/// will be preserved. For attachments with a depth/stencil format, this uses the access
		/// type VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_READ_BIT. For attachments with a color format,
		/// this uses the access type VK_ACCESS_COLOR_ATTACHMENT_READ_BIT.</summary>
		VK_ATTACHMENT_LOAD_OP_LOAD = 0,

		/// <summary>Specifies that the contents within the render area will be cleared to a
		/// uniform value, which is specified when a render pass instance is begun. For
		/// attachments with a depth/stencil format, this uses the access type
		/// VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT. For attachments with a color format,
		/// this uses the access type VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT.</summary>
		VK_ATTACHMENT_LOAD_OP_CLEAR = 1,

		/// <summary>Specifies that the previous contents within the area need not be preserved;
		/// the contents of the attachment will be undefined inside the render area. For
		/// attachments with a depth/stencil format, this uses the access type
		/// VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT. For attachments with a color format,
		/// this uses the access type VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT.</summary>
		VK_ATTACHMENT_LOAD_OP_DONT_CARE = 2,
	}

	/// <summary>Specify how contents of an attachment are treated at the end of a
	/// subpass.</summary>
	public enum VkAttachmentStoreOp
	{
		/// <summary>Specifies the contents generated during the render pass and within the
		/// render area are written to memory. For attachments with a depth/stencil format,
		/// this uses the access type VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT. For
		/// attachments with a color format, this uses the access type 
		/// VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT.</summary>
		VK_ATTACHMENT_STORE_OP_STORE = 0,

		/// <summary>Specifies the contents within the render area are not needed after
		/// rendering, and may be discarded; the contents of the attachment will be undefined
		/// inside the render area. For attachments with a depth/stencil format, this uses the
		/// access type VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT. For attachments with a
		/// color format, this uses the access type VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT.
		/// </summary>
		VK_ATTACHMENT_STORE_OP_DONT_CARE = 1,
	}

	/// <summary>Specify the bind point of a pipeline object to a command buffer.</summary>
	public enum VkPipelineBindPoint
	{
		/// <summary>Specifies binding as a graphics pipeline.</summary>
		VK_PIPELINE_BIND_POINT_GRAPHICS = 0,

		/// <summary>Specifies binding as a compute pipeline.</summary>
		VK_PIPELINE_BIND_POINT_COMPUTE = 1,
	}

	/// <summary>Bitmask specifying pipeline stages.
	/// <para>Several of the synchronization commands include pipeline stage parameters, restricting
	/// the synchronization scopes for that command to just those stages. This allows fine grained
	/// control over the exact execution dependencies and accesses performed by action commands.
	/// Implementations should use these pipeline stages to avoid unnecessary stalls or cache
	/// flushing.</para></summary>
	[Flags]
	public enum VkPipelineStageFlagBits
	{
		/// <summary>Specifies the stage of the pipeline where any commands are initially received
		/// by the queue.</summary>
		VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT = 0x00000001,

		/// <summary>Specifies the stage of the pipeline where Draw/DispatchIndirect data structures
		/// are consumed.</summary>
		VK_PIPELINE_STAGE_DRAW_INDIRECT_BIT = 0x00000002,

		/// <summary>Specifies the stage of the pipeline where vertex and index buffers are consumed.
		/// </summary>
		VK_PIPELINE_STAGE_VERTEX_INPUT_BIT = 0x00000004,

		/// <summary>Specifies the vertex shader stage.</summary>
		VK_PIPELINE_STAGE_VERTEX_SHADER_BIT = 0x00000008,

		/// <summary>Specifies the tessellation control shader stage.</summary>
		VK_PIPELINE_STAGE_TESSELLATION_CONTROL_SHADER_BIT = 0x00000010,

		/// <summary>Specifies the tessellation evaluation shader stage.</summary>
		VK_PIPELINE_STAGE_TESSELLATION_EVALUATION_SHADER_BIT = 0x00000020,

		/// <summary>Specifies the geometry shader stage.</summary>
		VK_PIPELINE_STAGE_GEOMETRY_SHADER_BIT = 0x00000040,

		/// <summary>Specifies the fragment shader stage.</summary>
		VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT = 0x00000080,

		/// <summary>Specifies the stage of the pipeline where early fragment tests (depth and stencil
		/// tests before fragment shading) are performed. This stage also includes subpass load
		/// operations for framebuffer attachments with a depth/stencil format.</summary>
		VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT = 0x00000100,

		/// <summary>Specifies the stage of the pipeline where late fragment tests (depth and stencil
		/// tests after fragment shading) are performed. This stage also includes subpass store
		/// operations for framebuffer attachments with a depth/stencil format.</summary>
		VK_PIPELINE_STAGE_LATE_FRAGMENT_TESTS_BIT = 0x00000200,

		/// <summary>Specifies the stage of the pipeline after blending where the final color values
		/// are output from the pipeline. This stage also includes subpass load and store operations
		/// and multisample resolve operations for framebuffer attachments with a color format.
		/// </summary>
		VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT = 0x00000400,

		/// <summary>Specifies the execution of a compute shader.</summary>
		VK_PIPELINE_STAGE_COMPUTE_SHADER_BIT = 0x00000800,

		/// <summary>Specifies the execution of copy commands. This includes the operations resulting
		/// from all copy commands, clear commands (with the exception of vkCmdClearAttachments), and
		/// vkCmdCopyQueryPoolResults.</summary>
		VK_PIPELINE_STAGE_TRANSFER_BIT = 0x00001000,

		/// <summary>Specifies the final stage in the pipeline where operations generated by all
		/// commands complete execution.</summary>
		VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT = 0x00002000,

		/// <summary>Specifies a pseudo-stage indicating execution on the host of reads/writes of
		/// device memory. This stage is not invoked by any commands recorded in a command buffer.
		/// </summary>
		VK_PIPELINE_STAGE_HOST_BIT = 0x00004000,

		/// <summary>Specifies the execution of all graphics pipeline stages, and is equivalent to
		/// the logical OR of: VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT, VK_PIPELINE_STAGE_DRAW_INDIRECT_BIT,
		/// VK_PIPELINE_STAGE_VERTEX_INPUT_BIT, VK_PIPELINE_STAGE_VERTEX_SHADER_BIT,
		/// VK_PIPELINE_STAGE_TESSELLATION_CONTROL_SHADER_BIT,
		/// VK_PIPELINE_STAGE_TESSELLATION_EVALUATION_SHADER_BIT,
		/// VK_PIPELINE_STAGE_GEOMETRY_SHADER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT,
		/// VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT, VK_PIPELINE_STAGE_LATE_FRAGMENT_TESTS_BIT,
		/// VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT and VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT.
		/// </summary>
		VK_PIPELINE_STAGE_ALL_GRAPHICS_BIT = 0x00008000,

		/// <summary>Is equivalent to the logical OR of every other pipeline stage flag that is
		/// supported on the queue it is used with.</summary>
		VK_PIPELINE_STAGE_ALL_COMMANDS_BIT = 0x00010000,
	}

	/// <summary>Bitmask specifying memory access types that will participate in a memory
	/// dependency.
	/// <para>Memory in Vulkan can be accessed from within shader invocations and via some
	/// fixed-function stages of the pipeline.The access type is a function of the descriptor
	/// type used, or how a fixed-function stage accesses memory.Each access type corresponds
	/// to a bit flag in VkAccessFlagBits.</para>
	/// <para>Some synchronization commands take sets of access types as parameters to define the
	/// access scopes of a memory dependency.If a synchronization command includes a source access
	/// mask, its first access scope only includes accesses via the access types specified in that
	/// mask.Similarly, if a synchronization command includes a destination access mask, its second
	/// access scope only includes accesses via the access types specified in that mask.
	/// </para></summary>
	[Flags]
	public enum VkAccessFlagBits
	{
		/// <summary>Specifies read access to an indirect command structure read as part of an
		/// indirect drawing or dispatch command.</summary>
		VK_ACCESS_INDIRECT_COMMAND_READ_BIT = 0x00000001,

		/// <summary>Specifies read access to an index buffer as part of an indexed drawing command,
		/// bound by vkCmdBindIndexBuffer.</summary>
		VK_ACCESS_INDEX_READ_BIT = 0x00000002,

		/// <summary>Specifies read access to a vertex buffer as part of a drawing command, bound by
		/// vkCmdBindVertexBuffers.</summary>
		VK_ACCESS_VERTEX_ATTRIBUTE_READ_BIT = 0x00000004,

		/// <summary>Specifies read access to a uniform buffer.</summary>
		VK_ACCESS_UNIFORM_READ_BIT = 0x00000008,

		/// <summary>Specifies read access to an input attachment within a render pass during fragment
		/// shading.</summary>
		VK_ACCESS_INPUT_ATTACHMENT_READ_BIT = 0x00000010,

		/// <summary>Specifies read access to a storage buffer, uniform texel buffer, storage texel
		/// buffer, sampled image, or storage image.</summary>
		VK_ACCESS_SHADER_READ_BIT = 0x00000020,

		/// <summary>Specifies write access to a storage buffer, storage texel buffer, or storage
		/// image.</summary>
		VK_ACCESS_SHADER_WRITE_BIT = 0x00000040,

		/// <summary>Specifies read access to a color attachment, such as via blending, logic
		/// operations, or via certain subpass load operations.</summary>
		VK_ACCESS_COLOR_ATTACHMENT_READ_BIT = 0x00000080,

		/// <summary>Specifies write access to a color or resolve attachment during a render pass
		/// or via certain subpass load and store operations.</summary>
		VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT = 0x00000100,

		/// <summary>Specifies read access to a depth/stencil attachment, via depth or stencil
		/// operations or via certain subpass load operations.</summary>
		VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_READ_BIT = 0x00000200,

		/// <summary>Specifies write access to a depth/stencil attachment, via depth or stencil
		/// operations or via certain subpass load and store operations.</summary>
		VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 0x00000400,

		/// <summary>Specifies read access to an image or buffer in a copy operation.</summary>
		VK_ACCESS_TRANSFER_READ_BIT = 0x00000800,

		/// <summary>Specifies write access to an image or buffer in a clear or copy operation.
		/// </summary>
		VK_ACCESS_TRANSFER_WRITE_BIT = 0x00001000,

		/// <summary>Specifies read access by a host operation. Accesses of this type are not
		/// performed through a resource, but directly on memory.</summary>
		VK_ACCESS_HOST_READ_BIT = 0x00002000,

		/// <summary>Specifies write access by a host operation. Accesses of this type are not
		/// performed through a resource, but directly on memory.</summary>
		VK_ACCESS_HOST_WRITE_BIT = 0x00004000,

		/// <summary>Specifies read access via non-specific entities. These entities include the
		/// Vulkan device and host, but may also include entities external to the Vulkan device or
		/// otherwise not part of the core Vulkan pipeline. When included in a destination access
		/// mask, makes all available writes visible to all future read accesses on entities known
		/// to the Vulkan device.</summary>
		VK_ACCESS_MEMORY_READ_BIT = 0x00008000,

		/// <summary>Specifies write access via non-specific entities. These entities include the
		/// Vulkan device and host, but may also include entities external to the Vulkan device or
		/// otherwise not part of the core Vulkan pipeline. When included in a source access mask,
		/// all writes that are performed by entities known to the Vulkan device are made available.
		/// When included in a destination access mask, makes all available writes visible to all
		/// future write accesses on entities known to the Vulkan device.</summary>
		VK_ACCESS_MEMORY_WRITE_BIT = 0x00010000,
	}

	/// <summary>Bitmask specifying how execution and memory dependencies are formed</summary>
	public enum VkDependencyFlagBits
	{
		/// <summary>VK_DEPENDENCY_BY_REGION_BIT specifies that dependencies will be framebuffer-local.</summary>
		VK_DEPENDENCY_BY_REGION_BIT = 0x00000001,
	}

	/// <summary>Structure specifying render pass begin info.</summary>
	public struct VkRenderPassBeginInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>is the render pass to begin an instance of.</summary>
		public VkRenderPass renderPass;

		/// <summary>Is the framebuffer containing the attachments that are used with the render
		/// pass.</summary>
		public VkFramebuffer framebuffer;

		/// <summary>Is the render area that is affected by the render pass instance, and is
		/// described in more detail below.</summary>
		public VkRect2D renderArea;

		/// <summary>Is the number of elements in pClearValues.</summary>
		public int clearValueCount;

		/// <summary>Array of VkClearValue structures that contains clear values for each attachment,
		/// if the attachment uses a loadOp value of VK_ATTACHMENT_LOAD_OP_CLEAR or if the attachment
		/// has a depth/stencil format and uses a stencilLoadOp value of VK_ATTACHMENT_LOAD_OP_CLEAR.
		/// The array is indexed by attachment number. Only elements corresponding to cleared
		/// attachments are used. Other elements of pClearValues are ignored.</summary>
		public VkClearValue[] pClearValues;
	}
}
