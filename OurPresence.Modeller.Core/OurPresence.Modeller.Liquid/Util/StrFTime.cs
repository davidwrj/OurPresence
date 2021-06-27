// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OurPresence.Modeller.Liquid.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class StrFTime
    {
        private delegate string DateTimeDelegate(DateTime dateTime);
        private delegate string DateTimeOffsetDelegate(DateTimeOffset dateTimeOffset);

        private static readonly Dictionary<string, DateTimeDelegate> s_formats = new Dictionary<string, DateTimeDelegate>
        {
            { "a", (dateTime) => dateTime.ToString("ddd", CultureInfo.CurrentCulture) },
            { "A", (dateTime) => dateTime.ToString("dddd", CultureInfo.CurrentCulture) },
            { "b", (dateTime) => dateTime.ToString("MMM", CultureInfo.CurrentCulture) },
            { "B", (dateTime) => dateTime.ToString("MMMM", CultureInfo.CurrentCulture) },
            { "c", (dateTime) => dateTime.ToString("ddd MMM dd HH:mm:ss yyyy", CultureInfo.CurrentCulture) },
            { "C", (dateTime) => ((int)Math.Floor(Convert.ToDouble(dateTime.ToString("yyyy"))/100)).ToString() },
            { "d", (dateTime) => dateTime.ToString("dd", CultureInfo.CurrentCulture) },
            { "e", (dateTime) => dateTime.ToString("%d", CultureInfo.CurrentCulture).PadLeft(2, ' ') },
            { "h", (dateTime) => dateTime.ToString("MMM", CultureInfo.CurrentCulture) },
            { "H", (dateTime) => dateTime.ToString("HH", CultureInfo.CurrentCulture) },
            { "I", (dateTime) => dateTime.ToString("hh", CultureInfo.CurrentCulture) },
            { "j", (dateTime) => dateTime.DayOfYear.ToString().PadLeft(3, '0') },
            { "m", (dateTime) => dateTime.ToString("MM", CultureInfo.CurrentCulture) },
            { "k", (dateTime) => dateTime.ToString("%H", CultureInfo.CurrentCulture) },
            { "l", (dateTime) => dateTime.ToString("%h", CultureInfo.CurrentCulture) },
            { "M", (dateTime) => dateTime.Minute.ToString().PadLeft(2, '0') },
            { "P", (dateTime) => dateTime.ToString("tt", CultureInfo.CurrentCulture).ToLower() },
            { "p", (dateTime) => dateTime.ToString("tt", CultureInfo.CurrentCulture).ToUpper() },
            { "s", (dateTime) => ((int)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
            { "S", (dateTime) => dateTime.ToString("ss", CultureInfo.CurrentCulture) },
            { "u", (dateTime) => (dateTime.DayOfWeek == 0 ? (int)dateTime.DayOfWeek + 7 : ((int)dateTime.DayOfWeek)).ToString() },
            { "U", (dateTime) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday).ToString().PadLeft(2, '0') },
            { "W", (dateTime) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday).ToString().PadLeft(2, '0') },
            { "w", (dateTime) => ((int) dateTime.DayOfWeek).ToString() },
            { "x", (dateTime) => dateTime.ToString("d", CultureInfo.CurrentCulture) },
            { "X", (dateTime) => dateTime.ToString("T", CultureInfo.CurrentCulture) },
            { "y", (dateTime) => dateTime.ToString("yy", CultureInfo.CurrentCulture) },
            { "Y", (dateTime) => dateTime.ToString("yyyy", CultureInfo.CurrentCulture) },
            { "Z", (dateTime) => dateTime.ToString("zzz", CultureInfo.CurrentCulture) },
            { "%", (dateTime) => "%" }
        };

        private static readonly Dictionary<string, DateTimeOffsetDelegate> s_offsetFormats = new Dictionary<string, DateTimeOffsetDelegate>
        {
            { "s", (dateTimeOffset) => ((long)(dateTimeOffset - new DateTimeOffset(1970, 1, 1, 0,0,0, TimeSpan.Zero)).TotalSeconds).ToString() },
            { "Z", (dateTimeOffset) => dateTimeOffset.ToString("zzz", CultureInfo.CurrentCulture) },
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string ToStrFTime(this DateTime dateTime, string pattern)
        {
            var output = new StringBuilder();

            var n = 0;

            while (n < pattern.Length)
            {
                var s = pattern.Substring(n, 1);

                if (n + 1 >= pattern.Length)
                {
                    output.Append(s);
                }
                else
                {
                    output.Append(s == "%"
                        ? s_formats.ContainsKey(pattern.Substring(++n, 1)) ? s_formats[pattern.Substring(n, 1)].Invoke(dateTime) : "%" + pattern.Substring(n, 1)
                        : s);
                }

                n++;
            }

            return output.ToString();
        }

        /// <summary>
        /// Formats a date using a ruby date format string
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string ToStrFTime(this DateTimeOffset dateTime, string pattern)
        {
            var output = new System.Text.StringBuilder();

            for (var n = 0; n < pattern.Length; n++)
            {
                var s = pattern.Substring(n, 1);

                if (s == "%" && pattern.Length > ++n)
                {
                    if (s_offsetFormats.ContainsKey(pattern.Substring(n, 1)))
                    {
                        output.Append(s_offsetFormats[pattern.Substring(n, 1)].Invoke(dateTime));
                    }
                    else if (s_formats.ContainsKey(pattern.Substring(n, 1)))
                    {
                        output.Append(s_formats[pattern.Substring(n, 1)].Invoke(dateTime.DateTime));
                    }
                    else
                    {
                        output.Append("%" + pattern.Substring(n, 1));
                    }
                }
                else
                {
                    output.Append(s);
                }
            }

            return output.ToString();
        }
    }
}
