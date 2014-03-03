using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
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
        // different state options
        public enum State
        {
            idle,
            busy,
            blocked,
            stalled, // optional for machine3???
            broken
        };

        // fields used to in the properties
        private static State machine1State;
        private static State machine2State;
        private static State machine3State;
        private static State machine4State;

        #region Properties

        // machine 1 can be busy, brocken or blocked
        public static State M1State
        {
            get { return machine1State; }
            set
            {
                if (value == State.busy
                    || value == State.broken
                    || value == State.blocked)
                {
                    machine1State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        // machine 2 can eb idle, busy, blocked
        public static State M2State
        {
            get { return machine2State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked)
                {
                    machine2State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        // machine 3 can be idle, busy, blocked or stalled
        public static State M3State
        {
            get { return machine3State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked
                    || value == State.stalled)
                {
                    machine3State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        // machine 4 can ene idle, busy or broken
        public static State M4State
        {
            get { return machine4State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.broken)
                {
                    machine4State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }
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
   
        static void Main(string[] args)
        {
            Program p = new Program();
            p.InitialiseVariables();
            

            EventList.eventList.ReadFromHead();


            Console.ReadLine();
        }

        public void InitialiseVariables()
        {
            // initialise the state of the machines.

            // initialise MasterTime to zero
            GeneralTime.MasterTime = 0;

            // schedule M1 breaking down event
            M1BreaksEvent m1Break = new M1BreaksEvent();            

            // schedule DvdM1FinishedEvent
            DvdM1FinishedEvent m1Finished = new DvdM1FinishedEvent();

            // update state of M1.
        }
    }
}
