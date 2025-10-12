using Pepro.Business.Security;

namespace Pepro.Business.Utilities;

/// <summary>
/// Provides methods for encrypting and decrypting strings
/// with integrated permission-based access control.
/// </summary>
public static class EncryptionConverter
{
    /// <summary>
    /// Encrypts a plain text string into a byte array using AES encryption.
    /// </summary>
    /// <param name="plainText">
    /// The input string to be encrypted.
    /// </param>
    /// <returns>
    /// The encrypted byte array, or <c>null</c> if the input is null or empty.
    /// </returns>
    public static byte[]? EncryptFromString(string? plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return null;
        }

        return AesEncryptor.Encrypt(plainText);
    }

    /// <summary>
    /// Decrypts a byte array back into a string,
    /// optionally masking the result if the user lacks read permission.
    /// </summary>
    /// <param name="cipherText">
    /// The encrypted byte array to be decrypted.
    /// </param>
    /// <returns>
    /// The decrypted string if permission is granted;
    /// masked text (<c>"**********"</c>) if permission is denied;
    /// or <c>null</c> if the input is null or empty.
    /// </returns>
    public static string? DecryptToString(byte[]? cipherText)
    {
        // Denies decryption and masks the output if the user lacks "Salary.Read" permission.
        if (!PermissionBusiness.Instance.Has("Salary.Read"))
        {
            return cipherText != null ? "**********" : null;
        }

        if (cipherText == null || cipherText.Length <= 0)
        {
            return null;
        }

        return AesEncryptor.Decrypt(cipherText);
    }
}
