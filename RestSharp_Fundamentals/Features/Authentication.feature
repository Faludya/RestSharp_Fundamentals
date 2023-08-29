@API

Feature: Authentication

Tests for auth endpoint

@Authentication
Scenario: Obtain an authentication token
	Given I setup the payload with the application json 
	When I make a POST request to the "auth" endpoint
	Then the response status code should be "200"
	And the response should contain an authentication token
