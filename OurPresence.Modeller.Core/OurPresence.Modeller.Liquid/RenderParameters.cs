// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// Rendering parameters
    /// </summary>
    public class RenderParameters
    {
        /// <summary>
        /// If you provide a Context object, you do not need to set any other parameters.
        /// </summary>
        public Context Context { get; set; }

        /// <summary>
        /// Hash of local variables used during rendering
        /// </summary>
        public Hash LocalVariables { get; set; }

        /// <summary>
        /// Filters used during rendering
        /// </summary>
        public IEnumerable<Type> Filters { get; set; }

        /// <summary>
        /// Hash of user-defined, internally-available variables
        /// </summary>
        public Hash Registers { get; set; }

        /// <summary>
        /// Errors output mode
        /// </summary>
        public ErrorsOutputMode ErrorsOutputMode { get; set; } = ErrorsOutputMode.Display;

        /// <summary>
        /// Maximum number of iterations for the For tag
        /// </summary>
        public int MaxIterations { get; set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public IFormatProvider FormatProvider { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="formatProvider"></param>
        public RenderParameters(IFormatProvider formatProvider)
        {
            FormatProvider = formatProvider ?? throw new ArgumentNullException( nameof(formatProvider) );
        }

        /// <summary>
        /// Rendering timeout in ms
        /// </summary>
        public int Timeout { get; set; } = 0;

        internal void Evaluate(Template template, out Context context, out Hash registers, out IEnumerable<Type> filters)
        {
            if (Context != null)
            {
                context = Context;
                registers = null;
                filters = null;
                context.RestartTimeout();
                return;
            }

            var environments = new List<Hash>();
            if (LocalVariables != null)
            {
                environments.Add(LocalVariables);
            }

            environments.Add(template.Assigns);
            context = new Context(template, environments, template.InstanceAssigns, template.Registers, ErrorsOutputMode,
                MaxIterations, FormatProvider, default);

            registers = Registers;
            filters = Filters;
        }

        /// <summary>
        /// Creates a RenderParameters from a context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static RenderParameters FromContext(Context context, IFormatProvider formatProvider)
        {
            if (context == null)
            {
                throw new ArgumentNullException( nameof(context) );
            }

            return new RenderParameters(formatProvider) { Context = context };
        }
    }
}
