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
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;
using VulkanCpu.Util;
using VulkanCpu.VulkanApi;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class SoftwareRasterizer
	{
		private static readonly Type P_WRITE_COLOR_TYPE = typeof(vec4);

		public SoftwareExecutionContext m_context;

		// public Dictionary<StageType, MyProgram> m_Stages = new Dictionary<StageType, MyProgram>();
		public DataStorage m_Storage = new DataStorage();
		public BuiltinVariables m_ShaderBuiltins = new BuiltinVariables();
		private VkViewport m_CurrentViewport;
		public int PrimitiveLength;
		public int PrimitiveVertexIndex;
		private StageType CurrentStage;
		public object m_VertexShaderInstance;
		public object m_FragmentShaderInstance;
		private bool m_FragCoordUsed;
		private bool m_FragDepthUsed;
		private ivec2 internal_FragCoord;
		private PrimitiveContext m_PrimiteContext;

		private ISoftwareDepthBuffer m_DepthBuffer;

		private SoftwarePipelineProgram m_PipelineProgramDefinition;
		private CompiledPipelineProgram m_CompiledPipelineProgram;

		// VERTEX SHADER INPUTS
		public int gl_VertexIndex;
		public int gl_InstanceIndex;

		// VERTEX SHADER OUTPUTS
		public vec4[] gl_Position;

		// FRAGMENT SHADER INPUTS
		public vec4 gl_FragCoord;   // IN / OUT(.z)
		public bool gl_FrontFacing;
		public vec2 gl_PointCoord;

		// FRAGMENT SHADER OUTPUTS
		public float gl_FragDepth;

		public SoftwareRasterizer()
		{
			m_ShaderBuiltins.Add(StageType.VertexShader, "gl_VertexIndex", BuiltinFlags.In);
			m_ShaderBuiltins.Add(StageType.VertexShader, "gl_InstanceIndex", BuiltinFlags.In);
			m_ShaderBuiltins.Add(StageType.VertexShader, "gl_Position", BuiltinFlags.Out | BuiltinFlags.IndexedPerVertex);
			m_ShaderBuiltins.Add(StageType.FragmentShader, "gl_FrontFacing", BuiltinFlags.In);
			m_ShaderBuiltins.Add(StageType.FragmentShader, "gl_PointCoord", BuiltinFlags.In);
			m_ShaderBuiltins.Add(StageType.FragmentShader, "gl_FragCoord", BuiltinFlags.InOut, () => { m_FragCoordUsed = true; });
			m_ShaderBuiltins.Add(StageType.FragmentShader, "gl_FragDepth", BuiltinFlags.InOut, () => { m_FragDepthUsed = true; });
		}

		public void Prepare()
		{
			switch (m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pInputAssemblyState.topology)
			{
				case VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_POINT_LIST:
					PrimitiveLength = 2;
					break;

				case VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST:
					PrimitiveLength = 3;
					break;

				default:
					throw new NotImplementedException();
			}

			// m_Stages.Clear();
			m_Storage.Clear();
			m_FragCoordUsed = false;
			m_FragDepthUsed = false;

			m_PrimiteContext = new PrimitiveContext();

			gl_Position = new vec4[PrimitiveLength];

			m_DepthBuffer = SoftwareDepthBuffer.Create(m_context);

			CurrentStage = StageType.Start;
			// m_Stages[StageType.InputAssembler] = MyProgram.Create(InputAssemblerStage);
			// m_Stages[StageType.Rasterization] = MyProgram.Create(RasterizationStage);
			// m_Stages[StageType.ColorBlending] = MyProgram.Create(ColorBlendingStage);

			m_PipelineProgramDefinition = new SoftwarePipelineProgram();

			foreach (var stageItem in m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pStages)
			{
				var compiledModule = SoftwareShaderModule.Compile(stageItem);

				switch (stageItem.stage)
				{
					case VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT:
						m_VertexShaderInstance = compiledModule.instance;
						m_PipelineProgramDefinition.ShaderEntry[StageType.VertexShader].Add(compiledModule.entryPoint);
						break;

					case VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT:
						m_FragmentShaderInstance = compiledModule.instance;
						m_PipelineProgramDefinition.ShaderEntry[StageType.FragmentShader].Add(compiledModule.entryPoint);
						break;

					default:
						throw new NotImplementedException();
				}
			}

			PrepareShaderStage(StageType.VertexShader, m_VertexShaderInstance);
			PrepareShaderStage(StageType.FragmentShader, m_FragmentShaderInstance);

			m_CurrentViewport = m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pViewportState.pViewports[0];

			m_CompiledPipelineProgram = m_PipelineProgramDefinition.Compile();
		}

		private void PrepareShaderStage(StageType stage, object shaderInstance)
		{
			Type type = shaderInstance.GetType();
			Expression primiteVertexIndexExpression = IntrospectionUtil.GetFieldExpression(this, nameof(PrimitiveVertexIndex));

			foreach (var fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
			{
				string debugFieldName = string.Format("{0}.{1}", type.Name, fieldInfo.Name);

				var fieldExpression = IntrospectionUtil.GetFieldExpression(shaderInstance, fieldInfo);

				// BUILTIN
				if (m_ShaderBuiltins.IsBuiltin(stage, fieldInfo.Name))
				{
					if (!m_ShaderBuiltins.Validate(stage, fieldInfo))
					{
						DebugMsg(string.Format("ERROR: failed to bind: invalid builtin {0}", debugFieldName));
						continue;
					}

					var builtin = m_ShaderBuiltins.Get(stage, fieldInfo.Name);
					var directionIn = builtin.Flags.HasFlag(BuiltinFlags.In);
					var directionOut = builtin.Flags.HasFlag(BuiltinFlags.Out);

					if (directionIn || directionOut)
					{
						Expression builtinVarExpression = IntrospectionUtil.GetFieldExpression(this, builtin.Name);
						Expression builtinExpression = builtinVarExpression;

						// Indexed?
						if (builtin.Flags.HasFlag(BuiltinFlags.IndexedPerVertex))
						{
							builtinExpression = IntrospectionUtil.GetArrayAccessExpression(builtinVarExpression, primiteVertexIndexExpression);
						}

						if (!ValidateFieldType(fieldInfo, builtinExpression.Type))
							continue;

						if (directionIn)
						{
							// m_Stages[stage].AddPreaction(IntrospectionUtil.CreateAssign(fieldExpression, builtinExpression));
							m_PipelineProgramDefinition.PreActions[stage].Add(Expression.Assign(fieldExpression, builtinExpression));
						}

						if (directionOut)
						{
							// m_Stages[stage].AddPostAction(IntrospectionUtil.CreateAssign(builtinExpression, fieldExpression));
							m_PipelineProgramDefinition.PostActions[stage].Add(Expression.Assign(builtinExpression, fieldExpression));
						}
					}

					if (builtin.Flags.HasFlag(BuiltinFlags.HasAction))
					{
						builtin.UseAction.Invoke();
					}

					continue;
				}

				// ----- Verify ShaderAttribute ----- 

				var attrs = fieldInfo.GetCustomAttributes(typeof(ShaderLayoutAttribute)).ToArray();

				if (attrs.Length < 1)
					continue;

				if (attrs.Length > 1)
				{
					DebugMsg(string.Format("Ambiguous attributes for field {0}", debugFieldName));
					continue;
				}

				ShaderLayoutAttribute atr = (ShaderLayoutAttribute)attrs[0];

				// ----- User variable with ShaderAttribute ----- 

				if (atr.type == ShaderLayoutType.Uniform)
				{
					var readUniformCall = GetReadUniformExpression(fieldInfo, atr.binding, atr.set);
					if (readUniformCall != null)
					{
						m_PipelineProgramDefinition.UniformLoader.Add(Expression.Assign(fieldExpression, readUniformCall));
					}
					continue;
				}

				if (stage == StageType.VertexShader)
				{
					// IF IN:  obj.var = ReadVertexBuffer(location)
					// IF OUT: storage[var][index] = obj.var					

					if (atr.type == ShaderLayoutType.In)
					{
						Type vertexBufferType = GetVertexBufferType(fieldInfo, atr.location);
						if (vertexBufferType == null)
						{
							// incompatible or not found
							continue;
						}

						if (!ValidateFieldType(fieldInfo, vertexBufferType))
						{
							continue;
						}

						var readVertexBufferCall = GetReadVertexBufferExpression(fieldInfo, atr.location);
						if (readVertexBufferCall != null)
						{
							m_PipelineProgramDefinition.PreActions[stage].Add(Expression.Assign(fieldExpression, readVertexBufferCall));
						}
					}
					else if (atr.type == ShaderLayoutType.Out)
					{
						DataStorageItem storage = m_Storage.AddGeneric(fieldInfo.Name, fieldInfo.FieldType, PrimitiveLength);
						Expression storageExpression = storage.GetIndexedExpression(primiteVertexIndexExpression);
						m_PipelineProgramDefinition.PostActions[stage].Add(Expression.Assign(storageExpression, fieldExpression));
					}
				}
				else if (stage == StageType.FragmentShader)
				{
					// IF IN:  obj.var = ReadSmoth(storage[var])
					// IF OUT: WriteColorOutput(location, obj.var)

					if (atr.type == ShaderLayoutType.In)
					{
						if (!m_Storage.HasKey(fieldInfo.Name))
						{
							DebugMsg(string.Format("ERROR: failed to bind field {0} (not found)", debugFieldName));
							continue;
						}

						DataStorageItem storage = m_Storage.Get(fieldInfo.Name);

						if (!ValidateFieldType(fieldInfo, storage.DataType))
							continue;

						var storageArrayExpression = storage.GetArrayExpression();
						var readSmothCall = GetReadSmothExpression(fieldInfo, storageArrayExpression);
						m_PipelineProgramDefinition.PreActions[stage].Add(Expression.Assign(fieldExpression, readSmothCall));
					}
					else if (atr.type == ShaderLayoutType.Out)
					{
						if (!ValidateFieldType(fieldInfo, P_WRITE_COLOR_TYPE))
							continue;

						var writeColorOutputCall = GetWriteColorOutputExpression(atr.location, fieldExpression);
						m_PipelineProgramDefinition.PostActions[stage].Add(writeColorOutputCall);
					}
				}
			}
		}

		private void RasterizationStage()
		{
			switch (m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pInputAssemblyState.topology)
			{
				case VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST:
					RasterizeTriangle();
					break;

				default:
					throw new NotImplementedException();
			}
		}

		private Type GetVertexBufferType(FieldInfo fieldInfo, int location)
		{
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);

			foreach (var item in m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pVertexInputState.pVertexAttributeDescriptions)
			{
				if (location == item.location)
				{
					switch (item.format)
					{
						case VkFormat.VK_FORMAT_R32G32_SFLOAT: return typeof(vec2);
						case VkFormat.VK_FORMAT_R32G32B32_SFLOAT: return typeof(vec3);
						case VkFormat.VK_FORMAT_R32G32B32A32_SFLOAT: return typeof(vec4);
					}

					DebugMsg(string.Format(
						"ERROR: unknow or unimplemented vertex buffer type {0}.{1} for field {2} (location={3}, binding={4})",
						nameof(VkFormat), item.format, debugFieldName,
						item.location, item.binding
						));
					return null;
				}
			}

			DebugMsg(string.Format(
				"ERROR: vertex buffer location={0} not found for field {1}",
				location, debugFieldName
				));

			return null;
		}

		private Expression GetReadVertexBufferExpression(FieldInfo fieldInfo, int location)
		{
			Type returnType = fieldInfo.FieldType;
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);

			foreach (var attributeDesc in m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pVertexInputState.pVertexAttributeDescriptions)
			{
				if (location == attributeDesc.location)
				{
					bool found = false;
					VkVertexInputBindingDescription inputBindingDesc = new VkVertexInputBindingDescription();
					foreach (var item in m_context.m_GraphicsPipeline.m_graphicsPipelineCreateInfo.pVertexInputState.pVertexBindingDescriptions)
					{
						if (item.binding == attributeDesc.binding)
						{
							inputBindingDesc = item;
							found = true;
							break;
						}
					}

					if (!found)
					{
						// Binding not found

						DebugMsg(string.Format(
							"ERROR: vertex buffer binding={0} not found for location={1} for field {2}",
							attributeDesc.binding, location, debugFieldName
							));
						return null;
					}

					var vb = m_context.m_CommandBuffer.m_context.m_VertexBuffers[attributeDesc.binding];
					var mem = vb.m_deviceMemory;

					int stride = inputBindingDesc.stride;
					int offset = attributeDesc.offset;
					byte[] memory = mem.m_bytes;

					if (vb.m_VertexBufferCache != null)
					{
						// Use vertex buffer cache
						Expression indexExpression = IntrospectionUtil.GetExpressionFor(() => this.gl_VertexIndex);
						return vb.m_VertexBufferCache.GetReadExpression(returnType, offset, stride, indexExpression);
					}
					else
					{
						// Read from raw
						return IntrospectionUtil.GetGenericMethodCallExpression(
							this, nameof(ReadVertexBuffer),
							new Type[] { returnType },
							Expression.Constant(memory),
							Expression.Constant(offset),
							Expression.Constant(stride));
					}
				}
			}

			DebugMsg(string.Format(
				"ERROR: vertex buffer location={0} not found for field {1}",
				location, debugFieldName
				));

			return null;
		}

		private Expression GetWriteColorOutputExpression(int location, Expression colorParam)
		{
			var framebuffer = (SoftwareFramebuffer)m_context.m_RenderPassBeginInfo.framebuffer;
			var imgView = framebuffer.m_createInfo.pAttachments[m_context.m_CurrentSubpass.pColorAttachments[location].attachment];
			Expression fragCoord = IntrospectionUtil.GetExpressionFor(() => internal_FragCoord);
			Expression call = IntrospectionUtil.GetMethodCallExpression(imgView, "SetPixel", fragCoord, colorParam);
			return call;
		}

		private Expression GetReadUniformExpression(FieldInfo fieldInfo, int binding, int set)
		{
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);

			if (binding < 0)
			{
				DebugMsg(string.Format("ERROR: missing 'binding=' form uniform field {0}", debugFieldName));
				return null;
			}

			if (set < 0 || m_context.m_DescriptorSets.Length <= set)
			{
				DebugMsg(string.Format("ERROR: uniform parameter set={0} not found for uniform field {1}", set, debugFieldName));
				return null;
			}

			if (m_context.m_DescriptorSets[set].m_Bindings.Length <= binding)
			{
				DebugMsg(string.Format("ERROR: uniform parameter binding={0} not found for uniform field {1}", binding, debugFieldName));
				return null;
			}

			Type returnType = fieldInfo.FieldType;

			var layout = m_context.m_CommandBuffer.m_context.m_DescriptorSets[set].m_Bindings[binding];

			switch (layout.descriptorType)
			{
				case VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER:
					return GetReadUniformBufferExpression(fieldInfo, binding, set);

				case VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER:
					return GetReadUniformCombinedImageSampler(fieldInfo, binding, set);

				default: throw new NotImplementedException();
			}
		}

		private Expression GetReadUniformCombinedImageSampler(FieldInfo fieldInfo, int binding, int set)
		{
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);
			Type returnType = fieldInfo.FieldType;

			var imageInfo = m_context.m_DescriptorSets[set].m_ImageInfo[binding];

			if (imageInfo.sampler == null)
			{
				DebugMsg(string.Format("ERROR: no sampler bound to uniform combined sampler set={0} binding={1} for field {2}", set, binding, debugFieldName));
				return null;
			}

			if (imageInfo.imageView == null)
			{
				DebugMsg(string.Format("ERROR: no image view bound to uniform combined sampler set={0} binding={1} for field {2}", set, binding, debugFieldName));
				return null;
			}

			SoftwareSampler2D sampler = new SoftwareSampler2D(imageInfo.imageLayout, (SoftwareImageView)imageInfo.imageView, (SoftwareSampler)imageInfo.sampler);
			return Expression.Constant(sampler);
		}

		private Expression GetReadUniformBufferExpression(FieldInfo fieldInfo, int binding, int set)
		{
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);
			Type returnType = fieldInfo.FieldType;

			var bufferInfo = m_context.m_DescriptorSets[set].m_BufferInfo[binding];

			if (bufferInfo.buffer == null)
			{
				DebugMsg(string.Format("ERROR: no buffer bound to uniform set={0} binding={1} for field {2}",
					set, binding, debugFieldName));
				return null;
			}

			SoftwareBuffer softwareBuffer = (SoftwareBuffer)bufferInfo.buffer;
			if (softwareBuffer.m_deviceMemory == null)
			{
				DebugMsg(string.Format("ERROR: no memory bound to buffer on uniform set={0} binding={1} for field {2}",
					set, binding, debugFieldName));
				return null;
			}

			SoftwareDeviceMemory mem = softwareBuffer.m_deviceMemory;

			byte[] memory = mem.m_bytes;
			int offset = bufferInfo.offset + softwareBuffer.m_memoryOffset;
			int size = Marshal.SizeOf(returnType);
			return IntrospectionUtil.GetGenericMethodCallExpression(
				this, nameof(ReadUniform),
				new Type[] { returnType },
				Expression.Constant(memory), Expression.Constant(offset), Expression.Constant(size));
		}

		private Expression GetReadSmothExpression(FieldInfo fieldInfo, Expression inputData)
		{
			string debugFieldName = string.Format("{0}.{1}", fieldInfo.DeclaringType.Name, fieldInfo.Name);

			try
			{
				return IntrospectionUtil.GetMethodCallExpression(
					this, nameof(ReadSmoth),
					inputData
					);
			}
			catch (InvalidOperationException) { }

			DebugMsg(string.Format(
				"WARNING: incompatible datatype {0} for fast interpolation for field {1}. Using slow-method instead.",
				fieldInfo.FieldType.Name, debugFieldName));

			return Expression.Convert(IntrospectionUtil.GetMethodCallExpression(
				this, nameof(ReadSmothDynamic),
				inputData
				), fieldInfo.FieldType);
		}

		private T ReadVertexBuffer<T>(byte[] memory, int offset, int stride) where T : struct
		{
			return MemoryCopyHelper.ReadStructure<T>(memory, (stride * this.gl_VertexIndex) + offset);
		}

		private T ReadUniform<T>(int binding, int set) where T : struct
		{
			var layout = m_context.m_CommandBuffer.m_context.m_DescriptorSets[set].m_Bindings[binding];
			var bufferInfo = m_context.m_CommandBuffer.m_context.m_DescriptorSets[set].m_BufferInfo[binding];

			SoftwareBuffer softwareBuffer = (SoftwareBuffer)bufferInfo.buffer;
			SoftwareDeviceMemory mem = softwareBuffer.m_deviceMemory;
			int startIndex = bufferInfo.offset + softwareBuffer.m_memoryOffset;
			T ret = new T();
			MemoryCopyHelper.ReadStructure(mem.m_bytes, startIndex, ref ret);
			return ret;
		}

		private T ReadUniform<T>(byte[] memory, int offset, int size) where T : struct
		{
			T ret = new T();
			MemoryCopyHelper.ReadStructure(memory, offset, ref ret);
			return ret;
		}

		private dynamic ReadSmothDynamic(dynamic data)
		{
			return
				(data[0] * m_PrimiteContext.RDX[0]) +
				(data[1] * m_PrimiteContext.RDX[1]) +
				(data[2] * m_PrimiteContext.RDX[2]);
		}

		private vec2 ReadSmoth(vec2[] data)
		{
			return
				(data[0] * m_PrimiteContext.RDX[0]) +
				(data[1] * m_PrimiteContext.RDX[1]) +
				(data[2] * m_PrimiteContext.RDX[2]);
		}

		private vec3 ReadSmoth(vec3[] data)
		{
			return
				(data[0] * m_PrimiteContext.RDX[0]) +
				(data[1] * m_PrimiteContext.RDX[1]) +
				(data[2] * m_PrimiteContext.RDX[2]);
		}

		/*
		private void RunStage(StageType stage)
		{
			var lastStage = CurrentStage;
			CurrentStage = stage;
			m_Stages[stage].Execute();
			CurrentStage = lastStage;
		} */

		private bool ValidateFieldType(FieldInfo fieldInfo, Type expectedType)
		{
			return ValidateFieldType(fieldInfo.DeclaringType.Name, fieldInfo.Name, fieldInfo.FieldType, expectedType);
		}

		private bool ValidateFieldType(string className, string fieldName, Type fieldType, Type expectedType)
		{
			if (fieldType == expectedType)
				return true;

			DebugMsg(string.Format("ERROR: type mismatch for field {0}.{1}. Found type {2}, expected {3}.",
				className, fieldName, fieldType.Name, expectedType.Name));
			return false;
		}

		private void DebugMsg(string msg)
		{
			Debug.WriteLine(msg);
		}

		public void Draw(int vertexCount, int instanceCount, int firstVertex, int firstInstance)
		{
			this.gl_InstanceIndex = firstInstance;
			this.CurrentStage = StageType.InputAssembler;

			for (int instanceNum = 0; instanceNum < instanceCount; instanceNum++)
			{
				this.gl_VertexIndex = firstVertex;
				while (vertexCount >= PrimitiveLength)
				{
					for (int i = 0; i < PrimitiveLength; i++)
					{
						this.PrimitiveVertexIndex = i;
						m_CompiledPipelineProgram.VertexShader();
						this.gl_VertexIndex++;
					}
					RasterizeTriangle();
					vertexCount -= this.PrimitiveLength;
				}
				this.gl_InstanceIndex++;
			}
		}

		public void DrawIndexed(int indexCount, int instanceCount, int firstIndex, int vertexOffset, int firstInstance)
		{
			m_CompiledPipelineProgram.InitializePipeline();

			this.gl_InstanceIndex = firstInstance;
			this.CurrentStage = StageType.InputAssembler;

			int indiceIndex = firstIndex;
			int[] vertexIndex32 = new int[1];
			UInt16[] vertexIndex16 = new ushort[1];
			SoftwareDeviceMemory sdmIndex = (SoftwareDeviceMemory)m_context.m_CommandBuffer.m_context.m_IndexBuffer.m_deviceMemory;
			int indexMemOffset = m_context.m_CommandBuffer.m_context.m_IndexBuffer.m_memoryOffset;
			Func<int> getIndiceFunc;

			switch (m_context.m_CommandBuffer.m_context.m_IndexBufferType)
			{
				case VkIndexType.VK_INDEX_TYPE_UINT16:
					getIndiceFunc = () =>
					{
						int srcOffset = indexMemOffset + (indiceIndex * 2);
						Buffer.BlockCopy(sdmIndex.m_bytes, srcOffset, vertexIndex16, 0, 2);
						return vertexIndex16[0];
					};
					break;

				case VkIndexType.VK_INDEX_TYPE_UINT32:
					getIndiceFunc = () =>
					{
						int srcOffset = indexMemOffset + (indiceIndex * 4);
						Buffer.BlockCopy(sdmIndex.m_bytes, srcOffset, vertexIndex32, 0, 4);
						return vertexIndex32[0];
					};
					break;

				default:
					return;
			}

			for (int instanceNum = 0; instanceNum < instanceCount; instanceNum++)
			{
				indiceIndex = firstIndex;

				while (indexCount >= PrimitiveLength)
				{
					for (int i = 0; i < PrimitiveLength; i++)
					{
						this.PrimitiveVertexIndex = i;
						this.gl_VertexIndex = getIndiceFunc();
						m_CompiledPipelineProgram.VertexShader();
						indiceIndex++;
					}
					RasterizeTriangle();
					indexCount -= this.PrimitiveLength;
				}
				this.gl_InstanceIndex++;
			}
		}

		private vec4 ViewportTransform(vec4 input)
		{
			vec4 wInput = input * (1f / input.w);
			return new vec4(
				(wInput.x + 1f) * m_CurrentViewport.width * 0.5f,
				(wInput.y + 1f) * m_CurrentViewport.height * 0.5f,
				wInput.z,
				wInput.w
				);
		}

		private void RasterizeTriangle()
		{
			m_PrimiteContext.Reset();

			for (int i = 0; i < PrimitiveLength; i++)
			{
				m_PrimiteContext.w[i] = this.gl_Position[i].w;
				this.gl_Position[i] = ViewportTransform(this.gl_Position[i]);
			}

			// back-face culling
			vec4[] v = this.gl_Position;
			float a =
				((v[1].x - v[0].x) * (v[2].y - v[0].y)) -
				((v[2].x - v[0].x) * (v[1].y - v[0].y));
			if (a > 0)
				return;

			m_PrimiteContext.v = this.gl_Position;
			m_PrimiteContext.baseIndex = 0;

			DrawTriangle(m_PrimiteContext);
		}

		private void DrawTriangle(PrimitiveContext ctx)
		{
			int itop = 0;
			if (ctx.v[ctx.iV1].y < ctx.v[itop].y) itop = ctx.iV1;
			if (ctx.v[ctx.iV2].y < ctx.v[itop].y) itop = ctx.iV2;

			ctx.iV0 = itop;
			ctx.iV1 = (itop + 1) % 3;
			ctx.iV2 = (itop + 2) % 3;

			ctx.Slope01 = GetSlope(ctx.v, ctx.iV0, ctx.iV1);
			ctx.Slope02 = GetSlope(ctx.v, ctx.iV0, ctx.iV2);

			if (ctx.Slope02 > ctx.Slope01)
			{
				int t = ctx.iV1;
				ctx.iV1 = ctx.iV2;
				ctx.iV2 = t;

				ctx.Slope01 = GetSlope(ctx.v, ctx.iV0, ctx.iV1);
				ctx.Slope02 = GetSlope(ctx.v, ctx.iV0, ctx.iV2);
			}

			ctx.Slope12 = GetSlope(ctx.v, ctx.iV1, ctx.iV2);
			ctx.Slope21 = GetSlope(ctx.v, ctx.iV2, ctx.iV1);

			int minY = (int)ctx.v[ctx.iV0].y;
			int maxY = (int)Math.Max(ctx.v[ctx.iV1].y, ctx.v[ctx.iV2].y);

			int startY = (minY < 0 ? 0 : minY);
			int endY = (maxY >= this.m_CurrentViewport.height ? (int)this.m_CurrentViewport.height - 1 : maxY);

			float yv0 = ctx.v[ctx.iV0].y;
			float yv1 = ctx.v[ctx.iV1].y;
			float yv2 = ctx.v[ctx.iV2].y;

			float xv0 = ctx.v[ctx.iV0].x;
			float xv1 = ctx.v[ctx.iV1].x;
			float xv2 = ctx.v[ctx.iV2].x;

			bool bEqY01 = (yv0 == yv1);
			bool bEqY12 = (yv1 == yv2);
			bool bEqY20 = (yv2 == yv0);

			float dy12 = bEqY12 ? 0f : 1f / (yv1 - yv2);
			float dy20 = bEqY20 ? 0f : 1f / (yv2 - yv0);
			float dy21 = bEqY12 ? 0f : 1f / (yv2 - yv1);
			float dy10 = bEqY01 ? 0f : 1f / (yv1 - yv0);


			float xe;
			float xd;
			float delta;

			for (int y = startY; y <= endY; y++)
			{

				if ((!bEqY12) && (y > yv2 || bEqY20))
				{
					xe = xv2 + ((y - yv2) * ctx.Slope21);
					ctx.dfe021 = 1f;
					delta = (y - yv2) * dy12;
					ctx.dfe21 = glm.Clamp(delta, 0f, 1f);
				}
				else if (!bEqY20)
				{
					xe = xv0 + ((y - yv0) * ctx.Slope02);
					ctx.dfe021 = 0f;
					delta = (y - yv0) * dy20;
					ctx.dfe02 = glm.Clamp(delta, 0f, 1f);
				}
				else continue;


				if ((!bEqY12) && (y > yv1 || bEqY01))
				{
					xd = xv1 + ((y - yv1) * ctx.Slope12);
					ctx.dfd012 = 1f;
					delta = (y - yv1) * dy21;
					ctx.dfd12 = glm.Clamp(delta, 0f, 1f);
				}
				else if (!bEqY01)
				{
					xd = xv0 + ((y - yv0) * ctx.Slope01);
					ctx.dfd012 = 0f;
					delta = (y - yv0) * dy10;
					ctx.dfd01 = glm.Clamp(delta, 0f, 1f);
				}
				else continue;


				DrawScanLine(ctx, y, xe, xd);
			}
		}

		private void DrawScanLine(PrimitiveContext ctx, int y, float xe, float xd)
		{
			int xmin = glm.Max((int)xe, 0);
			int xmax = glm.Min((int)xd, (int)m_CurrentViewport.width);

			int ib = ctx.baseIndex;

			float w0 = ctx.w[ctx.iV0];
			float w1 = ctx.w[ctx.iV1];
			float w2 = ctx.w[ctx.iV2];

			float dn01 = Linear2Homo(ctx.dfd01, w0, w1);
			float dn12 = Linear2Homo(ctx.dfd12, w1, w2);
			float dn02 = Linear2Homo(ctx.dfe02, w0, w2);
			float dn21 = Linear2Homo(ctx.dfe21, w2, w1);

			float dr01 = 1f - dn01;
			float dr12 = 1f - dn12;
			float dr02 = 1f - dn02;
			float dr21 = 1f - dn21;

			float bn012 = ctx.dfd012;
			float bn021 = ctx.dfe021;
			float br012 = 1f - bn012;
			float br021 = 1f - bn021;

			float R0 = dr01 * br012;
			float R1 = dr02 * br021;
			float R23 = dn01 * br012 + dr12 * bn012;
			float R4 = dn21 * bn021;
			float R5 = dn12 * bn012;
			float R67 = dn02 * br021 + dr21 * bn021;

			float wLeft = w0 * R1 + w1 * R4 + w2 * R67;
			float wRight = w0 * R0 + w1 * R23 + w2 * R5;

			float zLeft = ctx.v[ctx.iV0].z * R1 + ctx.v[ctx.iV1].z * R4 + ctx.v[ctx.iV2].z * R67;
			float zRight = ctx.v[ctx.iV0].z * R0 + ctx.v[ctx.iV1].z * R23 + ctx.v[ctx.iV2].z * R5;

			float deltaDx = (xd != xe) ? 1f / (float)(xd - xe) : 0f;

			for (int x = xmin; x <= xmax; x++)
			{
				float dx = (float)(x - xe) * deltaDx;

				dx = Linear2Homo(dx, wLeft, wRight);

				float z = zLeft + (zRight - zLeft) * dx;

				internal_FragCoord.x = x;
				internal_FragCoord.y = y;

				if (m_DepthBuffer.IsDepthTestEnabled())
				{
					if (!m_DepthBuffer.TestDepth(internal_FragCoord, z))
						continue;
				}
				m_DepthBuffer.WriteDepth(internal_FragCoord, z);

				ctx.RDX[ctx.iV0] = R1 + ((R0 - R1) * dx);
				ctx.RDX[ctx.iV1] = R4 + ((R23 - R4) * dx);
				ctx.RDX[ctx.iV2] = R67 + ((R5 - R67) * dx);

				gl_FragCoord.x = x;
				gl_FragCoord.y = y;
				gl_FragCoord.z = z;
				gl_FragCoord.w = 1; // TODO: gl_FragCoord.w = 1/w ?

				// RUN FRAGMENT SHADER
				// RunStage(StageType.FragmentShader);
				m_CompiledPipelineProgram.FragmentShader();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float GetSlope(vec4[] v, int i0, int i1)
		{
			float dy = v[i1].y - v[i0].y;
			float dx = v[i1].x - v[i0].x;
			return glm.Clamp(dx / dy, -1000f, +1000f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float Linear2Homo(float a, float w0, float w1)
		{ return (a * w0) / (w1 + (w0 - w1) * a); }
	}
}
