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
            EventList.eventList.Add(this);

        }


        public double CalculateEventTime()
        {
            double sputteringTime=40, lacquerCoatingTime=40, dryingTime=40; //completely arbitratry times, perhaps write method for each?
            
            
            double time = GeneralTime.MasterTime + sputteringTime + lacquerCoatingTime + dryingTime;
            return time;
        }

        public override void HandleEvent()
        {
            SystemState.machines4[machine3Index].buffer = new Queue<double>(startTimesDvds); //should we clone?
            SystemState.machines3[machine3Index].state = MachineState.State.blocked;

            if (SystemState.machines4[machine3Index].state == MachineState.State.idle)
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
