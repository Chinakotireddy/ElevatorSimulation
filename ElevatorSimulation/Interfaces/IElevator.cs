using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Interfaces
{
    public interface IElevator
    {
        public int ElevatorId { get; }
        public int CurrentFloor { get; }
        public int CurrentWeight { get; }
        public int CurrentNumberOfPeoples { get; }
        public bool IsMoving { get; }
        public int Direction { get; } // 1 for up, -1 for down

        /// <summary>
        /// This Method is to be called by the Application with Floor number as a parameter
        /// Floor number is the parameter, where Passenger wanted to go
        /// </summary>
        public void AddRequestedFloor(int floor);

        /// <summary>
        /// This Method is to be called by the Application to Go to the requested floor through Elevator
        /// This Method uses the requested floor number that Application has been set using Method AddRequestedFloor() 
        /// This Method is supposed to be called after AddRequestedFloor() Method
        /// </summary>
        public Task Move();

        /// <summary>
        /// This Method is to Check Whether Passenger can be Checkin into Current Eleveator
        /// </summary>
        /// <returns>Returns the bool value, true or false</returns>
        public bool CanAddPassenger(IPassenger passenger);

        /// <summary>
        /// This Method is to Add the passenger to Elevator, checkint the Passenger to Elevator
        /// This Method is supposed to be called based on the Result of CanAddPassenger()
        ///    if CanAddPassenger() returns true then only Application can call this method 
        /// </summary>
        public void CheckInPassenger(IPassenger passenger);

        /// <summary>
        /// This Method is to Checkout the passenger from Elevator
        /// </summary>
        public void CheckOutPassenger(IPassenger passenger);
    }
}
