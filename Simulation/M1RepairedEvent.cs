using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class M1RepairedEvent : Event
    {
        private int machine1Index;
        private string eventType = "M1RepairedEvent";

        public int M1Index
        {
            get { return machine1Index; }
            set
            {
                if (value == 0 || value == 1
                    || value == 2 || value == 3)
                {
                    machine1Index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1 {0}", machine1Index));
                }
            }
        }

        public M1RepairedEvent(int index)
        {
            // check which machine1 was fixed using the index
            M1Index = index;
            this.Time = CalculateEventTime();

            Program.AddNextNode(EventList.eventList, this);
        }

        public override double CalculateEventTime()
        {
            return GeneralTime.MasterTime + randomExpDist(2*3600);
        }

        public override void HandleEvent()
        {
            SystemState.machines1[machine1Index].ScheduleBreaksDown();

            int prodLine, otherMachine;
            if (machine1Index == 0 || machine1Index == 1)
            {
                prodLine = 0;
                if (machine1Index == 0)
                    otherMachine = 1;
                else
                    otherMachine = 0;
            }
            else
            {
                prodLine = 1;
                if (machine1Index == 2)
                    otherMachine = 3;
                else
                    otherMachine = 2;
            }

            // once the system is repaired it is either set to blocked or to busy
            if (SystemState.machines2[prodLine].buffer.Count == SystemState.machines2[prodLine].bufferSize ||
                ((SystemState.machines2[prodLine].buffer.Count == SystemState.machines2[prodLine].bufferSize - 1) &&
                (SystemState.machines1[otherMachine].M1State == MachineState.State.busy)))
            {
                SystemState.machines1[machine1Index].M1State = MachineState.State.blocked;
            }
            else
            {
                //due to our design decisions this section is not completely accurate 
                //This is because it is possible that while a M1 breaks and is repaired a Dvd is still in the making in the machine
                //After this situation there will be 5 M1finished loops running, this will be corrected the next time the same machine breaks down
                SystemState.machines1[machine1Index].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                SystemState.machines1[machine1Index].M1State = MachineState.State.busy;
            }
        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nMachine1 index: {1}\nTime: {2}",
                eventType, machine1Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
