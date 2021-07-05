// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class File : IFile
    {
        public File(string name, string content, string path = "", bool canOverwrite = false)
        {
            Name = name;
            Content = content;
            Path = path;
            CanOverwrite = canOverwrite;
        }

        public string Path { get; set; }

        public string Name { get; set; } = "NotNamed";

        public string Content { get; set; }

        public string FullName => System.IO.Path.Combine(Path, Name);

        public bool CanOverwrite { get; set; }
    }
}
