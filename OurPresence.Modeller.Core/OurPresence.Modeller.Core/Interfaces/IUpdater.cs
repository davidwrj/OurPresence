namespace OurPresence.Modeller.Interfaces
{
    public interface IUpdater
    {
        IGeneratorConfiguration GeneratorConfiguration { get; }

        void Refresh();
    }
}
