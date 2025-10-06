using System.Text;

namespace Pepro.Business.Utilities;

static class DefaultConverter
{
    public static string GetString(byte[] bytes)
    {
        if (bytes == null || bytes.Length <= 0)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        return Encoding.GetEncoding(1252).GetString(bytes);
    }

    public static byte[] GetBytes(string s)
    {
        ArgumentException.ThrowIfNullOrEmpty(s, nameof(s));

        return Encoding.GetEncoding(1252).GetBytes(s);
    }
}
