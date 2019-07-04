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
using System.Linq.Expressions;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareShaderModule : VkShaderModule
	{
		public SoftwareDevice m_device;
		public VkShaderModuleCreateInfo m_createInfo;
		public Type m_codeClassType;

		public SoftwareShaderModule(SoftwareDevice device, VkShaderModuleCreateInfo createInfo)
		{
			this.m_createInfo = createInfo;
			this.m_codeClassType = createInfo.pCode;
		}

		public object GetNewInstance()
		{
			return Activator.CreateInstance(m_codeClassType);
		}

		public static CompiledShaderModule Compile(VkPipelineShaderStageCreateInfo stageCreateInfo)
		{
			SoftwareShaderModule module = (SoftwareShaderModule)stageCreateInfo.module;
			string entryPointName = stageCreateInfo.pName;

			CompiledShaderModule ret = new CompiledShaderModule();
			ret.instance = module.GetNewInstance();
			ret.entryPoint = IntrospectionUtil.GetMethodCallExpression(ret.instance, entryPointName);
			return ret;
		}

		public class CompiledShaderModule
		{
			public object instance;
			public Expression entryPoint;
		}

		public void Destroy()
		{
		}
	}
}
