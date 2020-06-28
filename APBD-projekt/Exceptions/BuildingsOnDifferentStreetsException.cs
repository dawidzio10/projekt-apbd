using System;
using System.Runtime.Serialization;

namespace APBD_projekt.Exceptions
{
    [Serializable]
    internal class BuildingsOnDifferentStreetsException : Exception
    {
        public BuildingsOnDifferentStreetsException()
        {
        }

        public BuildingsOnDifferentStreetsException(string message) : base(message)
        {
        }

        public BuildingsOnDifferentStreetsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildingsOnDifferentStreetsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}