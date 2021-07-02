using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class UsingsFile : IGenerator
    {
        public UsingsFile(ISettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.Al("global using System;");
            sb.Al("global using System.Collections.Generic;");
            sb.Al("global using System.Linq;");
            sb.Al("global using System.Text;");
            sb.Al("global using System.Reflection;");
            sb.Al("global using System.Text.RegularExpressions;");
            sb.Al("global using System.Threading.Tasks;");
            return new File("usings.cs", sb.ToString());
        }
    }
}
