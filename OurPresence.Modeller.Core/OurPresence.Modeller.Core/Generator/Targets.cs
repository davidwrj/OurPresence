// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace OurPresence.Modeller.Generator
{
    public class Targets
    {
        private ICollection<string> _supported = new List<string> { "netcore3.1", "net5.0", "net6.0" };

        public static Targets Shared { get; } = new Targets();

        public Targets()
        {
            Reset();
        }

        public static string Default => "net5.0";

        public void RegisterTarget(string target)
        {
            var t = target.ToLowerInvariant();
            if (_supported.Contains(t)) return;
            _supported.Add(t);
        }

        public void Reset()
        {
            _supported = new List<string> { "netcore3.1", "net5.0", "net6.0" };
        }

        public IEnumerable<string> Supported => _supported;
    }
}
