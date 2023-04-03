using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulation.Interfaces;

namespace ElevatorSimulation.Passengers
{
    /// <summary>
    /// Class <c>Passenger</c> This class is responsible for storing passenger details.
    /// </summary>
    public class Passenger : IPassenger
    {
        public int Weight { get; }
        public int SourceFloor { get; }
        public int DestinationFloor { get; }

        public Passenger(int weight = 0, int sourceFloor = 0, int destinationFloor = 0)
        {
            Weight = weight;
            SourceFloor = sourceFloor;
            DestinationFloor = destinationFloor;
        }
    }

}
