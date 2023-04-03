using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulation.Interfaces;
using ElevatorSimulation.ElevatorUtility;
using GlobalOrchestrator.Constants.ProximityControl;

namespace ElevatorSimulation.ElevatorUtility
{
    /// <summary>
    /// Class <c>Elevator</c> This class is responsible for elevator movements like Moving to the requested floors.
    /// </summary>
    public class Elevator : IElevator
    {
        private const int _maxWeight = 550; // Maximum Weight Elevator can handle in KiloGrams
        private const int _maxNumberOfPeople = 7; // Maximum Number of People Elevator can handle
        private const int _timeToMoveBetweenNextOrPrevFloorInSeconds = 1; // Time in Seconds 
        private readonly List<int> _requestedFloors;


        public int ElevatorId { get; private set; }
        public int CurrentFloor { get; private set; }
        public int CurrentWeight { get; private set; }
        public int CurrentNumberOfPeoples { get; private set; }
        public bool IsMoving { get; private set; }
        public int Direction { get; private set; } // 1 for up, -1 for down

        public Elevator(int elevatorId)
        {
            ElevatorId = elevatorId;
            _requestedFloors = new List<int>();
            CurrentFloor = 0;
            Direction = (int)ElevatorEnums.StillNow;
            
        }

        /// <summary>
        /// This Method is to be called by the Application with Floor number as a parameter
        /// Floor number is the parameter, where Passenger wanted to go
        /// </summary>
        public void AddRequestedFloor(int floor)
        {
            _requestedFloors.Add(floor);
            _requestedFloors.Sort();
        }

        /// <summary>
        /// This Method is to be called by the Application to Go to the requested floor through Elevator
        /// This Method uses the requested floor number that Application has been set using Method AddRequestedFloor() 
        /// This Method is supposed to be called after AddRequestedFloor() Method
        /// </summary>
        public async Task Move()
        {
            if (_requestedFloors.Count == 0)
            {
                IsMoving = false;
                Direction = (int)ElevatorEnums.StillNow;
                return;
            }

            IsMoving = true;
            Direction = _requestedFloors[0] > CurrentFloor ? (int)ElevatorEnums.MovingUp : (int)ElevatorEnums.MovingDown;

            //Movement of the Elevator based on the Requested floor user is pressed 
            while (_requestedFloors.Count > 0 && _requestedFloors[0] != CurrentFloor)
            {
                CurrentFloor += Direction;
                await Task.Delay(_timeToMoveBetweenNextOrPrevFloorInSeconds * ElevatorConstants.MilliSeconds); // Time to go from one floor to another floor
            }

            //User reached the Requested Floor, so removing the requested floor entry that user pressed 
            if (_requestedFloors.Count > 0 && _requestedFloors[0] == CurrentFloor)
            {
                _requestedFloors.RemoveAt(0);
            }

            IsMoving = _requestedFloors.Count > 0;
            if(!IsMoving)
                Direction = (int)ElevatorEnums.StillNow;

        }

        /// <summary>
        /// This Method is to Check Whether Passenger can be Checkin into Current Eleveator
        /// </summary>
        /// <returns>Returns the bool value, true or false</returns>
        public bool CanAddPassenger(IPassenger passenger)
        {
            return CurrentWeight + passenger.Weight <= _maxWeight || CurrentNumberOfPeoples <= _maxNumberOfPeople;
        }

        /// <summary>
        /// This Method is to Add the passenger to Elevator, checkin the Passenger to Elevator
        /// This Method is supposed to be called based on the Result of CanAddPassenger()
        ///    if CanAddPassenger() returns true then only Application can call this method 
        /// </summary>
        public void CheckInPassenger(IPassenger passenger)
        {
            CurrentWeight += passenger.Weight;
            CurrentNumberOfPeoples += 1;

        }

        /// <summary>
        /// This Method is to Checkout the passenger from Elevator
        /// </summary>
        public void CheckOutPassenger(IPassenger passenger)
        {
            CurrentWeight -= passenger.Weight;
            CurrentNumberOfPeoples -= 1;
        }
    }


}
