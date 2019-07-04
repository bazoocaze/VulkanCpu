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
	public class SoftwarePipelineProgram
	{
		public List<Expression> UniformLoader = new List<Expression>();

		public Dictionary<StageType, List<Expression>> ShaderEntry = new Dictionary<StageType, List<Expression>>();
		public Dictionary<StageType, List<Expression>> PreActions = new Dictionary<StageType, List<Expression>>();
		public Dictionary<StageType, List<Expression>> PostActions = new Dictionary<StageType, List<Expression>>();

		public SoftwarePipelineProgram()
		{
			StageType[] list = new StageType[] { StageType.VertexShader, StageType.FragmentShader };
			foreach (var item in list)
			{
				ShaderEntry.Add(item, new List<Expression>());
				PreActions.Add(item, new List<Expression>());
				PostActions.Add(item, new List<Expression>());
			}
		}

		private Expression GetExpression(List<Expression> pre, List<Expression> execute, List<Expression> post)
		{
			List<Expression> list = new List<Expression>();

			if (pre != null)
				foreach (var item in pre)
					list.Add(item);

			if (execute != null)
				foreach (var item in execute)
					list.Add(item);

			if (post != null)
				foreach (var item in post)
					list.Add(item);

			if (list.Count == 0)
				return Expression.Empty();

			return Expression.Block(list);
		}

		private Expression GetExpression(StageType stage)
		{
			return GetExpression(PreActions[stage], ShaderEntry[stage], PostActions[stage]);
		}

		public CompiledPipelineProgram Compile()
		{
			CompiledPipelineProgram compiled = new CompiledPipelineProgram();

			compiled.VertexShader = IntrospectionUtil.CreateCall(GetExpression(StageType.VertexShader));
			compiled.FragmentShader = IntrospectionUtil.CreateCall(GetExpression(StageType.FragmentShader));
			compiled.InitializePipeline = IntrospectionUtil.CreateCall(GetExpression(UniformLoader, null, null));
			return compiled;
		}
	}

	public class CompiledPipelineProgram
	{
		public Action InitializePipeline;
		public Action FragmentShader;
		public Action VertexShader;
	}
}
