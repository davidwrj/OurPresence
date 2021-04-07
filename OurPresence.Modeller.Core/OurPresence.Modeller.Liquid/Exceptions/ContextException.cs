namespace OurPresence.Modeller.Liquid.Exceptions
{
    public class ContextException : LiquidException
    {
        public ContextException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }

        public ContextException()
        {
        }
    }
}