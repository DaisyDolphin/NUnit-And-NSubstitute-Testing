using System.Collections.Specialized;

namespace SmartBuilding
{
    public class BuildingController
    {
        //Write BuildingController code here...
        private string buildingID;
        private string currentState;
        private string historyState; // local variable? // can't create get-set functions for it.
        private ILightManager lightManager;
        private IFireAlarmManager fireAlarmManager;
        private IDoorManager doorManager;        
        private IWebService webService;
        private IEmailService emailService;
        // constructor
        public BuildingController(string id) // L1R1
        {
            buildingID = id;
            currentState = "out of hours";
        }
        public BuildingController(string id, string startState)
        {
            buildingID = id.ToLower();
            //currentState = SetCurrentState(startState); // L1R5 says set to 'out of hours', but L2R3 improves that
            if(!SetCurrentState(startState))
            {
                throw new ArgumentException("Argument Exception: BuildingController can only" +
                 "be initialised to the following states 'open', 'closed', 'out of hours'" + '\n');  
            }
        }
        public BuildingController(string id, ILightManager iLightManager, 
            IFireAlarmManager iFireAlarmManager, IDoorManager iDoorManager, 
            IWebService iWebService, IEmailService iEmailService)
        {
            // light, fire and door classes have composition from IManager, meaning it "HAS-A" relationship 
            buildingID = id.ToLower();
            lightManager = iLightManager;
            fireAlarmManager = iFireAlarmManager;
            doorManager = iDoorManager;
            webService = iWebService;
            emailService = iEmailService;
            // if i don't set the state myself, i will get errors in regards to my SetCurrentState for DoorManager functions etc
            currentState = "out of hours";
        }
        // functions
        public string GetCurrentState()
        {
            return currentState;
        }
        public bool SetCurrentState(string state)
        {
            bool result;
            state = state.ToLower();
            if (currentState == null)
            // if constructor making object, fire drill and alarm cannot work.
            // so check if currentState empty first, then validate.
            {
                // state cannot start in fire drill or fire alarm,
                // as their would be no history state.
                if (state == "open" || state == "closed" || state == "out of hours")
                {
                    currentState = state;
                    result = true;
                }
                else
                {
                    // if nothing else is initialised, "out of hours" = new state.
                    //state = "out of hours";
                    result = false;
                }
            }
            else if(currentState == "closed" || currentState == "open")
            {
                if (state == "out of hours" || state == "fire drill" || state == "fire alarm")
                {
                    historyState = currentState;
                    currentState = state;
                    result = true;
                }
                else if(state == currentState)
                {
                    // no change to state needed, and we don't want historyState to change
                    result = true;
                }
                else // state == opposite state (open -> closed), or invalid state
                {
                    // error not valid state
                    result = false;
                }
            }
            else if(currentState == "fire drill" || currentState == "fire alarm")
            {
                // needs to store thr previous state as "historyState" somehow,
                // then state == that
                if (state == "open" || state == "out of hours" || state == "closed")
                {
                    state = historyState;
                    result = true;
                }
                else if(state == currentState)
                {
                    // no change to state needed, as fire drill becomes fire drill or fire alarm becomes fire alarm
                    result = true;
                }
                else 
                {
                    // error not valid state
                    result = false; 
                }
            }
            else if(currentState == "out of hours")
            {
                if (state == "open" || state == "closed" || state == "fire alarm" || state == "fire drill")
                {
                    historyState = currentState;
                    currentState = state;
                    result = true;
                }
                else if (state == currentState)
                {
                    // no change to state needed, out of hours is valid
                    result = true;
                }
                else //invalid state
                {
                    result = false;
                }
            }
            else // if something else happens
            {
                result = false;
            }
            try // When using the Building Constructor with DoorManagers etc, this is needed to be attempted.
            {
                if (state == "open")// check if state = open -> OpenAllDoors()
                {
                    if (!doorManager.OpenAllDoors())
                    {
                        // L3R4: if open all doors is false, current state is same, and setCurrentState returns false
                        result = false;
                        //return result;
                    }
                    // else continue reading through the system.
                }
                if (state == "closed")
                {
                    // L4R1: Lights turned off and lock all doors
                    lightManager.SetAllLights(false);
                    doorManager.LockAllDoors();
                    if (!doorManager.LockAllDoors())
                    {
                        // current state remains the same, and return false
                        result = false;
                    }
                    // else continue reading through the system.
                }
                if (state == "fire alarm")
                {
                    // L4R2:
                    if (fireAlarmManager.SetAlarm(true))
                    {
                        lightManager.SetAllLights(true); // lights on                        
                        doorManager.OpenAllDoors(); // doors opened
                        try
                        {
                            webService.LogFireAlarm(state); // logged state
                        }
                        catch (Exception ex)// an Exception is thrown and the FireAlarm is not logged
                        {
                            // Send an email
                            string email = "smartbuilding@uclan.ac.uk";
                            string subject = "failed to log alarm";
                            emailService.SendEmail(email, subject, ex.ToString());
                        }
                        result = true;
                    }
                }
            }
            catch // we may get NullReference because of other constructor used.
            {
            }
            return result;
        }   
        public string GetBuildingID()
        {
            return buildingID;
        }
        public bool SetBuildingID(string id)
        {
            buildingID = id.ToLower();
            return true;
        }
        public string GetStatusReport() // display data?
        {
            // outputs each manager classes GetStatus()
            string statusReport = lightManager.GetStatus() + doorManager.GetStatus() + fireAlarmManager.GetStatus();
            //L4R3:
            string logFault = "";
            if (statusReport.Contains("FAULT"))
            {
                if (lightManager.GetStatus().Contains("FAULT,"))
                {
                    logFault += "Lights,";
                }
                if (doorManager.GetStatus().Contains("FAULT,"))
                {
                    logFault += "Doors,";
                }
                if (fireAlarmManager.GetStatus().Contains("FAULT,"))
                {
                    logFault += "FireAlarm,";
                }
                // fault occurred, webservice manager must LogEngineerRequired
                webService.LogEngineerRequired(logFault);
            }
            // “Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,Doors,OK,OK,OK,OK,OK,OK,
            // OK,OK,OK,OK,FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,” 
            return statusReport;
        }
    }
}