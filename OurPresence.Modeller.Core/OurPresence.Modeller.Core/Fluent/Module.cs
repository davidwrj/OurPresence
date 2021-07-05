// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Fluent
{
    public static class Module
    {
        public static ModuleBuilder Create(string company,string project)
        {
            var module = new Domain.Module(company, project);
            return new ModuleBuilder(module);
        }
    }
}
