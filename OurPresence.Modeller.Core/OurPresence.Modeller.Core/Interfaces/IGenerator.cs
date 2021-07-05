// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Interfaces
{
    /// <summary>
    /// The contract used to create a generator
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// The settings used by the generator
        /// </summary>
        ISettings Settings { get; }

        /// <summary>
        /// The command to use to create the generated output
        /// </summary>
        /// <returns>An <seealso cref="IOutput"/> type</returns>
        IOutput Create();
    }
}
