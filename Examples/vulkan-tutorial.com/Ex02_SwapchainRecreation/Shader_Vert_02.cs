using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex02_SwapchainRecreation
{
	public class Shader_Vert_02
	{
		vec2[] positions = new vec2[3]
		{
			new vec2(0.0f, -0.5f),
			new vec2(0.5f, 0.5f),
			new vec2(-0.5f, 0.5f),
		};

		vec3[] colors = new vec3[3]
		{
			new vec3(1.0f, 0.0f, 0.0f),
			new vec3(0.0f, 1.0f, 0.0f),
			new vec3(0.0f, 0.0f, 1.0f),
		};

		[ShaderLayout(type = ShaderLayoutType.Out)]
		public vec3 fragColor;

		[ShaderLayout]
		public vec4 gl_Position;

		[ShaderLayout]
		public int gl_VertexIndex;

		public void main()
		{
			gl_Position = new vec4(positions[gl_VertexIndex], 0.0f, 1.0f);
			fragColor = colors[gl_VertexIndex];
		}
	}
}
