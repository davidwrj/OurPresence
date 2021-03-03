using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class Package : IPackage
    {
        private string _name;
        private string _version;

        public Package(string name, string version)
        {
            _name = name;
            _version = version;
        }

        string IPackage.Name { get => _name; set => _name = value; }

        string IPackage.Version { get => _version; set => _version = value; }
    }
}
