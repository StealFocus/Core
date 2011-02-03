// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="StringExtensions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the StringExtensions type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    using System;
    using System.Globalization;

    /// <summary>
    /// StringExtensions Class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the given string with the given parameters using <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="text">The string to format.</param>
        /// <param name="values">An <see cref="object"/> array. The format values.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatWith(this string text, params object[] values)
        {
            return FormatWith(text, CultureInfo.CurrentCulture, values);
        }

        /// <summary>
        /// Formats the given string with the given parameters using <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="text">The string to format.</param>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/>. The format provider.</param>
        /// <param name="values">An <see cref="object"/> array. The format values.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatWith(this string text, IFormatProvider formatProvider, params object[] values)
        {
            return string.Format(formatProvider, text, values);
        }
    }
}
