namespace OurPresence.Modeller.Interfaces
{
    public interface IModuleLoader<T>
    {
        T Load(string filePath);
        bool TryLoad(string filePath, out T module);
    }
}
