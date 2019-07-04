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
using System.Text;

namespace VulkanCpu.Util
{
	public class FpsCounter
	{
		private const double P_MIN_TIME_MS = 0.000001;
		private const long P_DEFAULT_REPORT_TIME_MS = 5000;

		private readonly string m_Tag;
		private readonly long m_ReportTime;
		private readonly FpsReportCounters m_ReportCounters;

		private readonly Stopwatch m_Cumullative;
		private readonly Stopwatch m_Ellapsed;
		private int m_FrameCounter;

		public FpsCounter(string tag, long reportTime = P_DEFAULT_REPORT_TIME_MS, FpsReportCounters reportCounters = FpsReportCounters.Default)
		{
			m_Tag = tag;
			m_ReportTime = reportTime;
			m_ReportCounters = reportCounters;

			m_Cumullative = new Stopwatch();
			m_Ellapsed = new Stopwatch();

			// START
			m_FrameCounter = 0;
			m_Ellapsed.Start();
		}

		public int FrameCounter
		{
			get { return m_FrameCounter; }
			set { m_FrameCounter = Math.Max(1, value); }
		}

		public void Begin()
		{
			m_FrameCounter++;
			m_Cumullative.Start();
		}

		public void End()
		{
			m_Cumullative.Stop();
		}

		private string GetReport()
		{
			double cummulativeMs = m_Cumullative.Elapsed.TotalMilliseconds;
			double ellapsedMs = m_Ellapsed.Elapsed.TotalMilliseconds;
			double frames = Math.Max(m_FrameCounter, 1);

			cummulativeMs = Math.Max(cummulativeMs, P_MIN_TIME_MS);
			ellapsedMs = Math.Max(ellapsedMs, P_DEFAULT_REPORT_TIME_MS);

			double fps = frames * 1000f / ellapsedMs;
			double perc = cummulativeMs / ellapsedMs * 100;
			double frameTime = cummulativeMs / frames;

			StringBuilder ret = new StringBuilder();
			ret.AppendFormat("FPS({0}):", m_Tag);

			if ((m_ReportCounters & FpsReportCounters.Fps) > 0)
				ret.AppendFormat(" fps={0:N2}", fps);

			if ((m_ReportCounters & FpsReportCounters.Frames) > 0)
				ret.AppendFormat(" frames={0:N0}", frames);

			if ((m_ReportCounters & FpsReportCounters.CummulativeTime) > 0)
			{
				ret.AppendFormat(" time={0}", FormatTimeMs(cummulativeMs));
			}

			if ((m_ReportCounters & FpsReportCounters.EllapsedTime) > 0)
				ret.AppendFormat(" ellapsed={0}", FormatTimeMs(ellapsedMs));

			if ((m_ReportCounters & FpsReportCounters.PercentCummulative) > 0)
				ret.AppendFormat(" perc={0:N2}%", perc);

			if ((m_ReportCounters & FpsReportCounters.FrameTime) > 0)
			{
				ret.AppendFormat(" frame_time={0}", FormatTimeMs(frameTime));
			}

			return ret.ToString();
		}

		private string FormatTimeMs(double milliseconds)
		{
			if (milliseconds >= 1)
				return string.Format("{0:N2}ms", milliseconds);

			milliseconds *= 1000f;
			if (milliseconds >= 1)
				return string.Format("{0:N2}us", milliseconds);

			milliseconds *= 1000f;
			return string.Format("{0:N2}ns", milliseconds);
		}

		public void DebugReport()
		{
			var info = GetReport();

			Debug.WriteLine(info);
			Console.Out.WriteLine(info);

			// clean
			m_Cumullative.Reset();
			m_FrameCounter = 0;
			m_Ellapsed.Restart();
		}

		public void DebugPeriodicReport()
		{
			if (m_Ellapsed.ElapsedMilliseconds < P_DEFAULT_REPORT_TIME_MS)
				return;

			DebugReport();
		}
	}

	[Flags]
	public enum FpsReportCounters
	{
		Fps = 1,
		Frames = 2,
		CummulativeTime = 4,
		EllapsedTime = 8,
		PercentCummulative = 16,
		FrameTime = 32,

		Default = Fps | Frames | CummulativeTime | PercentCummulative,
		SimpleFrameTime = FrameTime | PercentCummulative,
	}
}
