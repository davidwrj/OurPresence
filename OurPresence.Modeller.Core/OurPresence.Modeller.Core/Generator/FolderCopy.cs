// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;
using System.IO;

namespace OurPresence.Modeller.Generator
{
    public class FolderCopy : IFolderCopy
    {
        public FolderCopy(string source, string destination, bool includeSubFolders) : this(source, destination)
        {
            IncludeSubFolders = includeSubFolders;
        }

        public FolderCopy(string source, string destination)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new System.ArgumentException("Source directory is required", nameof(source));

            if (string.IsNullOrWhiteSpace(destination))
                throw new System.ArgumentException("Destination directory is required", nameof(destination));

            if (Path.IsPathRooted(destination))
                throw new System.ArgumentException("Destination should be defined as a relative path", nameof(destination));

            Source = source;
            Destination = destination;
            Name = destination;
        }

        public string Source { get; }

        public string Destination { get; }

        public bool IncludeSubFolders { get; } = true;

        void IFolderCopy.SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = string.Empty;
            Name = string.IsNullOrWhiteSpace(Destination) ? path : Path.Combine(path, Destination);
        }

        public string Name { get; private set; }
    }
}
