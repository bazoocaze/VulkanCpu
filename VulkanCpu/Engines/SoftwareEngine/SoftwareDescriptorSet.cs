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
	public class SoftwareDescriptorSet : VkDescriptorSet
	{
		public VkDescriptorSetLayoutCreateInfo m_createInfo;
		public SoftwareDescriptorPool m_descriptorPool;
		public SoftwareDescriptorSetLayout m_descriptorSetLayout;

		public VkDescriptorSetLayoutBinding[] m_Bindings;
		public VkDescriptorBufferInfo[] m_BufferInfo;
		public VkDescriptorImageInfo[] m_ImageInfo;

		public SoftwareDescriptorSet(SoftwareDescriptorPool descriptorPool, SoftwareDescriptorSetLayout layout)
		{
			this.m_descriptorPool = descriptorPool;
			this.m_descriptorSetLayout = layout;
			this.m_createInfo = layout.m_createInfo;

			m_Bindings = new VkDescriptorSetLayoutBinding[m_createInfo.bindingCount];
			m_BufferInfo = new VkDescriptorBufferInfo[m_createInfo.bindingCount];
			m_ImageInfo = new VkDescriptorImageInfo[m_createInfo.bindingCount];

			for (int i = 0; i < m_createInfo.bindingCount; i++)
			{
				m_Bindings[i] = m_createInfo.pBindings[i];
			}
		}
	}

	public class SoftwareDescriptorSetLayout : VkDescriptorSetLayout
	{
		public SoftwareDevice m_device;
		public VkDescriptorSetLayoutCreateInfo m_createInfo;

		public SoftwareDescriptorSetLayout(SoftwareDevice device, VkDescriptorSetLayoutCreateInfo createInfo)
		{
			this.m_device = device;
			this.m_createInfo = createInfo;
		}
	}
}
