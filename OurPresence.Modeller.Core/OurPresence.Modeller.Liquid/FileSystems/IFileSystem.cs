// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid.FileSystems
{
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
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        string ReadTemplateFile(Context context, string templateName);
    }
}
