using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex04_StagingBuffer
{
	public class Shader_Frag_04
	{
		[ShaderLayout(type = ShaderLayoutType.In)]
		public vec3 fragColor;

		[ShaderLayout(type = ShaderLayoutType.Out, location = 0)]
		public vec4 outColor;

		public vec4 gl_FragCoord;

		public void main()
		{
			outColor = new vec4(fragColor, 1.0f);
			// outColor = new vec4(gl_FragCoord.z, gl_FragCoord.z, gl_FragCoord.z, 1.0f);
		}
	}
}
