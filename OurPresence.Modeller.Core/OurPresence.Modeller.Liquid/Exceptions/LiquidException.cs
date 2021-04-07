using System;

namespace OurPresence.Modeller.Liquid.Exceptions
{
    public abstract class LiquidException : Exception
    {
        protected LiquidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected LiquidException(string message)
            : base(message)
        {
        }

        protected LiquidException()
        {
        }
    }
}
