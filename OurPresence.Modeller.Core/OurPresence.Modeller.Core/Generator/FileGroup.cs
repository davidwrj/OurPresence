using OurPresence.Modeller.Generator.Exceptions;
using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public class FileGroup : IFileGroup
    {
        private readonly IList<IFile> _files = new List<IFile>();
        private readonly string _relativePath;

        public FileGroup()
        {
            _relativePath = string.Empty;
            Name = string.Empty;
        }

        public FileGroup(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException(nameof(relativePath));

            _relativePath = relativePath;
            Name = _relativePath;
        }

        public IEnumerable<IFile> Files => new ReadOnlyCollection<IFile>(_files);

        public string Name { get; private set; }

        void IFileGroup.SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = string.Empty;
            Name = string.IsNullOrWhiteSpace(_relativePath) ? path : System.IO.Path.Combine(path, _relativePath);

            foreach (var file in Files)
                file.Path = DirectoryHelper.MergePaths(Name, file.Path);
        }

        public void AddFile(IFile file)
        {
            if (file == null) return;

            file.Path = DirectoryHelper.MergePaths(Name, file.Path);

            if (_files.Any(f => f.FullName == file.FullName))
                throw new FileExistsException(file.FullName);

            _files.Add(file);
        }

        public IFile AddFile(string content, string name)
        {
            var file = new File(name, content);
            AddFile(file);
            return file;
        }
    }
}
