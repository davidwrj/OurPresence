// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Interfaces
{
    /// <summary>
    /// An output type that represents a file to copy.  No changes will be made to this output type.
    /// </summary>
    public interface IFileCopy : IOutput
    {
        /// <summary>
        /// The source file name
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// The destination file name
        /// </summary>
        string Destination { get; set; }
    }

}
