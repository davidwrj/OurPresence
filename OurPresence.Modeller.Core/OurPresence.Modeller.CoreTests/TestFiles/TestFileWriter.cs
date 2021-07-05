// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Interfaces;

namespace OurPresence.Modeller.Tests.TestFiles
{
    internal class TestFileWriter : IFileWriter
    {
        public string Output { get; private set; }

        void IFileWriter.Write(IFile file)
        {
            Output = file.Content;
        }
    }
}
