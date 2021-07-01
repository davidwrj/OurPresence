Feature: Domain Generator
Check the output of the domain class generators

Link to a feature: [Calculator](OurPresence.ModellerTests/Features/Domain.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@domain
Scenario: Create domain class with regen support
	Given a model
    And a domain generator with regen set to true
	When I proceed
	Then the result should be 2 domain files

Scenario: Create domain class without regen support
	Given a model
    And a domain generator with regen set to false
	When I proceed
	Then the result should be 1 domain files
