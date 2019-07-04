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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VulkanCpu.VulkanApi.Utils;

namespace VulkanCpu.Util
{
	public class Benchmark
	{
		private const int PASS_WARMUP = 0;
		private const int PASS_PROPER = 1;

		private static int m_BenchNumber = 0;

		public static void Do(IDictionary<string, Action> inputWork, int workSize, string title = null, TextWriter output = null)
		{
			var inputActions = inputWork.Values.ToArray();
			var inputDescriptions = inputWork.Keys.ToArray();
			Do(inputActions, workSize, title, inputDescriptions, output);
		}

		public static void Do(IEnumerable<Action> inputActions, int workSize, string title = null, IEnumerable<string> inputDescriptions = null, TextWriter output = null)
		{
			Action[] actionList = inputActions.ToArray();
			Action emptyDelegate = () => { };
			string[] descriptionList = null;
			double emptyDelegateTime = 0;
			int minimumWorkTimeMs = 500;

			if (title == null)
			{
				title = $"Benchmark {++m_BenchNumber}";
			}

			if (inputDescriptions != null)
			{
				descriptionList = inputDescriptions.ToArray();
				int padDesc = Math.Min(30, descriptionList.Max((s) => s.Length));
				for (int n = 0; n < descriptionList.Length; n++)
				{
					descriptionList[n] = string.Format("WORK[{0}]", descriptionList[n].PadRight(padDesc));
				}
			}
			else
			{
				descriptionList = new string[actionList.Length];
				int padDesc = (descriptionList.Length - 1).ToString().Length;
				for (int n = 0; n < descriptionList.Length; n++)
				{
					descriptionList[n] = string.Format("WORK[{0}]", n.ToString().PadLeft(padDesc));
				}
			}

			if (output == null)
			{
				output = Console.Out;
			}

			GC.Collect();

			OutputWriteLine(output, $"----- BEGIN BENCHMARK: {title} -----");

			for (int numPass = 0; numPass < 2; numPass++)
			{
				// Measure loop overhead
				emptyDelegateTime = RunAction(emptyDelegate, workSize, minimumWorkTimeMs);
				if (numPass == PASS_PROPER)
				{
					OutputWriteLine(output, "Loop overhead: time={0}", FormatTime(emptyDelegateTime));
				}

				for (int item = 0; item < actionList.Length; item++)
				{
					Action workDelegate = actionList[item];
					string workDescription = descriptionList[item];
					double workTime = 0;
					bool error = false;
					string errorDescription = null;

					try
					{
						workTime = RunAction(workDelegate, workSize, minimumWorkTimeMs);
					}
					catch (Exception ex)
					{
						error = true;
						errorDescription = ex.GetType().Name;
					}

					if (numPass == PASS_PROPER)
					{
						if (error)
						{
							OutputWriteLine(output, $"{workDescription}: FAILED[{errorDescription}]");
						}
						else
						{
							double corrected = workTime - emptyDelegateTime;
							if (workTime > (1000 * emptyDelegateTime))
							{
								OutputWriteLine(output, "{0}: time={1}", workDescription, FormatTime(workTime));
							}
							else
							{
								OutputWriteLine(output, "{0}: time={1} corrected={2}", workDescription, FormatTime(workTime), FormatTime(corrected));
							}
						}
					}
				}
			}

			OutputWriteLine(output, $"----- END BENCHMARK: {title} -----");
		}

		private static double RunAction(Action action, int workSize)
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			for (int p = 0; p < workSize; p++)
			{
				action.Invoke();
			}
			timer.Stop();
			return timer.Elapsed.TotalSeconds / workSize;
		}

		private static double RunAction(Action action, int workSize, int minMilliseconds)
		{
			double cummulative = 0;
			int countTimerRepetitions = 0;
			Stopwatch repetitionsTimer = new Stopwatch();
			repetitionsTimer.Start();

			while (true)
			{
				cummulative += RunAction(action, workSize);
				countTimerRepetitions++;

				if (repetitionsTimer.ElapsedMilliseconds >= minMilliseconds)
					break;
			}

			return cummulative / countTimerRepetitions;
		}

		public static double MeasureAction(Action action, int workSize)
		{
			VkPreconditions.CheckNull(action, nameof(action));
			VkPreconditions.CheckRange(workSize <= 0, nameof(workSize));
			return RunAction(action, workSize);
		}

		private static double MeasureAction(Action action, int workSize, int minMilliseconds)
		{
			VkPreconditions.CheckNull(action, nameof(action));
			VkPreconditions.CheckRange(workSize <= 0, nameof(workSize));
			VkPreconditions.CheckRange(minMilliseconds <= 0, nameof(minMilliseconds));
			return RunAction(action, workSize, minMilliseconds);
		}

		public static string FormatTime(double seconds)
		{
			string[] units = new string[] { "s", "ms", "us", "ns", "ps", "fs" };
			int finalUnit = units.Length - 1;

			for (int n = 0; n < units.Length; n++)
			{
				if (n == finalUnit || seconds <= 0 || seconds >= 1)
				{
					return string.Format("{0:N2}{1}", seconds, units[n]);
				}
				seconds = seconds * 1000;
			}

			// never reachs here
			return null;
		}

		private static void OutputWriteLine(TextWriter output, string message)
		{
			output.WriteLine(message);
			// Debug.WriteLine(message);
		}

		private static void OutputWriteLine(TextWriter output, string fmt, params object[] args)
		{
			string message = string.Format(fmt, args);
			OutputWriteLine(output, message);
		}
	}
}
