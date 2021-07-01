using OurPresence.Modeller.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace OurPresence.Modeller.Generator.Outputs
{
    public class Builder : IBuilder
    {
        private readonly IContext _context;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IOutputStrategy _outputStrategy;
        private readonly ILogger<Builder> _logger;

        public Builder(IContext context, ICodeGenerator codeGenerator, IOutputStrategy outputStrategy, ILogger<Builder> logger)
        {
            _context = context;
            _codeGenerator = codeGenerator;
            _outputStrategy = outputStrategy;
            _logger = logger;
        }

        public void Create(IGeneratorConfiguration configuration)
        {
            _logger.LogInformation("Generation started: {Started}",DateTime.Now.ToShortTimeString());
            
            var result = _context.ValidateConfiguration(configuration);
            if (!result.IsValid)
            {
                _logger.LogInformation("Generation failed: {Failed}",DateTime.Now.ToShortTimeString());
                return;
            }

            var output = _codeGenerator.Create(_context);
            try
            {
                _outputStrategy.Create(output, _context.Settings.OutputPath, _context.Settings.SupportRegen);
                _logger.LogInformation("Generation complete: {Complete}",DateTime.Now.ToShortTimeString());

            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Generation skipped for Module '{Module}' as there was no defined output strategy",_context.Module?.Name+string.Empty);
            }
        }
    }
}
