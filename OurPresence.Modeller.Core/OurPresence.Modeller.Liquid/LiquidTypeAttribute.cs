// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Specifies the type is safe to be rendered by OurPresence.Modeller.Liquid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LiquidTypeAttribute : Attribute
    {
        /// <summary>
        /// An array of property and method names that are allowed to be called on the object.
        /// </summary>
        public string[] AllowedMembers { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="allowedMembers">An array of property and method names that are allowed to be called on the object.</param>
        public LiquidTypeAttribute(params string[] allowedMembers)
        {
            AllowedMembers = allowedMembers;
        }
    }
}
