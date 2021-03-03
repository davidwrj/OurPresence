using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace OurPresence.Modeller.Loaders
{
    public class PackageFileLoader : ILoader<IEnumerable<IPackage>>
    {
        public IEnumerable<IPackage> Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Package file '{filePath}' does not exist.");
            }

            string packages;
            using (var reader = File.OpenText(filePath))
            {
                packages = reader.ReadToEnd();
            }
            var data = packages.FromJson<IEnumerable<Domain.Package>>();
            return data;
        }

        public bool TryLoad(string filePath, out IEnumerable<IPackage> packages)
        {
            try
            {
                packages = Load(filePath);
                return true;
            }
            catch
            {
                packages = new List<IPackage>();
                return false;
            }
        }
    }
}
