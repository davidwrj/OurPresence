// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator.Exceptions;
using OurPresence.Modeller.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public class FileGroup : IFileGroup
    {
        private readonly IList<IFile> _files = new List<IFile>();
        private readonly IList<IFileGroup> _fileGroups = new List<IFileGroup>();
        private readonly string _relativePath;

        public FileGroup()
        {
            _relativePath = string.Empty;
            Name = string.Empty;
        }

        public FileGroup(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                relativePath = string.Empty;

            _relativePath = relativePath;
            Name = _relativePath;
        }

        public IEnumerable<IFile> Files => new ReadOnlyCollection<IFile>(_files);
        public IEnumerable<IFileGroup> FileGroups => new ReadOnlyCollection<IFileGroup>(_fileGroups);

        public string Name { get; private set; }

        void IFileGroup.SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = string.Empty;
            Name = string.IsNullOrWhiteSpace(_relativePath) ? path : System.IO.Path.Combine(path, _relativePath);

            foreach (var file in Files)
                file.Path = DirectoryHelper.MergePaths(Name, file.Path);
        }

        public void AddFileGroup(IFileGroup fileGroup)
        {
            if(fileGroup == null) return;

            if(_fileGroups.Any(f => f.Name == fileGroup.Name))
                throw new FileGroupExistsException(fileGroup.Name);

            _fileGroups.Add(fileGroup);
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
