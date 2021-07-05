// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Request
    {
        public static RequestBuilder Create(ModuleBuilder module, string name)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            var request = new Domain.Request(name);
            return new RequestBuilder(module, request);
        }
    }
}
