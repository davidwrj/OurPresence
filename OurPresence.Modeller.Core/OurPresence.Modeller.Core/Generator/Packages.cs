// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class Packages
    {
        private readonly IList<IPackage> _packages = new List<IPackage>();

        public Packages(IPackageService packageService, ISettings settings)
        {
            packageService.Refresh(System.IO.Path.Combine(settings.LocalFolder, settings.Target,
                settings.Target + ".json"));
            Register(packageService.Items);
        }
        
        public int Count => _packages.Count;
        
        public string GetVersion(string name, string fallbackVersion)
        {
            var found = _packages.SingleOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
            return found == null ? fallbackVersion : found.Version;
        }

        public void Register(string name, string version) => Register(new Package(name, version));

        private void Register(IEnumerable<IPackage> packages)
        {
            foreach (var package in packages)
            {
                Register(package);
            }
        }
        
        private void Register(IPackage? package)
        {
            if (package is null || string.IsNullOrWhiteSpace(package.Name) || string.IsNullOrWhiteSpace(package.Version)) return;  

            var packages = _packages.Where(pa => pa.Name == package.Name).ToList();
            if (!packages.Any())
            {
                _packages.Add(package);
                return;
            }

            var p = packages.First();
            if (!Version.TryParse(p.Version, out var p1)) return;
            if (!Version.TryParse(package.Version, out var p2)) return;
            if (p1 < p2) p.Version = p2.ToString();
        }
    }
}