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

using GlmSharp;

namespace VulkanCpu.Engines.SoftwareEngine.Graphics
{
	public class PrimitiveContext
	{
		public vec4[] v;
		public float[] w;

		public int baseIndex;

		public int iV0;
		public int iV1;
		public int iV2;

		public float Slope01;
		public float Slope02;
		public float Slope12;
		public float Slope21;

		public float dfd01;
		public float dfd12;
		public float dfd012;

		public float dfe02;
		public float dfe21;
		public float dfe021;

		public float[] RDX;

		public PrimitiveContext()
		{
			v = new vec4[3];
			w = new float[4];
			RDX = new float[3];
		}

		public void Reset()
		{
			iV0 = 0;
			iV1 = 1;
			iV2 = 2;
		}
	}
}
