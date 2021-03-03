namespace OurPresence.Modeller.Interfaces
{
    public interface ILoader<T>
    {
        T Load(string filePath);

        bool TryLoad(string filePath, out T instances);
    }
}
