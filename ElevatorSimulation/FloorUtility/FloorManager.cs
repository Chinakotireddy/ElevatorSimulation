using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulation.Passengers;
using ElevatorSimulation.Interfaces;
using Microsoft.Extensions.Logging;
using ElevatorSimulation.ElevatorUtility;
using ElevatorSimulation.ElevatorLogger;

namespace ElevatorSimulation.FloorUtility
{
    /// <summary>
    /// Class <c>FloorManager</c> This class is responsible for Creating the Number of Floor Objects.
    /// </summary>
    public static class FloorManager
    {
        private static readonly List<IFloor> _floorList = new ();
        private static ILogger Logger { get; set; }

        /// <summary>
        /// This Method Creates the Floor Objects based on the setting numberOfFloors
        /// </summary>
        public static void CreateFloors(int numberOfFloors)
        {
            Logger = ELogger.GetLogger(); 
            for (int floorNumber = 0; floorNumber < numberOfFloors; floorNumber++)
            {
                _floorList.Add(new Floor(floorNumber));
            }
        }

        /// <summary>
        /// This Method Gets the Nearest Elvator for Passenger based on his current floor 
        /// from where he is requesting from
        /// </summary>
        /// <returns>Returns the elevator object</returns>
        public static IElevator PassengerRequestingForElevator(IPassenger passenger)
        {
            IElevator elevator = null;
            if (passenger.DestinationFloor != passenger.SourceFloor)
            {
                elevator = ElevatorManager.GetNearestElevatorForGivenFloor(passenger);
            }
            return elevator;
        }

        /// <summary>
        /// This Method Gets the Nearest Elvator for Passenger based on his current floor requesting from
        /// </summary>
        public static bool AddPassengersToFloor(int floorNumber, List<IPassenger> passengers)
        {
            bool status = false;
            try
            {
                IFloor floor = _floorList[floorNumber];
                if (floor != null) 
                {
                    foreach (IPassenger passenger in passengers)
                        floor.AddPassenger(passenger);
                    status = true;
                }
            }
            catch(Exception exceptionCached) 
            {
                Logger.LogError($"Exception Cached {exceptionCached.Message}");
            }
            return status;
        }

        /// <summary>
        /// This Method Gets the Nearest Elvator for floor App is requested
        /// </summary>
        /// <returns>Returns the elevator object</returns>
        public static IElevator CallEscalatorToFloor(int floorNumber)
        {
            try
            {
                return ElevatorManager.GetNearestElevatorForGivenFloor(floorNumber);
            }
            catch (Exception exceptionCached)
            {
                Logger.LogError($"Exception Cached {exceptionCached.Message}");
                return null;
            }
        }

        /// <summary>
        /// This Method Gets the Information about the Floor, howmany guys are waiting 
        /// </summary>
        public static void ShowStatusOfFloors()
        {
            _floorList.ForEach(floor =>
            {
                Logger.LogInformation($"Floor {floor.FloorNumber} has {floor.GetWaitingPassengersCount()} Passengers waiting");
            });

        }
    }


}
