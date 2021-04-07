// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Liquid.Exceptions;

namespace OurPresence.Liquid.FileSystems
{
    public class BlankFileSystem : IFileSystem
    {
        public string ReadTemplateFile(Context context, string templateName)
        {
            throw new FileSystemException("Blank File System Does Not Allow Includes");
        }
    }

    /// <summary>
    /// This implements a file system which retrieves template files from embedded resources in .NET assemblies.
    ///
    /// Its behavior is the same as with the Local File System, except this uses namespaces and embedded resources
    /// instead of directories and files.
    ///
    /// Example:
    ///
    /// var fileSystem = new EmbeddedFileSystem("My.Base.Namespace");
    ///
    /// fileSystem.FullPath("mypartial") # => "My.Base.Namespace._mypartial.liquid"
    /// fileSystem.FullPath("dir/mypartial") # => "My.Base.Namespace.dir._mypartial.liquid"
    /// </summary>
    public class EmbeddedFileSystem : IFileSystem
    {
        protected System.Reflection.Assembly Assembly { get; private set; }

        public string Root { get; private set; }

        public EmbeddedFileSystem(System.Reflection.Assembly assembly, string root)
        {
            Assembly = assembly;
            Root = root;
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string)context[templateName];
            var fullPath = FullPath(templatePath);

            var stream = Assembly.GetManifestResourceStream(fullPath);
            if (stream == null)
                throw new FileSystemException("Local File System Template Not Found {0}", templatePath);

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public string FullPath(string templatePath)
        {
            if (templatePath == null || !Regex.IsMatch(templatePath, @"^[^.\/][a-zA-Z0-9_\/]+$"))
                throw new FileSystemException("Local File System Illegal Template Name {0}", templatePath);

            var basePath = templatePath.Contains("/")
                ? Path.Combine(Root, Path.GetDirectoryName(templatePath))
                : Root;

            var fileName = string.Format("_{0}.liquid", Path.GetFileName(templatePath));

            var fullPath = Regex.Replace(Path.Combine(basePath, fileName), @"\\|/", ".");

            return fullPath;
        }
    }

    /// <summary>
    /// A Liquid file system is way to let your templates retrieve other templates for use with the include tag.
    ///
    /// You can implement subclasses that retrieve templates from the database, from the file system using a different
    /// path structure, you can provide them as hard-coded inline strings, or any manner that you see fit.
    ///
    /// You can add additional instance variables, arguments, or methods as needed.
    ///
    /// Example:
    ///
    /// Liquid::Template.file_system = Liquid::LocalFileSystem.new(template_path)
    /// liquid = Liquid::Template.parse(template)
    ///
    /// This will parse the template with a LocalFileSystem implementation rooted at 'template_path'.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Called by Liquid to retrieve a template file
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        string ReadTemplateFile(Context context, string templateName);
    }

    /// <summary>
    /// This interface allow you return a Template instance,
    /// it can reduce the template parsing time in some cases.
    /// Please also provide the implementation of ReadTemplateFile for fallback purpose.
    /// </summary>
    public interface ITemplateFileSystem : IFileSystem
    {
        /// <summary>
        /// Called by Liquid to retrieve a template instance
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        Template GetTemplate(Context context, string templateName);
    }

    /// <summary>
    /// This implements an abstract file system which retrieves template files named in a manner similar to Rails partials,
    /// ie. with the template name prefixed with an underscore. The extension ".liquid" is also added.
    ///
    /// For security reasons, template paths are only allowed to contain letters, numbers, and underscore.
    ///
    /// Example:
    ///
    /// file_system = Liquid::LocalFileSystem.new("/some/path")
    ///
    /// file_system.full_path("mypartial") # => "/some/path/_mypartial.liquid"
    /// file_system.full_path("dir/mypartial") # => "/some/path/dir/_mypartial.liquid"
    /// </summary>
    public class LocalFileSystem : IFileSystem
    {
        public string Root { get; set; }

        public LocalFileSystem(string root)
        {
            Root = root;
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string) context[templateName];
            var fullPath = FullPath(templatePath);
            if (!File.Exists(fullPath))
                throw new FileSystemException("Local File System Template Not Found {0}", templatePath);
            return File.ReadAllText(fullPath);
        }

        public string FullPath(string templatePath)
        {
            if (templatePath == null || !Regex.IsMatch(templatePath, @"^[^.\/][a-zA-Z0-9_\/]+$"))
                throw new FileSystemException("Local File System Illegal Template Name {0}", templatePath);

            var fullPath = templatePath.Contains("/")
                ? Path.Combine(Path.Combine(Root, Path.GetDirectoryName(templatePath)), string.Format("_{0}.liquid", Path.GetFileName(templatePath)))
                : Path.Combine(Root, string.Format("_{0}.liquid", templatePath));

            //string escapedPath = Root.Replace(@"\", @"\\").Replace("(", @"\(").Replace(")", @"\)");
            var escapedPath = Regex.Escape(Root);
            if (!Regex.IsMatch(Path.GetFullPath(fullPath), string.Format("^{0}", escapedPath)))
                throw new FileSystemException("Local File System Illegal Template Path {0}", Path.GetFullPath(fullPath));

            return fullPath;
        }
    }
}
