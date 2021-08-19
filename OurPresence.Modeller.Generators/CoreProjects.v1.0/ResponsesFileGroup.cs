// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class ResponsesFileGroup : IGenerator
    {
        private readonly Module _module;

        public ResponsesFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Responses");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Responses");
            sb.Al("{");
            sb.I(1).Al("public class PagedListResponse<T>");
            sb.I(1).Al("{");
            sb.I(2).Al("public IReadOnlyList<T> Items { get; }");
            sb.I(2).Al("public long TotalItemCount { get; }");
            sb.I(2).Al("public bool HasNextPage { get; }");
            sb.I(2).Al("public PagedListResponse(IEnumerable<T> items, long totalItemCount, bool hasNextPage)");
            sb.I(2).Al("{");
            sb.I(3).Al("Items = items.ToList();");
            sb.I(3).Al("TotalItemCount = totalItemCount;");
            sb.I(3).Al("HasNextPage = hasNextPage;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("PagedListResponse.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
