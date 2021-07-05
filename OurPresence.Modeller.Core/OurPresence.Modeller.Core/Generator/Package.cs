// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
