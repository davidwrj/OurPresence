using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Domain
{
    public class Package:IPackage
    {
        public Package(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; set; }

        public string Version { get; set; }
    }

}