namespace OurPresence.Modeller.Interfaces
{
    public interface IOutputStrategy
    {
        void Create(IOutput output, string path, bool overwrite);
    }
}
