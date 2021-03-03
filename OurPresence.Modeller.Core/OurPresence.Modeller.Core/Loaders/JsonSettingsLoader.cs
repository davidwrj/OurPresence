using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System.IO;

namespace OurPresence.Modeller.Loaders
{
    public sealed class JsonSettingsLoader : ILoader<ISettings>
    {
        private ISettings Load(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Settings file '{filePath}' does not exist.");

            using var reader = System.IO.File.OpenText(filePath);
            return JsonExtensions.FromJson<ISettings>(reader.ReadToEnd()) ?? throw new InvalidSettingsFileException(filePath);
        }

        ISettings ILoader<ISettings>.Load(string filePath)
        {
            return Load(filePath) ;
        }

        bool ILoader<ISettings>.TryLoad(string filePath, out ISettings instances)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    instances = Load(filePath);
                    return true;
                }
                catch { }
            }
            instances = new Settings();
            return false;
        }
    }
}
