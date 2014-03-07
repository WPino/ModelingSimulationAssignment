using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    
    class DvdM2FinishedEvent : Event
    {
        private int machine2Index;
        private string eventType = "DvdM2FinishedEvent";
        double startTimeDvd;

        public DvdM2FinishedEvent(int index, double starttime)
        {
            machine2Index = index;
            startTimeDvd = starttime;



            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();

            // adding event to the linkedlist
            EventList.eventList.Add(this);
        }

        public override double CalculateEventTime()
        {
            // same bs as in DvdM1finished
            
            Random rand = new Random();
            double finished = GeneralTime.MasterTime + rand.Next(Math.Abs(Guid.NewGuid().GetHashCode()) % 100);
            return finished;
        }

        public override void HandleEvent()
        {
            int nr1machine1, nr2machine1;
            if(machine2Index == 0)
            {
                nr1machine1 = 0;
                nr2machine1 = 1;
            }
            else
            {
                nr1machine1 = 2;
                nr2machine1 = 3;
            }
            
            SystemState.machines2[machine2Index].M2State = MachineState.State.idle;
            if (!DvdFails())
            {
                SystemState.machines2[machine2Index].buffer3InclConveyorContent++;
                SystemState.machines3[machine2Index].ScheduleDvdToBuffer3(startTimeDvd);
            }
            if (SystemState.machines2[machine2Index].buffer3InclConveyorContent == SystemState.machines3[machine2Index].bufferSize)
            {
                SystemState.machines2[machine2Index].M2State = MachineState.State.blocked;
            }
            else if (SystemState.machines2[machine2Index].buffer.Count != 0)
            {
                double startTimeDvdfromQ = SystemState.machines2[machine2Index].buffer.Dequeue();
                SystemState.machines2[machine2Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                SystemState.machines2[machine2Index].M2State = MachineState.State.busy;

                // if the buffer before machine 2 is full except for one (or except for 2) and one machine is busy (or 2) then do nothing
                if (!(SystemState.machines2[machine2Index].buffer.Count == SystemState.machines2[machine2Index].bufferSize - 1 &&
                    (SystemState.machines1[nr1machine1].M1State == MachineState.State.busy
                    || SystemState.machines1[nr2machine1].M1State == MachineState.State.busy)))
                {
                    if (!(SystemState.machines2[machine2Index].buffer.Count == SystemState.machines2[machine2Index].bufferSize - 2 &&
                    (SystemState.machines1[nr1machine1].M1State == MachineState.State.busy
                    && SystemState.machines1[nr2machine1].M1State == MachineState.State.busy)))
                    {
                        // if a machine is neither broken or busy, set it to busy and schedule a new event
                        if (SystemState.machines1[nr1machine1].M1State != MachineState.State.busy &&
                            SystemState.machines1[nr1machine1].M1State != MachineState.State.broken)
                        {
                            SystemState.machines1[nr1machine1].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                            SystemState.machines1[nr1machine1].M1State = MachineState.State.busy;
                        }
                        if (SystemState.machines1[nr2machine1].M1State != MachineState.State.busy &&
                            SystemState.machines1[nr2machine1].M1State != MachineState.State.broken)
                        {
                            SystemState.machines1[nr2machine1].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                            SystemState.machines1[nr2machine1].M1State = MachineState.State.busy;
                        }
                    }
                }
            }
        }

        private bool DvdFails()
        {
            Random R = new Random();
            int i = R.Next(100);
            if (i < 2)
                return true;
            else
                return false;
        }


        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nMachine2 index: {1}\nTime: {2}\n",
                eventType, machine2Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
