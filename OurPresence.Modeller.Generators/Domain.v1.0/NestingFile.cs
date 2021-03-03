using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace DomainProject
{
    internal class NestingFile : IGenerator
    {
        public NestingFile(ISettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            sb.al("{");
            sb.i(1).al($"\"help\": \"https://go.microsoft.com/fwlink/?linkid=866610\",");
            sb.i(1).al($"\"root\": false,");
            sb.b();
            sb.i(1).al($"\"dependentFileProviders\": {{");
            sb.i(2).al($"\"add\": {{ ");
            sb.i(3).al($"\"allExtensions\": {{");
            sb.i(4).al($"\"add\": {{");
            sb.i(5).al($"\".*\": [");
            sb.i(6).al($"\".cs\"");
            sb.i(5).al("]");
            sb.i(4).al("}");
            sb.i(3).al("}");
            sb.i(2).al("}");
            sb.i(1).al("}");
            sb.al("}");

            return new File(".filenesting.json", sb.ToString());
        }
    }
}
