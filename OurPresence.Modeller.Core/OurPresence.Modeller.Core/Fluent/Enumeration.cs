using System;

namespace OurPresence.Modeller.Fluent
{
    public static class Enumeration
    {
        public static EnumerationBuilder Create(ModuleBuilder module, string name)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            var enumeration = new Domain.Enumeration(name);
            return new EnumerationBuilder(module, enumeration);
        }
    }
}
