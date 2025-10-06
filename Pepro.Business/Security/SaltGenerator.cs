using System.Security.Cryptography;

namespace Pepro.Business.Security;

static class SaltGenerator
{
    public static byte[] GenerateSalt(int size)
    {
        using RandomNumberGenerator generator = RandomNumberGenerator.Create();
        byte[] salt = new byte[size];
        generator.GetBytes(salt);
        return salt;
    }
}
