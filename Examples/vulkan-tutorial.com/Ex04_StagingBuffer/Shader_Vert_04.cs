using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex04_StagingBuffer
{
	public class Shader_Vert_04
	{
		[ShaderLayout(type = ShaderLayoutType.In, location = 0)]
		vec2 inPosition;

		[ShaderLayout(type = ShaderLayoutType.In, location = 1)]
		vec3 inColor;

		[ShaderLayout(type = ShaderLayoutType.Out)]
		vec3 fragColor;

		vec4 gl_Position;

		public void main()
		{
			gl_Position = new vec4(inPosition, 0.5f, 1f);
			fragColor = inColor;
		}
	}
}
