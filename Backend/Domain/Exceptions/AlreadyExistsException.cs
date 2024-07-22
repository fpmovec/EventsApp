using Entities.Enums;

namespace Entities.Exceptions
{
    [Serializable]
    public class AlreadyExistsException : Exception
    {
        private const string MessageTemplate = "{0} already exists!";
        public AlreadyExistsException() { }

        public AlreadyExistsException(ExceptionSubject subject) : base(GetSubjectMessage(subject)) { }
        public AlreadyExistsException(string message) : base(message) { }

        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

        private static string GetSubjectMessage(ExceptionSubject subject)
            => string.Format(MessageTemplate, subject.GetDisplayName());
    }
}
