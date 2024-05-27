using System.Runtime.Serialization;

namespace Notepad.Database.Custom
{
    [Serializable]
    public class ConflictErrorException : Exception
    {
        public ConflictErrorException()
        {
        }

        public ConflictErrorException(string? message) : base(message)
        {
        }

        public ConflictErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConflictErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}