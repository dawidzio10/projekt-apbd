using System;

namespace APBD_projekt.Generators
{
    public class RefreshTokenGenerator
    {
        public static string CreateRefreshToken()
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            return newRefreshToken;
        }
    }
}
