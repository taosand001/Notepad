namespace Notepad.Database.Custom
{
    public class NotFoundErrorException : Exception
    {
        public NotFoundErrorException()
        {
        }

        public NotFoundErrorException(string message) : base(message)
        {
        }

        public NotFoundErrorException(string message, Exception inner) : base(message, inner)
        {
        }
    }


}
