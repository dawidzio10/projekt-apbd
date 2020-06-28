using System;
using System.Runtime.Serialization;

namespace APBD_projekt.Exceptions
{
    [Serializable]
    internal class BuildingDoesntExistException : Exception
    {
        public BuildingDoesntExistException()
        {
        }

        public BuildingDoesntExistException(string message) : base(message)
        {
        }

        public BuildingDoesntExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildingDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}