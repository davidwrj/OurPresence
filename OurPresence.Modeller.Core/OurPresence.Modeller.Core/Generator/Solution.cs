// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Generator.Exceptions;
using OurPresence.Modeller.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace OurPresence.Modeller.Generator
{
    public class Solution : FileGroup, ISolution
    {
        private readonly IList<IProject> _projects = new List<IProject>();

        public Solution()
        { }

        public Solution(string path) : base(path)
        { }

        public string? Namespace { get; set; }

        public string? Directory { get; set; }

        public IEnumerable<IProject> Projects => new ReadOnlyCollection<IProject>(_projects);
        
        public void AddProject(IProject project)
        {
            if (project == null) return;
            if (!(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(project.Path)))
                project.Path = Path.Combine(Name, project.Path);

            if (_projects.Any(p => p.Name == project.Name))
                throw new FileExistsException(project.Name);

            _projects.Add(project);
        }

        public IProject AddProject(Guid id, string name)
        {
            var project = new Project(name, id);
            AddProject(project);
            return project;
        }
    }
}
