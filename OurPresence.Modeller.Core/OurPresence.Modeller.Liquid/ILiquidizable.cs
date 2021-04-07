namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// This allows for extra security by only giving the template access to the specific
    /// variables you want it to have access to.
    /// </summary>
    public interface ILiquidizable
    {
        object ToLiquid();
    }
}
