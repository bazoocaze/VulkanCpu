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
using System.Threading;
using System.Windows.Forms;
using Examples.vulkan_tutorial.com.Ex01_Presentation;
using Examples.vulkan_tutorial.com.Ex02_SwapchainRecreation;
using Examples.vulkan_tutorial.com.Ex03_VertexBuffer;
using Examples.vulkan_tutorial.com.Ex04_StagingBuffer;
using Examples.vulkan_tutorial.com.Ex05_IndexBuffer;
using Examples.vulkan_tutorial.com.Ex06_DescriptorPoolAndSets;
using Examples.vulkan_tutorial.com.Ex07_TextureMapping;
using Examples.vulkan_tutorial.com.Ex08_DepthBuffering;
using Examples.vulkan_tutorial.com.Ex09_LoadingModels;
using VulkanCpu.VulkanApi;

namespace Examples
{
	public static class Program
	{
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main()
		{
			ThreadPool.SetMinThreads(64, 64);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainProgram();
		}

		private static void MainProgram()
		{
			var app = new HelloTriangleApplication08();
			app.run();
		}

		public static Exception Throw(string message)
		{
			return new Exception(String.Format("{0}", message));
		}

		public static Exception Throw(string message, VkResult result)
		{
			return new Exception(String.Format("{0}, VkResult=({1}): {2}", message, (int)result, result));
		}
	}
}
