using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace APBD_projekt.Generators
{
    public class HashGenerator
    {
        public static string CreateHashForPassword(string password, string salt)
        {
            var bytes = KeyDerivation.Pbkdf2(password, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA256, 1000, 256 / 8);
            return Convert.ToBase64String(bytes);
        }
    }
}
