/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;

namespace GlfwLib
{
	public delegate void GlfwWindowSizeCallbackDelegate(GLFWwindow window, int width, int height);
	public delegate void GlfwKeyCallbackDelegate(GLFWwindow window, int key, int scancode, int action, int mods);

	public class GLFW
	{
		public static void glfwInit()
		{

		}

		public static void glfwWindowHint(GLFWEnum param, GLFWEnum value)
		{
		}

		public static GLFWwindow glfwCreateWindow(int width, int height, string title, object p1, object p2)
		{
			var ret = new GLFWwindow(width, height, title);
			return ret;
		}

		public static bool glfwWindowShouldClose(GLFWwindow window)
		{
			return window.ShouldClose;
		}

		public static void glfwPollEvents()
		{
		}

		public static void glfwDestroyWindow(GLFWwindow window)
		{
			window.Dispose();
		}

		public static void glfwTerminate()
		{
		}

		public static void EventLoop(GLFWwindow window, Action loopCode)
		{
			GameLoop.Run(window.GetHandle(), () => !window.ShouldClose, loopCode);
		}

		public static void EventLoop(GLFWwindow window, Action<GLFWLoop> loopCode)
		{
			GLFWLoop loop = new GLFWLoop();
			GameLoop.Run(
				window.GetHandle(),
				() => !(loop.m_BreakLoop || window.ShouldClose),
				() => loopCode(loop)
			);
		}

		public static string[] glfwGetRequiredInstanceExtensions(out int glfwExtensionCount)
		{
			var ret = new List<string>();
			glfwExtensionCount = ret.Count;
			return ret.ToArray();
		}

		public static VkResult glfwCreateWindowSurface(VkInstance instance, GLFWwindow window, VkAllocationCallbacks pAllocator, out VkSurfaceKHR surface)
		{
			VkWin32SurfaceCreateInfoKHR pCreateInfo = new VkWin32SurfaceCreateInfoKHR();
			pCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;
			pCreateInfo.hwnd = window.GetHandle().Handle;
			return Vulkan.vkCreateWin32SurfaceKHR(instance, pCreateInfo, pAllocator, out surface);
		}

		public static void glfwSetWindowUserPointer(GLFWwindow window, object userData)
		{
			window.m_userData = userData;
		}

		public static object glfwGetWindowUserPointer(GLFWwindow window)
		{
			return window.m_userData;
		}

		public static void glfwGetWindowSize(GLFWwindow window, out int width, out int height)
		{
			window.GetSize(out width, out height);
		}

		public static void glfwSetWindowSizeCallback(GLFWwindow window, GlfwWindowSizeCallbackDelegate onWindowResized)
		{
			window.SetSizeCallback(onWindowResized);
		}

		public static void glfwSetKeyCallback(GLFWwindow window, GlfwKeyCallbackDelegate onKey)
		{
			window.SetKeyCallback(onKey);
		}
	}

	public class GLFWLoop
	{
		internal bool m_BreakLoop;

		public void BreakLoop()
		{
			m_BreakLoop = true;
		}
	}

	public enum GLFWEnum
	{
		GLFW_CLIENT_API,
		GLFW_NO_API,
		GLFW_RESIZABLE,
		GLFW_TRUE,
		GLFW_FALSE,
	}
}
