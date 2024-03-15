using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuilding
{
    public interface IFireAlarmManager : IManager
    {
        // could override GetStatus function from IManager
        bool SetAlarm(bool isActive); // void or bool??????
        string GetStatus(); // code here to output, outputs "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK," for 10 lights
    }
}
