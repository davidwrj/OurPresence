// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain.Extensions;

namespace OurPresence.Modeller.Domain
{
    public class Names
    {
        public Names(string value, string local, string stat, string display)
        {
            Value = value;
            LocalVariable = local.CheckKeyword();
            ModuleVariable = string.IsNullOrWhiteSpace(value) ? string.Empty : "_" + local;
            StaticVariable = stat;
            Display = display;
        }

        public string LocalVariable { get; }

        public string ModuleVariable { get; }

        public string StaticVariable { get; }

        public string Display { get; }

        public string Value { get; }

        public override string ToString() => Value;
    }
}
