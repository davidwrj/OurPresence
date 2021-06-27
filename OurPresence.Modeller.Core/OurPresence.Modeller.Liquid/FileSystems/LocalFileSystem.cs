// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.FileSystems
{
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
        /// <summary>
        /// 
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public LocalFileSystem(string root)
        {
            Root = root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string) context[templateName];
            var fullPath = FullPath(templatePath);
            if (!File.Exists(fullPath))
            {
                throw new FileSystemException(Liquid.ResourceManager.GetString("LocalFileSystemTemplateNotFoundException"), templatePath);
            }

            return File.ReadAllText(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        public string FullPath(string templatePath)
        {
            if (templatePath == null || !Regex.IsMatch(templatePath, @"^[^.\/][a-zA-Z0-9_\/]+$"))
            {
                throw new FileSystemException(Liquid.ResourceManager.GetString("LocalFileSystemIllegalTemplateNameException"), templatePath);
            }

            var fullPath = templatePath.Contains("/")
                ? Path.Combine(Path.Combine(Root, Path.GetDirectoryName(templatePath)), string.Format("_{0}.liquid", Path.GetFileName(templatePath)))
                : Path.Combine(Root, string.Format("_{0}.liquid", templatePath));

            //string escapedPath = Root.Replace(@"\", @"\\").Replace("(", @"\(").Replace(")", @"\)");
            var escapedPath = Regex.Escape(Root);
            if (!Regex.IsMatch(Path.GetFullPath(fullPath), string.Format("^{0}", escapedPath)))
            {
                throw new FileSystemException(Liquid.ResourceManager.GetString("LocalFileSystemIllegalTemplatePathException"), Path.GetFullPath(fullPath));
            }

            return fullPath;
        }
    }
}
