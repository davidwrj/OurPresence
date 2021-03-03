# Modeller
Code generation via a model is made easy with the Modeller global tool and a few generator components.

## Benefits:
- No need to have or learn/use yeoman, node or js to generate code.
- Code First generation, i.e. no need to create a database first.
- Versioned templates. 

Packages available on [NuGet.org](https://www.nuget.org/packages?q=OurPresence.modeller) include:
- [OurPresence.Modeller.Core](https://www.nuget.org/packages/OurPresence.Modeller.Core/) - defines the components that make up the code generator.
- [OurPresence.Modeller.Tool](https://www.nuget.org/packages/OurPresence.Modeller.Tool/) - a [dotnet global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) that can generate the code using an existing module definition file.

## Generators
The modeller tool can't do much without generator packages.  These packages must be installed locally on the developers computer to be able to generate code.  