// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace OurPresence.Modeller.Generator
{
    public static class DirectoryHelper
    {
        public static string MergePaths(string current, string proposed)
        {
            if (string.IsNullOrWhiteSpace(proposed))
                return current;

            if (string.IsNullOrWhiteSpace(current))
                return proposed;

            current = current.TrimEnd('\\');
            proposed = proposed.TrimStart('\\');

            for (var i = proposed.Length; i > 0; i--)
            {
                if (current.EndsWith(proposed.Substring(0, i), StringComparison.InvariantCultureIgnoreCase))
                    return Path.Combine(current, proposed.Substring(i).TrimStart('\\'));
            }
            return Path.Combine(current, proposed);
        }
    }
}
