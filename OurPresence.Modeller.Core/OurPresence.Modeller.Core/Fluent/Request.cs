using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Request
    {
        public static RequestBuilder Create(ModuleBuilder module, string name)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            var request = new Domain.Request(name);
            return new RequestBuilder(module, request);
        }
    }
}
