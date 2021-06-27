// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace OurPresence.Modeller.Liquid.Util
{
    internal class Symbol
    {
        public Func<object, bool> EvaluationFunction { get; set; }

        public Symbol(Func<object, bool> evaluationFunction)
        {
            EvaluationFunction = evaluationFunction;
        }
    }
}
