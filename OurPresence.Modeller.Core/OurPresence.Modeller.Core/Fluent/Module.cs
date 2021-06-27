namespace OurPresence.Modeller.Fluent
{
    public static class Module
    {
        public static ModuleBuilder Create(string company,string project)
        {
            var module = new Domain.Module(company, project);
            return new ModuleBuilder(module);
        }
    }
}
