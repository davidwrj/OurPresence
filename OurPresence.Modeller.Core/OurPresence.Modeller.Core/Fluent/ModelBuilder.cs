// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ModelBuilder : FluentBase
    {
        public ModelBuilder(ModuleBuilder module, Domain.Model model)
        {
            Build = module ?? throw new ArgumentNullException(nameof(module));
            Instance = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ModuleBuilder Build { get; }

        public Domain.Model Instance { get; }

        public ModelBuilder Name(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Instance.Name.SetName(name);
            return this;
        }

        public ModelBuilder IsAuditable(bool value = true)
        {
            Instance.HasAudit = value;
            return this;
        }

        public ModelBuilder IsRoot(bool value = true)
        {
            Instance.IsRoot = value;
            return this;
        }

        public ModelBuilder SupportCrud(CRUDSupport value = CRUDSupport.All)
        {
            Instance.SupportCrud = value;
            return this;
        }

        public ModelBuilder WithDefaultKey()
        {
            Instance.Key.Fields.Add(new Field("Id") { DataType = DataTypes.Int32, Nullable = false });
            return this;
        }

        public ModelBuilder Schema(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Instance.Schema = name;
            return this;
        }

        public KeyBuilder WithKey()
        {
            var key = Fluent.Key.Create(this, Instance);
            return key;
        }

        public FieldBuilder<ModelBuilder> AddField(string name)
        {
            var field = Field<ModelBuilder>.Create(this, name);
            Instance.Fields.Add(field.Instance);
            return field;
        }

        public RelationshipBuilder AddRelationship()
        {
            var relation = Fluent.Relationship.Create(Build, this);
            Instance.Relationships.Add(relation.Instance);
            return relation;
        }

        public IndexBuilder AddIndex(string name)
        {
            var index = Index.Create(this, name);
            Instance.Indexes.Add(index.Instance);
            return index;
        }

        public BehaviourBuilder<ModelBuilder> AddBehaviour(string requestName)
        {
            var behaviour = Behaviour<ModelBuilder>.Create(this, requestName);
            Instance.Behaviours.Add(behaviour.Instance);
            return behaviour;
        }
    }
}
