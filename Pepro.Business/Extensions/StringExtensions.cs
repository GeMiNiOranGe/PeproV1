using System.Text;

namespace Pepro.Business.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Gets the initials of each word in the specified string.
    /// </summary>
    /// <param name="message">
    /// The input string from which to extract initials.
    /// Words are determined by spaces (<c>' '</c>).
    /// </param>
    /// <returns>
    /// A string containing the first character of each word in <paramref name="message"/>.
    /// Returns an empty string if <paramref name="message"/> is <see langword="null"/> or empty.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method splits the input string by spaces, removes empty entries,
    /// and concatenates the first character of each resulting word.
    /// It does not alter the case of the initials.
    /// </para>
    /// <para>
    /// Example: <c>"Nguyen Van A"</c> → <c>"NVA"</c>
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use <see cref="GetWordInitials(string?)"/>:
    /// <code>
    /// string name = "Pham Minh Hoang";
    /// string initials = name.GetWordInitials();
    /// // initials = "PMH"
    /// </code>
    /// </example>
    public static string GetWordInitials(this string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return "";
        }

        string[] words = message.Split(
            " ",
            StringSplitOptions.RemoveEmptyEntries
        );

        StringBuilder initials = new();
        foreach (string word in words)
        {
            initials.Append(word[0]);
        }

        return initials.ToString();
    }
}
