using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        #region enums
        // different state options
        public enum State
        {
            idle,
            busy,
            blocked,
            stalled, // optional for machine3???
            broken
        };

        // up and down time of each machine. Not crucial to the simulation yet!
        #endregion

        #region fields State
        // machines 1
        private static State machine11State;
        private static State machine12State;
        private static State machine13State;
        private static State machine14State;
        // machine 2
        private static State machine21State;
        private static State machine22State;
        // machines 3
        private static State machine31State;
        private static State machine32State;
        // machines 4
        private static State machine41State;
        private static State machine42State;

        #endregion

        #region Properties State
        
        #region prop machines 1
        // machines 1 can be busy, brocken or blocked
        public static State M11State
        {
            get { return machine11State; }
            set
            {
                if (value == State.busy
                    || value == State.broken
                    || value == State.blocked)
                {
                    machine11State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        public static State M12State
        {
            get { return machine12State; }
            set
            {
                if (value == State.busy
                    || value == State.broken
                    || value == State.blocked)
                {
                    machine12State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        public static State M13State
        {
            get { return machine13State; }
            set
            {
                if (value == State.busy
                    || value == State.broken
                    || value == State.blocked)
                {
                    machine13State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        public static State M14State
        {
            get { return machine14State; }
            set
            {
                if (value == State.busy
                    || value == State.broken
                    || value == State.blocked)
                {
                    machine14State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }
        #endregion


        #region prop machines 2
        // machines 2 can eb idle, busy, blocked
        public static State M21State
        {
            get { return machine21State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked)
                {
                    machine21State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        public static State M22State
        {
            get { return machine22State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked)
                {
                    machine22State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }
#endregion


        #region prop machines 3
        // machine 3 can be idle, busy, blocked or stalled
        public static State M31State
        {
            get { return machine31State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked
                    || value == State.stalled)
                {
                    machine31State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        public static State M32State
        {
            get { return machine32State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.blocked
                    || value == State.stalled)
                {
                    machine32State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }
        #endregion


        #region prop machines 4
        // machines 4 can ene idle, busy or broken
        public static State M41State
        {
            get { return machine41State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.broken)
                {
                    machine41State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }
        public static State M42State
        {
            get { return machine42State; }
            set
            {
                if (value == State.idle
                    || value == State.busy
                    || value == State.broken)
                {
                    machine42State = value;
                }
                else
                {
                    throw new Exception("this machine may not have this state");
                }
            }
        }

        #endregion
        #endregion

        #region properties upDownTime
        public static int M11TotalUpTime { get; set; }
        public static int M12TotalUpTime { get; set; }
        public static int M13TotalUpTime { get; set; }
        public static int M14TotalUpTime { get; set; }

        public static int M21TotalUpTime { get; set; }
        public static int M23TotalUpTime { get; set; }

        public static int M31TotalUpTime { get; set; }
        public static int M32TotalUpTime { get; set; }

        public static int M41TotalUpTime { get; set; }
        public static int M42TotalUpTime { get; set; }
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
            //p.InitialiseVariables();

            // Adding 4 dvd machine 1 finished (one for each machine 1) and check the linked list.

            DvdM1FinishedEvent m11Finished = new DvdM1FinishedEvent(1);
           
            DvdM1FinishedEvent m12Finished = new DvdM1FinishedEvent(2);
           
            DvdM1FinishedEvent m13Finished = new DvdM1FinishedEvent(3);
           
            DvdM1FinishedEvent m14Finished = new DvdM1FinishedEvent(4);
           

            EventList.eventList.ReadFromHead();

            Console.ReadLine();
        }

        public void InitialiseVariables()
        {
            // initialise the state of the machines.

            // initialise MasterTime to zero
            GeneralTime.MasterTime = 0;

            // schedule M1 breaking down event
            M1BreaksEvent m1Break = new M1BreaksEvent(1);            

            // schedule DvdM1FinishedEvent
            DvdM1FinishedEvent m1Finished = new DvdM1FinishedEvent(1);
            m1Finished.PrintDetails();
            // update state of M1.
        }
    }
}
