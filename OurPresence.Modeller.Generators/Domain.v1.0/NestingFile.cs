// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            sb.Al("{");
            sb.I(1).Al($"\"help\": \"https://go.microsoft.com/fwlink/?linkid=866610\",");
            sb.I(1).Al($"\"root\": false,");
            sb.B();
            sb.I(1).Al($"\"dependentFileProviders\": {{");
            sb.I(2).Al($"\"add\": {{ ");
            sb.I(3).Al($"\"pathSegment\": {{");
            sb.I(4).Al($"\"add\": {{");
            sb.I(5).Al($"\".*\": [");
            sb.I(6).Al($"\".cs\"");
            sb.I(5).Al("]");
            sb.I(4).Al("}");
            sb.I(3).Al("}");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");

            return new File(".filenesting.json", sb.ToString());
        }
    }
}
