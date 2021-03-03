using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class OutputStrategy : IOutputStrategy
    {
        private readonly IEnumerable<IFileCreator> _creators;

        public OutputStrategy(IEnumerable<IFileCreator> creators)
        {
            _creators = creators ?? throw new ArgumentNullException(nameof(creators));
        }

        public void Create(IOutput output, string path = null, bool overwrite = false)
        {
            var creator = _creators.FirstOrDefault(c => c.SupportedType.IsAssignableFrom(output.GetType()));
            if (creator == null)
                throw new InvalidOperationException($"No IFileCreator implementation registered for {output.GetType().Name}");

            if (path != null)
            {
                var s = new System.IO.DirectoryInfo(path);
                if (!s.Exists)
                    s.Create();
            }

            creator.Create(output, path, overwrite);
        }
    }
}
