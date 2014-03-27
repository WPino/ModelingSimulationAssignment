using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

/* Using the C# linked list. Do not use the LinkedList class anymore. The linkedlist is still defined in the 
    public static class. The new method: AddNextNode, RemoveFirst and Display are static method defined in the 
    program class, so use Program.MethodName to use them*/ 

namespace Simulation
{
    public static class GeneralTime
    {
        private static double masterTime = 0;

        public static double MasterTime
        {
            get { return masterTime; }
            set
            {
                double temp = masterTime;
                if (value >= 0)
                {
                    masterTime = value;
                }
            }
        }
    }

    public static class SystemState
    {
        public static int _seed = 0;
        public static Random R = new Random(_seed);
        public static double timeResetPerformance { get; set; }


        public static Machine1[] machines1;
        public static Machine2[] machines2;
        public static Machine3[] machines3;
        public static Machine4[] machines4;

        public static int totalDVDFinished { get; set; }
        public static double averageThroughputTime { get; set; }

        public static void updateThroughputTime(double newThroughputTime)
        {
            averageThroughputTime = (((averageThroughputTime * (totalDVDFinished - 1)) + newThroughputTime) / totalDVDFinished);
        }


    }


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

    public static class EventList
    {
        public static LinkedList<Event> eventList = new LinkedList<Event>();
    }

    class Program
    {
        int[] buffersize2 = { 20, 40/*, 70, 100, 150, 200 */};
        int[] buffersize3 = { 20, 40/*, 70, 100, 150, 200 */};
        int[] buffersize4 = { 20, 40/*, 70, 100, 150, 200 */};

        double endTime = 200 * 3600;
        double prevEndTime = 0;

        // three dimensional arrays, one for each performance measure.

        
        

        public void LoopRun()
        {

            double[, ,] throuputs = new double[buffersize2.Length, buffersize3.Length, buffersize4.Length];
            double[, ,] prodPerHour = new double[buffersize2.Length, buffersize3.Length, buffersize4.Length];

            int repetition = 2;
            for (int b2 = 0; b2 < buffersize2.Length; b2++)
            {
                for (int b3 = 0; b3 < buffersize3.Length; b3++)
                {
                    for (int b4 = 0; b4 < buffersize3.Length; b4++)
                    {
                        double averageThr = 0;
                        double produtionPerHour = 0;
                        double totalProdH = 0;


                        for (int k = 0; k < repetition; k++)
                        {

                            if (buffersize3[b3] > buffersize4[b4])
                            {
                                break;
                            }

                            SystemState.R = new Random(k);
                            GeneralTime.MasterTime = 0;
                            InitialiseMachines(b2, b3, b4);
                            ScheduleEvents();
                            Run();

                           for (int i = 0; i < 4; i++)
                            {
                                SystemState.machines1[i].M1State = MachineState.State.idle;
                                if (i < 2)
                                {
                                    SystemState.machines2[i].M2State = MachineState.State.idle;
                                    SystemState.machines3[i].M3State = MachineState.State.idle;
                                    SystemState.machines4[i].M4State = MachineState.State.idle;
                                }
                            }


                            averageThr += SystemState.averageThroughputTime;
                            if (GeneralTime.MasterTime != 0)
                            {
                                produtionPerHour = SystemState.totalDVDFinished / ((GeneralTime.MasterTime - SystemState.timeResetPerformance) / 3600);
                            }

                            totalProdH += produtionPerHour;
                            //Analyze(b2, b3, b4);

                            //IdleBusyBrokenBlockedTimes();
                            Reset.ResetNew();
                        }

                        if (averageThr != 0 && totalProdH != 0)
                        {
                            averageThr = (averageThr / repetition) / 3600;
                            throuputs[b2, b3, b4] = averageThr;

                            totalProdH = totalProdH / repetition;
                            prodPerHour[b2, b3, b4] = totalProdH;

                            //Console.WriteLine("average ThroughPut {0}", averageThr);
                            //Console.WriteLine("average prod per hour {0}", totalProdH);

                        }
                        else
                        {
                            throuputs[b2, b3, b4] = 0;
                            prodPerHour[b2, b3, b4] = 0;
                        }
                    }        
                }
            }

            Read3DArray(throuputs);
        }
        


        static void Main(string[] args)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            Program p = new Program();
            p.LoopRun();
            sp.Stop();

            Console.WriteLine("DONE in {0}", sp.Elapsed);

            Console.ReadLine();
        }

            
        public void Run()
        {
            SystemState.timeResetPerformance = 0;
            bool passed = false;

            while (EventList.eventList.First.Value.Time < endTime)
	        {
                GeneralTime.MasterTime = EventList.eventList.First.Value.Time;
                Event nextEvent = Program.RemoveFirstNode(EventList.eventList);
                nextEvent.HandleEvent();

                if (passed == false && GeneralTime.MasterTime > 60 * 3600)
                {
                    passed = true;
                    Reset.ResetPerformance();
                }

            }
            
        }

        public void InitialiseMachines(int indexb2, int indexb3, int indexb4)
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
                SystemState.machines1[i].lastStateChange = 0;
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


                SystemState.machines2[i].bufferSize = this.buffersize2[indexb2];
                SystemState.machines3[i].bufferSize = this.buffersize3[indexb3];
                SystemState.machines4[i].bufferSize = this.buffersize4[indexb4];


                SystemState.machines2[i].lastStateChange = 0;
                SystemState.machines3[i].lastStateChange = 0;
                SystemState.machines4[i].lastStateChange = 0;
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

        public void Analyze(int b2, int b3, int b4)
        {
            Console.WriteLine(" ========= REPORT ========= ");

            Console.WriteLine("Total Runtime is {0} seconds or {1} hours.", GeneralTime.MasterTime, (GeneralTime.MasterTime / 3600));
            Console.WriteLine();
            Console.WriteLine("Performance reset at: {0} hours", SystemState.timeResetPerformance/ 3600);
            Console.WriteLine();
            Console.WriteLine("Total of {0} DVDs produced.", SystemState.totalDVDFinished);

            Console.WriteLine("Buffer two = {0}\nBuffer three = {1}\nBuffer four = {2}",
                this.buffersize2[b2], this.buffersize3[b3], this.buffersize4[b4]);
            Console.WriteLine();

            double avgBusyTimeM1 = 0;
            double avgBusyTimeM2 = 0;
            double avgBusyTimeM3 = 0;
            double avgBusyTimeM4 = 0;

            if (GeneralTime.MasterTime != 0)
            {
                avgBusyTimeM1 = (SystemState.machines1[0].busytime + SystemState.machines1[1].busytime +
                    SystemState.machines1[2].busytime + SystemState.machines1[3].busytime) / 
                    ((GeneralTime.MasterTime - SystemState.timeResetPerformance) * 4);
                avgBusyTimeM2 = (SystemState.machines2[0].busytime + SystemState.machines2[1].busytime) / 
                    ((GeneralTime.MasterTime - SystemState.timeResetPerformance) * 2);
                avgBusyTimeM3 = (SystemState.machines3[0].busytime + SystemState.machines3[1].busytime) / 
                    ((GeneralTime.MasterTime - SystemState.timeResetPerformance) * 2);
                avgBusyTimeM4 = (SystemState.machines4[0].busytime + SystemState.machines4[1].busytime) / 
                    ((GeneralTime.MasterTime - SystemState.timeResetPerformance) * 2);


            }

            
            Console.WriteLine("Percent of time the machines one are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM1*100);
            Console.WriteLine("Percent of time the machines two are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM2*100);
            Console.WriteLine("Percent of time the machines three are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM3*100);
            Console.WriteLine("Percent of time the machines four are busy (on average per machine):");
            Console.WriteLine("      {0} %", avgBusyTimeM4*100);
            Console.WriteLine();
            

            
            double prodHour = 0;
            if (GeneralTime.MasterTime != 0)
            {
                prodHour = SystemState.totalDVDFinished / ((GeneralTime.MasterTime-SystemState.timeResetPerformance) / 3600);
            }
            Console.WriteLine("Production per hour = {0}", prodHour);
            Console.WriteLine();


            Console.WriteLine("Average throughput time = {0} h", SystemState.averageThroughputTime / 3600);
            Console.WriteLine();

            Console.WriteLine(" ========================= ");
        }

        #region Linkedlist
        public static void AddNextNode(LinkedList<Event> list, Event ev)
        {

            if (list.Count == 0)
            {
                list.AddFirst(ev);
                return;
            }
            else
            {
                LinkedListNode<Event> fst = list.First;
                LinkedListNode<Event> toReturn = fst;
                bool found = false;
                while (found == false)
                {
                    if (fst.Value.Time < ev.Time)
                    {
                        if (fst.Next != null)
                        {
                            fst = fst.Next;
                        }
                        else
                        {
                            found = true;
                            list.AddLast(ev);
                        }
                    }
                    else
                    {
                        found = true;
                        list.AddBefore(fst, ev);
                    }
                }
                return;
            }
        }

        public static void Display(LinkedList<Event> words, string test)
        {
            Console.WriteLine(test);
            foreach (Event e in words)
            {
                e.PrintDetails();
                Console.WriteLine();
            }
        }

        public static Event RemoveFirstNode(LinkedList<Event> list)
        {
            Event first = list.First.Value;
            list.RemoveFirst();
            return first;
        }
        #endregion

        // prints the details of the idle, busy, broken and blocked times for all machines!
        public void IdleBusyBrokenBlockedTimes()
        {
            // machines 1
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Machine1[{0}]", i);
                Console.WriteLine("idle time {0}", SystemState.machines1[i].idletime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("busy time {0}", SystemState.machines1[i].busytime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("blocked time {0}", SystemState.machines1[i].blockedtime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("broken time {0}", SystemState.machines1[i].brokentime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100); 
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            // machines 2
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Machine2[{0}]", i);
                Console.WriteLine("idle time {0}", SystemState.machines2[i].idletime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("busy time {0}", SystemState.machines2[i].busytime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("blocked time {0}", SystemState.machines2[i].blockedtime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            // machines 3
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Machine3[{0}]", i);
                Console.WriteLine("idle time {0}", SystemState.machines3[i].idletime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("busy time {0}", SystemState.machines3[i].busytime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("blocked time {0}", SystemState.machines3[i].blockedtime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();

            // machines 4
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Machine4[{0}]", i);
                Console.WriteLine("idle time {0}", SystemState.machines4[i].idletime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("busy time {0}", SystemState.machines4[i].busytime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine("broken time {0}", SystemState.machines4[i].brokentime / 
                    (GeneralTime.MasterTime - SystemState.timeResetPerformance) * 100);
                Console.WriteLine();
            }
        }

        public static void ReadFromArray(double[,] multiArray)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    Console.Write(multiArray[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void Read3DArray(double[,,] dimArray)
        {
            Console.WriteLine("b2, b3, b4, variable");
            for (int i = 0; i < buffersize2.Length; i++)
            {
                for (int j = 0; j < buffersize3.Length; j++)
                {
                    for (int k = 0; k < buffersize4.Length; k++)
                    {
                        Console.Write("{0} {1} {2} -> {3}", buffersize2[i], buffersize3[j], buffersize4[k], dimArray[i, j, k]);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
