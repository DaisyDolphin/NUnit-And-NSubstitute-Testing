using NUnit.Framework;
using NUnit.Framework.Legacy;
using NSubstitute;
using NSubstitute.Core;
using SmartBuilding;

//using Newtonsoft.Json.Linq;
//using System.Reflection.Emit;
//using NSubstitute.ExceptionExtensions;
//using System.Net;
//using System.Dynamic;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//Note: it is important your method names and method signatures match the UML diagram.
//This is especially the case for your assignment. (if you don’t follow the UML my hidden
//unit tests will fail during assignment marking)

//lab 11 helps a lil
//fizz buzz activity
// [TestCase] if parameters in test
// [TearDown] to reset static resource between tests, to not ruin everything

namespace SmartBuildingTests
{
    [TestFixture]
    public class BuildingControllerTests
    {
        // MethodBeingTested_TestScenario_ExpectedOutput
        // so testcases inputs data into the test below

        // L1R1
        [Test, Category("Level 1 Requirements: ")]
        public void BuildingConstructor_OneParameterInput_AssignsBuildingID()
        {
            // L1R1 BuildingController contains a constructor method with a single string 
            //  parameter that assigns the buildingID object variable. The constructor will have 
            //  the following signature: BuildingController(string id)
            //Arrange
            string currentID = "boring_id";
            BuildingController UCLanBuilding = new BuildingController(currentID); // fake data in fake object
                                                                                  //Act
            string comparedID = UCLanBuilding.GetBuildingID();
            //Assert
            Assert.That(currentID, Is.EqualTo(comparedID));
        }
        // L1R2
        [Test, Category("Level 1 Requirements: ")]
        public void GetBuildingID_CompareIDs_CurrentIDEqualsComparedID()
        {
            // L1R2 GetBuildingID returns a value of buildingID variable
            //Arrange
            string currentID = "boring_id";
            BuildingController UCLanBuilding = new BuildingController(currentID, "open"); // fake data in fake object
                                                                                          //Act
            string comparedID = UCLanBuilding.GetBuildingID(); // two diff tests
            //Assert
            Assert.That(currentID, Is.EqualTo(comparedID));
        }
        // L1R3
        [Test, Category("Level 1 Requirements: ")]
        public void BuildingController_BuildingIDConvertsToLowerCase_ResultIsLowerCase()
        {
            // L1R3 FantasticID gets converted to lower case
            //Arrange
            string currentID = "FantasticID";
            BuildingController UCLanBuilding = new BuildingController(currentID, "open"); // fake data in fake object
                                                                                          //Act
            string comparedID = UCLanBuilding.GetBuildingID(); // result shows it is lower case
            //Assert
            Assert.That(currentID.ToLower(), Is.EqualTo(comparedID));
        }
        // L1R4
        [Test, Category("Level 1 Requirements: ")]
        public void SetBuildingID_NewIDReplacesOldID_ReturnTrueANDNewIDIsLowerCase()
        {
            // L1R4 SetBuildingID sets buildingID AND sets to lower case
            //Arrange
            string newID = "FantasticID";
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", "open");
            //Act
            bool result = UCLanBuilding.SetBuildingID(newID);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(newID.ToLower(), Is.EqualTo(UCLanBuilding.GetBuildingID()));
            });
        }
        // L1R5
        [Test, Category("Level 1 Requirements: ")]
        public void BuildingController_InitialStateIfEmptyIsOutOfHours_CurrentStateIsOutOfHours()
        {
            // L1R5: initial state "out of hours"
            //Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID"); // fake data in fake object
            //Act & Assert
            Assert.That(UCLanBuilding.GetCurrentState(), Is.EqualTo("out of hours"));
        }
        // L1R6
        [Test, Category("Level 1 Requirements: ")]
        public void GetCurrentState_CompareStates_CurrentStateEqualsComparedState()
        {
            // L1R6 GetCurrentState returns currentState
            //Arrange
            string currentState = "open";
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            //Act
            string comparedState = UCLanBuilding.GetCurrentState();
            //Assert
            Assert.That(comparedState, Is.EqualTo(currentState));
        }
        [Test, Category("Level 1 Requirements: ")]
        public void SetCurrentState_InvalidState_ReturnsFalseAndStateDoesNotChange()
        {
            // L1R7 SetCurrentState checks if the state inputted is invalid. If the state
            // inputted is invalid, then it will be unchanged and return FALSE.
            // Arrange
            string currentState = "out of hours";
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            string newState = "TheCoolState";

            // Act
            bool result = UCLanBuilding.SetCurrentState(newState);

            // Assert
            // State was valid unexpectedly
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(UCLanBuilding.GetCurrentState(), Is.EqualTo(currentState)); // state is unchanged, and NOT TheCoolState
            });
        }
        // L1R7
        [Test, Category("Level 1 Requirements: ")]
        public void SetCurrentState_ValidState_ReturnsTrueAndStateChanges([Values("Open", "closed", "out of hours", "fire alarm", "fire drill")] string currentState)
        {
            // L1R7 SetCurrentState If the string supplied is valid, the function
            // will set the currentState variable AND return true.
            // Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", "out of hours");
            // Act
            bool result = UCLanBuilding.SetCurrentState(currentState);
            // Assert
            Assert.That(result, Is.True);
        }
        // L2R1
        [TestCase("open", "fire drill")] // to test if history state works
        [TestCase("open", "fire alarm")]
        [TestCase("open", "out of hours")]
        [TestCase("closed", "fire drill")]
        [TestCase("closed", "fire alarm")]
        [TestCase("closed", "out of hours")]
        [TestCase("out of hours", "fire drill")]
        [TestCase("out of hours", "fire alarm")]
        [TestCase("out of hours", "open")]
        [TestCase("out of hours", "closed")]
        [Test, Category("Level 2 Requirements: ")]
        public void SetCurrentState_ValidNewState_StateChanges(string currentState, string newState)
        {
            // L2R1 Validation must be according to the STD tp change:
            // STD: fire alarm <-> (closed <-> out of hours <-> open) <-> fire drill
            //Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            //Act
            bool result = UCLanBuilding.SetCurrentState(newState);
            //Assert
            Assert.That(result, Is.True);
        }
        // L2R1
        [TestCase("open", "closed")]   
        [TestCase("closed", "open")]
        [TestCase("open", "TheCoolState")]
        [Test, Category("Level 2 Requirements: ")]
        public void SetCurrentState_InvalidNewState_StateDoesNotChange(string currentState, string newState)
        {
            // L2R1 Validation must be according to the STD tp change:
            // STD: fire alarm <-> (closed <-> out of hours <-> open) <-> fire drill
            //Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            //Act
            // used in case current state is fire alarm or fire drill, which cannot start at.
            //UCLanBuilding.SetCurrentState(currentState);
            bool result = UCLanBuilding.SetCurrentState(newState);
            //Assert
            Assert.That(result, Is.False);
        }
        // L2R1
        [TestCase("open", "fire drill")] // to test if history state works
        [TestCase("open", "fire alarm")]
        [TestCase("open", "out of hours")]
        [TestCase("closed", "fire drill")]
        [TestCase("closed", "fire alarm")]
        [TestCase("closed", "out of hours")]
        [TestCase("out of hours", "fire drill")]
        [TestCase("out of hours", "fire alarm")]
        [TestCase("out of hours", "open")]
        [TestCase("out of hours", "closed")]  
        [Test, Category("Level 2 Requirements: ")]       
        public void SetCurrentState_ValidNewStateAndHistoryStateChange_StateChanges(string currentState, string newState)
        {
            // L2R1 Validation must be according to the STD tp change:
            // STD: fire alarm <-> (closed <-> out of hours <-> open) <-> fire drill
            // This test. needs to go from    "open" -> "fire drill" -> historyState   which is actually("open")
            //Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            //Act
            UCLanBuilding.SetCurrentState(newState);
            bool result = UCLanBuilding.SetCurrentState(currentState); // this code should work, as historyState = open still
            //Assert
            Assert.That(result, Is.True);
        }
        // L2R2
        [TestCase("open", "open")]
        [TestCase("closed", "closed")]
        [TestCase("out of hours", "out of hours")]
        [Test, Category("Level 2 Requirements: ")]
        public void SetCurrentState_NewStateEqualsCurrentState_UnchangedState(string newState, string currentState)
        {
            // L2R2 SetCurrentState, if same state is attempted to me set to, remains TRUE, and remains same
            //Arrange
            BuildingController UCLanBuilding = new BuildingController("AwesomeID", currentState);
            //Act
            bool result = UCLanBuilding.SetCurrentState(newState);
            //Assert
            Assert.That(result, Is.True);
        }
        // L2R3
        [TestCase("fire alarm")]
        [TestCase("fire drill")]
        [TestCase("TheCoolState")]
        [TestCase(" ")]
        [TestCase("@-!?<>")]
        [Test, Category("Level 2 Requirements: ")]
        public void BuildingController_InvalidInitialState_ThrowsException(string currentState)
        {
            // L2R3: initial state open/closed/out of hours -> else exception
            // Arrange, Act and Assert
            Assert.Throws<ArgumentException>(() => new BuildingController("AwesomeID", currentState)); 
            // must learn what a lambda expression is.
        }
        // L3R1
        [Test, Category("Level 3 Requirements: ")]
        public void BuildingController_DependancyInjection_ConstructorTakesInParameters()
        {
            // L3R1: make the building controller unit friendly, by making another constructor, for all the classes as a parameter
            // Arrange
            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            // Act
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Assert
            // if UCLanFacility was passed in fine, with no errors.
            Assert.Pass();
        }
        /*// L3R2
        [Test, Category("Level 3 Requirements: ")]
        public void GetStatus_LightManagerHasFunction_OutputsEachString()
        {
            // L3R2: The GetStatus() methods of all 3 manager classes return a string containing
            // Arrange           
            string LightStatus = "Lights,OK,OK,OK,OK,FAULT,OK,OK,OK,OK,OK,";

            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.GetStatus().Returns(LightStatus);
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();

            // Act
            string resultLight = LucasLight.GetStatus();

            // Assert
            Assert.That(resultLight, Is.EqualTo(LightStatus));

        }
        // L3R2
        [Test, Category("Level 3 Requirements: ")]
        public void GetStatus_FireManagerHasFunction_OutputsEachString()
        {
            // L3R2: The GetStatus() methods of all 3 manager classes return a string containing
            // Arrange           
            string FireStatus = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT,";
            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            FreddyFire.GetStatus().Returns(FireStatus);
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            // Act
            string resultFire = FreddyFire.GetStatus();

            // Assert
            Assert.That(resultFire, Is.EqualTo(FireStatus));
        }
        // L3R2
        [Test, Category("Level 3 Requirements: ")]
        public void GetStatus_DoorManagerHasFunction_OutputsEachString()
        {
            // L3R2: The GetStatus() methods of all 3 manager classes return a string containing
            // Arrange           
            string DoorStatus = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT,";
            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.GetStatus().Returns(DoorStatus);
            // Act
            string resultDoor = DanielDoor.GetStatus();

            // Assert
            Assert.That(resultDoor, Is.EqualTo(DoorStatus));
        }*/
        [Test, Category("Level 3 Requirements: ")]
        public void GetStatus_AllManagersHaveGetStatusFunction_ReturnsExpectedStatusPerManager()
        {
            // Arrange
            string LightStatus = "Lights,OK,OK,OK,OK,FAULT,OK,OK,OK,OK,OK,";
            string FireStatus = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT,";
            string DoorStatus = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,FAULT,";

            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();

            LucasLight.GetStatus().Returns(LightStatus);
            FreddyFire.GetStatus().Returns(FireStatus);
            DanielDoor.GetStatus().Returns(DoorStatus);

            // Act
            string resultLight = LucasLight.GetStatus();
            string resultFire = FreddyFire.GetStatus();
            string resultDoor = DanielDoor.GetStatus();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultLight, Is.EqualTo(LightStatus), "LightManager status mismatch");
                Assert.That(resultFire, Is.EqualTo(FireStatus), "FireAlarmManager status mismatch");
                Assert.That(resultDoor, Is.EqualTo(DoorStatus), "DoorManager status mismatch");
            });
        }
        // L3R3
        [Test, Category("Level 3 Requirements: ")]
        public void GetStatusReport_CallsGetStatusForEachManager_OutputsExpectedStatus()
        {
            // L3R3: GetStatusReport() method calls the GetStatus() methods of all 3 manager classes
            //  and appends each string returned together into a single string
            // Arrange         
            string lightStatus = "Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,";
            string fireStatus = "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,";
            string doorStatus = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,";
            string expectedResult = "Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,";
            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.GetStatus().Returns(lightStatus);
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            FreddyFire.GetStatus().Returns(fireStatus);
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.GetStatus().Returns(doorStatus);
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();         
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            string result = UCLanFacility.GetStatusReport();
            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        // L3R4
        [Test, Category("Level 3 Requirements: ")]
        public void SetCurrentState_OpenStateFailureToOpensAllDoors_AllDoorsNotOpen()
        {
            // L3R4: If DoorManager.OpenAllDoors() returns false 
            // (indicating there was a failure to unlock all the doors) ‘false’ should be returned
            // from the SetCurrentState() function and the building should remain in its current state.
            // Arrange
            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.OpenAllDoors().Returns(false); // we pretend the door cannot open
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            // we expect the result to be false, because the OpenAllDoors function returns false
            // -> making SetCurrentState false too          
            bool result = UCLanFacility.SetCurrentState("open");
            // Assert
            DanielDoor.Received().OpenAllDoors(); // check the openAllDoors function was called
            // Output is false, as intended: 
            Assert.That(result, Is.False);
        }
        // L3R5
        [Test, Category("Level 3 Requirements: ")]
        public void SetCurrentState_OpenStateSucceedsToOpensAllDoors_AllDoorsOpened()
        {
            // L3R5: When moving to the “open” state, if DoorManager.OpenAllDoors() returns 
            // ‘true’ when attempting to move to the open state(indicating unlocking all the
            // doors was a success) ‘true’ should be returned from SetCurrentState() and the
            // building should move to the “open” state.
            // Arrange
            ILightManager LucasLight = Substitute.For<ILightManager>();
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.OpenAllDoors().Returns(true);
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            // we expect the result to be true, because the OpenAllDoors function returns true
            // -> making SetCurrentState true too
            bool result = UCLanFacility.SetCurrentState("open");
            // Assert
            Assert.Multiple(() =>
            {
                DanielDoor.Received().OpenAllDoors();
                // Output is true, as intended: 
                Assert.That(result, Is.True);
            });
        }
        // L4R1
        [Test, Category("Level 4 Requirements: ")]
        public void SetCurrentState_ClosedStateIsSet_LocksAllDoorsAndLightsOff()
        {
            /*// L4R1: When the BuildingController’s SetCurrentState() method moves to the 
            // “closed” state, all doors should be set to closed by calling the
            // DoorManager.LockAllDoors() method and all lights must be turned off by calling
            // the LightManager.SetAllLights(false)*/
            // Arrange
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.SetAllLights(true).Returns(true); // initially the lights are on, and gets turned off.
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.LockAllDoors().Returns(true); // LockDoors are set to true, because we're closing the building
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            // we expect the result to be true, because the OpenAllDoors function returns true
            // -> making SetCurrentState true too
            bool result = UCLanFacility.SetCurrentState("closed");

            // Assert
            Assert.Multiple(() =>
            {
                DanielDoor.Received().LockAllDoors();
                LucasLight.Received().SetAllLights(false);
            });
        }
        // L4R2
        [Test, Category("Level 4 Requirements: ")]
        public void SetCurrentState_MoveToFireAlarmState_TriggerAlarmUnlockAllDoorsTurnOnLightsLogFireAlarm()
        {
            /*// L4R2: When the BuildingController.SetCurrentState() method moves to the “fire 
            // alarm” state, the alarm should be triggered by calling
            // FireAlarmManager.SetAlarm(true), all doors should be unlocked by calling
            // DoorManager.OpenAllDoors(), all lights should be turned on using
            // LightManager.SetAllLights(true) and an online log should be made by calling
            // WebService.LogFireAlarm(“fire alarm”).*/
            // Arrange
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.SetAllLights(true).Returns(true); // initially the lights are on/off, and gets turned on.
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            FreddyFire.SetAlarm(true).Returns(true); // Fire alarm is on, so set it on.
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.OpenAllDoors().Returns(true); // OpenDoors are set to true, so people can escape
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            // we expect the result to be true to change state.
            bool result = UCLanFacility.SetCurrentState("fire alarm");

            // Assert
            Assert.Multiple(() =>
            {
                FreddyFire.Received().SetAlarm(true);
                LucasLight.Received().SetAllLights(true);
                DanielDoor.Received().OpenAllDoors();
                // Check that WebService has recieved the correct parameter
                WilliamWebber.Received().LogFireAlarm("fire alarm");
            });
        }
        // L4R3
        [Test, Category("Level 4 Requirements: ")]
        public void GetStatusReport_WhenFaultsDetectedWebServiceLogsEngineerRequired_LogsEngineerRequired()
        {
            /*L4R3: The GetStatusReport() method will parse each of the three status reports 
            (for Lights, FireAlarm and Doors) and if a fault is detected, the WebService object 
            will be used to log that an engineer is required to fix the fault by calling the 
            WebService.LogEngineerRequired() method, passing a string parameter. The 
            string parameter should contain the type of device that has shown a fault (e.g. 
            Lights, FireAlarm or Doors). If multiple device types have shown a fault then these 
            should be separated by a comma. E.g. If both Lights and Doors status reports 
            indicate a fault the following string should be logged to the web server 
            “Lights,Doors,”. */
            // Arrange
            string lightStatus = "Lights,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,";
            string fireStatus = "FireAlarm,OK,OK,FAULT,OK,OK,OK,OK,FAULT,OK,OK,";
            string doorStatus = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK,";
            string expectedResult = "Lights,FireAlarm,"; // we expect the Report to state "Lights,FireAlarm," as those systems are faulty
            // These are the names of my fake manager roles
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.GetStatus().Returns(lightStatus);
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            FreddyFire.GetStatus().Returns(fireStatus);
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.GetStatus().Returns(doorStatus);
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act & Assert
            // we expect the Report to state "Lights,FireAlarm," as those systems are faulty
            string statusReport = UCLanFacility.GetStatusReport();
            
            // Check WebService recieved the LogEngineer as expected.
            WilliamWebber.Received().LogEngineerRequired(expectedResult);
        }
        // L4R4
        [Test, Category("Level 4 Requirements: ")]
        public void SetCurrentState_WebServiceLogFireAlarmThrowsException_SendsEmailWithExceptionMessage()
        {
            /*L4R4: In addition to requirement L4R2, If WebService.LogFireAlarm( ) throws an 
            Exception when called, an email should be sent using the EmailService’s 
            SendMail( ) method. To smartbuilding@uclan.ac.uk, with the subject “failed to log 
            alarm” and the message parameter should contain the exception message 
            returned from the failed call to the  LogFireAlarm() function. */
            // Arrange
            ILightManager LucasLight = Substitute.For<ILightManager>();
            LucasLight.SetAllLights(true).Returns(true); // initially the lights are on/off, and gets turned on.
            IFireAlarmManager FreddyFire = Substitute.For<IFireAlarmManager>();
            FreddyFire.SetAlarm(true).Returns(true); // Fire alarm is on, so set it on.
            IDoorManager DanielDoor = Substitute.For<IDoorManager>();
            DanielDoor.OpenAllDoors().Returns(true); // OpenDoors are set to true, so people can escape
            IWebService WilliamWebber = Substitute.For<IWebService>();
            IEmailService EllieEmailer = Substitute.For<IEmailService>();
            BuildingController UCLanFacility = new BuildingController("AwesomeID", LucasLight, FreddyFire, DanielDoor, WilliamWebber, EllieEmailer);
            // Act
            string exceptionMessage = "Test exception message";
            // This now forces WebService.LogFireAlarm( ) to throw an Exception
            WilliamWebber.When(WebDummy => WebDummy.LogFireAlarm("fire alarm")).Do(WebDummy => { throw new Exception(exceptionMessage); });
       
            // we expect the result to be true to change state.
            bool result = UCLanFacility.SetCurrentState("fire alarm");

            // Assert            
            string email = "smartbuilding@uclan.ac.uk";
            string message = "failed to log alarm";
            // ex.Message should == "Test exception message"
            
            EllieEmailer.Received().SendEmail(email, message, Arg.Is<string>(msg => msg.Contains(exceptionMessage))); // .Contains string 
        }


        //use the following naming convention for your test method names MethodBeingTested_TestScenario_ExpectedOutput
        //E.g. SetCurrentState_InvalidState_ReturnsFalse


        // BuildingController <- (composition arrow) DoorManager, FireAlarmManager, LightManager
        // BuildingController <- (aggregation) EmailService, WebService
        // FireAlarmManager -> (generalization) Manager <<abstract>>
        // LightManager -> (generalization) Manager <<abstract>>
        // DoorManager-> (generalization) Manager <<abstract>>

        // composition = a class to own or "contain" one or more objects of other classes.
        // Like a Car 'has-a' an Engine, or a PC 'has-a' motherboard 
        // aggregation = a class can references another class existing independantly.
        // Like a Univeristy may have some Students and have many different students
        // DIFFERENCE: either CAN or CANNOT exist independantly from the other

        // generalization (inheritance) = Animals -> Dog -> Chihuahua 
        // association = (BASIC BITCH) a relationship between classes, any direction or bi-direction, simple or complex
    }
}