using GlmSharp;
using VulkanCpu.Engines.SoftwareEngine.Shaders;

namespace Examples.vulkan_tutorial.com.Ex06_DescriptorPoolAndSets
{
	public class Shader_Vert_06
	{
		[ShaderLayout(type = ShaderLayoutType.Uniform, binding = 0)]
		UniformBufferObject ubo;

		[ShaderLayout(type = ShaderLayoutType.In, location = 0)]
		public vec2 inPosition;

		[ShaderLayout(type = ShaderLayoutType.In, location = 1)]
		public vec3 inColor;

		[ShaderLayout(type = ShaderLayoutType.Out)]
		public vec3 fragColor;

		vec4 gl_Position;

		public void main()
		{
			gl_Position = ubo.proj * ubo.view * ubo.model * new vec4(inPosition, 0.0f, 1f);
			fragColor = inColor;
		}
	}
}
