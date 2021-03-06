﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{

    class M4NewInkEvent : Event
    {
        private int machine4Index;
        private string eventType = "M4NewInkEvent";

        public M4NewInkEvent(int index)
        {
            machine4Index = index;

            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();

            // adds event to the linkedlist
            Program.AddNextNode(EventList.eventList, this);
        }

        public override double CalculateEventTime()
        {
            double time = GeneralTime.MasterTime + randomNormDist(15*60, 1*60);
            return time;
        }

        public override void HandleEvent()
        {
            SystemState.machines4[machine4Index].calculateDeviation();
            SystemState.machines4[machine4Index].inkCounter = 0;
            double startTimefromQ = SystemState.machines4[machine4Index].buffer.Dequeue();
            SystemState.machines4[machine4Index].ScheduleDvdM4Finished(startTimefromQ);
            SystemState.machines4[machine4Index].M4State = MachineState.State.busy;
        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nMachine4 index: {1}\nTime: {2}\n",
                eventType, machine4Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
