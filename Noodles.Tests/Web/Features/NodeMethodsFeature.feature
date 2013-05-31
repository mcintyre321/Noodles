Feature: NodeMethodsFeature

@mytag
Scenario: Click Node Method link button
	Given I am on a page with a node method link
	When I click the node method link
	Then a node method form appears
