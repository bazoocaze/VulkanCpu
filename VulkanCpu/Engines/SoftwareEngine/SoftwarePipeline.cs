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

using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public abstract class SoftwarePipeline : VkPipeline
	{
		public abstract void Destroy();
	}

	public class SoftwareGraphicsPipeline : SoftwarePipeline
	{
		public SoftwareDevice m_softwareDevice;
		public VkGraphicsPipelineCreateInfo m_graphicsPipelineCreateInfo;

		private SoftwareGraphicsPipeline(SoftwareDevice softwareDevice, VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo)
		{
			this.m_softwareDevice = softwareDevice;
			this.m_graphicsPipelineCreateInfo = graphicsPipelineCreateInfo;
		}

		public static VkResult Create(SoftwareDevice softwareDevice, VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo, out VkPipeline pipeline)
		{
			pipeline = new SoftwareGraphicsPipeline(softwareDevice, graphicsPipelineCreateInfo);
			return VkResult.VK_SUCCESS;
		}

		public override void Destroy()
		{
		}
	}

	public class SoftwarePipelineLayout : VkPipelineLayout
	{
		public SoftwareDevice m_device;
		public VkPipelineLayoutCreateInfo m_createInfo;

		public SoftwarePipelineLayout(SoftwareDevice device, VkPipelineLayoutCreateInfo createInfo)
		{
			this.m_device = device;
			this.m_createInfo = createInfo;
		}

		public void Destroy()
		{
		}
	}
}
