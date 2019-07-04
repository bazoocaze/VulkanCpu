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
using System.Collections.Generic;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.VulkanApi.Internals
{
	public class SoftwareInstance : IVkInstance
	{
		public VkInstanceCreateInfo m_createInfo;
		public VkDebugReportCallbackEXT m_debugReportCallback;
		public List<VkPhysicalDevice> m_physicalDevices;
		public List<VkDebugReportCallbackEXT> m_ReportCallbacks;

		public SoftwareInstance(VkInstanceCreateInfo createInfo)
		{
			this.m_createInfo = createInfo;
			this.m_ReportCallbacks = new List<VkDebugReportCallbackEXT>();
		}

		public void Destroy()
		{
		}

		public VkResult CreateDebugReportCallback(VkDebugReportCallbackCreateInfoEXT pCreateInfo, out VkDebugReportCallbackEXT pCallback)
		{
			pCallback = new VkDebugReportCallbackEXT(this, pCreateInfo);
			m_ReportCallbacks.Add(pCallback);
			return VkResult.VK_SUCCESS;
		}

		public void DestroyDebugReportCallback(VkDebugReportCallbackEXT callback)
		{
			m_ReportCallbacks.Remove(callback);
		}

		public bool DebugReportMessage(VkDebugReportFlagBitsEXT flags, VkDebugReportObjectTypeEXT objectType, object obj, int location, int messageCode, string pLayerPrefix, string pMessage)
		{
			bool abort = false;
			foreach (var callback in m_ReportCallbacks)
			{
				if ((callback.m_pCreateInfo.flags & flags) > 0)
				{
					abort |= callback.m_pCreateInfo.pfnCallback(flags, objectType, obj, location, messageCode, pLayerPrefix, pMessage, callback.m_pCreateInfo.pUserData);
				}
			}
			return abort;
		}

		public VkResult EnumeratePhysicalDevices(ref int pPhysicalDeviceCount, VkPhysicalDevice[] pPhysicalDevices)
		{
			if (m_physicalDevices == null)
			{
				m_physicalDevices = new List<VkPhysicalDevice>();

				foreach (Type typeFactory in VkReflectionUtil.EnumerateInterfaces<IVkPhysicalDeviceFactory>())
				{
					IVkPhysicalDeviceFactory factory = (IVkPhysicalDeviceFactory)Activator.CreateInstance(typeFactory);
					foreach (var device in factory.EnumeratePhysicalDevices01(this))
					{
						m_physicalDevices.Add(device);
					}
				}
			}

			return VkArrayUtil.CopyToList(m_physicalDevices, pPhysicalDevices, ref pPhysicalDeviceCount);
		}

		public void DestroySurfaceKHR(VkSurfaceKHR surface)
		{
		}

		public VkResult CreateWin32SurfaceKHR(VkWin32SurfaceCreateInfoKHR pCreateInfo, out VkSurfaceKHR pSurface)
		{
			return SoftwareSurfaceFactory.Create(this, pCreateInfo, out pSurface);
		}
	}
}
