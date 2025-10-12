using System.Security.Authentication;
using System.Security.Cryptography;
using Pepro.Business.Utilities;

namespace Pepro.Business.Security;

/// <summary>
/// Provides hashing and verification operations with optional salting support,
/// using various hash algorithm types.
/// </summary>
internal class Hasher(HashAlgorithmType type, int saltSize)
{
    private readonly HashAlgorithmType _type = type;
    private readonly int _saltSize = saltSize;

    /// <summary>
    /// Computes the hash for a specified byte array.
    /// </summary>
    /// <param name="buffer">
    /// The byte array to be hashed.
    /// </param>
    /// <returns>
    /// The computed hash as a byte array.
    /// </returns>
    public byte[] ComputeHash(byte[] buffer)
    {
        using HashAlgorithm algorithm = CreateHashAlgorithm(_type);
        byte[] hashedMessage = algorithm.ComputeHash(buffer);
        return hashedMessage;
    }

    /// <summary>
    /// Computes the hash of a byte array with a randomly generated salt.
    /// </summary>
    /// <param name="buffer">
    /// The byte array to be hashed with a salt.
    /// </param>
    /// <returns>
    /// A <see cref="HashResult"/> containing the hash and the generated salt.
    /// </returns>
    public HashResult ComputeHashWithSalt(byte[] buffer)
    {
        // Generates a random salt and appends it to the original buffer before hashing.
        byte[] salt = SaltGenerator.GenerateSalt(_saltSize);
        byte[] saltedBuffer = ByteHandler.Combine(buffer, salt);
        byte[] hashedBuffer = ComputeHash(saltedBuffer);
        return new(hashedBuffer, salt);
    }

    /// <summary>
    /// Computes the hash of a string with a randomly generated salt.
    /// </summary>
    /// <param name="buffer">
    /// The string to be hashed with a salt.
    /// </param>
    /// <returns>
    /// A <see cref="HashResult"/> containing the hash and the generated salt.
    /// </returns>
    public HashResult ComputeHashWithSalt(string buffer)
    {
        byte[] castBuffer = DefaultConverter.GetBytes(buffer);
        return ComputeHashWithSalt(castBuffer);
    }

    /// <summary>
    /// Verifies whether a given byte array produces the expected hash.
    /// </summary>
    /// <param name="message">
    /// The byte array to verify.
    /// </param>
    /// <param name="expected">
    /// The expected hash to compare against.
    /// </param>
    /// <returns>
    /// <c>true</c> if the computed hash matches the expected hash; otherwise, <c>false</c>.
    /// </returns>
    public bool Verify(byte[] message, byte[] expected)
    {
        byte[] hashedMessage = ComputeHash(message);
        return hashedMessage.SequenceEqual(expected);
    }

    /// <summary>
    /// Verifies whether a string produces the expected hash.
    /// </summary>
    /// <param name="message">
    /// The string to verify.
    /// </param>
    /// <param name="expected">
    /// The expected hash to compare against.
    /// </param>
    /// <returns>
    /// <c>true</c> if the computed hash matches the expected hash; otherwise, <c>false</c>.
    /// </returns>
    public bool Verify(string message, byte[] expected)
    {
        byte[] castMessage = DefaultConverter.GetBytes(message);
        return Verify(castMessage, expected);
    }

    /// <summary>
    /// Verifies a salted byte array against an expected hash.
    /// </summary>
    /// <param name="message">
    /// The original byte array before salting.
    /// </param>
    /// <param name="expected">
    /// The expected hash value.
    /// </param>
    /// <param name="salt">
    /// The salt used during hashing.
    /// </param>
    /// <returns>
    /// <c>true</c> if the salted hash matches the expected hash; otherwise, <c>false</c>.
    /// </returns>
    public bool Verify(byte[] message, byte[] expected, byte[] salt)
    {
        // Combines the input message with the salt before hashing for verification.
        byte[] saltedMessage = ByteHandler.Combine(message, salt);
        return Verify(saltedMessage, expected);
    }

    /// <summary>
    /// Verifies a salted string against an expected hash.
    /// </summary>
    /// <param name="message">
    /// The original string before salting.
    /// </param>
    /// <param name="expected">
    /// The expected hash value.
    /// </param>
    /// <param name="salt">
    /// The salt used during hashing.
    /// </param>
    /// <returns>
    /// <c>true</c> if the salted hash matches the expected hash; otherwise, <c>false</c>.
    /// </returns>
    public bool Verify(string message, byte[] expected, byte[] salt)
    {
        byte[] castMessage = DefaultConverter.GetBytes(message);
        return Verify(castMessage, expected, salt);
    }

    /// <summary>
    /// Creates and returns a hash algorithm instance based on the specified algorithm type.
    /// </summary>
    /// <param name="type">
    /// The type of hash algorithm to create.
    /// </param>
    /// <returns>
    /// A concrete implementation of <see cref="HashAlgorithm"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when an unsupported hash algorithm type is provided.
    /// </exception>
    private static HashAlgorithm CreateHashAlgorithm(HashAlgorithmType type)
    {
        return type switch
        {
            HashAlgorithmType.Md5 => MD5.Create(),
            HashAlgorithmType.Sha1 => SHA1.Create(),
            HashAlgorithmType.Sha256 => SHA256.Create(),
            HashAlgorithmType.Sha384 => SHA384.Create(),
            HashAlgorithmType.Sha512 => SHA512.Create(),
            _ => throw new ArgumentException("Invalid hash algorithm type."),
        };
    }
}
