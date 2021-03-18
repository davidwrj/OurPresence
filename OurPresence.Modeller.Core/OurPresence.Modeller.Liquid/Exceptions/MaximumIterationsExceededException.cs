namespace OurPresence.Modeller.Liquid.Exceptions
{
    class MaximumIterationsExceededException : RenderException
    {
        public MaximumIterationsExceededException(string message, params string[] args)
            : base(string.Format(message, args))
        {
        }

        public MaximumIterationsExceededException()
        {
        }
    }
}
