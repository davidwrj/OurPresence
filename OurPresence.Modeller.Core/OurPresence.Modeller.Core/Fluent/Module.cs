namespace OurPresence.Modeller.Fluent
{
    public static class Module
    {
        public static ModuleBuilder Create(string name,string project)
        {
            var module = new Domain.Module(name, project);
            return new ModuleBuilder(module);
        }
    }
}
