// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid.FileSystems
{
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
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        Template GetTemplate(Context context, string templateName);
    }
}
