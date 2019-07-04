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

using System.Runtime.CompilerServices;
using GlmSharp;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	internal class SoftwareImageView : VkImageView
	{
		internal readonly SoftwareDevice m_device;
		internal readonly VkImageViewCreateInfo m_createInfo;
		internal readonly SoftwareImage m_image;

		internal SoftwareImageView(SoftwareDevice device, VkImageViewCreateInfo createInfo)
		{
			this.m_device = device;
			this.m_createInfo = createInfo;
			this.m_image = (SoftwareImage)createInfo.image;
		}

		internal void ClearColor(VkClearValue clearValue)
		{
			m_image.ClearColor(clearValue);
		}

		internal void ClearDepth(VkClearValue clearValue)
		{
			m_image.ClearDepth(clearValue);
		}

		internal int GetWidth()
		{
			return m_image.GetWidth();
		}

		internal int GetHeight()
		{
			return m_image.GetHeight();
		}

		internal void SetPixel(ivec2 position, vec4 color)
		{
			m_image.SetPixel(position, PixelFormatConverter.ConvertToUint(ref color));
		}

		internal void SetPixel(ivec2 position, uint color)
		{
			m_image.SetPixel(position, color);
		}

		internal uint GetPixel_uint32(ivec2 pos)
		{
			return m_image.GetPixel(pos);
		}

		internal vec4 GetPixel_vec4(ivec2 pos)
		{
			vec4 ret = new vec4();
			PixelFormatConverter.ConvertToVec4(m_image.GetPixel(pos), ref ret);
			return ret;
		}

		internal float ReadPixel_float(ivec2 pos)
		{
			return m_image.ReadPixel_float(pos);
		}

		internal void WritePixel(ivec2 coord, float depth)
		{
			m_image.WritePixel(coord, depth);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal float ReadDepth(ivec2 coord)
		{
			return m_image.ReadDepth(coord);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteDepth(ivec2 coord, float depth)
		{
			m_image.WriteDepth(coord, depth);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int ReadStencil(ivec2 coord)
		{
			return m_image.ReadStencil(coord);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteStencil(ivec2 coord, int stencil)
		{
			m_image.WriteStencil(coord, stencil);
		}

		public void Destroy()
		{
		}
	}
}
