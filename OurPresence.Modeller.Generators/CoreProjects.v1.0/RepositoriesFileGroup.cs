// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class RepositoriesFileGroup : IGenerator
    {
        private readonly Module _module;

        public RepositoriesFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Repositories");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Namespace}.Core.Repositories");
            sb.Al("{");
            sb.I(1).Al("public interface IRepository<T> where T : IAggregate");
            sb.I(1).Al("{");
            sb.I(2).Al("Task<T?> Find(Guid id, CancellationToken cancellationToken);");
            sb.I(2).Al("Task Add(T aggregate, CancellationToken cancellationToken);");
            sb.I(2).Al("Task Update(T aggregate, CancellationToken cancellationToken);");
            sb.I(2).Al("Task Delete(T aggregate, CancellationToken cancellationToken);");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("IRepository.cs", sb.ToString(), canOverwrite: true));

            sb.Clear();
            sb.Al($"namespace {_module.Namespace}.Core.Ids");
            sb.Al("{");
            sb.I(1).Al("public static class RepositoryExtensions");
            sb.I(1).Al("{");
            sb.I(2).Al("public static async Task<T> Get<T>(this IRepository<T> repository, Guid id, CancellationToken cancellationToken = default) where T : IAggregate");
            sb.I(2).Al("{");
            sb.I(3).Al("var entity = await repository.Find(id, cancellationToken);");
            sb.B();
            sb.I(3).Al("return entity ?? throw AggregateNotFoundException.For<T>(id);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public static async Task<Unit> GetAndUpdate<T>(this IRepository<T> repository, Guid id, Action<T> action, CancellationToken cancellationToken = default) where T : IAggregate");
            sb.I(2).Al("{");
            sb.I(3).Al("var entity = await repository.Get<T>(id, cancellationToken);");
            sb.I(3).Al("action(entity);");
            sb.I(3).Al("await repository.Update(entity, cancellationToken);");
            sb.B();
            sb.I(3).Al("return Unit.Value;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("RepositoryExtensions.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
