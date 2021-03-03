using OurPresence.Modeller.Interfaces;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OurPresence.Modeller.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class Module : INamedElement
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private string DebuggerDisplay => Namespace;

        public static Module Clone(Module module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            return new Module(module.Company, module.Project, module.Feature, module.DefaultSchema
                );
        }

        public Module(string company, string project)
            :this(company, new Name(project))
        { }

        private Module(string company, Name project, Name? feature = null, string? defaultSchema = null)            
        {
            if (company is null)
                throw new ArgumentNullException(nameof(company));

            Company = company.Trim().Dehumanize().Pascalize();
            Project = project;
            Feature = feature;
            DefaultSchema = defaultSchema;
            FinaliseNames();
        }

        private void FinaliseNames()
        {
            foreach (var model in Models)
                model.FinaliseNames();
        }

        [JsonProperty]
        public string Company { get; }

        [JsonProperty]
        public Name Project { get; } 

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Name? Feature { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? DefaultSchema { get; set; }

        [JsonProperty]
        public List<Model> Models { get; } = new List<Model>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Enumeration> Enumerations { get; } = new List<Enumeration>();

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<Request> Requests { get; } = new List<Request>();

        public string Namespace
        {
            get
            {
                var result = Company == null ? string.Empty : Company + ".";
                result += Project.ToString() + ".";
                result += Feature is null ? string.Empty : Feature.ToString() + ".";
                return result.TrimEnd('.');
            }
        }

        string INamedElement.Name => Namespace;

        internal Model GetModel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Model name must be supplied.", nameof(name));
            return Models.FirstOrDefault(m => m.Name.Value == name) ?? throw new ArgumentNullException(nameof(name), $"Model {name} not found.");
        }
    }
}