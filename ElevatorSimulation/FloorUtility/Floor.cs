using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulation.Passengers;
using ElevatorSimulation.Interfaces;
using ElevatorSimulation.ElevatorUtility;

namespace ElevatorSimulation.FloorUtility
{
    /// <summary>
    /// Class <c>Floor</c> This class is responsible for managing passengers waiting on the floor.
    /// </summary>
    public class Floor : IFloor
    {
        private readonly List<IPassenger> _waitingPassengers;

        /// <summary>
        /// This field return the Floor Number
        /// </summary>
        public int FloorNumber { get; set; }

        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
            _waitingPassengers = new List<IPassenger>();
        }

        /// <summary>
        /// This Method is to Add the Passesnger who is 
        /// Waiting for the Elevator in the current floor
        /// </summary>
        public void AddPassenger(IPassenger passenger)
        {
            _waitingPassengers.Add(passenger);
        }

        /// <summary>
        /// This Method is to Remove the Passesnger who is 
        /// Waiting for the Elevator in the current floor
        /// </summary>
        public void RemovePassenger(IPassenger passenger)
        {
            _waitingPassengers.Remove(passenger);
        }

        /// <summary>
        /// This Method returns the number of waiting Passengers
        /// </summary>
        /// <returns>Returns the number of waiting Passengers</returns>
        public int GetWaitingPassengersCount()
        {
            return _waitingPassengers.Count;
        }

    }


}
