using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using GlfwLib;
using GlmSharp;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;

namespace Examples.vulkan_tutorial.com.Ex06_DescriptorPoolAndSets
{
	/*
	 * Source: https://vulkan-tutorial.com/Uniform_buffers/Descriptor_pool_and_sets
	 * */

	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex
	{
		public vec2 pos;
		public vec3 color;

		public static VkVertexInputBindingDescription getBindingDescription()
		{
			VkVertexInputBindingDescription bindingDescription = new VkVertexInputBindingDescription();
			bindingDescription.binding = 0;
			bindingDescription.stride = Marshal.SizeOf(typeof(Vertex));
			bindingDescription.inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX;

			return bindingDescription;
		}

		public static VkVertexInputAttributeDescription[] getAttributeDescriptions()
		{
			VkVertexInputAttributeDescription[] attributeDescriptions = new VkVertexInputAttributeDescription[2];

			attributeDescriptions[0].binding = 0;
			attributeDescriptions[0].location = 0;
			attributeDescriptions[0].format = VkFormat.VK_FORMAT_R32G32_SFLOAT;
			attributeDescriptions[0].offset = (int)Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.pos));

			attributeDescriptions[1].binding = 0;
			attributeDescriptions[1].location = 1;
			attributeDescriptions[1].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
			attributeDescriptions[1].offset = (int)Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.color));

			return attributeDescriptions;
		}

		public static Vertex Create(float x, float y, float r, float g, float b)
		{
			Vertex ret = new Vertex();
			ret.pos = new vec2(x, y);
			ret.color = new vec3(r, g, b);
			return ret;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct UniformBufferObject
	{
		public mat4 model;
		public mat4 view;
		public mat4 proj;
	};

	public class QueueFamilyIndices
	{
		public int graphicsFamily = -1;
		public int presentFamily = -1;

		public bool isComplete()
		{
			return graphicsFamily >= 0 && presentFamily >= 0;
		}
	}

	public struct SwapChainSupportDetails
	{
		public VkSurfaceCapabilitiesKHR capabilities;
		public VkSurfaceFormatKHR[] formats;
		public VkPresentModeKHR[] presentModes;
	}

	public class HelloTriangleApplication06
	{
		public const int WIDTH = 800;
		public const int HEIGHT = 600;

		public static string[] validationLayers = new string[]
		{
			"VK_LAYER_LUNARG_standard_validation"
		};

		public static string[] deviceExtensions = new string[]
		{
			VkExtensionNames.VK_KHR_SWAPCHAIN_EXTENSION_NAME,
		};

#if !DEBUG
		public const bool enableValidationLayers = false;
#else
		public const bool enableValidationLayers = true;
#endif

		public Vertex[] vertices = new Vertex[]
		{
			Vertex.Create(-0.5f, -0.5f, 1.0f, 0.0f, 0.0f),
			Vertex.Create(0.5f, -0.5f, 0.0f, 1.0f, 0.0f),
			Vertex.Create(0.5f, 0.5f, 0.0f, 0.0f, 1.0f),
			Vertex.Create(-0.5f, 0.5f, 1.0f, 1.0f, 1.0f),
		};

		/*
		public Vertex[] vertices = new Vertex[]
		{
			Vertex.Create(0.0f, -0.5f, 1.0f, 1.0f, 1.0f),
			Vertex.Create(0.5f, 0.5f, 0.0f, 1.0f, 0.0f),
			Vertex.Create(-0.5f, 0.5f, 0.0f, 0.0f, 1.0f),
		}; */

		UInt16[] indices = new UInt16[] { 0, 1, 2, 2, 3, 0 };

		public void run()
		{
			initWindow();
			initVulkan();
			mainLoop();
			cleanup();
		}

		private GLFWwindow window;

		private VkInstance instance;
		private VkDebugReportCallbackEXT callback;
		private VkSurfaceKHR surface;

		private VkPhysicalDevice physicalDevice = null;
		private VkDevice device;

		private VkQueue graphicsQueue;
		private VkQueue presentQueue;

		private VkSwapchainKHR swapChain;
		private VkImage[] swapChainImages;
		private VkFormat swapChainImageFormat;
		private VkExtent2D swapChainExtent;
		private VkImageView[] swapChainImageViews;
		private VkFramebuffer[] swapChainFramebuffers;

		private VkRenderPass renderPass;
		private VkDescriptorSetLayout descriptorSetLayout;
		private VkPipelineLayout pipelineLayout;
		private VkPipeline graphicsPipeline;

		private VkCommandPool commandPool;

		private VkBuffer vertexBuffer;
		private VkDeviceMemory vertexBufferMemory;
		private VkBuffer indexBuffer;
		private VkDeviceMemory indexBufferMemory;

		private VkBuffer uniformBuffer;
		private VkDeviceMemory uniformBufferMemory;

		private VkDescriptorPool descriptorPool;
		private VkDescriptorSet descriptorSet;

		private VkCommandBuffer[] commandBuffers;

		private VkSemaphore imageAvailableSemaphore;
		private VkSemaphore renderFinishedSemaphore;

		private void initWindow()
		{
			GLFW.glfwInit();

			GLFW.glfwWindowHint(GLFWEnum.GLFW_CLIENT_API, GLFWEnum.GLFW_NO_API);
			GLFW.glfwWindowHint(GLFWEnum.GLFW_RESIZABLE, GLFWEnum.GLFW_FALSE);

			window = GLFW.glfwCreateWindow(WIDTH, HEIGHT, "Vulkan", null, null);

			GLFW.glfwSetWindowUserPointer(window, this);
			GLFW.glfwSetWindowSizeCallback(window, this.onWindowResized);
		}

		private void initVulkan()
		{
			createInstance();
			setupDebugCallback();
			createSurface();
			pickPhysicalDevice();
			createLogicalDevice();
			createSwapChain();
			createImageViews();
			createRenderPass();
			createDescriptorSetLayout();
			createGraphicsPipeline();
			createFramebuffers();
			createCommandPool();
			createVertexBuffer();
			createIndexBuffer();
			createUniformBuffer();
			createDescriptorPool();
			createDescriptorSet();
			createCommandBuffers();
			createSemaphores();
		}

		void mainLoop()
		{
			FpsCounter fpsCounter = new FpsCounter("mainLoop");
			FpsCounter fpsDrawFrame = new FpsCounter("drawFrame", reportCounters: FpsReportCounters.SimpleFrameTime);
			FpsControl fpsControl = new FpsControl();

			GLFW.EventLoop(window, () =>
			{
				fpsCounter.Begin();

				GLFW.glfwPollEvents();

				fpsDrawFrame.Begin();
				updateUniformBuffer();
				drawFrame();
				fpsDrawFrame.End();

				fpsCounter.End();

				fpsCounter.DebugPeriodicReport();
				fpsDrawFrame.DebugPeriodicReport();

				fpsControl.Update();
			});

			Vulkan.vkDeviceWaitIdle(device);
		}

		void cleanupSwapChain()
		{
			foreach (var framebuffer in swapChainFramebuffers)
			{
				Vulkan.vkDestroyFramebuffer(device, framebuffer, null);
			}

			Vulkan.vkFreeCommandBuffers(device, commandPool, commandBuffers.Length, commandBuffers);

			Vulkan.vkDestroyPipeline(device, graphicsPipeline, null);
			Vulkan.vkDestroyPipelineLayout(device, pipelineLayout, null);
			Vulkan.vkDestroyRenderPass(device, renderPass, null);

			foreach (var imageView in swapChainImageViews)
			{
				Vulkan.vkDestroyImageView(device, imageView, null);
			}

			Vulkan.vkDestroySwapchainKHR(device, swapChain, null);
		}

		void cleanup()
		{
			cleanupSwapChain();

			Vulkan.vkDestroyDescriptorPool(device, descriptorPool, null);

			Vulkan.vkDestroyDescriptorSetLayout(device, descriptorSetLayout, null);
			Vulkan.vkDestroyBuffer(device, uniformBuffer, null);
			Vulkan.vkFreeMemory(device, uniformBufferMemory, null);

			Vulkan.vkDestroyBuffer(device, vertexBuffer, null);
			Vulkan.vkFreeMemory(device, vertexBufferMemory, null);

			Vulkan.vkDestroySemaphore(device, renderFinishedSemaphore, null);
			Vulkan.vkDestroySemaphore(device, imageAvailableSemaphore, null);

			Vulkan.vkDestroyCommandPool(device, commandPool, null);

			Vulkan.vkDestroyDevice(device, null);
			Vulkan.vkDestroyDebugReportCallbackEXT(instance, callback, null);
			Vulkan.vkDestroySurfaceKHR(instance, surface, null);
			Vulkan.vkDestroyInstance(instance, null);

			GLFW.glfwDestroyWindow(window);

			GLFW.glfwTerminate();
		}

		private void onWindowResized(GLFWwindow window, int width, int height)
		{
			var app = (HelloTriangleApplication06)GLFW.glfwGetWindowUserPointer(window);
			app.recreateSwapChain();
		}

		void recreateSwapChain()
		{
			int width, height;
			GLFW.glfwGetWindowSize(window, out width, out height);

			if (width == 0 || height == 0)
				return;

			Vulkan.vkDeviceWaitIdle(device);

			cleanupSwapChain();

			createSwapChain();
			createImageViews();
			createRenderPass();
			createGraphicsPipeline();
			createFramebuffers();
			createCommandBuffers();
		}

		private void createInstance()
		{
			if (enableValidationLayers && !checkValidationLayerSupport())
			{
				throw Program.Throw("validation layers requested, but not available");
			}

			VkApplicationInfo appInfo = new VkApplicationInfo();
			appInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO;
			appInfo.pApplicationName = "Hello Triangle";
			appInfo.applicationVersion = Vulkan.VK_MAKE_VERSION(1, 0, 0);
			appInfo.pEngineName = "No Engine";
			appInfo.engineVersion = Vulkan.VK_MAKE_VERSION(1, 0, 0);
			appInfo.apiVersion = VkApiVersion.VK_API_VERSION_1_0;

			VkInstanceCreateInfo createInfo = new VkInstanceCreateInfo();
			createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
			createInfo.pApplicationInfo = appInfo;

			List<string> extensions = getRequiredExtensions();
			createInfo.enabledExtensionCount = extensions.Count;
			createInfo.ppEnabledExtensionNames = extensions.ToArray();

			if (enableValidationLayers)
			{
				createInfo.enabledLayerCount = validationLayers.Length;
				createInfo.ppEnabledLayerNames = validationLayers;
			}
			else
			{
				createInfo.enabledLayerCount = 0;
			}

			VkResult result = Vulkan.vkCreateInstance(createInfo, null, out instance);
			if (result != VkResult.VK_SUCCESS)
				throw Program.Throw("failed to create instance!", result);
		}

		private void setupDebugCallback()
		{
			if (!enableValidationLayers)
				return;

			VkDebugReportCallbackCreateInfoEXT createInfo = new VkDebugReportCallbackCreateInfoEXT();
			createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_REPORT_CALLBACK_CREATE_INFO_EXT;
			createInfo.flags = VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_ERROR_BIT_EXT | VkDebugReportFlagBitsEXT.VK_DEBUG_REPORT_WARNING_BIT_EXT;
			createInfo.pfnCallback = debugCallback;

			if (Vulkan.vkCreateDebugReportCallbackEXT(instance, createInfo, null, out callback) != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to set up debug callback!");
			}
		}

		private void createSurface()
		{
			VkResult result = GLFW.glfwCreateWindowSurface(instance, window, null, out surface);
			if (result != VkResult.VK_SUCCESS)
				throw Program.Throw("failed to create window surface!", result);
		}

		private void pickPhysicalDevice()
		{
			int deviceCount = 0;
			Vulkan.vkEnumeratePhysicalDevices(instance, ref deviceCount, null);

			if (deviceCount == 0)
				throw Program.Throw("failed to find GPUs with Vulkan support!");

			VkPhysicalDevice[] devices = new VkPhysicalDevice[deviceCount];
			Vulkan.vkEnumeratePhysicalDevices(instance, ref deviceCount, devices);

			physicalDevice = null;

			foreach (var device in devices)
			{
				if (isDeviceSuitable(device))
				{
					physicalDevice = device;
					break;
				}
			}

			if (physicalDevice == null)
				throw Program.Throw("failed to find a suitable GPU!");
		}

		private void createLogicalDevice()
		{
			QueueFamilyIndices indices = findQueueFamilies(physicalDevice);

			List<VkDeviceQueueCreateInfo> queueCreateInfos = new List<VkDeviceQueueCreateInfo>();

			HashSet<int> uniqueQueueFamilies = new HashSet<int>() { indices.graphicsFamily, indices.presentFamily };

			float[] queuePriorities = new float[] { 1.0f };

			foreach (var queueFamily in uniqueQueueFamilies)
			{
				VkDeviceQueueCreateInfo queueCreateInfo = new VkDeviceQueueCreateInfo();
				queueCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO;
				queueCreateInfo.queueFamilyIndex = queueFamily;
				queueCreateInfo.queueCount = 1;
				queueCreateInfo.pQueuePriorities = queuePriorities;

				queueCreateInfos.Add(queueCreateInfo);
			}

			VkPhysicalDeviceFeatures deviceFeatures = new VkPhysicalDeviceFeatures();

			VkDeviceCreateInfo createInfo = new VkDeviceCreateInfo();
			createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;

			createInfo.queueCreateInfoCount = queueCreateInfos.Count;
			createInfo.pQueueCreateInfos = queueCreateInfos.ToArray();

			createInfo.pEnabledFeatures = new VkPhysicalDeviceFeatures[] { deviceFeatures };

			createInfo.enabledExtensionCount = deviceExtensions.Length;
			createInfo.ppEnabledExtensionNames = deviceExtensions;

			if (enableValidationLayers)
			{
				createInfo.enabledLayerCount = validationLayers.Length;
				createInfo.ppEnabledLayerNames = validationLayers;
			}
			else
			{
				createInfo.enabledLayerCount = 0;
			}

			VkResult result = Vulkan.vkCreateDevice(physicalDevice, createInfo, null, out device);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create logical device!", result);
			}

			Vulkan.vkGetDeviceQueue(device, indices.graphicsFamily, 0, out graphicsQueue);
			Vulkan.vkGetDeviceQueue(device, indices.presentFamily, 0, out presentQueue);
		}

		private void createSwapChain()
		{
			SwapChainSupportDetails swapChainSupport = querySwapChainSupport(physicalDevice);

			VkSurfaceFormatKHR surfaceFormat = chooseSwapSurfaceFormat(swapChainSupport.formats);
			VkPresentModeKHR presentMode = chooseSwapPresentMode(swapChainSupport.presentModes);
			VkExtent2D extent = chooseSwapExtent(swapChainSupport.capabilities);

			int imageCount = swapChainSupport.capabilities.minImageCount + 1;
			if (swapChainSupport.capabilities.maxImageCount > 0 && imageCount > swapChainSupport.capabilities.maxImageCount)
			{
				imageCount = swapChainSupport.capabilities.maxImageCount;
			}

			VkSwapchainCreateInfoKHR createInfo = new VkSwapchainCreateInfoKHR();
			createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
			createInfo.surface = surface;

			createInfo.minImageCount = imageCount;
			createInfo.imageFormat = surfaceFormat.format;
			createInfo.imageColorSpace = surfaceFormat.colorSpace;
			createInfo.imageExtent = extent;
			createInfo.imageArrayLayers = 1;
			createInfo.imageUsage = VkImageUsageFlags.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;

			QueueFamilyIndices indices = findQueueFamilies(physicalDevice);
			var queueFamilyIndices = new List<int>() { indices.graphicsFamily, indices.presentFamily };

			if (indices.graphicsFamily != indices.presentFamily)
			{
				createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
				createInfo.queueFamilyIndexCount = queueFamilyIndices.Count;
				createInfo.pQueueFamilyIndices = queueFamilyIndices.ToArray();
			}
			else
			{
				createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
			}

			createInfo.preTransform = (VkSurfaceTransformFlagBitsKHR)swapChainSupport.capabilities.currentTransform;
			createInfo.compositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
			createInfo.presentMode = presentMode;
			createInfo.clipped = VkBool32.VK_TRUE;

			createInfo.oldSwapchain = null;

			VkResult result = Vulkan.vkCreateSwapchainKHR(device, createInfo, null, out swapChain);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create swap chain!", result);
			}

			swapChainImages = new VkImage[imageCount];
			Vulkan.vkGetSwapchainImagesKHR(device, swapChain, ref imageCount, swapChainImages);

			swapChainImageFormat = surfaceFormat.format;
			swapChainExtent = extent;
		}

		private void createImageViews()
		{
			swapChainImageViews = new VkImageView[swapChainImages.Length];

			for (int i = 0; i < swapChainImages.Length; i++)
			{
				VkImageViewCreateInfo createInfo = new VkImageViewCreateInfo();
				createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
				createInfo.image = swapChainImages[i];
				createInfo.viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;
				createInfo.format = swapChainImageFormat;
				createInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
				createInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
				createInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
				createInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
				createInfo.subresourceRange.aspectMask = VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
				createInfo.subresourceRange.baseMipLevel = 0;
				createInfo.subresourceRange.levelCount = 1;
				createInfo.subresourceRange.baseArrayLayer = 0;
				createInfo.subresourceRange.layerCount = 1;

				VkImageView imageView = null;
				VkResult result = Vulkan.vkCreateImageView(device, createInfo, default(VkAllocationCallbacks), out imageView);
				if (result != VkResult.VK_SUCCESS)
				{
					throw Program.Throw("failed to create image views!", result);
				}
				swapChainImageViews[i] = imageView;
			}
		}

		public void createRenderPass()
		{
			VkAttachmentDescription colorAttachment = new VkAttachmentDescription();
			colorAttachment.format = swapChainImageFormat;
			colorAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
			colorAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
			colorAttachment.storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE;
			colorAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
			colorAttachment.stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE;
			colorAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
			colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;

			VkAttachmentReference colorAttachmentRef = new VkAttachmentReference();
			colorAttachmentRef.attachment = 0;
			colorAttachmentRef.layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;

			VkSubpassDescription subpass = new VkSubpassDescription();
			subpass.pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS;
			subpass.colorAttachmentCount = 1;
			subpass.pColorAttachments = new VkAttachmentReference[] { colorAttachmentRef };

			VkSubpassDependency dependency = new VkSubpassDependency();
			dependency.srcSubpass = Vulkan.VK_SUBPASS_EXTERNAL;
			dependency.dstSubpass = 0;
			dependency.srcStageMask = VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;
			dependency.srcAccessMask = 0;
			dependency.dstStageMask = VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;
			dependency.dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_READ_BIT | VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT;

			VkRenderPassCreateInfo renderPassInfo = new VkRenderPassCreateInfo();
			renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO;
			renderPassInfo.attachmentCount = 1;
			renderPassInfo.pAttachments = new VkAttachmentDescription[] { colorAttachment };
			renderPassInfo.subpassCount = 1;
			renderPassInfo.pSubpasses = new VkSubpassDescription[] { subpass };
			renderPassInfo.dependencyCount = 1;
			renderPassInfo.pDependencies = new VkSubpassDependency[] { dependency };

			VkResult result = Vulkan.vkCreateRenderPass(device, renderPassInfo, null, out renderPass);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create render pass!", result);
			}
		}

		private void createDescriptorSetLayout()
		{
			VkDescriptorSetLayoutBinding uboLayoutBinding = new VkDescriptorSetLayoutBinding();
			uboLayoutBinding.binding = 0;
			uboLayoutBinding.descriptorCount = 1;
			uboLayoutBinding.descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
			uboLayoutBinding.pImmutableSamplers = null;
			uboLayoutBinding.stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;

			VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo();
			layoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO;
			layoutInfo.bindingCount = 1;
			layoutInfo.pBindings = new VkDescriptorSetLayoutBinding[] { uboLayoutBinding };

			VkResult result = Vulkan.vkCreateDescriptorSetLayout(device, layoutInfo, null, out descriptorSetLayout);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create descriptor set layout!", result);
			}
		}

		private void createGraphicsPipeline()
		{
			var vertShaderModule = createShaderModule(typeof(Shader_Vert_06));
			var fragShaderModule = createShaderModule(typeof(Shader_Frag_06));

			VkPipelineShaderStageCreateInfo vertShaderStageInfo = new VkPipelineShaderStageCreateInfo();
			vertShaderStageInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
			vertShaderStageInfo.stage = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
			vertShaderStageInfo.module = vertShaderModule;
			vertShaderStageInfo.pName = "main";

			VkPipelineShaderStageCreateInfo fragShaderStageInfo = new VkPipelineShaderStageCreateInfo();
			fragShaderStageInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
			fragShaderStageInfo.stage = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;
			fragShaderStageInfo.module = fragShaderModule;
			fragShaderStageInfo.pName = "main";

			VkPipelineShaderStageCreateInfo[] shaderStages = new VkPipelineShaderStageCreateInfo[2] { vertShaderStageInfo, fragShaderStageInfo };

			VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo();
			vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;

			var bindingDescription = Vertex.getBindingDescription();
			var attributeDescriptions = Vertex.getAttributeDescriptions();

			vertexInputInfo.vertexBindingDescriptionCount = 1;
			vertexInputInfo.vertexAttributeDescriptionCount = attributeDescriptions.Length;
			vertexInputInfo.pVertexBindingDescriptions = new VkVertexInputBindingDescription[] { bindingDescription };
			vertexInputInfo.pVertexAttributeDescriptions = attributeDescriptions;

			VkPipelineInputAssemblyStateCreateInfo inputAssembly = new VkPipelineInputAssemblyStateCreateInfo();
			inputAssembly.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
			inputAssembly.topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
			inputAssembly.primitiveRestartEnable = VkBool32.VK_FALSE;

			VkViewport viewport = new VkViewport();
			viewport.x = 0.0f;
			viewport.y = 0.0f;
			viewport.width = (float)swapChainExtent.width;
			viewport.height = (float)swapChainExtent.height;
			viewport.minDepth = 0.0f;
			viewport.maxDepth = 1.0f;

			VkRect2D scissor = new VkRect2D();
			scissor.offset = new VkOffset2D() { x = 0, y = 0 };
			scissor.extent = swapChainExtent;

			VkPipelineViewportStateCreateInfo viewportState = new VkPipelineViewportStateCreateInfo();
			viewportState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO;
			viewportState.viewportCount = 1;
			viewportState.pViewports = new VkViewport[] { viewport };
			viewportState.scissorCount = 1;
			viewportState.pSicssors = new VkRect2D[] { scissor };

			VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo();
			rasterizer.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO;
			rasterizer.depthClampEnable = VkBool32.VK_FALSE;
			rasterizer.rasterizerDiscardEnable = VkBool32.VK_FALSE;
			rasterizer.polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL;
			rasterizer.lineWidth = 1.0f;
			rasterizer.cullMode = VkCullModeFlagBits.VK_CULL_MODE_BACK_BIT;
			rasterizer.frontFace = VkFrontFace.VK_FRONT_FACE_CLOCKWISE;
			rasterizer.depthBiasEnable = VkBool32.VK_FALSE;

			VkPipelineMultisampleStateCreateInfo multisampling = new VkPipelineMultisampleStateCreateInfo();
			multisampling.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
			multisampling.sampleShadingEnable = VkBool32.VK_FALSE;
			multisampling.rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;

			VkPipelineColorBlendAttachmentState colorBlendAttachment = new VkPipelineColorBlendAttachmentState();
			colorBlendAttachment.colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT;
			colorBlendAttachment.blendEnable = VkBool32.VK_FALSE;

			VkPipelineColorBlendStateCreateInfo colorBlending = VkPipelineColorBlendStateCreateInfo.Create();
			colorBlending.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO;
			colorBlending.logicOpEnable = VkBool32.VK_FALSE;
			colorBlending.logicOp = VkLogicOp.VK_LOGIC_OP_COPY;
			colorBlending.attachmentCount = 1;
			colorBlending.pAttachments = new VkPipelineColorBlendAttachmentState[] { colorBlendAttachment };
			colorBlending.blendConstants[0] = 0.0f;
			colorBlending.blendConstants[1] = 0.0f;
			colorBlending.blendConstants[2] = 0.0f;
			colorBlending.blendConstants[3] = 0.0f;

			VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo();
			pipelineLayoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
			pipelineLayoutInfo.setLayoutCount = 0;
			pipelineLayoutInfo.pushConstantRangeCount = 0;

			VkResult result = Vulkan.vkCreatePipelineLayout(device, pipelineLayoutInfo, null, out pipelineLayout);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create pipeline layout!");
			}

			VkGraphicsPipelineCreateInfo pipelineInfo = new VkGraphicsPipelineCreateInfo();
			pipelineInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO;
			pipelineInfo.stageCount = shaderStages.Length;
			pipelineInfo.pStages = shaderStages;
			pipelineInfo.pVertexInputState = vertexInputInfo;
			pipelineInfo.pInputAssemblyState = inputAssembly;
			pipelineInfo.pViewportState = viewportState;
			pipelineInfo.pRasterizationState = rasterizer;
			pipelineInfo.pMultisampleState = multisampling;
			pipelineInfo.pColorBlendState = colorBlending;
			pipelineInfo.layout = pipelineLayout;
			pipelineInfo.renderPass = renderPass;
			pipelineInfo.subpass = 0;
			pipelineInfo.basePipelineHandle = null;

			VkPipeline[] pipelineResult = new VkPipeline[1];
			result = Vulkan.vkCreateGraphicsPipelines(device, null, 1, new VkGraphicsPipelineCreateInfo[] { pipelineInfo }, null, pipelineResult);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create graphics pipeline!", result);
			}

			graphicsPipeline = pipelineResult[0];

			Vulkan.vkDestroyShaderModule(device, fragShaderModule, null);
			Vulkan.vkDestroyShaderModule(device, vertShaderModule, null);
		}

		private void createFramebuffers()
		{
			swapChainFramebuffers = new VkFramebuffer[swapChainImageViews.Length];

			for (int i = 0; i < swapChainImageViews.Length; i++)
			{
				VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo();
				framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
				framebufferInfo.renderPass = renderPass;
				framebufferInfo.attachmentCount = 1;
				framebufferInfo.pAttachments = new VkImageView[] { swapChainImageViews[i] };
				framebufferInfo.width = swapChainExtent.width;
				framebufferInfo.height = swapChainExtent.height;
				framebufferInfo.layers = 1;

				VkFramebuffer frameBuffer = null;
				VkResult result = Vulkan.vkCreateFramebuffer(device, framebufferInfo, null, out frameBuffer);
				if (result != VkResult.VK_SUCCESS)
				{
					throw Program.Throw("failed to create framebuffer!", result);
				}
				swapChainFramebuffers[i] = frameBuffer;
			}
		}

		private void createCommandPool()
		{
			QueueFamilyIndices queueFamilyIndices = findQueueFamilies(physicalDevice);

			VkCommandPoolCreateInfo poolInfo = new VkCommandPoolCreateInfo();
			poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
			poolInfo.queueFamilyIndex = queueFamilyIndices.graphicsFamily;

			VkResult result = Vulkan.vkCreateCommandPool(device, poolInfo, null, out commandPool);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create command pool!", result);
			}
		}

		private void createVertexBuffer()
		{
			int bufferSize = Marshal.SizeOf(vertices[0]) * vertices.Length;

			VkBuffer stagingBuffer;
			VkDeviceMemory stagingBufferMemory;
			createBuffer(bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, out stagingBuffer, out stagingBufferMemory);

			byte[] data;
			Vulkan.vkMapMemory(device, stagingBufferMemory, 0, bufferSize, 0, out data);
			MemoryCopyHelper.Copy(vertices, data, 0, bufferSize);
			Vulkan.vkUnmapMemory(device, stagingBufferMemory);

			createBuffer(bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT | VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, out vertexBuffer, out vertexBufferMemory);
			copyBuffer(stagingBuffer, vertexBuffer, bufferSize);

			Vulkan.vkDestroyBuffer(device, stagingBuffer, null);
			Vulkan.vkFreeMemory(device, stagingBufferMemory, null);
		}

		private void createIndexBuffer()
		{
			int bufferSize = Marshal.SizeOf(indices[0]) * indices.Length;

			VkBuffer stagingBuffer;
			VkDeviceMemory stagingBufferMemory;
			createBuffer(bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, out stagingBuffer, out stagingBufferMemory);

			byte[] data;
			Vulkan.vkMapMemory(device, stagingBufferMemory, 0, bufferSize, 0, out data);
			MemoryCopyHelper.Copy(indices, data, 0, bufferSize);
			Vulkan.vkUnmapMemory(device, stagingBufferMemory);

			createBuffer(bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT | VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, out indexBuffer, out indexBufferMemory);

			copyBuffer(stagingBuffer, indexBuffer, bufferSize);

			Vulkan.vkDestroyBuffer(device, stagingBuffer, null);
			Vulkan.vkFreeMemory(device, stagingBufferMemory, null);
		}

		private void createUniformBuffer()
		{
			int bufferSize = Marshal.SizeOf(typeof(UniformBufferObject));
			createBuffer(bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, out uniformBuffer, out uniformBufferMemory);
		}

		private void createDescriptorPool()
		{
			VkDescriptorPoolSize poolSize = new VkDescriptorPoolSize();
			poolSize.type = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
			poolSize.descriptorCount = 1;

			VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo();
			poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO;
			poolInfo.poolSizeCount = 1;
			poolInfo.pPoolSizes = new VkDescriptorPoolSize[] { poolSize };
			poolInfo.maxSets = 1;

			VkResult result = Vulkan.vkCreateDescriptorPool(device, poolInfo, null, out descriptorPool);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create descriptor pool!", result);
			}
		}

		private void createDescriptorSet()
		{
			VkDescriptorSetLayout[] layouts = new VkDescriptorSetLayout[] { descriptorSetLayout };
			VkDescriptorSetAllocateInfo allocInfo = new VkDescriptorSetAllocateInfo();
			allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO;
			allocInfo.descriptorPool = descriptorPool;
			allocInfo.descriptorSetCount = 1;
			allocInfo.pSetLayouts = layouts;

			VkDescriptorSet[] listDescriptoorSet = new VkDescriptorSet[1];
			VkResult result = Vulkan.vkAllocateDescriptorSets(device, allocInfo, listDescriptoorSet);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to allocate descriptor set!", result);
			}
			descriptorSet = listDescriptoorSet[0];

			VkDescriptorBufferInfo bufferInfo = new VkDescriptorBufferInfo();
			bufferInfo.buffer = uniformBuffer;
			bufferInfo.offset = 0;
			bufferInfo.range = Marshal.SizeOf(typeof(UniformBufferObject));

			VkWriteDescriptorSet descriptorWrite = new VkWriteDescriptorSet();
			descriptorWrite.sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
			descriptorWrite.dstSet = descriptorSet;
			descriptorWrite.dstBinding = 0;
			descriptorWrite.dstArrayElement = 0;
			descriptorWrite.descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
			descriptorWrite.descriptorCount = 1;
			descriptorWrite.pBufferInfo = new VkDescriptorBufferInfo[] { bufferInfo };

			Vulkan.vkUpdateDescriptorSets(device, 1, new VkWriteDescriptorSet[] { descriptorWrite }, 0, null);
		}

		private void createBuffer(int size, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, out VkBuffer buffer, out VkDeviceMemory bufferMemory)
		{
			VkBufferCreateInfo bufferInfo = new VkBufferCreateInfo();
			bufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
			bufferInfo.size = size;
			bufferInfo.usage = usage;
			bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

			VkResult result = Vulkan.vkCreateBuffer(device, bufferInfo, null, out buffer);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create buffer!", result);
			}

			VkMemoryRequirements memRequirements;
			Vulkan.vkGetBufferMemoryRequirements(device, buffer, out memRequirements);

			VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo();
			allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
			allocInfo.allocationSize = memRequirements.size;
			allocInfo.memoryTypeIndex = findMemoryType(memRequirements.memoryTypeBits, properties);

			result = Vulkan.vkAllocateMemory(device, allocInfo, null, out bufferMemory);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to allocate buffer memory!", result);
			}

			Vulkan.vkBindBufferMemory(device, buffer, bufferMemory, 0);
		}

		void copyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, int size)
		{
			VkCommandBufferAllocateInfo allocInfo = new VkCommandBufferAllocateInfo();
			allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
			allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
			allocInfo.commandPool = commandPool;
			allocInfo.commandBufferCount = 1;

			VkCommandBuffer[] commandBuffers = new VkCommandBuffer[1];
			VkCommandBuffer commandBuffer;
			Vulkan.vkAllocateCommandBuffers(device, allocInfo, commandBuffers);
			commandBuffer = commandBuffers[0];

			VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo();
			beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
			beginInfo.flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

			Vulkan.vkBeginCommandBuffer(commandBuffer, beginInfo);

			VkBufferCopy copyRegion = new VkBufferCopy();
			copyRegion.size = size;
			Vulkan.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, new VkBufferCopy[] { copyRegion });

			Vulkan.vkEndCommandBuffer(commandBuffer);

			VkSubmitInfo submitInfo = new VkSubmitInfo();
			submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
			submitInfo.commandBufferCount = 1;
			submitInfo.pCommandBuffers = commandBuffers;

			Vulkan.vkQueueSubmit(graphicsQueue, 1, new VkSubmitInfo[] { submitInfo }, null);
			Vulkan.vkQueueWaitIdle(graphicsQueue);

			Vulkan.vkFreeCommandBuffers(device, commandPool, 1, commandBuffers);
		}

		private int findMemoryType(int typeFilter, VkMemoryPropertyFlagBits properties)
		{
			VkPhysicalDeviceMemoryProperties memProperties;
			Vulkan.vkGetPhysicalDeviceMemoryProperties(physicalDevice, out memProperties);

			for (int i = 0; i < memProperties.memoryTypeCount; i++)
			{
				if ((typeFilter & (1 << i)) != 0 && (memProperties.memoryTypes[i].propertyFlags & properties) == properties)
				{
					return i;
				}
			}

			throw new Exception("failed to find suitable memory type!");
		}

		private void createCommandBuffers()
		{
			int commandBuffersCount = swapChainFramebuffers.Length;
			commandBuffers = new VkCommandBuffer[commandBuffersCount];

			VkCommandBufferAllocateInfo allocInfo = new VkCommandBufferAllocateInfo();
			allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
			allocInfo.commandPool = commandPool;
			allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
			allocInfo.commandBufferCount = commandBuffersCount;

			VkResult result = Vulkan.vkAllocateCommandBuffers(device, allocInfo, commandBuffers);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to allocate command buffers!", result);
			}

			for (int i = 0; i < commandBuffersCount; i++)
			{
				VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo();
				beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
				beginInfo.flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;

				Vulkan.vkBeginCommandBuffer(commandBuffers[i], beginInfo);

				VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo();
				renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO;
				renderPassInfo.renderPass = renderPass;
				renderPassInfo.framebuffer = swapChainFramebuffers[i];
				renderPassInfo.renderArea.offset = VkOffset2D.Create(0, 0);
				renderPassInfo.renderArea.extent = swapChainExtent;

				VkClearValue clearColor = VkClearValue.Create(0.01f, 0.03f, 0.01f, 1.0f);
				renderPassInfo.clearValueCount = 1;
				renderPassInfo.pClearValues = new VkClearValue[] { clearColor };

				Vulkan.vkCmdBeginRenderPass(commandBuffers[i], renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);

				Vulkan.vkCmdBindPipeline(commandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, graphicsPipeline);

				VkBuffer[] vertexBuffers = new VkBuffer[] { vertexBuffer };
				int[] offsets = new int[] { 0 };
				Vulkan.vkCmdBindVertexBuffers(commandBuffers[i], 0, 1, vertexBuffers, offsets);

				Vulkan.vkCmdBindIndexBuffer(commandBuffers[i], indexBuffer, 0, VkIndexType.VK_INDEX_TYPE_UINT16);

				Vulkan.vkCmdBindDescriptorSets(commandBuffers[i], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, new VkDescriptorSet[] { descriptorSet }, 0, null);

				// Vulkan.vkCmdDraw(commandBuffers[i], vertices.Length, 1, 0, 0);
				Vulkan.vkCmdDrawIndexed(commandBuffers[i], indices.Length, 1, 0, 0, 0);

				Vulkan.vkCmdEndRenderPass(commandBuffers[i]);

				result = Vulkan.vkEndCommandBuffer(commandBuffers[i]);
				if (result != VkResult.VK_SUCCESS)
				{
					throw Program.Throw("failed to record command buffer!", result);
				}
			}
		}

		void createSemaphores()
		{
			VkSemaphoreCreateInfo semaphoreInfo = new VkSemaphoreCreateInfo();
			semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;

			if (Vulkan.vkCreateSemaphore(device, semaphoreInfo, null, out imageAvailableSemaphore) != VkResult.VK_SUCCESS ||
				Vulkan.vkCreateSemaphore(device, semaphoreInfo, null, out renderFinishedSemaphore) != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create semaphores!");
			}
		}

		static DateTime startTime = DateTime.Now;

		private void updateUniformBuffer()
		{
			var currentTime = DateTime.Now;
			float time = (float)((currentTime - startTime).TotalSeconds);

			UniformBufferObject ubo = new UniformBufferObject();
			ubo.model = mat4.Rotate(time * glm.Radians(90.0f), new vec3(0.0f, 0.0f, 1.0f));
			ubo.view = mat4.LookAt(new vec3(2.0f, 2.0f, 2.0f), new vec3(0.0f, 0.0f, 0.0f), new vec3(0.0f, 0.0f, 1.0f));
			ubo.proj = mat4.Perspective(glm.Radians(45.0f), swapChainExtent.width / (float)swapChainExtent.height, 0.1f, 10.0f);

			// ubo.proj[1][1] *= -1;
			ubo.proj.m11 *= -1;

			byte[] data;
			Vulkan.vkMapMemory(device, uniformBufferMemory, 0, Marshal.SizeOf(ubo), 0, out data);
			MemoryCopyHelper.Copy(ubo, data, 0, Marshal.SizeOf(ubo));
			Vulkan.vkUnmapMemory(device, uniformBufferMemory);
		}

		FpsCounter fpsSubmitQueue = new FpsCounter("submitQueue", reportCounters: FpsReportCounters.SimpleFrameTime);
		FpsCounter fpsPresentQueue = new FpsCounter("presentQueue", reportCounters: FpsReportCounters.SimpleFrameTime);

		void drawFrame()
		{
			int imageIndex;
			VkResult result = Vulkan.vkAcquireNextImageKHR(device, swapChain, long.MaxValue, imageAvailableSemaphore, null, out imageIndex);

			if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
			{
				recreateSwapChain();
				return;
			}
			else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR)
			{
				throw Program.Throw("failed to acquire swap chain image!", result);
			}

			VkSubmitInfo submitInfo = new VkSubmitInfo();
			submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;

			VkSemaphore[] waitSemaphores = new VkSemaphore[] { imageAvailableSemaphore };
			VkPipelineStageFlagBits[] waitStages = new VkPipelineStageFlagBits[] { VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT };
			submitInfo.waitSemaphoreCount = 1;
			submitInfo.pWaitSemaphores = waitSemaphores;
			submitInfo.pWaitDstStageMask = waitStages;

			submitInfo.commandBufferCount = 1;
			submitInfo.pCommandBuffers = new VkCommandBuffer[] { commandBuffers[imageIndex] };

			VkSemaphore[] signalSemaphores = new VkSemaphore[] { renderFinishedSemaphore };
			submitInfo.signalSemaphoreCount = 1;
			submitInfo.pSignalSemaphores = signalSemaphores;

			fpsSubmitQueue.Begin();
			result = Vulkan.vkQueueSubmit(graphicsQueue, 1, new VkSubmitInfo[] { submitInfo }, null);
			fpsSubmitQueue.End(); fpsSubmitQueue.DebugPeriodicReport();

			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to submit draw command buffer!");
			}

			VkPresentInfoKHR presentInfo = new VkPresentInfoKHR();
			presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR;

			presentInfo.waitSemaphoreCount = 1;
			presentInfo.pWaitSemaphores = signalSemaphores;

			VkSwapchainKHR[] swapChains = new VkSwapchainKHR[] { swapChain };
			presentInfo.swapchainCount = 1;
			presentInfo.pSwapchains = swapChains;
			presentInfo.pImageIndices = new int[] { imageIndex };

			fpsPresentQueue.Begin();
			result = Vulkan.vkQueuePresentKHR(presentQueue, presentInfo);
			fpsPresentQueue.End(); fpsPresentQueue.DebugPeriodicReport();

			if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR)
			{
				recreateSwapChain();
			}
			else if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to present swap chain image!", result);
			}

			Vulkan.vkQueueWaitIdle(presentQueue);
		}

		private VkShaderModule createShaderModule(Type shaderCode)
		{
			VkShaderModuleCreateInfo createInfo = new VkShaderModuleCreateInfo();
			createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
			createInfo.pCode = shaderCode;

			VkShaderModule shaderModule = null;
			VkResult result;
			result = Vulkan.vkCreateShaderModule(device, createInfo, default(VkAllocationCallbacks), out shaderModule);
			if (result != VkResult.VK_SUCCESS)
			{
				throw Program.Throw("failed to create shader module!", result);
			}

			return shaderModule;
		}

		private VkSurfaceFormatKHR chooseSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats)
		{
			if (availableFormats.Length == 1 && availableFormats[0].format == VkFormat.VK_FORMAT_UNDEFINED)
			{
				return VkSurfaceFormatKHR.Create(VkFormat.VK_FORMAT_B8G8R8A8_UNORM, VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR);
			}

			foreach (var availableFormat in availableFormats)
			{
				if (availableFormat.format == VkFormat.VK_FORMAT_B8G8R8A8_UNORM && availableFormat.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
				{
					return availableFormat;
				}
			}

			return availableFormats[0];
		}

		private VkPresentModeKHR chooseSwapPresentMode(VkPresentModeKHR[] availablePresentModes)
		{
			VkPresentModeKHR bestMode = VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;

			foreach (var availablePresentMode in availablePresentModes)
			{
				if (availablePresentMode == VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR)
				{
					return availablePresentMode;
				}
				else if (availablePresentMode == VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR)
				{
					bestMode = availablePresentMode;
				}
			}

			return bestMode;
		}

		private VkExtent2D chooseSwapExtent(VkSurfaceCapabilitiesKHR capabilities)
		{
			if (capabilities.currentExtent.width != Int32.MaxValue)
			{
				return capabilities.currentExtent;
			}
			else
			{
				int width, height;
				GLFW.glfwGetWindowSize(window, out width, out height);

				VkExtent2D actualExtent = VkExtent2D.Create(width, height);

				actualExtent.width = Math.Max(capabilities.minImageExtent.width, Math.Min(capabilities.maxImageExtent.width, actualExtent.width));
				actualExtent.height = Math.Max(capabilities.minImageExtent.height, Math.Min(capabilities.maxImageExtent.height, actualExtent.height));

				return actualExtent;
			}
		}

		private SwapChainSupportDetails querySwapChainSupport(VkPhysicalDevice physicalDevice)
		{
			SwapChainSupportDetails details;

			Vulkan.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, out details.capabilities);

			int formatCount = 0;
			Vulkan.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, null);
			details.formats = new VkSurfaceFormatKHR[formatCount];
			Vulkan.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, details.formats);

			int presentModeCount = 0;
			Vulkan.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, null);
			details.presentModes = new VkPresentModeKHR[presentModeCount];
			Vulkan.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, details.presentModes);

			return details;
		}

		private bool isDeviceSuitable(VkPhysicalDevice physicalDevice)
		{
			QueueFamilyIndices indices = findQueueFamilies(physicalDevice);

			bool extensionsSupported = checkDeviceExtensionsSupport(physicalDevice);

			bool swapChainAdequate = false;
			if (extensionsSupported)
			{
				SwapChainSupportDetails swapChainSupport = querySwapChainSupport(physicalDevice);
				swapChainAdequate = swapChainSupport.formats.Any() && swapChainSupport.presentModes.Any();
			}

			return indices.isComplete() && extensionsSupported && swapChainAdequate;
		}

		private bool checkDeviceExtensionsSupport(VkPhysicalDevice physicalDevice)
		{
			int extensionCount = 0;
			Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice, null, ref extensionCount, null);
			VkExtensionProperties[] availableExtensions = new VkExtensionProperties[extensionCount];
			Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice, null, ref extensionCount, availableExtensions);

			HashSet<string> requiredExtensions = new HashSet<string>(deviceExtensions);

			foreach (var extension in availableExtensions)
			{
				requiredExtensions.Remove(extension.extensionName);
			}

			return !requiredExtensions.Any();
		}

		private QueueFamilyIndices findQueueFamilies(VkPhysicalDevice device)
		{
			QueueFamilyIndices indices = new QueueFamilyIndices();

			int queueFamilyCount = 0;
			Vulkan.vkGetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilyCount, null);
			VkQueueFamilyProperties[] queueFamilies = new VkQueueFamilyProperties[queueFamilyCount];
			Vulkan.vkGetPhysicalDeviceQueueFamilyProperties(device, ref queueFamilyCount, queueFamilies);

			int i = 0;
			foreach (var queueFamily in queueFamilies)
			{
				if (queueFamily.queueCount > 0 && (queueFamily.queueFlags & VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT) > 0)
				{
					indices.graphicsFamily = i;
				}

				bool presentSupport = false;
				Vulkan.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, surface, out presentSupport);

				if (queueFamily.queueCount > 0 && presentSupport)
				{
					indices.presentFamily = i;
				}

				if (indices.isComplete())
				{
					break;
				}

				i++;
			}

			return indices;
		}

		private List<string> getRequiredExtensions()
		{
			int glfwExtensionCount = 0;
			List<string> glfwExtensions = new List<string>();
			var ret = GLFW.glfwGetRequiredInstanceExtensions(out glfwExtensionCount);
			glfwExtensions.AddRange(ret);

			if (enableValidationLayers)
				glfwExtensions.Add(VkExtensionNames.VK_EXT_DEBUG_REPORT_EXTENSION_NAME);

			return glfwExtensions;
		}

		private bool checkValidationLayerSupport()
		{
			int layerCount = 0;
			Vulkan.vkEnumerateInstanceLayerProperties(ref layerCount, null);
			VkLayerProperties[] availableLayers = new VkLayerProperties[layerCount];
			Vulkan.vkEnumerateInstanceLayerProperties(ref layerCount, availableLayers);

			foreach (var layerName in validationLayers)
			{
				bool layerFound = false;
				foreach (var layerProperties in availableLayers)
				{
					if (layerName == layerProperties.layerName)
					{
						layerFound = true;
						break;
					}
				}

				if (!layerFound)
					return false;
			}

			return true;
		}

		private static bool debugCallback(VkDebugReportFlagBitsEXT flags, VkDebugReportObjectTypeEXT objType, object obj, int location, int messageCode, string pLayerPrefix, string msg, object userData)
		{
			Debug.WriteLine(string.Format("ERROR: validation layer: {0}", msg));
			return false;
		}
	}
}
