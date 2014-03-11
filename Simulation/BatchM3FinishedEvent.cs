using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class BatchM3FinishedEvent : Event
    {
        private int machine3Index;
        private string eventType = "BatchM3FinishedEvent";
        Queue<double> startTimesDvds;
        
        public BatchM3FinishedEvent(int index, Queue<double> starttimes)
        {
            machine3Index = index;
            startTimesDvds = starttimes;

            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();

            // adding event to the linkedlist
            //EventList.eventList.Add(this);
            Program.AddNextNode(EventList.eventList, this);

        }


        public override double CalculateEventTime()
        {
            double sputteringTime = 0, lacquerCoatingTime = 0, dryingTime = (3 * 60), delay = 0;

            for (int i = 0; i < SystemState.machines3[machine3Index].bufferSize; i++)
            {
                sputteringTime += randomExpDist(10);
                lacquerCoatingTime += randomExpDist(6);
                delay += (DvdStalls() * 5 * 60);
            }
            
            double time = sputteringTime + lacquerCoatingTime + dryingTime + delay;
            return GeneralTime.MasterTime + time;
        }

        private int DvdStalls()
        {
            Random R = new Random();
            int i = R.Next(100);
            if (i < 3)
                return 1;
            else
                return 0;
        }

        public override void HandleEvent()
        {
            SystemState.machines4[machine3Index].buffer = new Queue<double>(startTimesDvds); //should we clone?
            SystemState.machines3[machine3Index].M3State = MachineState.State.blocked;

            if (SystemState.machines4[machine3Index].M4State == MachineState.State.idle)
            {
                if (SystemState.machines4[machine3Index].inkCounter == 200 + SystemState.machines4[machine3Index].deviation)
                {
                    SystemState.machines4[machine3Index].ScheduleM4NewInk();
                }
                else
                {
                    double startTimefromQ = SystemState.machines4[machine3Index].buffer.Dequeue();
                    SystemState.machines4[machine3Index].ScheduleDvdM4Finished(startTimefromQ);
                }
            }
        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nMachine3 index: {1}\nTime: {2}\n",
                eventType, machine3Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
