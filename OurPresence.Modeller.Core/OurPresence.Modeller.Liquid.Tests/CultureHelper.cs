// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace OurPresence.Modeller.Liquid
{
    internal static class CultureHelper
    {
        public static IDisposable SetCulture(string name)
        {
            var scope = new CultureScope(CultureInfo.CurrentCulture);
            
#if CORE
            CultureInfo.CurrentCulture = new CultureInfo(name);
#else
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
#endif
            return scope;
        }

        private class CultureScope : IDisposable
        {
            private readonly CultureInfo culture;

            public CultureScope(CultureInfo culture)
            {
                this.culture = culture;
            }

            public void Dispose()
            {
#if CORE
                CultureInfo.CurrentCulture = this.culture;
#else
                System.Threading.Thread.CurrentThread.CurrentCulture =  this.culture;
#endif
            }
        }
    }
}