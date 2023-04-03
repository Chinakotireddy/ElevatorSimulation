using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Interfaces
{
    public interface IFloor
    {
        /// <summary>
        /// This field return the Floor Number
        /// </summary>
        public int FloorNumber { get; }

        /// <summary>
        /// This Method is to Add the Passesnger with Weight who is 
        /// Waiting for the Elevator in the current floor
        /// </summary>
        public void AddPassenger(IPassenger passenger);

        /// <summary>
        /// This Method is to Remove the Passesnger with Weight who is 
        /// Waiting for the Elevator in the current floor
        /// </summary>
        public void RemovePassenger(IPassenger passenger);

        /// <summary>
        /// This Method returns the number of waiting Passengers
        /// </summary>
        /// <returns>Returns the number of waiting Passengers</returns>
        public int GetWaitingPassengersCount();
    }
}
