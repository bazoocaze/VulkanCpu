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

using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class SoftwareSampler2D : Shader.sampler2D
	{
		private VkImageLayout imageLayout;
		private SoftwareImageView imageView;
		private SoftwareSampler sampler;

		private int m_Width;
		private int m_Height;

		internal SoftwareSampler2D(VkImageLayout imageLayout, SoftwareImageView imageView, SoftwareSampler sampler)
		{
			this.imageLayout = imageLayout;
			this.imageView = imageView;
			this.sampler = sampler;

			this.m_Width = imageView.GetWidth();
			this.m_Height = imageView.GetHeight();
		}

		public vec4 GetTexel(vec2 coord)
		{
			int cx = glm.Clamp((int)(coord.x * m_Width), 0, m_Width - 1);
			int cy = glm.Clamp((int)(coord.y * m_Height), 0, m_Height - 1);
			return imageView.GetPixel_vec4(new ivec2(cx, cy));
		}

		public vec4 GetTexel(ivec2 pos)
		{
			return imageView.GetPixel_vec4(pos);
		}
	}
}
