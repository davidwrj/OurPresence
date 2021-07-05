// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Generator
{
    public class Snippet : ISnippet
    {
        public Snippet(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                content = string.Empty;
            Content = content;
            Name = string.Empty;
        }

        public Snippet(string name, string content)
            : this(content)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = string.Empty;
            Name = name;
        }

        public string Content { get; }

        public string Name { get; }
    }
}
