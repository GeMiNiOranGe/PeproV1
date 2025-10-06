using System.Text;

namespace Pepro.Business.Extensions;

public static class StringExtensions
{
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
