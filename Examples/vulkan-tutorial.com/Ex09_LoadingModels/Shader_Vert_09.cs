using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex09_LoadingModels
{
	public class Shader_Vert_09
	{
		[ShaderLayout(type = ShaderLayoutType.Uniform, binding = 0)]
		UniformBufferObject ubo;



		[ShaderLayout(type = ShaderLayoutType.In, location = 0)]
		public vec3 inPosition;

		/*
		[ShaderLayout(type = ShaderLayoutType.In, location = 1)]
		public vec3 inColor;
		*/

		[ShaderLayout(type = ShaderLayoutType.In, location = 2)]
		public vec2 inTextCoord;


		/*
		[ShaderLayout(type = ShaderLayoutType.Out, location = 0)]
		public vec3 fragColor;
		*/

		[ShaderLayout(type = ShaderLayoutType.Out, location = 1)]
		public vec2 fragTextCoord;

		vec4 gl_Position;


		public void main()
		{
			gl_Position = ubo.proj * ubo.view * ubo.model * new vec4(inPosition, 1f);
			// fragColor = inColor;
			fragTextCoord = inTextCoord;
		}
	}
}
