// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace OurPresence.Modeller.Interfaces
{
    public interface ISolution : IOutput
    {
        string? Directory { get; set; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<IProject> Projects { get; }

        void AddFile(IFile file);

        void AddProject(IProject project);
    }

}
