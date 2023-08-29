@API 
Feature: Ping

Test for the /ping endpoint

@Ping
Scenario: Verify ping is up and running
	When I make a GET request to the "ping" endpoint
	Then the response status code should be "201"
	And the response message should be "Created"