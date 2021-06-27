// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// This allows for extra security by only giving the template access to the specific
    /// variables you want it to have access to.
    /// </summary>
    public interface ILiquidizable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object ToLiquid();
    }
}
