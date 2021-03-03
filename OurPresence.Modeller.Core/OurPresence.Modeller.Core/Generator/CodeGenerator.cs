using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace OurPresence.Modeller.Generator
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly ILogger<ICodeGenerator> _logger;

        public CodeGenerator(ILogger<ICodeGenerator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IOutput Create(IContext context)
        {
            if (!context.IsValid())
                return new EmptyOutput();

            _logger.LogInformation(context.ToString());

            var type = context.Generator?.Metadata.EntryPoint;
            if (type == null)
                return new EmptyOutput();

            var cis = type.GetConstructors();
            if (cis.Length != 1)
            {
                _logger.LogError("Modeller only supports single public constructors for a generator.");
                return new EmptyOutput();
            }
            var ci = cis[0];
            var args = new List<object>();

            foreach (var p in ci.GetParameters())
            {
                if (p.ParameterType.FullName == typeof(ISettings).FullName && context.Settings != null)
                    args.Add(context.Settings);
                else if (p.ParameterType.FullName == typeof(Domain.Module).FullName && context.Module != null)
                    args.Add(context.Module);
                else if (p.ParameterType.FullName == typeof(Domain.Model).FullName && context.Model != null)
                    args.Add(context.Model);
                else
                {
                    _logger.LogError($"{p.ParameterType.ToString()} is not a supported argument type on Generator constructors.");
                    return new EmptyOutput();
                }
            }
            return ((IGenerator)ci.Invoke(args.ToArray())).Create();
        }
    }
}
