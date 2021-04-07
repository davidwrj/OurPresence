namespace OurPresence.Modeller.Liquid
{
    public interface IIndexable
    {
        object this[object key] { get; }
        bool ContainsKey(object key);
    }
}
