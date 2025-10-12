using System.Security.Cryptography;

namespace Pepro.Business.Security;

/// <summary>
/// Provides functionality for generating cryptographically secure random salt values.
/// </summary>
static class SaltGenerator
{
    /// <summary>
    /// Generates a cryptographically secure random salt.
    /// </summary>
    /// <param name="size">
    /// The length of the salt in bytes. Must be a positive integer.
    /// </param>
    /// <returns>
    /// A byte array containing the generated random salt.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="RandomNumberGenerator"/>
    /// to generate random bytes suitable for cryptographic operations such as password hashing.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="size"/> is less than or equal to zero.
    /// </exception>
    public static byte[] GenerateSalt(int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(size),
                "Salt size must be greater than zero."
            );
        }

        using RandomNumberGenerator generator = RandomNumberGenerator.Create();
        byte[] salt = new byte[size];
        generator.GetBytes(salt);
        return salt;
    }
}
