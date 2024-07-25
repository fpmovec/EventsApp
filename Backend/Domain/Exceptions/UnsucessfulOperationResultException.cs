namespace Domain.Exceptions
{
    [Serializable]
    public class UnsucessfulOperationResultException : Exception
    {
        public UnsucessfulOperationResultException() : base() { }

        public UnsucessfulOperationResultException(string message) : base(message) { }

        public UnsucessfulOperationResultException(string message, Exception innerException) : base(message, innerException) { }
    }
}
