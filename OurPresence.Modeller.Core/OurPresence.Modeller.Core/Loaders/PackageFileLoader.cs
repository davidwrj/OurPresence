// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace OurPresence.Modeller.Loaders
{
    public class PackageFileLoader : ILoader<IEnumerable<IPackage>>
    {
        public IEnumerable<IPackage> Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<IPackage>();
            }

            string packages;
            using (var reader = File.OpenText(filePath))
            {
                packages = reader.ReadToEnd();
            }
            var data = packages.FromJson<IEnumerable<Domain.Package>>();
            return data is null ? new List<IPackage>() : data;
        }

        public bool TryLoad(string filePath, out IEnumerable<IPackage> packages)
        {
            try
            {
                packages = Load(filePath);
                return true;
            }
            catch
            {
                packages = new List<IPackage>();
                return false;
            }
        }
    }
}
