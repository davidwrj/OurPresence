namespace OurPresence.Modeller.Liquid.Exceptions
{
    public class FileSystemException : LiquidException
    {
        public FileSystemException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
