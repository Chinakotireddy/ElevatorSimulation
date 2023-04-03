using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulation.Interfaces
{
    public interface IPassenger
    {
        int Weight { get; }
        int SourceFloor { get; }
        int DestinationFloor { get; }
    }
}
