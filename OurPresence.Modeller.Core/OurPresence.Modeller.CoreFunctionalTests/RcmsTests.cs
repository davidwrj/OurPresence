// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using ApprovalTests;
using OurPresence.Modeller.Interfaces;
using NSubstitute;
using System.Linq;
using OurPresence.Modeller.Generator;
using Xunit;

namespace OurPresence.Modeller.CoreFunctionalTests
{
    public class RcmsTests
    {
        [Fact]
        public void GivenDomainProjectGenerator_WhenGenerating_ThenProjectCreated()
        {
            var packageService = Substitute.For<IPackageService>();
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(true);
            var packages = new Packages(packageService, settings);
            settings.Packages.Returns(packages);
            var module = RcmsModuleBuilders.CreateProject();

            var c = new DomainProject.Generator(settings, module);

            IProject output = c.Create() as IProject;
            Approvals.VerifyAll(output.FileGroups.SelectMany(f=>f.Files), "file", f => $"{f.FullName}\r\n{f.Content}");
        }
    }
}
