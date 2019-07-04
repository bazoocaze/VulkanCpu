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

namespace VulkanCpu.VulkanApi.Utils
{
	internal static class VkPreconditions
	{
		/// <summary>
		/// Throw an <see cref="ArgumentNullException"/> if input is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input">Input to test</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <returns>Returns input</returns>
		/// <exception cref="ArgumentNullException"></exception>
		[DebuggerStepThrough]
		internal static T CheckNull<T>(T input, string paramName) where T : class
		{
			if (input == null)
				throw new ArgumentNullException(paramName);
			return input;
		}

		/// <summary>
		/// Throw an <see cref="ArgumentNullException"/> if throwCondition is true.
		/// </summary>
		/// <param name="throwCondition">Condition to throw exception</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <exception cref="ArgumentNullException"></exception>
		[DebuggerStepThrough]
		internal static void CheckNull(bool throwCondition, string paramName)
		{
			if (throwCondition)
				throw new ArgumentNullException(paramName);
		}

		/// <summary>
		/// Throw an <see cref="ArgumentNullException"/> if input is null or contain some null itens.
		/// </summary>
		/// <typeparam name="TList">IEnumetable data type for the list.</typeparam>
		/// <typeparam name="TElem">Data type of the items on the list.</typeparam>
		/// <param name="input">List input</param>
		/// <param name="paramName">Parameter name</param>
		/// <returns>The input list.</returns>
		[DebuggerStepThrough]
		internal static TList CheckListNotNull<TList, TElem>(TList input, string paramName) where TList : IEnumerable<TElem> where TElem : class
		{
			if (input == null)
				throw new ArgumentNullException(paramName);

			foreach (var item in input)
			{
				if (item == null)
					throw new ArgumentNullException(paramName, $"The parameter {paramName} must not contains null elements");
			}

			return input;
		}

		/// <summary>
		/// Throw an <see cref="ArgumentOutOfRangeException"/> if input is out of range.
		/// </summary>
		/// <param name="input">Input to check</param>
		/// <param name="min">Minimum value</param>
		/// <param name="max">Maximum value</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <returns>Returns input</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		[DebuggerStepThrough]
		internal static int CheckRange(int input, int min, int max, string paramName)
		{
			if (input < min || input > max)
				throw new ArgumentOutOfRangeException(paramName, input, string.Format("The parameter {0} is out of range", paramName));
			return input;
		}

		/// <summary>
		/// Throw an <see cref="ArgumentOutOfRangeException"/> if input is out of range.
		/// </summary>
		/// <param name="input">Input to check</param>
		/// <param name="min">Minimum value</param>
		/// <param name="max">Maximum value</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <returns>Returns input</returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		[DebuggerStepThrough]
		internal static T CheckRange<T>(T input, T min, T max, string paramName) where T : IComparable<T>
		{
			if (input.CompareTo(min) < 0 || input.CompareTo(max) > 0)
				throw new ArgumentOutOfRangeException(paramName, input, string.Format("The paramter {0} is out of range", paramName));
			return input;
		}

		/// <summary>
		/// Throw an <see cref="ArgumentOutOfRangeException"/> if throwCondition is true.
		/// </summary>
		/// <param name="throwCondition">Condition to throw exception</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		[DebuggerStepThrough]
		internal static void CheckRange(bool throwCondition, string paramName)
		{
			if (throwCondition)
				throw new ArgumentOutOfRangeException(paramName, string.Format("The parameter {0} is out of range", paramName));
		}

		/// <summary>
		/// Throw an <see cref="ArgumentException"/> if input is null or empty string.
		/// </summary>
		/// <param name="input">Input to check</param>
		/// <param name="paramName">Parameter name for error message</param>
		/// <returns>Returns input</returns>
		/// <exception cref="ArgumentException"></exception>
		[DebuggerStepThrough]
		internal static string CheckString(string input, string paramName)
		{
			if (String.IsNullOrWhiteSpace(input))
				throw new ArgumentException(string.Format("Input string is not valid"), paramName);
			return input;
		}

		/// <summary>
		/// Throw an <see cref="InvalidOperationException"/> if throwCondition is true.
		/// </summary>
		/// <param name="throwCondition">Condition to throw exception</param>
		/// <param name="message">Error message</param>
		/// <exception cref="InvalidOperationException"></exception>
		[DebuggerStepThrough]
		internal static void CheckOperation(bool throwCondition, string message)
		{
			if (throwCondition)
				throw new InvalidOperationException(message);
		}
	}
}
