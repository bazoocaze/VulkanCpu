using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;
using static VulkanCpu.Engines.SoftwareEngine.Shaders.Shader;

namespace Examples.vulkan_tutorial.com.Ex09_LoadingModels
{
	public class Shader_Frag_09
	{
		[ShaderLayout(type = ShaderLayoutType.Uniform, binding = 1)]
		public sampler2D textSampler;

		/*
		[ShaderLayout(type = ShaderLayoutType.In, location = 0)]
		public vec3 fragColor;
		*/

		[ShaderLayout(type = ShaderLayoutType.In, location = 1)]
		public vec2 fragTextCoord;


		[ShaderLayout(type = ShaderLayoutType.Out, location = 0)]
		public vec4 outColor;


		public void main()
		{
			outColor = texture(textSampler, fragTextCoord);
		}
	}
}
