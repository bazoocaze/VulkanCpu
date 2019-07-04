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
using System.Reflection;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class BuiltinVariables
	{
		private Dictionary<string, BuiltinVariableInfo> m_Builtins;

		public BuiltinVariables()
		{
			m_Builtins = new Dictionary<string, BuiltinVariableInfo>();
		}

		private string GetKey(BuiltinVariableInfo info)
		{
			return GetKey(info.Stage, info.Name);
		}

		private string GetKey(StageType stage, string name)
		{
			return string.Format("{0}.{1}", stage, name);
		}

		public void Add(StageType stage, string name, BuiltinFlags direction)
		{
			Add(stage, name, direction, null);
		}

		public void Add(StageType stage, string name, BuiltinFlags direction, Action onUseAction)
		{
			BuiltinVariableInfo info = new BuiltinVariableInfo();
			info.Name = name;
			info.Stage = stage;
			info.Flags = direction | (onUseAction != null ? BuiltinFlags.HasAction : 0);
			info.UseAction = onUseAction;
			m_Builtins.Add(GetKey(info), info);
		}

		public bool IsBuiltin(StageType stage, string fieldName)
		{
			if (fieldName.StartsWith("gl_"))
				return true;
			return m_Builtins.ContainsKey(GetKey(stage, fieldName));
		}

		public BuiltinVariableInfo Get(StageType stage, string fieldName)
		{
			return m_Builtins[GetKey(stage, fieldName)];
		}

		public bool Validate(StageType stage, FieldInfo fieldInfo)
		{
			string key = GetKey(stage, fieldInfo.Name);
			if (!m_Builtins.ContainsKey(key))
				return false;
			return true;
		}
	}

	public class BuiltinVariableInfo
	{
		public string Name;
		public StageType Stage;
		public BuiltinFlags Flags;
		public Action UseAction;

		public override string ToString()
		{
			return string.Format("Name={0} Stage={1} Flags={2}", Name, Stage, Flags);
		}
	}

	[Flags]
	public enum BuiltinFlags
	{
		None = 0,
		In = 1,
		Out = 2,
		InOut = 3,
		IndexedPerVertex = 4,
		HasAction = 8,
	}
}
