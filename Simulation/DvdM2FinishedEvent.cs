﻿using System;
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
            //EventList.eventList.Add(this);
            Program.AddNextNode(EventList.eventList, this);
        }

        public override double CalculateEventTime()
        {
            // same bs as in DvdM1finished
            
            Random rand = new Random();
            //double finished = GeneralTime.MasterTime + rand.Next(Math.Abs(Guid.NewGuid().GetHashCode()) % 100);

            // using the gamma distribution. The shape and scale are found by fitting the model. 
            // I did that using R, ill include it in the report.
            double finished = GeneralTime.MasterTime + GammaDistribution(1.92185748, 1 / 0.07857608);
            
            return finished;
        }

        public override void HandleEvent()
        {            
            SystemState.machines2[machine2Index].M2State = MachineState.State.idle;
            if (!DvdFails())
            {
                SystemState.machines2[machine2Index].onConveyor.Enqueue(startTimeDvd);
                SystemState.machines2[machine2Index].timeDifferencesConveyor.Enqueue(
                    GeneralTime.MasterTime - SystemState.machines2[machine2Index].lastToConveyor);
                SystemState.machines2[machine2Index].lastToConveyor = GeneralTime.MasterTime;

                if (SystemState.machines2[machine2Index].onConveyor.Count == 0)
                {
                    SystemState.machines3[machine2Index].ScheduleDvdToBuffer3();
                }
            }

            
            //If the buffer before machine 2 is not empty, and machine two is not blocked schedule a new event 
            if (SystemState.machines2[machine2Index].buffer.Count != 0 && 
                SystemState.machines2[machine2Index].M2State != MachineState.State.blocked)
            {
                double startTimeDvdfromQ = SystemState.machines2[machine2Index].buffer.Dequeue();
                SystemState.machines2[machine2Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                SystemState.machines2[machine2Index].M2State = MachineState.State.busy;

                SystemState.machines2[machine2Index].checkRebootMachines1();

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
