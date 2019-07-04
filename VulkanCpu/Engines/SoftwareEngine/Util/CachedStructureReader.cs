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
using System.Runtime.InteropServices;

namespace VulkanCpu.Engines.SoftwareEngine.Util
{
	public abstract class CachedStructureReader
	{
		public abstract Expression GetReadExpression(Expression indexExpression);

		public static CachedStructureReader Create(Type dataType, byte[] source, int offset, int stride)
		{
			return (CachedStructureReader)Activator.CreateInstance(typeof(CachedStructureReader<>).MakeGenericType(dataType), source, offset, stride);
		}
	}

	public class CachedStructureReader<T> : CachedStructureReader where T : struct
	{
		private const byte POS_FREE = 0;
		private const byte POS_CACHED = 1;

		private byte[] m_SourceData;
		private int m_SourceOffset;
		private int m_SourceStride;

		private int m_DataSize;
		private Type m_DataType;
		private T[] m_CachedData;
		private byte[] m_CachedDataPresent;
		private int m_MaxDataLength;
		private int m_DataLength;

		public CachedStructureReader(byte[] source, int offset, int stride)
		{
			m_SourceData = source;
			m_SourceOffset = offset;
			m_SourceStride = stride;

			m_DataType = typeof(T);
			m_DataSize = Marshal.SizeOf(m_DataType);
			m_MaxDataLength = (int)Math.Ceiling((double)(source.Length - offset) / stride);
		}

		public T Read(int index)
		{
			if (index < m_DataLength && m_CachedDataPresent[index] != POS_FREE)
			{
				return m_CachedData[index];
			}
			T ret = new T();
			Load(index, ref ret);
			return ret;
		}

		public void Read(int index, ref T output)
		{
			if (index < m_DataLength && m_CachedDataPresent[index] != POS_FREE)
			{
				output = m_CachedData[index];
			}
			else
			{
				Load(index, ref output);
			}
		}

		private void Load(int index, ref T output)
		{
			if (index >= m_DataLength)
			{
				Resize(index);
			}

			T ret = new T();
			LoadRaw(index, ref ret);
			m_CachedData[index] = ret;
			m_CachedDataPresent[index] = POS_CACHED;
			output = ret;
		}

		private void LoadRaw(int index, ref T output)
		{
			int offset = m_SourceOffset + (index * m_SourceStride);
			var pinned = GCHandle.Alloc(m_SourceData, GCHandleType.Pinned);
			try
			{
				IntPtr ptr = pinned.AddrOfPinnedObject();
				output = (T)Marshal.PtrToStructure(ptr + offset, m_DataType);
			}
			finally
			{
				pinned.Free();
			}
		}

		private void Resize(int nextIndex)
		{
			var newSize = Math.Max(8, m_DataLength);

			while (newSize <= nextIndex)
			{
				newSize = newSize * 2;
			}

			if (newSize > (m_CachedData?.Length ?? 0))
			{
				Array.Resize(ref m_CachedData, newSize);
			}

			if (newSize > (m_CachedDataPresent?.Length ?? 0))
			{
				Array.Resize(ref m_CachedDataPresent, newSize);
			}

			m_DataLength = newSize;
		}

		public override Expression GetReadExpression(Expression indexExpression)
		{
			return Expression.Call(
				Expression.Constant(this),
				nameof(Read),
				null,
				indexExpression);
		}
	}
}
