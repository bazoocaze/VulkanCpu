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

namespace VulkanCpu.VulkanApi
{
	/// <summary>Structure specifying parameters of a newly created pipeline rasterization state.
	/// </summary>
	public struct VkPipelineRasterizationStateCreateInfo
	{
		/// <summary>Is the type of this structure.</summary>
		public VkStructureType sType;

		/// <summary>Is NULL or a pointer to an extension-specific structure.</summary>
		public object pNext;

		/// <summary>RESERVED</summary>
		public int flags;

		/// <summary>Controls whether to clamp the fragment’s depth values instead of clipping
		/// primitives to the z planes of the frustum, as described in Primitive Clipping.</summary>
		public VkBool32 depthClampEnable;

		/// <summary>Controls whether primitives are discarded immediately before the rasterization
		/// stage.</summary>
		public VkBool32 rasterizerDiscardEnable;

		/// <summary>Is the triangle rendering mode. See VkPolygonMode.</summary>
		public VkPolygonMode polygonMode;

		/// <summary>Is the triangle facing direction used for primitive culling. See
		/// VkCullModeFlagBits.</summary>
		public VkCullModeFlagBits cullMode;

		/// <summary>Is a VkFrontFace value specifying the front-facing triangle orientation to be
		/// used for culling.</summary>
		public VkFrontFace frontFace;

		/// <summary>Controls whether to bias fragment depth values.</summary>
		public VkBool32 depthBiasEnable;

		/// <summary>Is a scalar factor controlling the constant depth value added to each
		/// fragment.</summary>
		public float depthBiasConstantFactor;

		/// <summary>Is the maximum (or minimum) depth bias of a fragment.</summary>
		public float depthBiasClamp;

		/// <summary>Is a scalar factor applied to a fragment’s slope in depth bias calculations.
		/// </summary>
		public float depthBiasSlopeFactor;

		/// <summary>Is the width of rasterized line segments.</summary>
		public float lineWidth;
	}

	/// <summary>Control polygon rasterization mode.</summary>
	public enum VkPolygonMode
	{
		/// <summary>Specifies that polygon vertices are drawn as points.</summary>
		VK_POLYGON_MODE_FILL = 0,

		/// <summary>Specifies that polygon edges are drawn as line segments.</summary>
		VK_POLYGON_MODE_LINE = 1,

		/// <summary>Specifies that polygon vertices are drawn as points.</summary>
		VK_POLYGON_MODE_POINT = 2,

		/// <summary>Specifies that polygons are rendered using polygon rasterization rules,
		/// modified to consider a sample within the primitive if the sample location is inside
		/// the axis-aligned bounding box of the triangle after projection.<para>Note that the
		/// barycentric weights used in attribute interpolation can extend outside the range [0,1]
		/// when these primitives are shaded. Special treatment is given to a sample position on the
		/// boundary edge of the bounding box. In such a case, if two rectangles lie on either side
		/// of a common edge (with identical endpoints) on which a sample position lies, then exactly
		/// one of the triangles must produce a fragment that covers that sample during
		/// rasterization.</para></summary>
		VK_POLYGON_MODE_FILL_RECTANGLE_NV = 1000153000,
	}

	/// <summary>Bitmask controlling triangle culling.</summary>
	public enum VkCullModeFlagBits
	{
		/// <summary>Specifies that no triangles are discarded</summary>
		VK_CULL_MODE_NONE = 0,

		/// <summary>Specifies that front-facing triangles are discarded</summary>
		VK_CULL_MODE_FRONT_BIT = 0x00000001,

		/// <summary>Specifies that back-facing triangles are discarded</summary>
		VK_CULL_MODE_BACK_BIT = 0x00000002,

		/// <summary>specifies that all triangles are discarded.</summary>
		VK_CULL_MODE_FRONT_AND_BACK = 0x00000003,
	}

	/// <summary>
	/// Interpret polygon front-facing orientation.
	/// Any triangle which is not front-facing is back-facing, including zero-area triangles.
	/// </summary>
	public enum VkFrontFace
	{
		/// <summary>Specifies that a triangle with positive area is considered front-facing.
		/// </summary>
		VK_FRONT_FACE_COUNTER_CLOCKWISE = 0,

		/// <summary>Specifies that a triangle with negative area is considered front-facing.
		/// </summary>
		VK_FRONT_FACE_CLOCKWISE = 1,
	}
}
