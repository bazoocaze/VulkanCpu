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
using System.Text;

namespace VulkanCpu.Engines.SoftwareEngine.Shaders
{
	public class ShaderLayoutAttribute : Attribute
	{
		public ShaderLayoutType type;
		public int location = -1;
		public int binding = -1;
		public int set = 0;

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("ShaderAttribute: type={0}", type);

			if (location != -1)
			{
				sb.AppendFormat(" location={0}", location);
			}

			if (binding != -1)
			{
				sb.AppendFormat(" binding={0}", binding);
				sb.AppendFormat(" set={0}", set);
			}

			return sb.ToString();
		}
	}

	public enum ShaderLayoutType
	{
		None = 0,
		In = 1,
		Out = 2,
		Uniform = 3,
	}
}
