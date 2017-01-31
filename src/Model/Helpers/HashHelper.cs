using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Model.Helpers
{
    public class HashHelper
    {
        public static string ComputeHash(string text, byte[] salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                text,
                salt,
                KeyDerivationPrf.HMACSHA512,
                12782,
                256/8));
            return hashed;
        }

        public static byte[] GenerateSalt()
        {
            var salt = new byte[128/8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}