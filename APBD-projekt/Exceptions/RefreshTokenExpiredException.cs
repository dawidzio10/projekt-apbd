using System;
using System.Runtime.Serialization;

namespace APBD_projekt.Exceptions
{
    [Serializable]
    internal class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException()
        {
        }

        public RefreshTokenExpiredException(string message) : base(message)
        {
        }

        public RefreshTokenExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RefreshTokenExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}