// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class SubscriptionsFileGroup : IGenerator
    {
        private readonly Module _module;

        public SubscriptionsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Subscriptions");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Subscriptions");
            sb.Al("{");
            sb.I(1).Al("public class InMemorySubscriptionCheckpointRepository: ISubscriptionCheckpointRepository");
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly ConcurrentDictionary<string, ulong> checkpoints = new();");
            sb.B();
            sb.I(2).Al("public ValueTask<ulong?> Load(string subscriptionId, CancellationToken ct)");
            sb.I(2).Al("{");
            sb.I(3).Al("return new(checkpoints.TryGetValue(subscriptionId, out var checkpoint) ? checkpoint : null);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public ValueTask Store(string subscriptionId, ulong position, CancellationToken ct)");
            sb.I(2).Al("{");
            sb.I(3).Al("checkpoints.AddOrUpdate(subscriptionId, position,(_, _) => position);");
            sb.I(3).Al("return ValueTask.CompletedTask;");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("InMemorySubscriptionCheckpointRepository.cs", sb.ToString(), canOverwrite: true));
            sb.Clear();

            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Subscriptions");
            sb.Al("{");
            sb.I(1).Al("public interface ISubscriptionCheckpointRepository");
            sb.I(1).Al("{");
            sb.I(2).Al("ValueTask<ulong?> Load(string subscriptionId, CancellationToken ct);");
            sb.B();
            sb.I(2).Al("ValueTask Store(string subscriptionId, ulong position, CancellationToken ct);");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("ISubscriptionCheckpointRepository.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
