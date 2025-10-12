using System.Text;

namespace Pepro.Business.Utilities;

/// <summary>
/// Provides default conversion methods between byte arrays and strings
/// using Windows-1252 (ANSI) encoding.
/// </summary>
static class DefaultConverter
{
    /// <summary>
    /// Converts a byte array to a string using Windows-1252 encoding.
    /// </summary>
    /// <param name="bytes">
    /// The byte array to be converted.
    /// </param>
    /// <returns>
    /// The decoded string representation of the provided byte array.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="bytes"/> is null or empty.
    /// </exception>
    public static string GetString(byte[] bytes)
    {
        if (bytes == null || bytes.Length <= 0)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        // Decodes the byte array using code page 1252 (Western European encoding).
        return Encoding.GetEncoding(1252).GetString(bytes);
    }

    /// <summary>
    /// Converts a string to a byte array using Windows-1252 encoding.
    /// </summary>
    /// <param name="s">
    /// The string to be converted.
    /// </param>
    /// <returns>
    /// A byte array representing the encoded string.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="s"/> is null or empty.
    /// </exception>
    public static byte[] GetBytes(string s)
    {
        ArgumentException.ThrowIfNullOrEmpty(s, nameof(s));

        // Encodes the string into a byte array using code page 1252.
        return Encoding.GetEncoding(1252).GetBytes(s);
    }
}
