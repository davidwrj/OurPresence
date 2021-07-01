using System.Linq;
using FluentAssertions;
using NSubstitute;
using OurPresence.Modeller.Domain;
using OurPresence.Modeller.Interfaces;
using TechTalk.SpecFlow;

namespace OurPresence.ModellerTests.Steps
{
    [Binding]
    public class DomainGeneratorSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public DomainGeneratorSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"a model")]
        public void GivenAModel()
        {
            _scenarioContext.Add("module", Modeller.Fluent.Module
                .Create("Hy", "Modeller")
                .AddModel("Model")
                    .WithDefaultKey()
                    .AddField("Name").BusinessKey(true).MaxLength(5).Build
                    .AddRelationship().Relate("Model", new[] { "Id" }, "Key", new[] { "ModelId" }, RelationshipTypes.One, RelationshipTypes.One).Build
                    .Build.Build);
        }
        
        [Given(@"a (.*) generator with regen set to (.*)")]
        public void GivenAGeneratorSupportingRegen(string generatorType, bool regenSupported)
        {
            var settings = Substitute.For<ISettings>();
            settings.SupportRegen.Returns(regenSupported);

            var module = _scenarioContext.Get<Module>("module");
            var model = module.Models.First();
            IGenerator generator;
            switch(generatorType)
            {
                case "domain":
                    generator = new DomainClass.Generator(settings, module, model);
                    break;
                default:
                    throw new System.Exception($"Generator not available '{generatorType}'");
            }
                        
            _scenarioContext.Add("generator", generator);
        }
        
        [When(@"I proceed")]
        public void WhenIProceed()
        {
            var generator = _scenarioContext.Get<IGenerator>("generator");
            var result = generator.Create();
            _scenarioContext.Add("result", result);
        }

        [Then("the result should be (.*) domain files")]
        public void ThenTheResultShouldBe(int expected)
        {
            var result = _scenarioContext.Get<IOutput>("result");

            result.Should().NotBeNull().And.BeAssignableTo<IFileGroup>();
            ((IFileGroup)result).Files.Should().HaveCount(expected);
        }
    }
}
