// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class EnumerationExtensions
    {
        public static Enumeration AddItem(this Enumeration enumeration, string name)
        {
            int value;
            if (enumeration.Items.Count == 0)
                value = 0;
            else if (enumeration.Flag)
                value = 1 << (enumeration.Items.Count - 1);
            else
                value = enumeration.Items.Count;

            enumeration.Items.Add(new EnumerationItem(value, name));
            return enumeration;
        }

        public static Enumeration AddItem(this Enumeration enumeration, string name, int value, string? display = null)
        {
            if (enumeration.Items.Any(i => i.Value == value))
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value already exists in the enumeration");

            if (string.IsNullOrWhiteSpace(display)) display = name;

            enumeration.Items.Add(new EnumerationItem(value, name, display));
            return enumeration;
        }
    }
}
