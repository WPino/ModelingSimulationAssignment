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
        
        public BatchM3FinishedEvent(int index)
        {
            machine3Index = index;

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
            //following hsa to happen for both machines 4
            bool FirstSucceeded = false;

            //if the buffer has no space for the batch, set machine to blocked
            if (SystemState.machines3[machine3Index].bufferSize <
                (SystemState.machines4[0].bufferSize - SystemState.machines4[0].buffer.Count))
            {
                SystemState.machines3[machine3Index].M3State = MachineState.State.blocked;
            }
            else
            {
                FirstSucceeded = true;
                while(SystemState.machines3[machine3Index].batch.Count != 0)
                {
                    double transfer = SystemState.machines3[machine3Index].batch.Dequeue();
                    SystemState.machines4[0].buffer.Enqueue(transfer);
                }
                if (SystemState.machines4[0].M4State == MachineState.State.idle)
                {
                    if (SystemState.machines4[0].inkCounter == 200 + SystemState.machines4[0].deviation)
                    {
                        SystemState.machines4[0].ScheduleM4NewInk();
                        SystemState.machines4[0].M4State = MachineState.State.broken;
                    }
                    else
                    {
                        double startTimefromQ = SystemState.machines4[0].buffer.Dequeue();
                        SystemState.machines4[0].ScheduleDvdM4Finished(startTimefromQ);
                        SystemState.machines4[0].M4State = MachineState.State.busy;

                    }
                }
                SystemState.machines3[machine3Index].M3State = MachineState.State.idle;
                SystemState.machines3[machine3Index].checkRebootMachine3();
            }

            if (!FirstSucceeded)
            {
                if (SystemState.machines3[machine3Index].bufferSize <
                (SystemState.machines4[1].bufferSize - SystemState.machines4[1].buffer.Count))
                {
                    SystemState.machines3[machine3Index].M3State = MachineState.State.blocked;
                }
                else
                {
                    while (SystemState.machines3[machine3Index].batch.Count != 0)
                    {
                        double transfer = SystemState.machines3[machine3Index].batch.Dequeue();
                        SystemState.machines4[1].buffer.Enqueue(transfer);
                    }
                    if (SystemState.machines4[1].M4State == MachineState.State.idle)
                    {
                        if (SystemState.machines4[1].inkCounter == 200 + SystemState.machines4[1].deviation)
                        {
                            SystemState.machines4[1].ScheduleM4NewInk();
                            SystemState.machines4[1].M4State = MachineState.State.broken;
                        }
                        else
                        {
                            double startTimefromQ = SystemState.machines4[0].buffer.Dequeue();
                            SystemState.machines4[1].ScheduleDvdM4Finished(startTimefromQ);
                            SystemState.machines4[1].M4State = MachineState.State.busy;
                        }
                    }
                    SystemState.machines3[machine3Index].M3State = MachineState.State.idle;
                    SystemState.machines3[machine3Index].checkRebootMachine3();
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
