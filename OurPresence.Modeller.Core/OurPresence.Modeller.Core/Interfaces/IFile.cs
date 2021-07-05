// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Interfaces
{

    /// <summary>
    /// An output type that represents a code generated file
    /// </summary>
    public interface IFile : IOutput
    {
        /// <summary>
        /// Generated file content
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// The path to the file
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Returns the full path and filename
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// True if this file can overwrite an existing file, False if not.
        /// </summary>
        bool CanOverwrite { get; set; }
    }
}
