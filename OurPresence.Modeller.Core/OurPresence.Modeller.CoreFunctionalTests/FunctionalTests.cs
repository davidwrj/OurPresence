using ApprovalTests;
using OurPresence.Modeller.Interfaces;
using NSubstitute;
using System.Linq;
using OurPresence.Modeller.Generator;
using Xunit;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public class FunctionalTests
    {
        [Fact]
        public void GivenDataAccessProjectGenerator_WhenGenerating_ThenProjectCreated()
        {
            var packageService = Substitute.For<IPackageService>();
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(true);
            var packages = new Packages(packageService, settings);
            settings.Packages.Returns(packages);
            var module = ModuleBuilders.CreateProject();

            var c = new EntityFrameworkProject.Generator(settings, module);

            IProject output = c.Create() as IProject;
            Approvals.VerifyAll(output.FileGroups.SelectMany(f=>f.Files), "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenDataAccessConfigGenerator_WhenSingleSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen = true;
            var module = ModuleBuilders.CreateModule();

            var c = new EntityFrameworkClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenDataAccessConfigGenerator_WhenSingleNotSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            var module = ModuleBuilders.CreateModule();

            var c = new EntityFrameworkClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenBusinessLogicProjectGenerator_WhenGenerating_ThenProjectCreated()
        {
            var packageService = Substitute.For<IPackageService>();
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(true);
            var packages = new Packages(packageService, settings);
            settings.Packages.Returns(packages);
            var module = ModuleBuilders.CreateProject();

            var c = new BusinessLogicProject.Generator(settings, module);

            IProject output = c.Create() as IProject;
            Approvals.VerifyAll(output.FileGroups.SelectMany(f => f.Files), "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenBusinessLogicBehaviourGenerator_WhenSingleSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen = true;
            var module = ModuleBuilders.CreateModule();

            var c = new BusinessLogicBehaviour.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenBusinessLogicBehaviourGenerator_WhenSingleNotSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            var module = ModuleBuilders.CreateModule();

            var c = new BusinessLogicBehaviour.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenApplicationProjectGenerator_WhenGenerating_ThenProjectCreated()
        {
            var packageService = Substitute.For<IPackageService>();
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(true);
            var packages = new Packages(packageService, settings);
            settings.Packages.Returns(packages);
            var module = ModuleBuilders.CreateProject();

            var c = new ApplicationProject.Generator(settings, module);

            IProject output = c.Create() as IProject;

            Approvals.VerifyAll(output.FileGroups.SelectMany(f => f.Files), $"file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenControllerClassGenerator_WhenSingleSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen = true;
            var module = ModuleBuilders.CreateModule();

            var c = new ControllerClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

        [Fact]
        public void GivenControllerClassGenerator_WhenSingleNotSupportRegen_ThenSingleClassCreated()
        {
            var settings = Substitute.For<ISettings>();
            var module = ModuleBuilders.CreateModule();

            var c = new ControllerClass.Generator(settings, module, module.Models.First());

            IFileGroup output = c.Create() as IFileGroup;
            Approvals.VerifyAll(output.Files, "file", f => $"{f.FullName}\r\n{f.Content}");
        }

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
            var packageService = Substitute.For<IPackageService>();
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(true);
            var packages = new Packages(packageService, settings);
            settings.Packages.Returns(packages);
            var module = ModuleBuilders.CreateProject();

            var c = new DomainProject.Generator(settings, module);

            IProject output = c.Create() as IProject;
            Approvals.VerifyAll(output.FileGroups.SelectMany(f => f.Files), "file", f => $"{f.FullName}\r\n{f.Content}");
        }
    }
}
