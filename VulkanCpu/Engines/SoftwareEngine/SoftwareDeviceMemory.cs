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
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine
{
	public class SoftwareDeviceMemory : VkDeviceMemory
	{
		public readonly VkMemoryAllocateInfo m_allocateInfo;
		public readonly SoftwareDevice m_device;
		public byte[] m_bytes;

		public bool m_Mapped;
		public byte[] m_MapSubBuffer;
		public int m_MapSubOffset;
		public int m_MapSubSize;

		public SoftwareDeviceMemory(SoftwareDevice device, VkMemoryAllocateInfo allocateInfo)
		{
			this.m_device = device;
			this.m_allocateInfo = allocateInfo;
			this.m_bytes = new byte[m_allocateInfo.allocationSize];
		}

		public VkResult MapMemory(int offset, int size, int memoryMapFlags, out byte[] ppData)
		{
			if (m_Mapped)
			{
				ppData = null;
				return VkResult.VK_ERROR_MEMORY_MAP_FAILED;
			}

			if (offset == 0)
			{
				ppData = m_bytes;
			}
			else
			{
				m_MapSubBuffer = new byte[size];
				m_MapSubOffset = offset;
				m_MapSubSize = size;
				Buffer.BlockCopy(m_bytes, offset, m_MapSubBuffer, 0, size);
				ppData = m_MapSubBuffer;
			}
			m_Mapped = true;
			return VkResult.VK_SUCCESS;
		}

		public void UnmapMemory()
		{
			if (!m_Mapped)
			{
				return;
			}

			if (m_MapSubBuffer != null)
			{
				Array.Copy(m_MapSubBuffer, 0, m_bytes, m_MapSubOffset, m_MapSubSize);
				m_MapSubBuffer = null;
			}

			m_Mapped = false;
		}

		public void Destroy()
		{
		}
	}
}