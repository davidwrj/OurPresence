using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public class Project : IProject
    {
        private readonly IList<IFileGroup> _fileGroups = new List<IFileGroup>();
        private readonly IList<IFolderCopy> _folders = new List<IFolderCopy>();

        public Project(string name) : this(name, Guid.NewGuid())
        {
        }

        public Project(string name, Guid id)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("A project name is required", nameof(name));

            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException(nameof(id), "Project Id should not be empty");

            Name = name;
            Path = name;
            Id = id;
        }

        public Guid Id { get; }

        public IEnumerable<IFileGroup> FileGroups => new ReadOnlyCollection<IFileGroup>(_fileGroups);

        public IEnumerable<IFolderCopy> Folders => new ReadOnlyCollection<IFolderCopy>(_folders);

        public string Path { get; set; }

        public string Name { get; }

        public IFileGroup AddFileGroup(IFileGroup fileGroup)
        {
            if (fileGroup == null)
            {
                var emptyFg = _fileGroups.FirstOrDefault(f => string.IsNullOrWhiteSpace(f.Name));
                if (emptyFg == null)
                    emptyFg = new FileGroup();
                return emptyFg;
            }

            var exsiting = _fileGroups.FirstOrDefault(f => f.Name == fileGroup.Name);
            if (exsiting == null)
            {
                _fileGroups.Add(fileGroup);
                return fileGroup;
            }
            else
            {
                foreach (var file in fileGroup.Files)
                    exsiting.AddFile(file);
                return exsiting;
            }
        }

        public void AddFolder(IFolderCopy folder)
        {
            if (folder == null) return;

            folder.SetPath(Path);

            if (_folders.Any(f => f.Name == folder.Name))
                return;

            _folders.Add(folder);
        }
    }
}
