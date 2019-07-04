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

using System.Collections.Generic;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareDescriptorPool : VkDescriptorPool
	{
		public SoftwareDevice m_device;
		public VkDescriptorPoolCreateInfo m_createinfo;

		public List<SoftwareDescriptorSet> m_DescriptorSets;

		public SoftwareDescriptorPool(SoftwareDevice device, VkDescriptorPoolCreateInfo createinfo)
		{
			this.m_device = device;
			this.m_createinfo = createinfo;
			this.m_DescriptorSets = new List<SoftwareDescriptorSet>();
		}

		public VkResult AllocateDescriptorSets(VkDescriptorSetAllocateInfo pAllocateInfo, VkDescriptorSet[] pDescriptorSets)
		{
			for (int i = 0; i < pAllocateInfo.descriptorSetCount; i++)
			{
				var descriptorSet = new SoftwareDescriptorSet(this, (SoftwareDescriptorSetLayout)pAllocateInfo.pSetLayouts[i]);
				m_DescriptorSets.Add(descriptorSet);
				pDescriptorSets[i] = descriptorSet;
			}
			return VkResult.VK_SUCCESS;
		}

		public void Destroy()
		{
		}
	}
}
