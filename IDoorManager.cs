using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuilding
{
    public interface IDoorManager : IManager // how am i supposed to composite this?? : BuildingController
    {
        bool OpenDoor(int doorID);
        bool LockDoor(int doorID);
        bool OpenAllDoors();
        bool LockAllDoors();
        string GetStatus(); // code here to output, outputs "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK," for 10 lights
        
    }
}
