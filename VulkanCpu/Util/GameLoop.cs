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
using System.Windows.Forms;
using VulkanCpu.Platform.win32;

namespace VulkanCpu.Util
{
	public class GameLoop
	{
		public static void Run(Func<bool> whilePredicate, Action loopCode)
		{
			InternalRun(null, whilePredicate, loopCode);
		}

		public static void Run(Form form, Func<bool> whilePredicate, Action loopCode)
		{
			InternalRun(form, whilePredicate, loopCode);
		}

		private static void InternalRun(Form form, Func<bool> whilePredicate, Action loopCode)
		{
			EventHandler idleHandler = (s, a) =>
			{
				User32.Message message;

				while (!User32.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
				{
					bool close = false;
					close = close || (!whilePredicate());

					if (form != null)
						close = close || (form.IsDisposed);

					if (close)
					{
						Application.ExitThread();
						return;
					}

					loopCode();
				}
				return;
			};

			Application.Idle += idleHandler;
			try
			{
				if (form != null)
					Application.Run(form);
				else
					Application.Run();
			}
			finally
			{
				Application.Idle -= idleHandler;
			}
		}
	}
}
