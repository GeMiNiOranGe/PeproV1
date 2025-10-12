using System.Text;
using System.Text.RegularExpressions;

namespace Pepro.Business.Extensions;

public static partial class AccentStrippingExtensions
{
    /// <summary>
    /// Removes all Vietnamese diacritics (accent marks) from the specified string.
    /// </summary>
    /// <param name="vietnameseText">
    /// The input string that may contain Vietnamese accented characters.
    /// </param>
    /// <returns>
    /// A new string with all Vietnamese diacritics removed and special characters
    /// <c>đ</c> and <c>Đ</c> replaced with <c>d</c> and <c>D</c> respectively.
    /// </returns>
    /// <remarks>
    /// This method:
    /// <list type="bullet">
    /// <item>Normalizes the input string to <see cref="NormalizationForm.FormD"/> to separate base letters and combining marks.</item>
    /// <item>Uses a precompiled regular expression to remove all combining diacritical marks.</item>
    /// <item>Replaces Vietnamese-specific characters <c>đ</c> and <c>Đ</c> with their ASCII equivalents.</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use <see cref="ToNonAccentVietnamese(string)"/>:
    /// <code>
    /// string input = "Trần Đình Đạt";
    /// string output = input.ToNonAccentVietnamese();
    /// // output: "Tran Dinh Dat"
    /// </code>
    /// </example>
    public static string ToNonAccentVietnamese(this string vietnameseText)
    {
        return VietnameseDiacriticsRegex()
            .Replace(
                vietnameseText.Normalize(NormalizationForm.FormD),
                string.Empty
            )
            .Replace('\u0111', 'd')
            .Replace('\u0110', 'D');
    }

    [GeneratedRegex(@"\p{IsCombiningDiacriticalMarks}+")]
    private static partial Regex VietnameseDiacriticsRegex();
}
