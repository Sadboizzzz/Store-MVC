using System;
using System.Security.Cryptography;

namespace WebApplication1.Models
{
    public static class PasswordHelper
    {
        public static string CreateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string Hash(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool Verify(string password, string salt, string storedHash)
        {
            string newHash = Hash(password, salt);
            return newHash == storedHash;
        }
    }
}
