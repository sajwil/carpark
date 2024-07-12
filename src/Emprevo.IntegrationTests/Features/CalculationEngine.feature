Feature: Calcualte Parking Rate

Scenario: Apply Early Bird flat rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-12T07:00"
	And the customer exits the parking lot at "2024-07-12T16:00"
	Then the total parking price should be $13.00
	And the rate applied should be "Early Bird"

Scenario: Apply Night flat rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-12T18:30"
	And the customer exits the parking lot at "2024-07-12T20:00"
	Then the total parking price should be $6.50
	And the rate applied should be "Night Rate"

Scenario: Apply Weekend flat rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-13T10:11"
	And the customer exits the parking lot at "2024-07-13T19:11"
	Then the total parking price should be $10.00
	And the rate applied should be "Weekend Rate"

Scenario: Apply Standard one hour rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-11T10:11"
	And the customer exits the parking lot at "2024-07-11T11:10"
	Then the total parking price should be $5.00
	And the rate applied should be "Standard Rate"

Scenario: Apply Standard two hour rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-11T10:11"
	And the customer exits the parking lot at "2024-07-11T12:10"
	Then the total parking price should be $10.00
	And the rate applied should be "Standard Rate"

Scenario: Apply Standard three hour rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-11T10:11"
	And the customer exits the parking lot at "2024-07-11T13:10"
	Then the total parking price should be $15.00
	And the rate applied should be "Standard Rate"

Scenario: Apply Standard flat rate when entry and exit conditions are met
	When a customer parks their car
	And the customer enters the parking lot at "2024-07-11T10:11"
	And the customer exits the parking lot at "2024-07-11T14:10"
	Then the total parking price should be $20.00
	And the rate applied should be "Standard Rate"

