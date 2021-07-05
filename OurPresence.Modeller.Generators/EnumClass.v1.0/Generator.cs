// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace EnumClass
{
    public class Generator : IGenerator
    {
        private readonly Module _module;
        private readonly Enumeration _enumeration;

        public Generator(ISettings settings, Module module, Enumeration enumeration)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _enumeration = enumeration ?? throw new ArgumentNullException(nameof(enumeration));
        }

        public IOutput Create()
        {
            var sb = new StringBuilder();
            if (Settings.SupportRegen)
            {
                sb.Al(((ISnippet)new OverwriteHeader.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            else
            {
                sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            sb.Al($"namespace {_module.Namespace}.Common.Enums");
            sb.Al("{");
            if (_enumeration.Flag)
            {
                sb.I(1).Al("[Flags]");
            }
            sb.I(1).Al($"public enum {_enumeration.Name.Plural.Value}");
            sb.I(1).Al("{");
            foreach (var field in _enumeration.Items)
            {
                if (!string.IsNullOrWhiteSpace(field.Display) && !string.Equals(field.Display, field.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.I(2).Al($"[Display(Name = \"{field.Display}\")]");
                }
                sb.I(2).Al($"{field.Name} = {field.Value},");
            }
            sb.TrimEnd(",");
            sb.I(1).Al("}");
            sb.Al("}");

            var filename = _enumeration.Name.ToString();
            if (Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), path: "Enums", canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
