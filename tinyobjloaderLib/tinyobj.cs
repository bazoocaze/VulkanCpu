/*
The MIT License (MIT)

Copyright (c) 2012-2019 Syoyo Fujita and many contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Ported and adapted for C# from: https://github.com/syoyo/tinyobjloader by Jose Ferreira (Bazoocaze)
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GlmSharp;

namespace tinyobjloaderLib
{
	using real_t = System.Single;

	public static class tinyobj
	{
		public enum texture_type_t
		{
			TEXTURE_TYPE_NONE,  // default
			TEXTURE_TYPE_SPHERE,
			TEXTURE_TYPE_CUBE_TOP,
			TEXTURE_TYPE_CUBE_BOTTOM,
			TEXTURE_TYPE_CUBE_FRONT,
			TEXTURE_TYPE_CUBE_BACK,
			TEXTURE_TYPE_CUBE_LEFT,
			TEXTURE_TYPE_CUBE_RIGHT
		}

		public struct texture_option_t
		{
			public texture_type_t type;      // -type (default TEXTURE_TYPE_NONE)
			public real_t sharpness;         // -boost (default 1.0?)
			public real_t brightness;        // base_value in -mm option (default 0)
			public real_t contrast;          // gain_value in -mm option (default 1)
			public vec3 origin_offset;  // -o u [v [w]] (default 0 0 0)
			public vec3 scale;          // -s u [v [w]] (default 1 1 1)
			public vec3 turbulence;     // -t u [v [w]] (default 0 0 0)
										// int   texture_resolution; // -texres resolution (default = ?) TODO
			public bool clamp;    // -clamp (default false)
			public char imfchan;  // -imfchan (the default for bump is 'l' and for decal is 'm')
			public bool blendu;   // -blendu (default on)
			public bool blendv;   // -blendv (default on)
			public real_t bump_multiplier;  // -bm (for bump maps only, default 1.0)
		}

		public struct material_t
		{
			public string name;

			public vec3 ambient;
			public vec3 diffuse;
			public vec3 specular;
			public vec3 transmittance;
			public vec3 emission;
			public real_t shininess;
			public real_t ior;       // index of refraction
			public real_t dissolve;  // 1 == opaque; 0 == fully transparent
									 // illumination model (see http://www.fileformat.info/format/material/)
			public int illum;

			public int dummy;  // Suppress padding warning.

			public string ambient_texname;             // map_Ka
			public string diffuse_texname;             // map_Kd
			public string specular_texname;            // map_Ks
			public string specular_highlight_texname;  // map_Ns
			public string bump_texname;                // map_bump, map_Bump, bump
			public string displacement_texname;        // disp
			public string alpha_texname;               // map_d
			public string reflection_texname;          // refl

			public texture_option_t ambient_texopt;
			public texture_option_t diffuse_texopt;
			public texture_option_t specular_texopt;
			public texture_option_t specular_highlight_texopt;
			public texture_option_t bump_texopt;
			public texture_option_t displacement_texopt;
			public texture_option_t alpha_texopt;
			public texture_option_t reflection_texopt;

			// PBR extension
			// http://exocortex.com/blog/extending_wavefront_mtl_to_support_pbr
			public real_t roughness;            // [0, 1] default 0
			public real_t metallic;             // [0, 1] default 0
			public real_t sheen;                // [0, 1] default 0
			public real_t clearcoat_thickness;  // [0, 1] default 0
			public real_t clearcoat_roughness;  // [0, 1] default 0
			public real_t anisotropy;           // aniso. [0, 1] default 0
			public real_t anisotropy_rotation;  // anisor. [0, 1] default 0
			public real_t pad0;
			public string roughness_texname;  // map_Pr
			public string metallic_texname;   // map_Pm
			public string sheen_texname;      // map_Ps
			public string emissive_texname;   // map_Ke
			public string normal_texname;     // norm. For normal mapping.

			public texture_option_t roughness_texopt;
			public texture_option_t metallic_texopt;
			public texture_option_t sheen_texopt;
			public texture_option_t emissive_texopt;
			public texture_option_t normal_texopt;

			public int pad2;

			public Dictionary<string, string> unknown_parameter;

			public static material_t Create()
			{
				return new material_t() { unknown_parameter = new Dictionary<string, string>() };
			}
		}

		public struct tag_t
		{
			public string name;

			public List<int> intValues;
			public List<real_t> floatValues;
			public List<string> stringValues;

			public static tag_t Create()
			{
				return new tag_t() { intValues = new List<int>(), floatValues = new List<float>(), stringValues = new List<string>() };
			}
		}

		// Index struct to support different indices for vtx/normal/texcoord.
		// -1 means not used.
		public struct index_t
		{
			public int vertex_index;
			public int normal_index;
			public int texcoord_index;
		}

		public struct mesh_t
		{
			public List<index_t> indices;
			public List<byte> num_face_vertices;  // The number of vertices per
												  // face. 3 = polygon, 4 = quad,
												  // ... Up to 255.
			public List<int> material_ids;                 // per-face material ID
			public List<uint> smoothing_group_ids;  // per-face smoothing group
													// ID(0 = off. positive value
													// = group id)
			public List<tag_t> tags;                        // SubD tag

			public static mesh_t Create()
			{
				return new mesh_t() { indices = new List<index_t>(), num_face_vertices = new List<byte>(), material_ids = new List<int>(), smoothing_group_ids = new List<uint>(), tags = new List<tag_t>() };
			}
		}

		public struct shape_t
		{
			public string name;
			public mesh_t mesh;

			public static shape_t Create()
			{
				return new shape_t() { mesh = mesh_t.Create() };
			}
		}

		// Vertex attributes
		public struct attrib_t
		{
			public List<real_t> vertices;   // 'v'
			public List<real_t> normals;    // 'vn'
			public List<real_t> texcoords;  // 'vt'
			public List<real_t> colors;     // extension: vertex colors

			public static attrib_t Create()
			{
				return new attrib_t() { vertices = new List<float>(), normals = new List<float>(), texcoords = new List<float>(), colors = new List<float>() };
			}
		}

		protected class LineReader
		{
			private TextReader m_inputLine;
			private bool m_EOF;

			public LineReader(string inputLine)
			{
				if (inputLine == null)
				{
					m_EOF = true;
				}
				else
				{
					m_inputLine = new StringReader(inputLine);
				}
			}

			public bool Eof { get { return m_EOF; } }

			public string ReadWord()
			{
				if (m_EOF)
				{
					return "";
				}

				StringBuilder ret = new StringBuilder();
				while (m_inputLine.Peek() != -1)
				{
					char c = (char)m_inputLine.Read();
					if (c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0')
					{
						if (ret.Length > 0)
							break;
						continue;

					}
					ret.Append(c);
				}

				if (ret.Length == 0 || m_inputLine.Peek() == -1)
					m_EOF = true;

				return ret.ToString();
			}

			public string[] ReadWordSplit(params char[] separator)
			{
				var str = ReadWord();
				if (str != null)
					return str.Split(separator);
				return new string[0];
			}

			public int ReadInt(int default_value = 0)
			{
				return atoi(ReadWord(), default_value);
			}

			public string[] ReadSplit(params char[] separator)
			{
				var str = m_inputLine.ReadLine();
				if (str == null)
					return new string[0];
				return str.Split(separator);
			}

			public string ReadLine()
			{
				try
				{
					return m_inputLine.ReadLine();
				}
				finally
				{
					m_EOF = (m_inputLine.Peek() == -1);
				}
			}
		}

		protected class CharReader
		{
			StringReader m_input;

			public CharReader(string input)
			{
				m_input = new StringReader(input);
			}

			public bool Eof { get { return m_input.Peek() == -1; } }

			public char Current
			{
				get
				{
					int val = m_input.Peek();
					if (val == -1)
						return (char)0;
					return (char)val;
				}
			}

			public void MoveNext()
			{
				m_input.Read();
			}
		}

		protected abstract class MaterialReader
		{
			public abstract bool Load(string matId, List<material_t> materials, Dictionary<string, int> matMap, out string err);
		}

		protected class MaterialFileReader : MaterialReader
		{
			private string m_mtlBaseDir;

			public MaterialFileReader(string mtl_basedir)
			{
				this.m_mtlBaseDir = mtl_basedir;
			}

			public override bool Load(string matId, List<material_t> materials, Dictionary<string, int> matMap, out string err)
			{
				string filepath;
				err = null;

				if (!string.IsNullOrWhiteSpace(m_mtlBaseDir))
				{
					filepath = m_mtlBaseDir + matId;
				}
				else
				{
					filepath = matId;
				}

				try
				{
					using (var matIStream = new StreamReader(filepath))
					{
						string warning;
						LoadMtl(matMap, materials, matIStream, out warning);

						if (!string.IsNullOrWhiteSpace(warning))
						{
							err = warning;
						}

						return true;
					}
				}
				catch (IOException ex)
				{
					err = string.Format("WARN: Material file [ {0} ] not found: {1}", filepath, ex.Message);
					return false;
				}
			}
		}

		protected class MaterialStreamReader : MaterialReader
		{
			private Stream m_inStream;

			public MaterialStreamReader(Stream inStream)
			{
				m_inStream = inStream;
			}

			public override bool Load(string matId, List<material_t> materials, Dictionary<string, int> matMap, out string err)
			{
				err = null;

				if (!m_inStream.CanRead)
				{
					err = "WARN: Material stream in error state.";
					return false;
				}

				try
				{
					using (var matIStream = new StreamReader(m_inStream))
					{
						string warning;
						LoadMtl(matMap, materials, matIStream, out warning);

						if (!string.IsNullOrWhiteSpace(warning))
						{
							err = warning;
						}

						return true;
					}
				}
				catch (IOException ex)
				{
					err = string.Format("WARN: Material file not ready: {0}", ex.Message);
					return false;
				}
			}
		}

		protected struct vertex_index_t
		{
			public int v_idx;
			public int vt_idx;
			public int vn_idx;

			public static vertex_index_t Create()
			{
				return new vertex_index_t() { v_idx = -1, vt_idx = -1, vn_idx = -1 };
			}

			public static vertex_index_t Create(int idx)
			{
				return new vertex_index_t() { v_idx = idx, vt_idx = idx, vn_idx = idx };
			}

			public static vertex_index_t Create(int vidx, int vtidx, int vnidx)
			{
				return new vertex_index_t() { v_idx = vidx, vt_idx = vtidx, vn_idx = vnidx };
			}
		}

		// Internal data structure for face representation
		// index + smoothing group.
		protected struct face_t
		{
			public uint smoothing_group_id;  // smoothing group id. 0 = smoothing groupd is off.
			public int pad_;
			public List<vertex_index_t> vertex_indices;  // face vertex indices.

			public static face_t Create()
			{
				return new face_t() { vertex_indices = new List<vertex_index_t>() };
			}
		}

		protected struct tag_sizes
		{
			public int num_ints;
			public int num_reals;
			public int num_strings;
		}

		protected struct obj_shape
		{
			public List<real_t> v;
			public List<real_t> vn;
			public List<real_t> vt;

			public static obj_shape Create()
			{
				return new obj_shape() { v = new List<float>(), vn = new List<real_t>(), vt = new List<real_t>() };
			}
		}

		private static bool IS_SPACE(char x)
		{
			return ((x == ' ') || (x == '\t'));
		}

		private static bool IS_DIGIT(char x)
		{
			return ((uint)(x - '0') < 10);
		}

		private static bool IS_NEW_LINE(char x)
		{
			return ((x == '\r') || (x == '\n') || (x == '\0'));
		}

		// Make index zero-base, and also support relative index.
		private static bool fixIndex(int idx, int n, ref int ret)
		{
			if (idx > 0)
			{
				ret = idx - 1;
				return true;
			}

			if (idx == 0)
			{
				// zero is not allowed according to the spec.
				return false;
			}

			if (idx < 0)
			{
				ret = n + idx;  // negative value = relative
				return true;
			}

			return false;  // never reach here.
		}

		private static string parseString(LineReader token)
		{
			return token.ReadWord();
		}

		private static int parseInt(LineReader token)
		{
			return token.ReadInt();
		}

		// Tries to parse a floating point number located at s.
		//
		// s_end should be a location in the string where reading should absolutely
		// stop. For example at the end of the string, to prevent buffer overflows.
		//
		// Parses the following EBNF grammar:
		//   sign    = "+" | "-" ;
		//   END     = ? anything not in digit ?
		//   digit   = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" ;
		//   integer = [sign] , digit , {digit} ;
		//   decimal = integer , ["." , integer] ;
		//   float   = ( decimal , END ) | ( decimal , ("E" | "e") , integer , END ) ;
		//
		//  Valid strings are for example:
		//   -0  +3.1417e+2  -0.0E-3  1.0324  -1.41   11e2
		//
		// If the parsing is a success, result is set to the parsed value and true
		// is returned.
		//
		// The function is greedy and will parse until any of the following happens:
		//  - a non-conforming character is encountered.
		//  - s_end is reached.
		//
		// The following situations triggers a failure:
		//  - s >= s_end.
		//  - parse failure.
		//
		private static bool tryParseDouble(string s, object s_end, ref double result)
		{
			double mantissa = 0.0;

			// This exponent is base 2 rather than 10.
			// However the exponent we parse is supposed to be one of ten,
			// thus we must take care to convert the exponent/and or the
			// mantissa to a * 2^E, where a is the mantissa and E is the
			// exponent.
			// To get the final double we will use ldexp, it requires the
			// exponent to be in base 2.
			int exponent = 0;

			// NOTE: THESE MUST BE DECLARED HERE SINCE WE ARE NOT ALLOWED
			// TO JUMP OVER DEFINITIONS.
			char sign = '+';
			char exp_sign = '+';
			CharReader curr = new CharReader(s);

			// How many characters were read in a loop.
			int read = 0;
			// Tells whether a loop terminated due to reaching s_end.
			bool end_not_reached = false;

			/*
					BEGIN PARSING.
			*/

			// Find out what sign we've got.
			if (curr.Current == '+' || curr.Current == '-')
			{
				sign = curr.Current;
				curr.MoveNext();
			}
			else if (IS_DIGIT(curr.Current))
			{ /* Pass through. */
			}
			else
			{
				goto fail;
			}

			// Read the integer part.
			while (!curr.Eof && IS_DIGIT(curr.Current))
			{
				mantissa *= 10;
				mantissa += (int)(curr.Current - 0x30);
				curr.MoveNext();
				read++;
			}

			// We must make sure we actually got something.
			if (read == 0) goto fail;
			// We allow numbers of form "#", "###" etc.
			if (curr.Eof) goto assemble;

			// Read the decimal part.
			if (curr.Current == '.')
			{
				curr.MoveNext();
				read = 1;
				// end_not_reached = (curr != s_end);
				while ((!curr.Eof) && IS_DIGIT(curr.Current))
				{
					double[] pow_lut = { 1.0, 0.1, 0.01, 0.001, 0.0001, 0.00001, 0.000001, 0.0000001, };
					int lut_entries = pow_lut.Length;

					// NOTE: Don't use powf here, it will absolutely murder precision.
					mantissa += (int)(curr.Current - 0x30) *
								(read < lut_entries ? pow_lut[read] : Math.Pow(10.0, -read));
					read++;
					curr.MoveNext();
				}
			}
			else if (curr.Current == 'e' || curr.Current == 'E')
			{
			}
			else
			{
				goto assemble;
			}

			if (curr.Eof) goto assemble;

			// Read the exponent part.
			if (curr.Current == 'e' || curr.Current == 'E')
			{
				curr.MoveNext();
				// Figure out if a sign is present and if it is.

				if ((!curr.Eof) && (curr.Current == '+' || curr.Current == '-'))
				{
					exp_sign = curr.Current;
					curr.MoveNext();
				}
				else if (IS_DIGIT(curr.Current))
				{ /* Pass through. */
				}
				else
				{
					// Empty E is not allowed.
					goto fail;
				}

				read = 0;

				while ((!curr.Eof) && IS_DIGIT(curr.Current))
				{
					exponent *= 10;
					exponent += (int)(curr.Current - 0x30);
					curr.MoveNext();
					read++;
					end_not_reached = (curr != s_end);
				}
				exponent *= (exp_sign == '+' ? 1 : -1);
				if (read == 0) goto fail;
			}

		assemble:
			result = (sign == '+' ? 1 : -1) *
					  (exponent != 0 ? std_ldexp(mantissa * Math.Pow(5.0, exponent), exponent) : mantissa);
			return true;

		fail:
			return false;
		}

		private static double std_ldexp(double number, int exponent)
		{
			// return = number * 2 ^ exponent
			return number * Math.Pow(2, exponent);
		}

		private static real_t parseReal(LineReader token, double default_value = 0.0)
		{
			double val = default_value;
			tryParseDouble(token.ReadWord(), null, ref val);
			return (real_t)val;
		}

		private static bool parseReal(LineReader token, ref real_t outVal)
		{
			double val = 0;
			bool ret = tryParseDouble(token.ReadWord(), null, ref val);
			if (ret)
			{
				outVal = (real_t)val;
			}
			return ret;
		}

		private static void parseReal2(out real_t x, out real_t y, LineReader token, double default_x = 0.0, double default_y = 0.0)
		{
			x = parseReal(token, default_x);
			y = parseReal(token, default_y);
		}

		private static void parseReal3(out real_t x, out real_t y, out real_t z, LineReader token, double default_x = 0.0, double default_y = 0.0, double default_z = 0.0)
		{
			x = parseReal(token, default_x);
			y = parseReal(token, default_y);
			z = parseReal(token, default_z);
		}

		private static void parseV(out real_t x, out real_t y, out real_t z, out real_t w, LineReader token, double default_x = 0.0, double default_y = 0.0, double default_z = 0.0, double default_w = 1.0)
		{
			x = parseReal(token, default_x);
			y = parseReal(token, default_y);
			z = parseReal(token, default_z);
			w = parseReal(token, default_w);
		}

		// Extension: parse vertex with colors(6 items)
		private static void parseVertexWithColor(out real_t x, out real_t y, out real_t z, out real_t r, out real_t g, out real_t b, LineReader token, double default_x = 0.0, double default_y = 0.0, double default_z = 0.0)
		{
			x = parseReal(token, default_x);
			y = parseReal(token, default_y);
			z = parseReal(token, default_z);

			r = parseReal(token, 1.0);
			g = parseReal(token, 1.0);
			b = parseReal(token, 1.0);
		}

		private static bool parseOnOff(LineReader token, bool default_value = true)
		{
			bool ret = default_value;
			var str = token.ReadWord();
			if (str != null)
			{
				if (str == "on")
					ret = true;
				else if (str == "off")
					ret = false;
			}
			return ret;
		}

		private static texture_type_t parseTextureType(LineReader token, texture_type_t default_value = texture_type_t.TEXTURE_TYPE_NONE)
		{
			var str = token.ReadWord();

			if (str == null)
				return default_value;

			switch (str)
			{
				case "cube_top": return texture_type_t.TEXTURE_TYPE_CUBE_TOP;
				case "cube_bottom": return texture_type_t.TEXTURE_TYPE_CUBE_BOTTOM;
				case "cube_left": return texture_type_t.TEXTURE_TYPE_CUBE_LEFT;
				case "cube_right": return texture_type_t.TEXTURE_TYPE_CUBE_RIGHT;
				case "cube_front": return texture_type_t.TEXTURE_TYPE_CUBE_FRONT;
				case "cube_back": return texture_type_t.TEXTURE_TYPE_CUBE_BACK;
				case "sphere": return texture_type_t.TEXTURE_TYPE_SPHERE;
			}

			return default_value;
		}

		private static tag_sizes parseTagTriple(LineReader token)
		{
			tag_sizes ts = new tag_sizes();
			var list = token.ReadWordSplit('/');

			if (list.Length >= 1)
				ts.num_ints = atoi(list[0]);

			if (list.Length >= 2)
				ts.num_reals = atoi(list[1]);

			if (list.Length >= 3)
				ts.num_strings = atoi(list[2]);

			return ts;
		}

		// Parse triples with index offsets: i, i/j/k, i//k, i/j
		private static bool parseTriple(LineReader token, int vsize, int vnsize, int vtsize, ref vertex_index_t ret)
		{
			vertex_index_t vi = vertex_index_t.Create(-1);

			var list = token.ReadWordSplit('/');

			if (list.Length >= 1 && list[0].Length > 0)
			{
				if (!fixIndex(atoi(list[0]), vsize, ref vi.v_idx))
				{
					return false;
				}
			}

			if (list.Length >= 2 && list[1].Length > 0)
			{
				if (!fixIndex(atoi(list[1]), vtsize, ref vi.vt_idx))
				{
					return false;
				}
			}

			if (list.Length >= 3 && list[2].Length > 0)
			{
				if (!fixIndex(atoi(list[2]), vnsize, ref vi.vn_idx))
				{
					return false;
				}
			}

			ret = vi;
			return true;
		}

		// Parse raw triples: i, i/j/k, i//k, i/j
		private static vertex_index_t parseRawTriple(LineReader token)
		{
			vertex_index_t vi = vertex_index_t.Create(0); // 0 is an invalid index in OBJ

			var list = token.ReadWordSplit('/');

			if (list.Length >= 1 && list[0].Length > 0)
			{
				vi.v_idx = atoi(list[0]);
			}

			if (list.Length >= 2 && list[1].Length > 0)
			{
				vi.vt_idx = atoi(list[1]);
			}

			if (list.Length >= 1 && list[2].Length > 0)
			{
				vi.vn_idx = atoi(list[2]);
			}

			return vi;
		}

		private static bool ParseTextureNameAndOption(out string texname, ref texture_option_t texopt, LineReader linebuf, bool is_bump)
		{
			// @todo { write more robust lexer and parser. }
			bool found_texname = false;
			string texture_name = null;

			// Fill with default value for texopt.
			if (is_bump)
			{
				texopt.imfchan = 'l';
			}
			else
			{
				texopt.imfchan = 'm';
			}
			texopt.bump_multiplier = 1.0f;
			texopt.clamp = false;
			texopt.blendu = true;
			texopt.blendv = true;
			texopt.sharpness = 1.0f;
			texopt.brightness = 0.0f;
			texopt.contrast = 1.0f;
			texopt.origin_offset[0] = 0.0f;
			texopt.origin_offset[1] = 0.0f;
			texopt.origin_offset[2] = 0.0f;
			texopt.scale[0] = 1.0f;
			texopt.scale[1] = 1.0f;
			texopt.scale[2] = 1.0f;
			texopt.turbulence[0] = 0.0f;
			texopt.turbulence[1] = 0.0f;
			texopt.turbulence[2] = 0.0f;
			texopt.type = texture_type_t.TEXTURE_TYPE_NONE;

			string token;

			while (linebuf.Eof)
			{
				token = linebuf.ReadWord();
				if (string.IsNullOrWhiteSpace(token))
					break;

				if (token == "-blendu")
				{
					texopt.blendu = parseOnOff(linebuf, /* default */ true);
				}
				else if (token == "-blendv")
				{
					texopt.blendv = parseOnOff(linebuf, /* default */ true);
				}
				else if (token == "-clamp")
				{
					texopt.clamp = parseOnOff(linebuf, /* default */ true);
				}
				else if (token == "-boost")
				{
					texopt.sharpness = parseReal(linebuf, 1.0);
				}
				else if (token == "-bm")
				{
					texopt.bump_multiplier = parseReal(linebuf, 1.0);
				}
				else if (token == "-o")
				{
					float x, y, z;
					parseReal3(out x, out y, out z, linebuf);
					texopt.origin_offset[0] = x;
					texopt.origin_offset[1] = y;
					texopt.origin_offset[2] = z;
				}
				else if (token == "-s")
				{
					float x, y, z;
					parseReal3(out x, out y, out z, linebuf, 1, 1, 1);
					texopt.scale[0] = x;
					texopt.scale[1] = y;
					texopt.scale[2] = z;
				}
				else if (token == "-t")
				{
					float x, y, z;
					parseReal3(out x, out y, out z, linebuf);
					texopt.turbulence[0] = x;
					texopt.turbulence[1] = y;
					texopt.turbulence[2] = z;
				}
				else if (token == "-type")
				{
					texopt.type = parseTextureType(linebuf, texture_type_t.TEXTURE_TYPE_NONE);
				}
				else if (token == "-imfchan")
				{
					var str = linebuf.ReadWord();
					if (str != null && str.Length >= 1)
						texopt.imfchan = str[0];
				}
				else if (token == "-mm")
				{
					parseReal2(out texopt.brightness, out texopt.contrast, linebuf, 0.0, 1.0);
				}
				else
				{
					// Assume texture filename
					// Read filename until line end to parse filename containing whitespace
					// TODO(syoyo): Support parsing texture option flag after the filename.
					texture_name = token;
					found_texname = true;
				}
			}

			if (found_texname)
			{
				texname = texture_name;
				return true;
			}
			else
			{
				texname = "";
				return false;
			}
		}

		private static void InitMaterial(ref material_t material)
		{
			material = new material_t();

			material.name = "";
			material.ambient_texname = "";
			material.diffuse_texname = "";
			material.specular_texname = "";
			material.specular_highlight_texname = "";
			material.bump_texname = "";
			material.displacement_texname = "";
			material.reflection_texname = "";
			material.alpha_texname = "";

			for (int i = 0; i < 3; i++)
			{
				material.ambient[i] = 0f;
				material.diffuse[i] = 0f;
				material.specular[i] = 0f;
				material.transmittance[i] = 0f;
				material.emission[i] = 0f;
			}

			material.illum = 0;
			material.dissolve = 1f;
			material.shininess = 1f;
			material.ior = 1f;

			material.roughness = 0f;
			material.metallic = 0f;
			material.sheen = 0f;
			material.clearcoat_thickness = 0f;
			material.clearcoat_roughness = 0f;
			material.anisotropy_rotation = 0f;
			material.anisotropy = 0f;
			material.roughness_texname = "";
			material.metallic_texname = "";
			material.sheen_texname = "";
			material.emissive_texname = "";
			material.normal_texname = "";

			material.unknown_parameter.Clear();
		}

		// code from https://wrf.ecse.rpi.edu//Research/Short_Notes/pnpoly.html
		private static bool pnpoly(int nvert, float[] vertx, float[] verty, float testx, float testy)
		{
			int i, j;
			bool c = false;
			for (i = 0, j = nvert - 1; i < nvert; j = i++)
			{
				if (((verty[i] > testy) != (verty[j] > testy)) &&
					(testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
					c = !c;
			}
			return c;
		}

		// TODO(syoyo): refactor function.
		private static bool exportFaceGroupToShape(ref shape_t shape, List<face_t> faceGroup, List<tag_t> tags, int material_id, string name, bool triangulate, List<float> v)
		{
			if (!faceGroup.Any())
			{
				return false;
			}

			// Flatten vertices and indices
			for (int i = 0; i < faceGroup.Count; i++)
			{
				face_t face = faceGroup[i];

				if (face.vertex_indices.Count < 3)
				{
					// Face must have 3+ vertices.
					continue;
				}

				vertex_index_t i0 = face.vertex_indices[0];
				vertex_index_t i1 = vertex_index_t.Create(-1);
				vertex_index_t i2 = face.vertex_indices[1];

				int npolys = face.vertex_indices.Count;

				if (triangulate)
				{
					// find the two axes to work in
					int[] axes = new int[] { 1, 2 };
					for (int k = 0; k < npolys; ++k)
					{
						i0 = face.vertex_indices[(k + 0) % npolys];
						i1 = face.vertex_indices[(k + 1) % npolys];
						i2 = face.vertex_indices[(k + 2) % npolys];
						int vi0 = i0.v_idx;
						int vi1 = i1.v_idx;
						int vi2 = i2.v_idx;
						real_t v0x = v[vi0 * 3 + 0];
						real_t v0y = v[vi0 * 3 + 1];
						real_t v0z = v[vi0 * 3 + 2];
						real_t v1x = v[vi1 * 3 + 0];
						real_t v1y = v[vi1 * 3 + 1];
						real_t v1z = v[vi1 * 3 + 2];
						real_t v2x = v[vi2 * 3 + 0];
						real_t v2y = v[vi2 * 3 + 1];
						real_t v2z = v[vi2 * 3 + 2];
						real_t e0x = v1x - v0x;
						real_t e0y = v1y - v0y;
						real_t e0z = v1z - v0z;
						real_t e1x = v2x - v1x;
						real_t e1y = v2y - v1y;
						real_t e1z = v2z - v1z;
						float cx = Math.Abs(e0y * e1z - e0z * e1y);
						float cy = Math.Abs(e0z * e1x - e0x * e1z);
						float cz = Math.Abs(e0x * e1y - e0y * e1x);
						const float epsilon = 0.0001f;
						if (cx > epsilon || cy > epsilon || cz > epsilon)
						{
							// found a corner
							if (cx > cy && cx > cz)
							{
							}
							else
							{
								axes[0] = 0;
								if (cz > cx && cz > cy) axes[1] = 1;
							}
							break;
						}
					}

					real_t area = 0;
					for (int k = 0; k < npolys; ++k)
					{
						i0 = face.vertex_indices[(k + 0) % npolys];
						i1 = face.vertex_indices[(k + 1) % npolys];
						int vi0 = i0.v_idx;
						int vi1 = i1.v_idx;
						real_t v0x = v[vi0 * 3 + axes[0]];
						real_t v0y = v[vi0 * 3 + axes[1]];
						real_t v1x = v[vi1 * 3 + axes[0]];
						real_t v1y = v[vi1 * 3 + axes[1]];
						area += (v0x * v1y - v0y * v1x) * 0.5f;
					}

					int maxRounds =
						10;  // arbitrary max loop count to protect against unexpected errors

					face_t remainingFace = face;  // copy
					int guess_vert = 0;
					vertex_index_t[] ind = new vertex_index_t[3];
					real_t[] vx = new real_t[3];
					real_t[] vy = new real_t[3];
					while (remainingFace.vertex_indices.Count > 3 && maxRounds > 0)
					{
						npolys = remainingFace.vertex_indices.Count;
						if (guess_vert >= npolys)
						{
							maxRounds -= 1;
							guess_vert -= npolys;
						}
						for (int k = 0; k < 3; k++)
						{
							ind[k] = remainingFace.vertex_indices[(guess_vert + k) % npolys];
							int vi = ind[k].v_idx;
							vx[k] = v[vi * 3 + axes[0]];
							vy[k] = v[vi * 3 + axes[1]];
						}
						real_t e0x = vx[1] - vx[0];
						real_t e0y = vy[1] - vy[0];
						real_t e1x = vx[2] - vx[1];
						real_t e1y = vy[2] - vy[1];
						real_t cross = e0x * e1y - e0y * e1x;
						// if an internal angle
						if (cross * area < 0.0f)
						{
							guess_vert += 1;
							continue;
						}

						// check all other verts in case they are inside this triangle
						bool overlap = false;
						for (int otherVert = 3; otherVert < npolys; ++otherVert)
						{
							int ovi = (int)(
								remainingFace.vertex_indices[(guess_vert + otherVert) % npolys]
									.v_idx);
							real_t tx = v[ovi * 3 + axes[0]];
							real_t ty = v[ovi * 3 + axes[1]];
							if (pnpoly(3, vx, vy, tx, ty))
							{
								overlap = true;
								break;
							}
						}

						if (overlap)
						{
							guess_vert += 1;
							continue;
						}

						// this triangle is an ear
						{
							index_t idx0, idx1, idx2;
							idx0.vertex_index = ind[0].v_idx;
							idx0.normal_index = ind[0].vn_idx;
							idx0.texcoord_index = ind[0].vt_idx;
							idx1.vertex_index = ind[1].v_idx;
							idx1.normal_index = ind[1].vn_idx;
							idx1.texcoord_index = ind[1].vt_idx;
							idx2.vertex_index = ind[2].v_idx;
							idx2.normal_index = ind[2].vn_idx;
							idx2.texcoord_index = ind[2].vt_idx;

							shape.mesh.indices.Add(idx0);
							shape.mesh.indices.Add(idx1);
							shape.mesh.indices.Add(idx2);

							shape.mesh.num_face_vertices.Add(3);
							shape.mesh.material_ids.Add(material_id);
							shape.mesh.smoothing_group_ids.Add(face.smoothing_group_id);
						}

						// remove v1 from the list
						int removed_vert_index = (guess_vert + 1) % npolys;
						while (removed_vert_index + 1 < npolys)
						{
							remainingFace.vertex_indices[removed_vert_index] =
								remainingFace.vertex_indices[removed_vert_index + 1];
							removed_vert_index += 1;
						}
						remainingFace.vertex_indices.RemoveAt(remainingFace.vertex_indices.Count - 1);
					}

					if (remainingFace.vertex_indices.Count == 3)
					{
						i0 = remainingFace.vertex_indices[0];
						i1 = remainingFace.vertex_indices[1];
						i2 = remainingFace.vertex_indices[2];
						{
							index_t idx0, idx1, idx2;
							idx0.vertex_index = i0.v_idx;
							idx0.normal_index = i0.vn_idx;
							idx0.texcoord_index = i0.vt_idx;
							idx1.vertex_index = i1.v_idx;
							idx1.normal_index = i1.vn_idx;
							idx1.texcoord_index = i1.vt_idx;
							idx2.vertex_index = i2.v_idx;
							idx2.normal_index = i2.vn_idx;
							idx2.texcoord_index = i2.vt_idx;

							shape.mesh.indices.Add(idx0);
							shape.mesh.indices.Add(idx1);
							shape.mesh.indices.Add(idx2);

							shape.mesh.num_face_vertices.Add(3);
							shape.mesh.material_ids.Add(material_id);
							shape.mesh.smoothing_group_ids.Add(face.smoothing_group_id);
						}
					}
				}
				else
				{
					for (int k = 0; k < npolys; k++)
					{
						index_t idx;
						idx.vertex_index = face.vertex_indices[k].v_idx;
						idx.normal_index = face.vertex_indices[k].vn_idx;
						idx.texcoord_index = face.vertex_indices[k].vt_idx;
						shape.mesh.indices.Add(idx);
					}

					shape.mesh.num_face_vertices.Add((byte)npolys);
					shape.mesh.material_ids.Add(material_id);  // per face
					shape.mesh.smoothing_group_ids.Add(face.smoothing_group_id);  // per face
				}
			}

			shape.name = name;
			shape.mesh.tags = tags;

			return true;
		}

		private static void LoadMtl(Dictionary<string, int> material_map, List<material_t> materials, TextReader inStream, out string warning)
		{
			warning = "";
			// Create a default material anyway.
			material_t material = material_t.Create();
			InitMaterial(ref material);

			// Issue 43. `d` wins against `Tr` since `Tr` is not in the MTL specification.
			bool has_d = false;
			bool has_tr = false;

			while (inStream.Peek() != -1)
			{
				var lineData = inStream.ReadLine();
				if (String.IsNullOrWhiteSpace(lineData))
					continue;

				LineReader linebuf = new LineReader(lineData);

				string token = linebuf.ReadWord();
				if (string.IsNullOrWhiteSpace(token))
					continue;

				if (token.StartsWith("#"))
					continue;  // comment line

				// new mtl
				if (token == "newmtl")
				{
					// flush previous material.
					if (!String.IsNullOrWhiteSpace(material.name))
					{
						material_map.Add(material.name, materials.Count);
						materials.Add(material);
					}

					// initial temporary material
					InitMaterial(ref material);

					has_d = false;
					has_tr = false;

					material.name = linebuf.ReadWord();
					continue;
				}

				// ambient
				if (token == "Ka")
				{
					real_t r, g, b;
					parseReal3(out r, out g, out b, linebuf);
					material.ambient[0] = r;
					material.ambient[1] = g;
					material.ambient[2] = b;
					continue;
				}

				// diffuse
				if (token == "Kd")
				{
					real_t r, g, b;
					parseReal3(out r, out g, out b, linebuf);
					material.diffuse[0] = r;
					material.diffuse[1] = g;
					material.diffuse[2] = b;
					continue;
				}

				// specular
				if (token == "Ks")
				{
					real_t r, g, b;
					parseReal3(out r, out g, out b, linebuf);
					material.specular[0] = r;
					material.specular[1] = g;
					material.specular[2] = b;
					continue;
				}

				// transmittance
				if (token == "Kt" || token == "Tf")
				{
					real_t r, g, b;
					parseReal3(out r, out g, out b, linebuf);
					material.transmittance[0] = r;
					material.transmittance[1] = g;
					material.transmittance[2] = b;
					continue;
				}

				// ior(index of refraction)
				if (token == "Ni")
				{
					material.ior = parseReal(linebuf);
					continue;
				}

				// emission
				if (token == "Ke")
				{
					real_t r, g, b;
					parseReal3(out r, out g, out b, linebuf);
					material.emission[0] = r;
					material.emission[1] = g;
					material.emission[2] = b;
					continue;
				}

				// shininess
				if (token == "Ns")
				{
					material.shininess = parseReal(linebuf);
					continue;
				}

				// illum model
				if (token == "illum")
				{
					material.illum = parseInt(linebuf);
					continue;
				}

				// dissolve
				if (token == "d")
				{
					material.dissolve = parseReal(linebuf);

					if (has_tr)
					{
						warning += "WARN: Both `d` and `Tr` parameters defined for \"" + material.name + "\". Use the value of `d` for dissolve.";
					}
					has_d = true;
					continue;
				}

				if (token == "Tr")
				{
					if (has_d)
					{
						// `d` wins. Ignore `Tr` value.
						warning += "WARN: Both `d` and `Tr` parameters defined for \"" + material.name + "\". Use the value of `d` for dissolve.";
					}
					else
					{
						// We invert value of Tr(assume Tr is in range [0, 1])
						// NOTE: Interpretation of Tr is application(exporter) dependent. For
						// some application(e.g. 3ds max obj exporter), Tr = d(Issue 43)
						material.dissolve = 1.0f - parseReal(linebuf);
					}
					has_tr = true;
					continue;
				}

				// PBR: roughness
				if (token == "Pr")
				{
					material.roughness = parseReal(linebuf);
					continue;
				}

				// PBR: metallic
				if (token == "Pm")
				{
					material.metallic = parseReal(linebuf);
					continue;
				}

				// PBR: sheen
				if (token == "Ps")
				{
					material.sheen = parseReal(linebuf);
					continue;
				}

				// PBR: clearcoat thickness
				if (token == "Pc")
				{
					material.clearcoat_thickness = parseReal(linebuf);
					continue;
				}

				// PBR: clearcoat roughness
				if (token == "Pcr")
				{
					material.clearcoat_roughness = parseReal(linebuf);
					continue;
				}

				// PBR: anisotropy
				if (token == "aniso")
				{
					material.anisotropy = parseReal(linebuf);
					continue;
				}

				// PBR: anisotropy rotation
				if (token == "anisor")
				{
					material.anisotropy_rotation = parseReal(linebuf);
					continue;
				}

				// ambient texture
				if (token == "map_Ka")
				{
					ParseTextureNameAndOption(
						out material.ambient_texname,
						ref material.ambient_texopt,
						linebuf, /* is_bump */ false);
					continue;
				}

				// diffuse texture
				if (token == "map_Kd")
				{
					ParseTextureNameAndOption(out (material.diffuse_texname),
											  ref (material.diffuse_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// specular texture
				if (token == "map_Ks")
				{
					ParseTextureNameAndOption(out (material.specular_texname),
											  ref (material.specular_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// specular highlight texture
				if (token == "map_Ns")
				{
					ParseTextureNameAndOption(out (material.specular_highlight_texname),
											  ref (material.specular_highlight_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// bump texture
				if (token == "map_bump" || token == "map_Bump" || token == "bump")
				{
					ParseTextureNameAndOption(out (material.bump_texname),
											  ref (material.bump_texopt),
											  linebuf,
											  /* is_bump */ true);
					continue;
				}

				// alpha texture
				if (token == "map_d")
				{
					material.alpha_texname = linebuf.ReadWord();
					ParseTextureNameAndOption(out (material.alpha_texname),
											  ref (material.alpha_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// displacement texture
				if (token == "disp")
				{
					ParseTextureNameAndOption(out (material.displacement_texname),
											  ref (material.displacement_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// reflection map
				if (token == "refl")
				{
					ParseTextureNameAndOption(out (material.reflection_texname),
											  ref (material.reflection_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// PBR: roughness texture
				if (token == "map_Pr")
				{
					ParseTextureNameAndOption(out (material.roughness_texname),
											  ref (material.roughness_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// PBR: metallic texture
				if (token == "map_Pm")
				{
					ParseTextureNameAndOption(out (material.metallic_texname),
											  ref (material.metallic_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// PBR: sheen texture
				if (token == "map_Ps")
				{
					ParseTextureNameAndOption(out (material.sheen_texname),
											  ref (material.sheen_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// PBR: emissive texture
				if (token == "map_Ke")
				{
					ParseTextureNameAndOption(out (material.emissive_texname),
											  ref (material.emissive_texopt),
											  linebuf,
											  /* is_bump */ false);
					continue;
				}

				// PBR: normal map texture
				if (token == "norm")
				{
					ParseTextureNameAndOption(
						out (material.normal_texname), ref (material.normal_texopt), linebuf,
						/* is_bump */ false);  // @fixme { is_bump will be true? }
					continue;
				}

				material.unknown_parameter.Add(token, linebuf.ReadLine());
			}

			// flush last material.
			material_map.Add(material.name, materials.Count);
			materials.Add(material);
		}

		public static bool LoadObj(out attrib_t attrib, List<shape_t> shapes, List<material_t> materials, out string err, string filename, string mtl_basedir = null, bool triangulate = true)
		{
			attrib = attrib_t.Create();
			shapes.Clear();

			try
			{
				using (StreamReader ifs = new StreamReader(filename))
				{
					MaterialFileReader matFileReader = new MaterialFileReader(mtl_basedir);

					return LoadObj(ref attrib, shapes, materials, out err, ifs, matFileReader, triangulate);
				}
			}
			catch (IOException ex)
			{
				err = string.Format("Cannot open file {0}: {1}", filename, ex.Message);
				return false;
			}
		}

		private static bool LoadObj(ref attrib_t attrib, List<shape_t> shapes, List<material_t> materials, out string err, TextReader inStream, MaterialReader readMatFn, bool triangulate)
		{
			List<real_t> v = new List<real_t>();
			List<real_t> vn = new List<real_t>();
			List<real_t> vt = new List<real_t>();
			List<real_t> vc = new List<real_t>();
			List<tag_t> tags = new List<tag_t>();
			List<face_t> faceGroup = new List<face_t>();
			string name = null;

			// material
			Dictionary<string, int> material_map = new Dictionary<string, int>();
			int material = -1;

			// smoothing group id
			uint current_smoothing_id = 0; // Initial value. 0 means no smoothing.

			shape_t shape = shape_t.Create();

			err = "";

			while (inStream.Peek() != -1)
			{
				string linebuf = inStream.ReadLine();

				if (string.IsNullOrWhiteSpace(linebuf))
					continue;

				LineReader lineReader = new LineReader(linebuf);
				string token = lineReader.ReadWord();

				if (token.StartsWith("#") || string.IsNullOrWhiteSpace(token))
				{
					continue;
				}

				// vertex
				if (token == "v")
				{
					real_t x = default(real_t);
					real_t y = default(real_t);
					real_t z = default(real_t);
					real_t r = default(real_t);
					real_t g = default(real_t);
					real_t b = default(real_t);
					parseVertexWithColor(out x, out y, out z, out r, out g, out b, lineReader);
					v.Add(x);
					v.Add(y);
					v.Add(z);
					vc.Add(r);
					vc.Add(g);
					vc.Add(b);
					continue;
				}

				// normal
				if (token == "vn")
				{
					real_t x = default(real_t);
					real_t y = default(real_t);
					real_t z = default(real_t);
					parseReal3(out x, out y, out z, lineReader);
					vn.Add(x);
					vn.Add(y);
					vn.Add(z);
					continue;
				}

				// texcoord
				if (token == "vt")
				{
					real_t x = default(real_t);
					real_t y = default(real_t);
					parseReal2(out x, out y, lineReader);
					vt.Add(x);
					vt.Add(y);
					continue;
				}

				// face
				if (token == "f")
				{
					face_t face = face_t.Create();
					face.smoothing_group_id = current_smoothing_id;

					while (!lineReader.Eof)
					{
						vertex_index_t vi = vertex_index_t.Create();
						if (!parseTriple(lineReader, (int)(v.Count / 3), (int)(vn.Count / 3), (int)(vt.Count / 2), ref vi))
						{
							err = "Failed parse 'f' line(e.g zero value for face index).";
							return false;
						}

						face.vertex_indices.Add(vi);
					}

					faceGroup.Add(face);

					continue;
				}

				// use mtl
				if (token == "usemtl")
				{
					int newMaterialId = -1;
					token = lineReader.ReadWord();

					if (material_map.ContainsKey(token))
					{
						newMaterialId = material_map[token];
					}
					else
					{
						// { error!! material not found }
					}

					if (newMaterialId != material)
					{
						// Create per-face material. Thus we don't add `shape` to `shapes` at
						// this time.
						// just clear `faceGroup` after `exportFaceGroupToShape()` call.
						exportFaceGroupToShape(ref shape, faceGroup, tags, material, name, triangulate, v);
						faceGroup.Clear();
						material = newMaterialId;
					}
				}

				// load mtl
				if (token == "mtllib")
				{
					if (readMatFn != null)
					{
						string[] filenames = lineReader.ReadSplit(' ', '\t');

						if (filenames.Length == 0)
						{
							err += "WARN: Looks like empty filename for mtllib. Use default material. ";
						}
						else
						{
							bool found = false;
							for (int s = 0; s < filenames.Length; s++)
							{
								string err_mtl;
								bool ok = readMatFn.Load(filenames[s].Trim(), materials, material_map, out err_mtl);
								if (!string.IsNullOrWhiteSpace(err_mtl))
								{
									err += err_mtl;
								}

								if (ok)
								{
									found = true;
									break;
								}
							}

							if (!found)
							{
								err += "WARN: Failed to load material file(s). Use default material. ";
							}
						}
					}
					continue;
				}

				if (token == "g")
				{
					// flush previous face group;
					exportFaceGroupToShape(ref shape, faceGroup, tags, material, name, triangulate, v);

					if (shape.mesh.indices.Count > 0)
					{
						shapes.Add(shape);
					}

					// material = -1;
					faceGroup.Clear();
					shape = shape_t.Create();

					name = lineReader.ReadWord() ?? "";

					continue;
				}

				// object name
				if (token == "o")
				{
					// flush previous face group;
					bool ret1 = exportFaceGroupToShape(ref shape, faceGroup, tags, material, name, triangulate, v);

					if (ret1)
					{
						shapes.Add(shape);
					}

					// material = -1;
					faceGroup.Clear();
					shape = shape_t.Create();

					/// TODO: { multiple object name? }
					name = lineReader.ReadWord() ?? "";

					continue;
				}

				if (token == "t")
				{
					tag_t tag = tag_t.Create();

					tag.name = parseString(lineReader);

					tag_sizes ts = parseTagTriple(lineReader);

					for (int i = 0; i < ts.num_ints; i++)
					{
						tag.intValues.Add(parseInt(lineReader));
					}

					for (int i = 0; i < ts.num_reals; i++)
					{
						tag.floatValues.Add(parseReal(lineReader));
					}

					for (int i = 0; i < ts.num_strings; i++)
					{
						tag.stringValues.Add(parseString(lineReader));
					}

					tags.Add(tag);

					continue;
				}

				if (token == "s")
				{
					// smoothing group id
					token = lineReader.ReadWord();

					if (string.IsNullOrWhiteSpace(token))
						continue;

					if (token.Length >= 3)
					{
						if (token.StartsWith("off"))
						{
							current_smoothing_id = 0;
						}
					}
					else
					{
						// assume number
						int smGroupId = parseInt(new LineReader(token));
						if (smGroupId < 0)
						{
							// parse error. force set to 0.
							// FIXME(syoyo): Report warning.
							current_smoothing_id = 0;
						}
						else
						{
							current_smoothing_id = (uint)smGroupId;
						}
					}

					continue;
				} // smoothing group id

				// Ignore unknown command.
			}

			bool ret = exportFaceGroupToShape(ref shape, faceGroup, tags, material, name, triangulate, v);
			// exportFaceGroupToShape return false when `usemtl` is called in the last
			// line.
			// we also add `shape` to `shapes` when `shape.mesh` has already some
			// faces(indices)
			if (ret || shape.mesh.indices.Count > 0)
			{
				shapes.Add(shape);
			}
			faceGroup.Clear();  // for safety

			attrib.vertices = v;
			attrib.normals = vn;
			attrib.texcoords = vt;
			attrib.colors = vc;

			return true;
		}

		private static int atoi(string input, int default_value = 0)
		{
			int ret;
			if (input != null && Int32.TryParse(input, out ret))
				return ret;
			return default_value;
		}
	}
}
