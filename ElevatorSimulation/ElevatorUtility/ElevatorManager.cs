using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulation.FloorUtility;
using ElevatorSimulation.Interfaces;
using ElevatorSimulation.ElevatorUtility;
using GlobalOrchestrator.Constants.ProximityControl;
using Microsoft.Extensions.Logging;
using ElevatorSimulation.ElevatorLogger;
using Polly;

namespace ElevatorSimulation.ElevatorUtility
{
    /// <summary>
    /// Class <c>ElevatorManager</c> This class is responsible for Creating the Number of Elevator Objects.
    /// </summary>
    public static class ElevatorManager
    {
        private static readonly List<IElevator> _elevatorList = new();

        private static ILogger Logger { get; set; }


        /// <summary>
        /// This Method Creates the Floor Objects based on the setting numberOfElevators
        /// </summary>
        public static void CreateElevators(int numberOfElevators)
        {
            Logger = ELogger.GetLogger();
            for (int elevatorNumber = 0; elevatorNumber < numberOfElevators; elevatorNumber++)
            {
                _elevatorList.Add(new Elevator(elevatorNumber));
            }
        }

        /// <summary>
        /// This Method Displayes the Status OF Elevators
        /// Show the status of the elevators, including which floor they are on, 
        /// weather they are moving and in which direction, and how many people they are carrying
        /// </summary>
        public static void ShowStatusOfElevators()
        {
            _elevatorList.ForEach(elevator =>
            {
                string elevatorMovingDirection = elevator.Direction == 1 ?
                    ElevatorConstants.MovingDirectionUp : ElevatorConstants.movingDirectionDown;
                Logger.LogInformation($"Information of Elevator {elevator.ElevatorId} => " +
                    $"Elevator is on {elevator.CurrentFloor}, " +
                    $"Elevator is {(elevator.IsMoving ? ElevatorConstants.StatusMoving : ElevatorConstants.StatusNotMoving)}, " +
                    $"Moving Direction is {(elevator.IsMoving ? elevatorMovingDirection : ElevatorConstants.NotApplicable)}, " +
                    $"Number of People on Elevator is {elevator.CurrentNumberOfPeoples}");
            });
            
        }

        /// <summary>
        /// This Method Gets the Nearest Elvator for Passenger based on his current floor 
        /// from where he is requesting from
        /// </summary>
        /// <returns>Returns the elevator object</returns>
        public static IElevator GetNearestElevatorForGivenFloor(IPassenger passenger)
        {
            IElevator bestElevator = null;
            int shortestDistance = int.MaxValue;
            int desiredDirection = passenger.DestinationFloor - passenger.SourceFloor > 0 ?
                                        (int)ElevatorEnums.MovingUp : (int)ElevatorEnums.MovingDown;
            foreach (var elevator in _elevatorList)
            {
                if (elevator.Direction == desiredDirection || elevator.Direction == (int)ElevatorEnums.StillNow)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - passenger.SourceFloor);

                    if (((elevator.Direction == (int)ElevatorEnums.MovingUp || elevator.Direction == (int)ElevatorEnums.StillNow) && 
                        elevator.CurrentFloor <= passenger.SourceFloor 
                        ||
                        (elevator.Direction == (int)ElevatorEnums.MovingDown || elevator.Direction == (int)ElevatorEnums.StillNow) && 
                        elevator.CurrentFloor >= passenger.SourceFloor) && 
                        (distance < shortestDistance))
                    {
                        bestElevator = elevator;
                        shortestDistance = distance;
                    }
                }
            }
            return bestElevator;
        }

        /// <summary>
        /// This Method Gets the Nearest Elvator for Passenger based on his floor number requested 
        /// </summary>
        /// <returns>Returns the elevator object</returns>
        public static IElevator GetNearestElevatorForGivenFloor(int floorNumber)
        {
            IElevator bestElevator = null;
            int shortestDistance = int.MaxValue;
            foreach (var elevator in _elevatorList)
            {
                int distance = Math.Abs(elevator.CurrentFloor - floorNumber);
                if (((elevator.Direction == (int)ElevatorEnums.MovingUp || elevator.Direction == (int)ElevatorEnums.StillNow) &&
                    elevator.CurrentFloor <= floorNumber
                    ||
                    (elevator.Direction == (int)ElevatorEnums.MovingDown || elevator.Direction == (int)ElevatorEnums.StillNow) &&
                    elevator.CurrentFloor >= floorNumber) &&
                    (distance < shortestDistance))
                {
                    bestElevator = elevator;
                    shortestDistance = distance;
                }
            }
            return bestElevator;
        }

    }

}
