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

namespace VulkanCpu.VulkanApi
{
	/// <summary>Structure specifying a pipeline color blend attachment state.</summary>
	public struct VkPipelineColorBlendAttachmentState
	{
		/// <summary>Controls whether blending is enabled for the corresponding color attachment.
		/// If blending is not enabled, the source fragment’s color for that attachment is passed
		/// through unmodified.</summary>
		public VkBool32 blendEnable;

		/// <summary>Selects which blend factor is used to determine the source
		/// factors (Sr,Sg,Sb).</summary>
		public VkBlendFactor srcColorBlendFactor;

		/// <summary>Selects which blend factor is used to determine the destination
		/// factors (Dr,Dg,Db).</summary>
		public VkBlendFactor dstColorBlendFactor;

		/// <summary>Selects which blend operation is used to calculate the RGB values to write
		/// to the color attachment.</summary>
		public VkBlendOp colorBlendOp;

		/// <summary>Selects which blend factor is used to determine the source factor Sa.</summary>
		public VkBlendFactor srcAlphaBlendFactor;

		/// <summary>Selects which blend factor is used to determine the destination factor Da.</summary>
		public VkBlendFactor dstAlphaBlendFactor;

		/// <summary>Selects which blend operation is use to calculate the alpha values to write
		/// to the color attachment.</summary>
		public VkBlendOp alphaBlendOp;

		/// <summary>Is a bitmask of VkColorComponentFlagBits specifying which of the R, G, B,
		/// and/or A components are enabled for writing, as described for the Color Write
		/// Mask.</summary>
		public VkColorComponentFlagBits colorWriteMask;
	}

	/// <summary>Framebuffer blending factors.
	/// Legend:
	/// <para>(R, G, B, A)s0 represent the first source color components.</para>
	/// <para>(R, G, B, A)s1 represent the second source color components.</para>
	/// <para>(R, G, B, A)d represent the destination color components.</para>
	/// <para>(R, G, B, A)c represent the const color components.</para>
	/// </summary>
	public enum VkBlendFactor
	{
		/// <summary>BF = (0, 0, 0, 0)</summary>
		VK_BLEND_FACTOR_ZERO = 0,

		/// <summary>BF = (1, 1, 1, 1)</summary>
		VK_BLEND_FACTOR_ONE = 1,

		/// <summary>BF = (Rs0, Gs0, Bs0, As0)</summary>
		VK_BLEND_FACTOR_SRC_COLOR = 2,

		/// <summary>BF = (1-Rs0, 1-Gs0, 1-Bs0, 1-As0)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_SRC_COLOR = 3,

		/// <summary>BF = (Rd, Gd, Bd, Ad)</summary>
		VK_BLEND_FACTOR_DST_COLOR = 4,

		/// <summary>BF = (1-Rd, 1-Gd, 1-Bd, 1-Ad)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_DST_COLOR = 5,

		/// <summary>BF = (As0, As0, As0, As0)</summary>
		VK_BLEND_FACTOR_SRC_ALPHA = 6,

		/// <summary>BF = (1-As0, 1-As0, 1-As0, 1-As0)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA = 7,

		/// <summary>BF = (Ad, Ad, Ad, Ad)</summary>
		VK_BLEND_FACTOR_DST_ALPHA = 8,

		/// <summary>BF = (1-Ad, 1-Ad, 1-Ad, 1-Ad)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_DST_ALPHA = 9,

		/// <summary>BF = (Rc, Gc, Bc, Ac)</summary>
		VK_BLEND_FACTOR_CONSTANT_COLOR = 10,

		/// <summary>BF = (1-Rc, 1-Gc, 1-Bc, 1-Ac)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_CONSTANT_COLOR = 11,

		/// <summary>BF = (Ac, Ac, Ac, Ac)</summary>
		VK_BLEND_FACTOR_CONSTANT_ALPHA = 12,

		/// <summary>BF = (1-Ac, 1-Ac, 1-Ac, 1-Ac)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_CONSTANT_ALPHA = 13,

		/// <summary>f=min(As0, 1-Ad); BF = (f, f, f, 1)</summary>
		VK_BLEND_FACTOR_SRC_ALPHA_SATURATE = 14,

		/// <summary>BF = (Rs1, Gs1, Bs1, As1)</summary>
		VK_BLEND_FACTOR_SRC1_COLOR = 15,

		/// <summary>BF = (1-Rs1, 1-Gs1, 1-Bs1, 1-As1)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_SRC1_COLOR = 16,

		/// <summary>BF = (As1, As1, As1, As1)</summary>
		VK_BLEND_FACTOR_SRC1_ALPHA = 17,

		/// <summary>BF = (1-As1, 1-As1, 1-As1, 1-As1)</summary>
		VK_BLEND_FACTOR_ONE_MINUS_SRC1_ALPHA = 18,
	}

	/// <summary>Framebuffer blending operations.
	/// <para>Once the source and destination blend factors have been selected, they along with
	/// the source and destination components are passed to the blending operations. RGB and
	/// alpha components can use different operations.</para>
	/// Legend:
	/// <para>(R, G, B, A)s0 represent the first source color components.</para>
	/// <para>(R, G, B, A)d represent the destination color components.</para>
	/// <para>S(r, g, b, a) represent the source blend factor components.</para>
	/// <para>D(r, g, b, a) represent the destination blend factor components.</para>
	/// </summary>
	public enum VkBlendOp
	{
		/// <summary>
		/// out = src x srcBlend + dst x dstBlenc
		/// <para>R = Rs0 × Sr + Rd × Dr</para>
		/// <para>G = Gs0 × Sg + Gd × Dg</para>
		/// <para>B = Bs0 × Sb + Bd × Db</para>
		/// <para>A = As0 × Sa + Ad × Da</para>
		/// </summary>
		VK_BLEND_OP_ADD = 0,

		/// <summary>
		/// out = src x srcBlend - dst x dstBlenc
		/// <para>R = Rs0 × Sr - Rd × Dr</para>
		/// <para>G = Gs0 × Sg - Gd × Dg</para>
		/// <para>B = Bs0 × Sb - Bd × Db</para>
		/// <para>A = As0 × Sa - Ad × Da</para>
		/// </summary>
		VK_BLEND_OP_SUBTRACT = 1,

		/// <summary>
		/// out = dst x dstBlenc - src x srcBlend
		/// <para>R = Rd × Dr - Rs0 × Sr</para>
		/// <para>G = Gd × Dg - Gs0 × Sg</para>
		/// <para>B = Bd × Db - Bs0 × Sb</para>
		/// <para>A = Ad × Da - As0 × Sa</para>
		/// </summary>
		VK_BLEND_OP_REVERSE_SUBTRACT = 2,

		/// <summary>
		/// out = min(src, dst)
		/// <para>R = min(Rs0, Rd)</para>
		/// <para>G = min(Gs0, Gd)</para>
		/// <para>B = min(Bs0, Bd)</para>
		/// <para>A = min(As0, Ad)</para>
		/// </summary>
		VK_BLEND_OP_MIN = 3,

		/// <summary>
		/// out = max(src, dst)
		/// <para>R = max(Rs0, Rd)</para>
		/// <para>G = max(Gs0, Gd)</para>
		/// <para>B = max(Bs0, Bd)</para>
		/// <para>A = max(As0, Ad)</para>
		/// </summary>
		VK_BLEND_OP_MAX = 4,
	}

	/// <summary>Bitmask controlling which components are written to the framebuffer.</summary>
	[Flags]
	public enum VkColorComponentFlagBits
	{
		/// <summary>Specifies that the R value is written to the color attachment for the
		/// appropriate sample. Otherwise, the value in memory is unmodified.</summary>
		VK_COLOR_COMPONENT_R_BIT = 0x00000001,

		/// <summary>Specifies that the G value is written to the color attachment for the
		/// appropriate sample. Otherwise, the value in memory is unmodified.</summary>
		VK_COLOR_COMPONENT_G_BIT = 0x00000002,

		/// <summary>Specifies that the B value is written to the color attachment for the
		/// appropriate sample. Otherwise, the value in memory is unmodified.</summary>
		VK_COLOR_COMPONENT_B_BIT = 0x00000004,

		/// <summary>Specifies that the A value is written to the color attachment for the
		/// appropriate sample. Otherwise, the value in memory is unmodified.</summary>
		VK_COLOR_COMPONENT_A_BIT = 0x00000008,
	}
}
