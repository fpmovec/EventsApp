namespace Entities.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        private const string MessageTemplate = "Bad request! Check provided data";
        public BadRequestException() : base(MessageTemplate) { }

        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
