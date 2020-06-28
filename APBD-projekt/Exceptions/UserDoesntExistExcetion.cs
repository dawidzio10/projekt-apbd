using System;
using System.Runtime.Serialization;

namespace APBD_projekt.Exceptions
{
    [Serializable]
    internal class UserDoesntExistExcetion : Exception
    {
        public UserDoesntExistExcetion()
        {
        }

        public UserDoesntExistExcetion(string message) : base(message)
        {
        }

        public UserDoesntExistExcetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserDoesntExistExcetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}