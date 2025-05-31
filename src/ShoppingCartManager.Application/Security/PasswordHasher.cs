using System.Security.Cryptography;
using System.Text;

namespace ShoppingCartManager.Application.Security;

public static class PasswordHasher
{
    public static (byte[] Hash, byte[] Salt) Hash(string password)
    {
        using var hmac = new HMACSHA512();

        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return (hash, salt);
    }

    public static bool Verify(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(hash);
    }
}
