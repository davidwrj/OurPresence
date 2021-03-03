using ApprovalTests;
using OurPresence.Modeller.Interfaces;
using NSubstitute;
using System.Linq;
using Xunit;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public class FunctionalTests
    {
        [Fact]
        public void GivenDomainClassGenerator_WhenSingleNotSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            var module = ModuleBuilders.CreateModule();

            var c = new DomainClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}" );
        }

        [Fact]
        public void GivenDomainClassGenerator_WhenSingleSupportRegen_ThenTwoClassedCreated()
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen = true;
            var module = ModuleBuilders.CreateModule();

            var c = new DomainClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenDomainProjectGenerator_WhenGenerating_ThenProjectCreated()
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen = true;
            var module = ModuleBuilders.CreateProject();

            var c = new DomainProject.Generator(settings, module);

            IProject output = c.Create() as IProject;
            foreach (var item in output.FileGroups)
            {
                Approvals.VerifyAll(item.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
            }
        }
    }
}
