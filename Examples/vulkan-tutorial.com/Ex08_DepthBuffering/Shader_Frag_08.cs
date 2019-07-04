using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex08_DepthBuffering
{
	public class Shader_Frag_08
	{
		[ShaderLayout(type = ShaderLayoutType.Uniform, binding = 1)]
		public Shader.sampler2D textSampler;


		[ShaderLayout(type = ShaderLayoutType.In, location = 0)]
		public vec3 fragColor;

		[ShaderLayout(type = ShaderLayoutType.In, location = 1)]
		public vec2 fragTextCoord;


		public vec4 gl_FragCoord;

		[ShaderLayout(type = ShaderLayoutType.Out, location = 0)]
		public vec4 outColor;

		public void main()
		{
			outColor = Shader.texture(textSampler, fragTextCoord);
		}
	}
}
