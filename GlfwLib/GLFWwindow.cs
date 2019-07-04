/*
MIT License

Copyright (c) 2019 Jose Ferreira (Bazoocaze)

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
*/

using System;
using System.Windows.Forms;

namespace GlfwLib
{
	public class GLFWwindow : IDisposable
	{
		private Form m_Form;
		private int m_Width;
		private int m_Height;
		internal object m_userData;
		private GlfwWindowSizeCallbackDelegate m_onWindowResized;
		private GlfwKeyCallbackDelegate m_onKey;

		public GLFWwindow(int width, int height, string title)
		{
			m_Form = new Form();
			m_Form.Width = width;
			m_Form.Height = height;
			m_Form.Text = title;

			// Adjust size to compensate borders and title bar
			int diffWidth = width - m_Form.ClientSize.Width;
			int diffHeight = height - m_Form.ClientSize.Height;

			if (diffWidth > 0)
				m_Form.Width = width + diffWidth;

			if (diffHeight > 0)
				m_Form.Height = height + diffHeight;

			m_Form.FormClosing += M_Form_FormClosing;
			m_Form.Resize += M_Form_Resize;
			m_Form.KeyDown += M_Form_KeyDown;
			m_Form.KeyUp += M_Form_KeyUp;

			m_Form.Show();

			m_Width = m_Form.ClientSize.Width;
			m_Height = m_Form.ClientSize.Height;
		}

		private void M_Form_KeyUp(object sender, KeyEventArgs e)
		{
			OnKey(GetKeyValue(e), GetKeyScanCode(e), GLFWConstants.GLFW_RELEASE, GetMods(e));
		}

		private int GetKeyScanCode(KeyEventArgs e)
		{
			return e.KeyValue;
		}

		private int GetKeyValue(KeyEventArgs e)
		{
			return e.KeyValue;
		}

		private int GetMods(KeyEventArgs e)
		{
			return 0;
		}

		private void M_Form_KeyDown(object sender, KeyEventArgs e)
		{
			OnKey(GetKeyValue(e), GetKeyScanCode(e), GLFWConstants.GLFW_PRESS, GetMods(e));
		}

		private void OnKey(int key, int scancode, int action, int mods)
		{
			m_onKey?.Invoke(this, key, scancode, action, mods);
		}

		private void M_Form_Resize(object sender, EventArgs e)
		{
			m_Width = m_Form.ClientSize.Width;
			m_Height = m_Form.ClientSize.Height;

			m_onWindowResized?.Invoke(this, m_Width, m_Height);
		}

		private void M_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			ShouldClose = true;
		}

		public bool ShouldClose { get; protected set; }

		public Form GetHandle()
		{
			return m_Form;
		}

		public void Dispose()
		{
			ShouldClose = true;
			m_Form?.Close();
			m_Form?.Dispose();
			m_Form = null;
		}

		internal int GetWidth()
		{
			return m_Width;
		}

		internal int GetHeight()
		{
			return m_Height;
		}

		internal void GetSize(out int width, out int height)
		{
			width = m_Width;
			height = m_Height;
		}

		internal void SetSizeCallback(GlfwWindowSizeCallbackDelegate onWindowResized)
		{
			this.m_onWindowResized = onWindowResized;
		}

		internal void SetKeyCallback(GlfwKeyCallbackDelegate onKey)
		{
			this.m_onKey = onKey;
		}
	}
}