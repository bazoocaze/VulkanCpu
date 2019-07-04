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
	/// <summary>Structure specifying physical device properties.</summary>
	public struct VkPhysicalDeviceProperties
	{
		/// <summary>Is the version of Vulkan supported by the device, encoded as described in the API Version Numbers and Semantics section.</summary>
		public uint apiVersion;

		/// <summary>Is the vendor-specified version of the driver.</summary>
		public uint driverVersion;

		/// <summary>Is a unique identifier for the vendor (see below) of the physical device.</summary>
		public uint vendorID;

		/// <summary>Is a unique identifier for the physical device among devices available from the vendor.</summary>
		public uint deviceID;

		/// <summary>Is a VkPhysicalDeviceType specifying the type of device.</summary>
		public VkPhysicalDeviceType deviceType;

		/// <summary>Is a null-terminated UTF-8 string containing the name of the device.</summary>
		public string deviceName;

		/// <summary>Is an array of size VK_UUID_SIZE, containing 8-bit values that represent a universally unique identifier for the device.</summary>
		public byte[] pipelineCacheUUID;

		/// <summary>Is the VkPhysicalDeviceLimits structure which specifies device-specific limits of the physical device. See Limits for details.</summary>
		public VkPhysicalDeviceLimits limits;

		/// <summary>Is the VkPhysicalDeviceSparseProperties structure which specifies various sparse related properties of the physical device. See Sparse Properties for details.</summary>
		public VkPhysicalDeviceSparseProperties sparseProperties;
	}

	/// <summary>Structure describing the fine-grained features that can be supported by an implementation.</summary>
	public struct VkPhysicalDeviceFeatures
	{
		public VkBool32 robustBufferAccess;
		public VkBool32 fullDrawIndexUint32;
		public VkBool32 imageCubeArray;
		public VkBool32 independentBlend;
		public VkBool32 geometryShader;
		public VkBool32 tessellationShader;
		public VkBool32 sampleRateShading;
		public VkBool32 dualSrcBlend;
		public VkBool32 logicOp;
		public VkBool32 multiDrawIndirect;
		public VkBool32 drawIndirectFirstInstance;
		public VkBool32 depthClamp;
		public VkBool32 depthBiasClamp;
		public VkBool32 fillModeNonSolid;
		public VkBool32 depthBounds;
		public VkBool32 wideLines;
		public VkBool32 largePoints;
		public VkBool32 alphaToOne;
		public VkBool32 multiViewport;
		public VkBool32 samplerAnisotropy;
		public VkBool32 textureCompressionETC2;
		public VkBool32 textureCompressionASTC_LDR;
		public VkBool32 textureCompressionBC;
		public VkBool32 occlusionQueryPrecise;
		public VkBool32 pipelineStatisticsQuery;
		public VkBool32 vertexPipelineStoresAndAtomics;
		public VkBool32 fragmentStoresAndAtomics;
		public VkBool32 shaderTessellationAndGeometryPointSize;
		public VkBool32 shaderImageGatherExtended;
		public VkBool32 shaderStorageImageExtendedFormats;
		public VkBool32 shaderStorageImageMultisample;
		public VkBool32 shaderStorageImageReadWithoutFormat;
		public VkBool32 shaderStorageImageWriteWithoutFormat;
		public VkBool32 shaderUniformBufferArrayDynamicIndexing;
		public VkBool32 shaderSampledImageArrayDynamicIndexing;
		public VkBool32 shaderStorageBufferArrayDynamicIndexing;
		public VkBool32 shaderStorageImageArrayDynamicIndexing;
		public VkBool32 shaderClipDistance;
		public VkBool32 shaderCullDistance;
		public VkBool32 shaderFloat64;
		public VkBool32 shaderInt64;
		public VkBool32 shaderInt16;
		public VkBool32 shaderResourceResidency;
		public VkBool32 shaderResourceMinLod;
		public VkBool32 sparseBinding;
		public VkBool32 sparseResidencyBuffer;
		public VkBool32 sparseResidencyImage2D;
		public VkBool32 sparseResidencyImage3D;
		public VkBool32 sparseResidency2Samples;
		public VkBool32 sparseResidency4Samples;
		public VkBool32 sparseResidency8Samples;
		public VkBool32 sparseResidency16Samples;
		public VkBool32 sparseResidencyAliased;
		public VkBool32 variableMultisampleRate;
		public VkBool32 inheritedQueries;
	}

	/// <summary>Supported physical device types</summary>
	public enum VkPhysicalDeviceType
	{
		/// <summary>The device does not match any other available types.</summary>
		VK_PHYSICAL_DEVICE_TYPE_OTHER = 0,

		/// <summary>The device is typically one embedded in or tightly coupled with the host.</summary>
		VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU = 1,

		/// <summary>The device is typically a separate processor connected to the host via an interlink.</summary>
		VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU = 2,

		/// <summary>The device is typically a virtual node in a virtualization environment.</summary>
		VK_PHYSICAL_DEVICE_TYPE_VIRTUAL_GPU = 3,

		/// <summary>The device is typically running on the same processors as the host.</summary>
		VK_PHYSICAL_DEVICE_TYPE_CPU = 4,
	}

	/// <summary>Structure reporting implementation-dependent physical device limits.</summary>
	public struct VkPhysicalDeviceLimits
	{
		public uint maxImageDimension1D;
		public uint maxImageDimension2D;
		public uint maxImageDimension3D;
		public uint maxImageDimensionCube;
		public uint maxImageArrayLayers;
		public uint maxTexelBufferElements;
		public uint maxUniformBufferRange;
		public uint maxStorageBufferRange;
		public uint maxPushConstantsSize;
		public uint maxMemoryAllocationCount;
		public uint maxSamplerAllocationCount;
		public uint bufferImageGranularity;
		public uint sparseAddressSpaceSize;
		public uint maxBoundDescriptorSets;
		public uint maxPerStageDescriptorSamplers;
		public uint maxPerStageDescriptorUniformBuffers;
		public uint maxPerStageDescriptorStorageBuffers;
		public uint maxPerStageDescriptorSampledImages;
		public uint maxPerStageDescriptorStorageImages;
		public uint maxPerStageDescriptorInputAttachments;
		public uint maxPerStageResources;
		public uint maxDescriptorSetSamplers;
		public uint maxDescriptorSetUniformBuffers;
		public uint maxDescriptorSetUniformBuffersDynamic;
		public uint maxDescriptorSetStorageBuffers;
		public uint maxDescriptorSetStorageBuffersDynamic;
		public uint maxDescriptorSetSampledImages;
		public uint maxDescriptorSetStorageImages;
		public uint maxDescriptorSetInputAttachments;
		public uint maxVertexInputAttributes;
		public uint maxVertexInputBindings;
		public uint maxVertexInputAttributeOffset;
		public uint maxVertexInputBindingStride;
		public uint maxVertexOutputComponents;
		public uint maxTessellationGenerationLevel;
		public uint maxTessellationPatchSize;
		public uint maxTessellationControlPerVertexInputComponents;
		public uint maxTessellationControlPerVertexOutputComponents;
		public uint maxTessellationControlPerPatchOutputComponents;
		public uint maxTessellationControlTotalOutputComponents;
		public uint maxTessellationEvaluationInputComponents;
		public uint maxTessellationEvaluationOutputComponents;
		public uint maxGeometryShaderInvocations;
		public uint maxGeometryInputComponents;
		public uint maxGeometryOutputComponents;
		public uint maxGeometryOutputVertices;
		public uint maxGeometryTotalOutputComponents;
		public uint maxFragmentInputComponents;
		public uint maxFragmentOutputAttachments;
		public uint maxFragmentDualSrcAttachments;
		public uint maxFragmentCombinedOutputResources;
		public uint maxComputeSharedMemorySize;
		public uint[] maxComputeWorkGroupCount;
		public uint maxComputeWorkGroupInvocations;
		public uint[] maxComputeWorkGroupSize;
		public uint subPixelPrecisionBits;
		public uint subTexelPrecisionBits;
		public uint mipmapPrecisionBits;
		public uint maxDrawIndexedIndexValue;
		public uint maxDrawIndirectCount;
		public float maxSamplerLodBias;
		public float maxSamplerAnisotropy;
		public uint maxViewports;
		public uint[] maxViewportDimensions;
		public float[] viewportBoundsRange;
		public uint viewportSubPixelBits;
		public uint minMemoryMapAlignment;
		public uint minTexelBufferOffsetAlignment;
		public uint minUniformBufferOffsetAlignment;
		public uint minStorageBufferOffsetAlignment;
		public int minTexelOffset;
		public uint maxTexelOffset;
		public int minTexelGatherOffset;
		public uint maxTexelGatherOffset;
		public float minInterpolationOffset;
		public float maxInterpolationOffset;
		public uint subPixelInterpolationOffsetBits;
		public uint maxFramebufferWidth;
		public uint maxFramebufferHeight;
		public uint maxFramebufferLayers;
		public VkSampleCountFlagBits framebufferColorSampleCounts;
		public VkSampleCountFlagBits framebufferDepthSampleCounts;
		public VkSampleCountFlagBits framebufferStencilSampleCounts;
		public VkSampleCountFlagBits framebufferNoAttachmentsSampleCounts;
		public uint maxColorAttachments;
		public VkSampleCountFlagBits sampledImageColorSampleCounts;
		public VkSampleCountFlagBits sampledImageIntegerSampleCounts;
		public VkSampleCountFlagBits sampledImageDepthSampleCounts;
		public VkSampleCountFlagBits sampledImageStencilSampleCounts;
		public VkSampleCountFlagBits storageImageSampleCounts;
		public uint maxSampleMaskWords;
		public VkBool32 timestampComputeAndGraphics;
		public float timestampPeriod;
		public uint maxClipDistances;
		public uint maxCullDistances;
		public uint maxCombinedClipAndCullDistances;
		public uint discreteQueuePriorities;
		public float[] pointSizeRange;
		public float[] lineWidthRange;
		public float pointSizeGranularity;
		public float lineWidthGranularity;
		public VkBool32 strictLines;
		public VkBool32 standardSampleLocations;
		public uint optimalBufferCopyOffsetAlignment;
		public uint optimalBufferCopyRowPitchAlignment;
		public uint nonCoherentAtomSize;
	}

	/// <summary>Structure specifying physical device sparse memory properties.</summary>
	public struct VkPhysicalDeviceSparseProperties
	{
		public VkBool32 residencyStandard2DBlockShape;
		public VkBool32 residencyStandard2DMultisampleBlockShape;
		public VkBool32 residencyStandard3DBlockShape;
		public VkBool32 residencyAlignedMipSize;
		public VkBool32 residencyNonResidentStrict;
	}
}
