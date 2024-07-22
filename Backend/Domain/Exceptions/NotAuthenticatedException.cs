using Entities.Enums;

namespace Domain.Exceptions
{
    [Serializable]
    public class NotAuthenticatedException : Exception
    {
        private const string MessageTemplate = "User is not authenticated!";
        public NotAuthenticatedException() : base(MessageTemplate) { }

        public NotAuthenticatedException(string message) : base(message) { }

        public NotAuthenticatedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
