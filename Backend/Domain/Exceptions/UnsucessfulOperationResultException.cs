namespace Domain.Exceptions
{
    [Serializable]
    public class UnsucessfulOperationResultException : Exception
    {
        private const string MessageTemplate = "Error! Unsucessful operation result";

        public UnsucessfulOperationResultException() : base(MessageTemplate)
        { }

        public UnsucessfulOperationResultException(string message) : base(message) { }

        public UnsucessfulOperationResultException(string message, Exception innerException) : base(message, innerException) { }
    }
}
