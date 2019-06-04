Feature: Executing this should not hang

@working
Scenario: SignalR server should be able to raise client's events #1
	Given SignalR server is running
	When I establish SignalR connection and invoke method
	Then this step should be called

@bugged
Scenario: SignalR server should be able to raise client's events #2
	Given SignalR server is running
	And SignalR connection is established
	When I invoke method on the connection
	Then this step should be called
