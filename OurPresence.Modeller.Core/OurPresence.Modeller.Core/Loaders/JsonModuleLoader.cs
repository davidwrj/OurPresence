using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace OurPresence.Modeller.Loaders
{
    public class JsonModuleLoader : ILoader<IEnumerable<INamedElement>>
    {
        private Module Process(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Module file '{filePath}' does not exist.");

            string model;
            using (var reader = File.OpenText(filePath))
            {
                model = reader.ReadToEnd();
            }
            return model.FromJson<Module>()??throw new InvalidModuleFileException(filePath);
        }

        IEnumerable<INamedElement> ILoader<IEnumerable<INamedElement>>.Load(string filePath)
        {
            return new[] { Process(filePath) };
        }

        bool ILoader<IEnumerable<INamedElement>>.TryLoad(string filePath, out IEnumerable<INamedElement> modules)
        {
            try
            {
                modules = new[] { Process(filePath) };
                return true;
            }
            catch
            {
                modules = new List<INamedElement>();
                return false;
            }
        }
    }
}
