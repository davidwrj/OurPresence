// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

namespace OurPresence.Core.Money.Tests.Helpers
{
    public static class StreamExtensions
    {
        public static string ReadToString(this Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

}
