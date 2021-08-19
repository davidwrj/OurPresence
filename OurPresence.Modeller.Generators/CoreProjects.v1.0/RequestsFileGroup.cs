// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace CoreProjects
{
    internal class RequestsFileGroup : IGenerator
    {
        private readonly Module _module;

        public RequestsFileGroup(ISettings settings, Module module)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        public ISettings Settings { get; }

        public IOutput Create()
        {
            var group = new FileGroup("Requests");

            var sb = new StringBuilder();
            sb.Al($"namespace {_module.Company}.{_module.Project}.Core.Requests");
            sb.Al("{");
            sb.I(1).Al("public interface IExternalCommandBus");
            sb.I(1).Al("{");
            sb.I(2).Al("Task Post<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand;");
            sb.I(2).Al("Task Put<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand;");
            sb.I(2).Al("Task Delete<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand;");
            sb.I(1).Al("}");
            sb.B();
            sb.I(1).Al("public class ExternalCommandBus: IExternalCommandBus");
            sb.I(1).Al("{");
            sb.I(2).Al("public Task Post<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand");
            sb.I(2).Al("{");
            sb.I(3).Al("var client = new RestClient(url);");
            sb.B();
            sb.I(3).Al("var request = new RestRequest(path, DataFormat.Json);");
            sb.I(3).Al("request.AddJsonBody(command);");
            sb.B();
            sb.I(3).Al("return client.PostAsync<dynamic>(request, cancellationToken);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public Task Put<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand");
            sb.I(2).Al("{");
            sb.I(3).Al("var client = new RestClient(url);");
            sb.B();
            sb.I(3).Al("var request = new RestRequest(path, DataFormat.Json);");
            sb.I(3).Al("request.AddJsonBody(command);");
            sb.B();
            sb.I(3).Al("return client.PutAsync<dynamic>(request, cancellationToken);");
            sb.I(2).Al("}");
            sb.B();
            sb.I(2).Al("public Task Delete<T>(string url, string path, T command, CancellationToken cancellationToken = default) where T: ICommand");
            sb.I(2).Al("{");
            sb.I(3).Al("var client = new RestClient(url);");
            sb.B();
            sb.I(3).Al("var request = new RestRequest(path, DataFormat.Json);");
            sb.I(3).Al("request.AddJsonBody(command);");
            sb.B();
            sb.I(3).Al("return client.DeleteAsync<dynamic>(request, cancellationToken);");
            sb.I(2).Al("}");
            sb.I(1).Al("}");
            sb.Al("}");
            group.AddFile(new File("ExternalCommandBus.cs", sb.ToString(), canOverwrite: true));

            return group;
        }
    }
}
