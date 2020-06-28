using System;
using System.Security.Cryptography;

namespace APBD_projekt.Generators
{
    public class SaltGenerator
    {
        public static string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var gen = RandomNumberGenerator.Create())
            {
                gen.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}
