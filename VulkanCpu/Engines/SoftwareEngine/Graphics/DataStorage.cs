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
using VulkanCpu.Util;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class DataStorage
	{
		private Dictionary<string, DataStorageItem> m_Storage = new Dictionary<string, DataStorageItem>();

		public void Clear()
		{
			m_Storage.Clear();
		}

		public DataStorageItem AddGeneric(string name, Type dataType, int length)
		{
			var methodCall = IntrospectionUtil.GetGenericMethodCallExpression(this, nameof(Add), new Type[] { dataType }, new Expression[] { Expression.Constant(name), Expression.Constant(length) });
			return IntrospectionUtil.CreateFunc<DataStorageItem>(methodCall).Invoke();
		}

		public DataStorageItem<T> Add<T>(string name, int length)
		{
			DataStorageItem<T> ret = new DataStorageItem<T>(name, length);
			m_Storage.Add(name, ret);
			return ret;
		}

		public DataStorageItem Get(string name)
		{
			return m_Storage[name];
		}

		public DataStorageItem<T> Get<T>(string name)
		{
			return (DataStorageItem<T>)m_Storage[name];
		}

		public bool HasKey(string name)
		{
			return m_Storage.ContainsKey(name);
		}
	}

	public abstract class DataStorageItem
	{
		public string Name;
		public Type DataType;
		public int Length;

		protected DataStorageItem(string name, Type dataType, int length)
		{
			this.Name = name;
			this.DataType = dataType;
			this.Length = length;
		}

		public abstract Expression GetArrayExpression();
		public abstract Expression GetIndexedExpression(int index);
		public abstract Expression GetIndexedExpression(Expression index);
	}

	public class DataStorageItem<T> : DataStorageItem
	{
		public T[] Data;

		public DataStorageItem(string name, int length) : base(name, typeof(T), length)
		{
			Data = new T[length];
		}

		public override Expression GetArrayExpression()
		{
			return IntrospectionUtil.GetFieldExpression(this, nameof(Data));
		}

		public override Expression GetIndexedExpression(int index)
		{
			var arrayExpression = IntrospectionUtil.GetFieldExpression(this, nameof(Data));
			return IntrospectionUtil.GetArrayAccessExpression(arrayExpression, Expression.Constant(index));
		}

		public override Expression GetIndexedExpression(Expression index)
		{
			var arrayExpression = IntrospectionUtil.GetFieldExpression(this, nameof(Data));
			return IntrospectionUtil.GetArrayAccessExpression(arrayExpression, index);
		}

		public override string ToString()
		{
			return string.Format("Name={0} DataType={1} Length={2} First={3}",
				this.Name,
				this.DataType.Name,
				this.Length,
				this.Data[0]);
		}
	}
}
