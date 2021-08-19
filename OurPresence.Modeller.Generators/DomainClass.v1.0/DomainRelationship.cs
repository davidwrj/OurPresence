// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Domain.Extensions;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;

namespace DomainClass
{
    public class DomainRelationship : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public DomainRelationship(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var sb = new StringBuilder();

            foreach (var relate in _model.Relationships)
            {
                if (relate.PrincipalType == RelationshipTypes.One && relate.DependantType == RelationshipTypes.Many)
                {
                    sb.I(2).Al($"public IList<{relate.DependantModel.Singular.Value}> {relate.DependantModel.Plural.Value} {{ get; set; }} = new List<{relate.DependantModel.Singular.Value}>();");
                    sb.B();
                }
                if (relate.PrincipalType == RelationshipTypes.Many && relate.DependantType == RelationshipTypes.One)
                {
                    sb.I(2).Al($"public {relate.DependantModel.Singular.Value} {relate.DependantModel.Singular.Value} {{ get; set; }}");
                    sb.B();
                }
            }
            sb.TrimEnd(Environment.NewLine);

            return new Snippet(sb.ToString());
        }
    }
}
