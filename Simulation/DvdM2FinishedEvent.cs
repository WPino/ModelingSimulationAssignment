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

            // adds event to the linkedlist
            Program.AddNextNode(EventList.eventList, this);
        }

        public override double CalculateEventTime()
        {
            // using the gamma distribution.
            double gamma = GammaDistribution(1.92185748, 1 / 0.07857608);
            if (gamma == -1)
                throw new Exception("Gamma Distribution Malfunction");

            double finished = GeneralTime.MasterTime + gamma;
            return finished;
        }

        public override void HandleEvent()
        {            
            //set machine to idle
            SystemState.machines2[machine2Index].M2State = MachineState.State.idle;

            // if the dvd does not fail, update the content of the conveyor and the buffer combined and schedule a new DvdToBuffer3 event
            if (!DvdFails())
            {  
                SystemState.machines2[machine2Index].buffer3InclConveyorContent++;
                SystemState.machines3[machine2Index].ScheduleDvdToBuffer3(startTimeDvd);              
            }

            // if the content of the conveyor and the buffer combined is equal to the buffer size set machine to blocked
            // this is inefficient and inaccurate
            if (SystemState.machines2[machine2Index].buffer3InclConveyorContent == SystemState.machines3[machine2Index].bufferSize)
            {
                SystemState.machines2[machine2Index].M2State = MachineState.State.blocked;
            }
            else if (SystemState.machines2[machine2Index].buffer.Count != 0)
            {
                //if there is still space, schedule a new dvdM2event from the buffer (if it isnt empty)
                double startTimeDvdfromQ = SystemState.machines2[machine2Index].buffer.Dequeue();
                SystemState.machines2[machine2Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                SystemState.machines2[machine2Index].M2State = MachineState.State.busy;

                SystemState.machines2[machine2Index].checkRebootMachines1();
            }
        }

        private bool DvdFails()
        {
            int i = SystemState.R.Next(100);
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
