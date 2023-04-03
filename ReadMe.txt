First, let's define the requirements of the elevator challenge:

The elevator can move up and down between floors.
The elevator can only stop at the floors requested by the users.
The elevator has a limit of maximum weight that it can carry.
The elevator should be able to handle multiple requests at once.

1) 
Single Responsibility Principle : this is to separate the responsibilities of each class
------------------------------------------------------------------------------------------
	Elevator class is responsible for elevator movements and stopping at requested floors.
	Passenger class is responsible for storing passenger details.
	Floor class is responsible for managing passengers waiting on the floor.
------------------------------------------------------------------------------------------

2)
Open/Closed Principle : This is to allow our classes to be extended without modifying their source code.


3)
Dependency Inversion Principle:  to reduce coupling between the classes.