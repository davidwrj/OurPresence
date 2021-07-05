// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Domain
{
    public class Package:IPackage
    {
        public Package(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; set; }

        public string Version { get; set; }
    }

}