using Entities.Enums;

namespace Domain.Exceptions
{
    [Serializable]
    public class NullObjectException : Exception
    {
        private const string MessageTemplate = "{0} cannot be null!";

        public NullObjectException() { }

        public NullObjectException(ExceptionSubject subject) : base(GetSubjectMessage(subject)) { }
        public NullObjectException(string message) : base(message) { }

        public NullObjectException(string message, Exception innerException) : base(message, innerException) { }

        private static string GetSubjectMessage(ExceptionSubject subject)
            => string.Format(MessageTemplate, subject.GetDisplayName());
    }
}
