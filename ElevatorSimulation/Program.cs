using Microsoft.Extensions.Logging;
using ElevatorSimulation.Configuration;
using ElevatorSimulation.ElevatorLogger;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ElevatorSimulation.Interfaces;
using ElevatorSimulation.ElevatorUtility;
using ElevatorSimulation.Passengers;
using ElevatorSimulation.FloorUtility;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace ElevatorSimulation
{
    public class Program
    {
        protected Program() { }
        static ILogger logger { get; set; }    
        public static IPassenger CreatePassenger(int weight, int sourceFloor, int destFloor)
        {
            return new Passenger(weight: weight, sourceFloor: sourceFloor, destinationFloor: destFloor);
        }

        public static async Task DealWithPerson(int weight, int sourceFloor, int destFloor)
        {
            IPassenger passenger = CreatePassenger(weight, sourceFloor, destFloor);
            IElevator elevator = FloorManager.PassengerRequestingForElevator(passenger);
            elevator.CheckInPassenger(passenger);
            elevator.AddRequestedFloor(passenger.DestinationFloor);
            await Task.Run(async () => await elevator.Move());
            elevator.CheckOutPassenger(passenger);
        }

        public static void AddNumberOfPeopleWaitingAtFloor(int floorNum, int numberOfPeopleWaiting)
        {
            logger.LogInformation("Note: One person parameters will be taken for all persons");
            logger.LogInformation("Enter person weight=");
            int weight = int.Parse(Console.ReadLine());
            logger.LogInformation("Floor he wants to go=");
            int destFloor = int.Parse(Console.ReadLine());

            List<IPassenger> passengers = new();
            for (int passengerNumber = 0; passengerNumber < numberOfPeopleWaiting; passengerNumber++)
            {
                passengers.Add(CreatePassenger(weight, floorNum, destFloor));
            }
            FloorManager.AddPassengersToFloor(floorNum, passengers);
        }

        public static void CallEscalatorToSpecificFloor(int floorNum)
        {
            IElevator elevator = FloorManager.CallEscalatorToFloor(floorNum);
            elevator.AddRequestedFloor(floorNum);
            elevator.Move();
        }

        public static void ShowStatusOfElevatorsAndFloors()
        {
            ElevatorManager.ShowStatusOfElevators();
            FloorManager.ShowStatusOfFloors();
        }

        public static async Task Main(string[] args)
        {
            logger = ELogger.GetLogger();
            int numberOfFloorsAvailable = int.Parse(ElevatorAppConfig.GetConfig("Number_Of_Floors_Available"));
            int numberOfElevatorsAvailable = int.Parse(ElevatorAppConfig.GetConfig("Number_Of_Elevators_Available"));
            FloorManager.CreateFloors(numberOfFloorsAvailable);
            ElevatorManager.CreateElevators(numberOfElevatorsAvailable);
            ShowStatusOfElevatorsAndFloors();


            bool running = true;
            while (running)
            {
                logger.LogInformation("\nWelcome to Elevator Simulation Program:");
                logger.LogInformation($"\nFloor Numbers Available: 0 to {numberOfFloorsAvailable}");
                logger.LogInformation($"\nNumber Of Elevators in the building: {numberOfElevatorsAvailable}");
                logger.LogInformation("\nChoose below Options:");
                logger.LogInformation("1. Add Passenger for Elevator Request");
                logger.LogInformation("2. Set/Add number of peoples waiting in Floor");
                logger.LogInformation("3. Call Escalator To Specific Floor");
                logger.LogInformation("4. Display Information About Elevators");
                logger.LogInformation("5. Quit the Program");
                logger.LogInformation("\nEnter your choice: ");
                int choice = int.Parse(Console.ReadLine());
                int floor;
                switch (choice)
                {
                    case 1:
                        logger.LogInformation("Enter person weight=");
                        int weight = int.Parse(Console.ReadLine());
                        logger.LogInformation("Floor he is in currently=");
                        int sourceFloor = int.Parse(Console.ReadLine());
                        logger.LogInformation("Floor he wants to go=");
                        int destFloor = int.Parse(Console.ReadLine());
                        await DealWithPerson(weight, sourceFloor, destFloor);
                        break;
                    case 2:
                        logger.LogInformation("Enter floor number: ");
                        floor = int.Parse(Console.ReadLine());
                        logger.LogInformation("Enter number of people waiting: ");
                        int numOfPeopleWaiting = int.Parse(Console.ReadLine());
                        AddNumberOfPeopleWaitingAtFloor(floor, numOfPeopleWaiting);
                        break;
                    case 3:
                        logger.LogInformation("Enter floor number:");
                        floor = int.Parse(Console.ReadLine());
                        CallEscalatorToSpecificFloor(floor);
                        break;
                    case 4:
                        ShowStatusOfElevatorsAndFloors();
                        break;
                    case 5:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}