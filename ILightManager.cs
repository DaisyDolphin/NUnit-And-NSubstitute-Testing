using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuilding
{
    public interface ILightManager : IManager
    {
        bool SetLight(bool isOn, int lightID); // should this be a void or a bool type? Probably void -> Unlike OpenAllDoors, this one is SET, rather than TurnLightsOn.
        bool SetAllLights(bool isOn); // should this be a void or a bool type? 
        string GetStatus();  // code here to output, outputs "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK," for 10 lights

    }
}
