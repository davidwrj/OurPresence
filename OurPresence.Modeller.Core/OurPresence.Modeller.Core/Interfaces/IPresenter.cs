namespace OurPresence.Modeller.Interfaces
{
    public interface IPresenter
    {
        IGeneratorConfiguration GeneratorConfiguration { get; }

        void Display();
    }
}
