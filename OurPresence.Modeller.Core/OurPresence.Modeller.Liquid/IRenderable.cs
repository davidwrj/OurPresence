// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Object that can render itslef
    /// </summary>
    internal interface IRenderable
    {
        Template Template { get; }

        void Render(Context context, TextWriter result);
    }
}
