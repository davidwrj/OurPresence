using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class FileCopy : IFileCopy
    {
        public FileCopy(string name, string source, string destination)
        {
            Name = name;
            Source = source;
            Destination = destination;
        }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string Name { get; }
    }
}
