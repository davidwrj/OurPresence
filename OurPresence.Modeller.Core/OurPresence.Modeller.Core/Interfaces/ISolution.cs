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
