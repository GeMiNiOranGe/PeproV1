using System.Security.Cryptography;
using System.Text;
using Pepro.Business.Utilities;

namespace Pepro.Business.Security;

/// <summary>
/// Provides AES encryption and decryption functionality
/// using a password-based key derivation algorithm (PBKDF2).
/// </summary>
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

    /// <summary>
    /// Encrypts a plain text string using AES encryption.
    /// </summary>
    /// <param name="plainText">
    /// The text to be encrypted.
    /// </param>
    /// <returns>
    /// A byte array containing the encrypted data,
    /// with the initialization vector (IV) prepended for later decryption.
    /// </returns>
    public static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        using Rfc2898DeriveBytes rfc = new(_password, _salt, _iter, _hash);

        aes.Key = rfc.GetBytes(_keySize);
        // AES uses a 16-byte (128-bit) initialization vector by default.
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
            // Writes the plain text data into the crypto stream for encryption.
            streamWriter.Write(plainText);
        }

        // Combines the IV and the encrypted data for storage or transmission.
        return ByteHandler.Combine(aes.IV, memoryStream.ToArray());
    }

    /// <summary>
    /// Decrypts a previously AES-encrypted byte array back into a string.
    /// </summary>
    /// <param name="cipherText">
    /// The encrypted byte array containing the prepended initialization vector.
    /// </param>
    /// <returns>
    /// The decrypted plain text string.
    /// </returns>
    public static string Decrypt(byte[] cipherText)
    {
        using Aes aes = Aes.Create();
        using Rfc2898DeriveBytes rfc = new(_password, _salt, _iter, _hash);

        aes.Key = rfc.GetBytes(_keySize);

        // Extracts the IV from the start of the encrypted byte array.
        aes.IV = [.. cipherText.Take(_initialVectorSize)];

        // Extracts the actual encrypted content after the IV.
        byte[] buffer = [.. cipherText.Skip(_initialVectorSize)];

        ICryptoTransform decryptor = aes.CreateDecryptor();

        using MemoryStream memoryStream = new(buffer);
        using CryptoStream cryptoStream = new(
            memoryStream,
            decryptor,
            CryptoStreamMode.Read
        );
        using StreamReader streamReader = new(cryptoStream);

        // Reads and returns the fully decrypted text from the crypto stream.
        return streamReader.ReadToEnd();
    }
}
