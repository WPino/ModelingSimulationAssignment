﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// new version ;p

namespace Simulation
{

    // static class that allows us to update a general time in all classes
    public static class GeneralTime
    {
        private static double masterTime = 0;

        // making sure time set is never negative
        // or smaller than previous time.

        public static double MasterTime
        {
            get { return masterTime; }
            set
            {
                double temp = masterTime;
                if (value >= 0 && value >= temp)
                {
                    masterTime = value;
                }
            }
        }
    }

    public static class SystemState
    {
        public static Random R = new Random();

        public static Machine1[] machines1;
        public static Machine2[] machines2;
        public static Machine3[] machines3;
        public static Machine4[] machines4;

        public static int totalDVDFinished { get; set; }
        public static double averageThroughputTime { get; set; }

        //public static List<double> throughputTimes = new List<double>();

        public static void updateThroughputTime(double newThroughputTime)
        {
            averageThroughputTime = (((averageThroughputTime * (totalDVDFinished - 1)) + newThroughputTime) / totalDVDFinished);
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
        
        int buffersize2 = 20;
        int buffersize3 = 20;
        int buffersize4 = 20;
        double endTime = 400;

   
        static void Main(string[] args)
        {
            Program p = new Program();
            GeneralTime.MasterTime = 0;
            p.Initialisation();

            
//            EventList.eventList.ReadFromHead();

            p.Run();
            //p.Analyze();

            Console.ReadLine();
        }

        public void Run()
        {
            
            while (EventList.eventList.HeadEvent.Time < endTime)
	        {
                Console.WriteLine("length {0}", EventList.eventList.Length);
                Console.WriteLine("master time {0}", GeneralTime.MasterTime);
                EventList.eventList.ReadFromHead();

                GeneralTime.MasterTime = EventList.eventList.HeadEvent.Time;

                Console.ReadLine();

<<<<<<< HEAD
                //GeneralTime.MasterTime = EventList.eventList.HeadEvent.Time;
                //Console.WriteLine("Mastertime is {0}",GeneralTime.MasterTime);
                //Console.ReadLine();

=======

                //GeneralTime.MasterTime = EventList.eventList.HeadEvent.Time;
                //Console.WriteLine("Mastertime is {0}",GeneralTime.MasterTime);
                //Console.ReadLine();
                
>>>>>>> fa4b1beea6aa06294d33ee3a253f48e62830c15a
                Event nextEvent = EventList.eventList.Remove();
                //GeneralTime.MasterTime = nextEvent.Time;
                nextEvent.HandleEvent();

                
                
            }



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
            

            SystemState.machines1 = new Machine1[4];
            SystemState.machines2 = new Machine2[2];
            SystemState.machines3 = new Machine3[2];
            SystemState.machines4 = new Machine4[2];

            // intialisation machines 1
            for (int i = 0; i < 4; i++)
            {
                SystemState.machines1[i] = new Machine1(i);
                SystemState.machines1[i].M1State = MachineState.State.idle;
            }

            // initialising machines 2, 3, 4
            for (int i = 0; i < 2; i++)
            {
                SystemState.machines2[i] = new Machine2(i);
                SystemState.machines3[i] = new Machine3(i);
                SystemState.machines4[i] = new Machine4(i);

                SystemState.machines2[i].M2State = MachineState.State.idle;
                SystemState.machines3[i].M3State = MachineState.State.idle;
                SystemState.machines4[i].M4State = MachineState.State.idle;

                SystemState.machines2[i].bufferSize = this.buffersize2;
                SystemState.machines3[i].bufferSize = this.buffersize3;
                SystemState.machines4[i].bufferSize = this.buffersize4;
            }
        }

        public void ScheduleEvents()
        {
            for (int i = 0; i < 4; i++)
            {
                SystemState.machines1[i].ScheduleBreaksDown();
                SystemState.machines1[i].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                SystemState.machines1[i].M1State = MachineState.State.busy;
            }
        }

        public void Analyze()
        {
            Console.WriteLine(" ========= REPORT ========= ");

            Console.WriteLine("Total Runtime is {0} seconds or {1} hours.", GeneralTime.MasterTime, (GeneralTime.MasterTime / 3600));
            Console.WriteLine();

            Console.WriteLine("Buffer two = {0}\nBuffer three = {1}\nBuffer four = {2}",
                this.buffersize2, this.buffersize3, this.buffersize4);
            Console.WriteLine();

            double avgBusyTimeM1 = 0;
            double avgBusyTimeM2 = 0;
            double avgBusyTimeM3 = 0;
            double avgBusyTimeM4 = 0;

            if (GeneralTime.MasterTime != 0)
            {
                avgBusyTimeM1 = (SystemState.machines1[0].busytime + SystemState.machines1[1].busytime +
                    SystemState.machines1[2].busytime + SystemState.machines1[3].busytime) / (GeneralTime.MasterTime * 4);
                avgBusyTimeM2 = (SystemState.machines2[0].busytime + SystemState.machines2[1].busytime) / (GeneralTime.MasterTime * 2);
                avgBusyTimeM3 = (SystemState.machines3[0].busytime + SystemState.machines3[1].busytime) / (GeneralTime.MasterTime * 2);
                avgBusyTimeM4 = (SystemState.machines4[0].busytime + SystemState.machines4[1].busytime) / (GeneralTime.MasterTime * 2);
            }

            Console.WriteLine("Percent of time the machines one are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM1);
            Console.WriteLine("Percent of time the machines two are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM2);
            Console.WriteLine("Percent of time the machines three are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM3);
            Console.WriteLine("Percent of time the machines four are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM4);
            Console.WriteLine();

            double prodHour = 0;
            if (GeneralTime.MasterTime != 0)
            {
                prodHour = SystemState.totalDVDFinished / (GeneralTime.MasterTime / 3600);
            }
            Console.WriteLine("Production per hour = {0}", prodHour);
            Console.WriteLine();


            Console.WriteLine("Average throughput time = {0}", SystemState.averageThroughputTime);
            Console.WriteLine();

            Console.WriteLine(" ========================= ");
        }
    }
}
