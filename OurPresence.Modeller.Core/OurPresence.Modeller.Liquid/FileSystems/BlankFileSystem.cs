// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Liquid.Exceptions;

namespace OurPresence.Modeller.Liquid.FileSystems
{
    /// <summary>
    /// 
    /// </summary>
    public class BlankFileSystem : IFileSystem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string ReadTemplateFile(Context context, string templateName)
        {
            throw new FileSystemException(Liquid.ResourceManager.GetString("BlankFileSystemDoesNotAllowIncludesException"));
        }
    }
}
