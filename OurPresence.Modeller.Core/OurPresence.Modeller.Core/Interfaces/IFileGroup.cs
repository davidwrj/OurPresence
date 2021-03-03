using System.Collections.Generic;

namespace OurPresence.Modeller.Interfaces
{
    /// <summary>
    /// An output type that represents a group of generated files
    /// </summary>
    public interface IFileGroup : IOutput
    {
        /// <summary>
        /// A collection of generated files
        /// </summary>
        IEnumerable<IFile> Files { get; }

        /// <summary>
        /// Adds a file to the current folder path
        /// </summary>
        /// <param name="file">An <see cref="IFile"/> instance</param>
        /// <remarks>Files will automatically have their path set when added to a group.</remarks>
        void AddFile(IFile file);

        /// <summary>
        /// Allows the path to be altered
        /// </summary>
        /// <param name="path">new path</param>
        void SetPath(string path);

        /// <summary>
        /// Adds a file to the current folder path
        /// </summary>
        /// <param name="content">File content</param>
        /// <param name="name">Filename</param>
        /// <returns>Returns a <see cref="IFile"/> instance</returns>
        /// <remarks>Files will automatically have their path set when added to a group.</remarks>
        IFile AddFile(string content, string name);
    }

}
