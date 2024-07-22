using Entities.Enums;

namespace Entities.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        private const string MessageTemplate = "{0} not found!";
        public NotFoundException() : base() { }

        public NotFoundException(ExceptionSubject subject) : base(GetSubjectMessage(subject)) { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        private static string GetSubjectMessage(ExceptionSubject subject)
            => string.Format(MessageTemplate, subject.GetDisplayName());
    }
}
