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
using System.Linq.Expressions;

namespace VulkanCpu.Engines.SoftwareEngine.Util
{
	public class VertexBufferCache
	{
		private byte[] m_SourceData;
		private int m_SourceOffset;
		private Dictionary<string, CachedStructureReader> m_Cache;

		public VertexBufferCache(byte[] source, int offset)
		{
			m_SourceData = source;
			m_SourceOffset = offset;
			m_Cache = new Dictionary<string, CachedStructureReader>();
		}

		public Expression GetReadExpression(Type dataType, int offset, int stride, Expression indexExpression)
		{
			var buffer = GetBuffer(dataType, offset, stride);
			return buffer.GetReadExpression(indexExpression);
		}

		private string GetKey(Type dataType, int offset, int stride)
		{
			return string.Format("{0}|{1}|{2}", dataType.Name, offset, stride);
		}

		private CachedStructureReader GetBuffer(Type dataType, int offset, int stride)
		{
			string key = GetKey(dataType, offset, stride);
			if (m_Cache.ContainsKey(key))
				return m_Cache[key];
			var ret = CachedStructureReader.Create(dataType, m_SourceData, m_SourceOffset + offset, stride);
			m_Cache[key] = ret;
			return ret;
		}
	}
}
