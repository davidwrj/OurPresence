// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Generator;
using OurPresence.Modeller.Interfaces;
using System;
using System.Text;

namespace ControllerClass
{
    public class ControllerGenerated : IGenerator
    {
        private readonly Module _module;
        private readonly Model _model;

        public ControllerGenerated(ISettings settings, Module module, Model model)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public IOutput Create()
        {
            if (!_model.IsRoot)
                return null;

            var sb = new StringBuilder();
            if (Settings.SupportRegen)
            {
                sb.Al(((ISnippet)new OverwriteHeader.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }
            else
            {
                sb.Al(((ISnippet)new Header.Generator(Settings, new GeneratorDetails()).Create()).Content);
            }

            foreach(var behaviour in _model.Behaviours)
            {
                sb.Al($"using {_module.Namespace}.BusinessLogic.{_model.Name}.{behaviour.Name};");
            }
            sb.B();
            sb.Al($"namespace {_module.Namespace}.Application.Controllers");
            sb.Al("{");
            sb.I(1).Al("[Route(\"api/[controller]/[action]\")]");
            sb.I(1).A(Settings.SupportRegen ? "partial" : "public");
            sb.A($" class {_model.Name}Controller");
            if(!Settings.SupportRegen)
            {
                sb.A(": ApplicationController");
            }
            sb.B();
            sb.I(1).Al("{");
            sb.I(2).Al("private readonly IMediator _mediator;");

            if(!Settings.SupportRegen)
            {
                sb.B();
                sb.I(2).Al($"public {_model.Name}Controller(IMediator mediator)");
                sb.I(2).Al("{");
                sb.I(3).Al("_mediator = mediator;");
                sb.I(2).Al("}");
            }

            foreach(var behaviour in _model.Behaviours)
            {
                sb.B();
                sb.I(2).Al($"[Http{behaviour.Verb}]");
                sb.I(2).A($"public async Task<IActionResult> {behaviour.Name}Async(");
                if(behaviour.Request is not null)
                {
                    sb.A($"[FromBody] {behaviour.Request.Name} {behaviour.Request.Name.Singular.LocalVariable}");
                }
                sb.Al(")");
                sb.I(2).Al("{");

                var request = behaviour.Request is null ?
                    new Name($"{_model.Name}{behaviour.Name}Request") :
                    behaviour.Request.Name;

                if(behaviour.Request is null)
                {
                    sb.I(3).Al($"var {request.Singular.LocalVariable} = new {request}();");
                }
                sb.I(3).A($"var result = await _mediator.Send(");
                sb.Al($"{request.Singular.LocalVariable});");
                if (behaviour.Response is not null)
                {
                    sb.I(3).A("return FromResult");
                    sb.A(behaviour.Response.IsCollection ? "<IEnumerable" : "");
                    sb.A($"<{behaviour.Response.Name}>");
                    sb.A(behaviour.Response.IsCollection ? ">" : "");
                    sb.Al("(result);");
                }
                else
                {
                    sb.I(3).Al("return NoContent();");
                }
                sb.I(2).Al("}");
            }

            sb.I(1).Al("}");
            sb.Al("}");

            var filename = _model.Name.ToString();
            filename += "Controller";
            if(Settings.SupportRegen)
            {
                filename += ".generated";
            }
            filename += ".cs";

            return new File(filename, sb.ToString(), canOverwrite: Settings.SupportRegen);
        }

        public ISettings Settings { get; }
    }
}
