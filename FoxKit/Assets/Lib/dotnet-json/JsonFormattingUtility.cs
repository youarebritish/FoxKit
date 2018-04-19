// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Globalization;

namespace Rotorz.Json
{
    /// <summary>
    /// Utility functionality for formatting JSON values.
    /// </summary>
    public static class JsonFormattingUtility
    {
        private static bool DoubleStringIsIntegerValue(string value)
        {
            for (int i = 0; i < value.Length; ++i) {
                char c = value[i];
                if (c == '.' || c == 'e' || c == 'E') {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Format double precision floating point value as string but ensure that value
        /// always contains a trailing zero.
        /// </summary>
        /// <remarks>
        /// <para>Since the JSON specification does not cater for values of NaN, Infinity
        /// and Negative Infinity several special values have been assumed to represent
        /// these in output files.</para>
        /// <para>Special tokens are returned when input has one of the following special
        /// values:</para>
        /// <list type="bullet">
        /// <item>NaN = "NaN"</item>
        /// <item>+Infinity = "Infinity"</item>
        /// <item>-Infinity = "-Infinity"</item>
        /// </list>
        /// </remarks>
        /// <param name="value">Double precision value.</param>
        /// <returns>
        /// Formatted string.
        /// </returns>
        public static string DoubleToString(double value)
        {
            if (double.IsNaN(value)) {
                return "\"NaN\"";
            }
            else if (double.IsPositiveInfinity(value)) {
                return "\"Infinity\"";
            }
            else if (double.IsNegativeInfinity(value)) {
                return "\"-Infinity\"";
            }

            string str = value.ToString("g", CultureInfo.InvariantCulture);
            if (DoubleStringIsIntegerValue(str)) {
                str += ".0";
            }
            return str;
        }
    }
}
