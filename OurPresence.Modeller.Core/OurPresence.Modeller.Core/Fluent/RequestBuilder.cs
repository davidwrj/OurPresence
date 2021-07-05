// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using System;
using System.ComponentModel;

namespace OurPresence.Modeller.Fluent
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RequestBuilder : FluentBase
    {
        public RequestBuilder(ModuleBuilder module, Domain.Request request)
        {
            Build = module ?? throw new ArgumentNullException(nameof(module));
            Instance = request ?? throw new ArgumentNullException(nameof(request));
        }

        public ModuleBuilder Build { get; }

        public Domain.Request Instance { get; }

        public RequestBuilder Name(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            Instance.Name.SetName(name);
            return this;
        }
        
        public FieldBuilder<RequestBuilder> AddField(string name)
        {
            var field = Field<RequestBuilder>.Create(this, name);
            Instance.Fields.Add(field.Instance);
            return field;
        }

        public ResponseBuilder WithResponse()
        {
            Instance.Response = new Response();            
            return new ResponseBuilder(this, Instance.Response);
        }
    }
}
