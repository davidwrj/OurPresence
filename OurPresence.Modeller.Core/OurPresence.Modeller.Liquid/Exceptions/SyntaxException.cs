namespace OurPresence.Modeller.Liquid.Exceptions
{
    public class SyntaxException : LiquidException
    {
        public SyntaxException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
