using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// new version ;p

namespace Simulation
{

    // static class that allows us to update a general time in all classes
    public static class GeneralTime
    {
        private static int masterTime = 0;

        // making sure time set is never negative
        // or smaller than previous time.

        public static int MasterTime
        {
            get { return masterTime; }
            set
            {
                int temp = masterTime;
                if (value >= 0 && value >= temp)
                {
                    masterTime = value;
                }
            }
        }
    }




    // public static class to keep track of the state of the machines. Those can be updated in every class
    public static class MachineState
    {
        #region enums
        public enum State
        {
            idle,
            busy,
            blocked,
            stalled, // optional for machine3???
            broken
        };
        #endregion

    }



    // we can use this static class with static event list to 
    // add our events to the list within the class
    // so from the constructor of the event, the event can be added to the list.

    public static class EventList
    {
        public static LinkedList eventList = new LinkedList();
    }

           

    class Program
    {
        Machine1[] machines1 = new Machine1[4];
        Machine2[] machines2 = new Machine2[2];
        Machine3[] machines3 = new Machine3[2];
        Machine4[] machines4 = new Machine4[2];
   
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Initialisation();

            EventList.eventList.ReadFromHead();


            Console.ReadLine();
        }

        public void Initialisation()
        {
            
 
            // initialise all machines
            InitialiseMachines();

            // schedule first events
            ScheduleEvents();
            
        }

        public void InitialiseMachines()
        {
            // intialisation machines 1
            for (int i = 0; i < 4; i++)
            {
                machines1[i] = new Machine1(i);
                machines1[i].M1State = MachineState.State.idle;
            }

            // initialising machines 2, 3, 4
            for (int i = 0; i < 2; i++)
            {
                machines2[i] = new Machine2(i);
                machines3[i] = new Machine3(i);
                machines4[i] = new Machine4(i);

                machines2[i].M2State = MachineState.State.idle;
                machines3[i].M3State = MachineState.State.idle;
                machines4[i].M4State = MachineState.State.idle;
            }
        }

        public void ScheduleEvents()
        {
            for (int i = 0; i < 4; i++)
            {
                machines1[i].ScheduleBreaksDown();
                machines1[i].ScheduleDvdM1Finished();
            }
        }
    }
}
