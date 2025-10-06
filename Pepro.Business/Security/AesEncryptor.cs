using System.Security.Cryptography;
using System.Text;
using Pepro.Business.Utilities;

namespace Pepro.Business.Security;

static class AesEncryptor
{
    private const string _password = "W34kP4ssw0rd@";
    private static readonly byte[] _salt = Encoding.UTF8.GetBytes(
        "SecuritySalt"
    );
    private const int _iter = 10000;
    private static readonly HashAlgorithmName _hash = HashAlgorithmName.SHA256;
    private const int _keySize = 32;
    private const int _initialVectorSize = 16;

    public static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        using Rfc2898DeriveBytes rfc = new(_password, _salt, _iter, _hash);

        aes.Key = rfc.GetBytes(_keySize);
        // default: 16 byte for AES
        aes.GenerateIV();

        ICryptoTransform encryptor = aes.CreateEncryptor();

        using MemoryStream memoryStream = new();
        using CryptoStream cryptoStream = new(
            memoryStream,
            encryptor,
            CryptoStreamMode.Write
        );
        using (StreamWriter streamWriter = new(cryptoStream))
        {
            //Write all data to the stream
            streamWriter.Write(plainText);
        }

        // Prepend the IV to the encrypted data so that it can be decrypted later
        return ByteHandler.Combine(aes.IV, memoryStream.ToArray());
    }

    public static string Decrypt(byte[] cipherText)
    {
        using Aes aes = Aes.Create();
        using Rfc2898DeriveBytes rfc = new(_password, _salt, _iter, _hash);

        aes.Key = rfc.GetBytes(_keySize);
        aes.IV = [.. cipherText.Take(_initialVectorSize)];
        byte[] buffer = [.. cipherText.Skip(_initialVectorSize)];

        ICryptoTransform decryptor = aes.CreateDecryptor();

        using MemoryStream memoryStream = new(buffer);
        using CryptoStream cryptoStream = new(
            memoryStream,
            decryptor,
            CryptoStreamMode.Read
        );
        using StreamReader streamReader = new(cryptoStream);

        // Read the decrypted bytes from the decrypting stream and place them in a string
        return streamReader.ReadToEnd();
    }
}
