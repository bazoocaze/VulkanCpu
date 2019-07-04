using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex02_SwapchainRecreation
{
	public class Shader_Frag_02
	{
		[ShaderLayout(type = ShaderLayoutType.In)]
		public vec3 fragColor;

		[ShaderLayout(location = 0, type = ShaderLayoutType.Out)]
		public vec4 outColor;

		public void main()
		{
			outColor = new vec4(fragColor, 1.0f);
		}
	}
}
