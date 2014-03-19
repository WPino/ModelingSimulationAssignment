using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class DvdM1FinishedEvent : Event
    {
        private int machine1Index;
        private string eventType = "DvdM1FinishedEvent";
        double startTimeDvd;
        //Random rand = new Random(10);


        public DvdM1FinishedEvent(int index, double starttime)
	    {
            machine1Index = index;
            startTimeDvd = starttime;
            

            
            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();
            
            // adding event to the linkedlist
            //EventList.eventList.Add(this);
            Program.AddNextNode(EventList.eventList, this);

	    }


        // I am not using the proper data but random number (just for experimentations)
        /*public double PlanWhenEventFinished(double startTime)
        {
            double finished = GeneralTime.MasterTime + rand.Next(0, 100);
            
            return finished;
        }
        */
        // calculate when new Event of type DvdFinishedEvent will happen
        public override double CalculateEventTime()
        {
            // read from processing times (exponential distribution) argue why 60 is right
            return GeneralTime.MasterTime + randomExpDist(59.93954);
        }


        public override void HandleEvent()
        {
            
            // Checks which production line the machine is in (which machine two has to be checked)
            // and what the other machine in that production line is
            int prodLine, otherMachine;
            if (machine1Index == 0 || machine1Index == 1)
            {
                prodLine = 0;
                if(machine1Index == 0)
                    otherMachine = 1;
                else
                    otherMachine = 0;
            }
            else
            {
                prodLine = 1;
                if(machine1Index == 2)
                    otherMachine = 3;
                else
                    otherMachine = 2;
            }

            // if machine 2 is idle schedule a dvdM2 finished event and set the M2 to busy, otherwise put in buffer
            if (SystemState.machines2[prodLine].M2State == MachineState.State.idle)
            {
                SystemState.machines2[prodLine].ScheduleDvdM2Finished(startTimeDvd);
                SystemState.machines2[prodLine].M2State = MachineState.State.busy;
            }
            else
            {
                SystemState.machines2[prodLine].buffer.Enqueue(startTimeDvd);
            }
            // if the buffer is full (or has one spot left which the other machine will fill) set to blocked (but don't if the machine is broken)
            if (SystemState.machines1[machine1Index].M1State != MachineState.State.blocked &&
                (SystemState.machines2[prodLine].buffer.Count == SystemState.machines2[prodLine].bufferSize ||
                ((SystemState.machines2[prodLine].buffer.Count == SystemState.machines2[prodLine].bufferSize - 1) &&
                (SystemState.machines1[otherMachine].M1State == MachineState.State.busy))))
            {
                SystemState.machines1[machine1Index].M1State = MachineState.State.blocked;
            }

            // if the machine is neither blocked nor broken, schedule new event and set to busy
            if (SystemState.machines2[prodLine].M2State != MachineState.State.blocked && 
                SystemState.machines2[prodLine].M2State != MachineState.State.broken)
            {
                SystemState.machines1[machine1Index].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                SystemState.machines1[machine1Index].M1State = MachineState.State.busy;
            }
            
        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nMachine1 index: {1}\nTime: {2}\n",
                eventType, machine1Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
