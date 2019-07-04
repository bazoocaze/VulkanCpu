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
using System.Diagnostics;
using System.Threading;

namespace VulkanCpu.Util
{
	public class FpsControl
	{
		private readonly Stopwatch m_Ellapsed;
		private readonly double m_targetFps;
		private readonly double m_FrameTime;

		private int m_FrameCount;
		private double m_MeanIdleTimeMs;
		private bool m_Enabled;

		public FpsControl()
			: this(30)
		{
		}

		public FpsControl(int targetFps)
		{
			m_FrameCount = 0;
			m_targetFps = targetFps;
			m_Ellapsed = new Stopwatch();
			m_Ellapsed.Start();

			m_FrameTime = 1000f / m_targetFps;
		}

		public bool Enabled
		{
			get { return m_Enabled; }
			set { UpdateState(value); }
		}

		private void UpdateState(bool enabled)
		{
			if (enabled == m_Enabled)
				return;

			if (enabled)
			{
				m_Ellapsed.Restart();
				m_FrameCount = 0;
				m_Enabled = enabled;
			}
			else
			{
				m_Enabled = enabled;
				m_Ellapsed.Stop();
			}
		}

		public void Update()
		{
			int timeToSleep = 1;

			if (Enabled)
			{
				m_FrameCount++;
				var calculatedEllapsedMs = m_FrameTime * m_FrameCount;
				var timeDiff = calculatedEllapsedMs - m_Ellapsed.ElapsedMilliseconds;

				TestReset();

				if (timeDiff >= 1)
				{
					m_MeanIdleTimeMs = (m_MeanIdleTimeMs + timeDiff) / 2;
					timeToSleep = (int)Math.Max(timeDiff, m_FrameTime);
				}
			}

			Thread.Sleep(timeToSleep);
		}

		private void Reset()
		{
			m_Ellapsed.Restart();
			m_FrameCount = 0;
		}

		private void TestReset()
		{
			if (m_Ellapsed.ElapsedMilliseconds > 1000)
				Reset();
		}
	}
}
