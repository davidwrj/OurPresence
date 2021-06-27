// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text.RegularExpressions;
using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.FileSystems
{
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
        /// <summary>
        /// 
        /// </summary>
        protected System.Reflection.Assembly Assembly { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Root { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="root"></param>
        public EmbeddedFileSystem(System.Reflection.Assembly assembly, string root)
        {
            Assembly = assembly;
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
            var templatePath = (string)context[templateName];
            var fullPath = FullPath(templatePath);

            var stream = Assembly.GetManifestResourceStream(fullPath);
            if (stream == null)
            {
                throw new FileSystemException(
                    Liquid.ResourceManager.GetString("LocalFileSystemTemplateNotFoundException"), templatePath);
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
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
                throw new FileSystemException(
                    Liquid.ResourceManager.GetString("LocalFileSystemIllegalTemplateNameException"), templatePath);
            }

            var basePath = templatePath.Contains("/")
                ? Path.Combine(Root, Path.GetDirectoryName(templatePath))
                : Root;

            var fileName = string.Format("_{0}.liquid", Path.GetFileName(templatePath));

            var fullPath = Regex.Replace(Path.Combine(basePath, fileName), @"\\|/", ".");

            return fullPath;
        }
    }
}
