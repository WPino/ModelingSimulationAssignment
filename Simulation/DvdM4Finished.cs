using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class DvdM4Finished : Event
    {
        private int machine4Index;
        private string eventType = "DvdM4FinishedEvent";
        double startTimeDvd;

        public DvdM4Finished(int index, double starttime)
        {
            machine4Index = index;
            startTimeDvd = starttime;

            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();

            // adds event to the linkedlist
            Program.AddNextNode(EventList.eventList, this);
        }

        public override double CalculateEventTime()
        {
            //HAS TO BE IMPLEMENTED
            return GeneralTime.MasterTime + UniformDistribution(20, 30); 
        }

        public override void HandleEvent()
        {
            //update the inkCounter, the total amount of dvds finished and the average throughput time
            SystemState.machines4[machine4Index].inkCounter++;
            SystemState.totalDVDFinished++;
            double newThroughputTime = GeneralTime.MasterTime - startTimeDvd;
            SystemState.updateThroughputTime(newThroughputTime);

            //if there are still dvds in the buffer, schedule a new newink or M4finished event
            if (SystemState.machines4[machine4Index].buffer.Count != 0)
            {
                if (SystemState.machines4[machine4Index].inkCounter == 200 + SystemState.machines4[machine4Index].deviation)
                {
                    SystemState.machines4[machine4Index].ScheduleM4NewInk();
                    SystemState.machines4[machine4Index].M4State = MachineState.State.broken;
                }
                else
                {
                    double startTimefromQ = SystemState.machines4[machine4Index].buffer.Dequeue();
                    SystemState.machines4[machine4Index].ScheduleDvdM4Finished(startTimefromQ);
                    SystemState.machines4[machine4Index].M4State = MachineState.State.busy;
                }
            }
            else
            {
                SystemState.machines4[machine4Index].M4State = MachineState.State.idle;
            }

            // check whether there is room in the buffer for a batch of machine 3
            if (SystemState.machines3[machine4Index].bufferSize <=
                (SystemState.machines4[machine4Index].bufferSize - SystemState.machines4[machine4Index].buffer.Count))
            {
                // check if machine 3 is blocked is if so unload batch DO FOR BOTH
                // if it was idle before it cannot reboot now since that has nothing to do with space being available in the buffer after
                if (SystemState.machines3[0].M3State == MachineState.State.blocked)
                {
                    // put que from batch in buffer 4
                    while (SystemState.machines3[0].batch.Count != 0)
                    {
                        double transfer = SystemState.machines3[0].batch.Dequeue();
                        SystemState.machines4[machine4Index].buffer.Enqueue(transfer);
                        
                    }
                    if (SystemState.machines4[machine4Index].M4State == MachineState.State.idle)
                    {
                        if (SystemState.machines4[machine4Index].inkCounter == 200 + SystemState.machines4[machine4Index].deviation)
                        {
                            SystemState.machines4[machine4Index].ScheduleM4NewInk();
                            SystemState.machines4[machine4Index].M4State = MachineState.State.broken;
                        }
                        else
                        {
                            double startTimefromQ = SystemState.machines4[machine4Index].buffer.Dequeue();
                            SystemState.machines4[machine4Index].ScheduleDvdM4Finished(startTimefromQ);
                            SystemState.machines4[machine4Index].M4State = MachineState.State.busy;
                        }
                    }

                    // if machine 3 is not busy it is now idle (since it is not blocked anymore either) 
                    // check if it can be rebooted
                    if (SystemState.machines3[0].M3State != MachineState.State.busy)
                    {
                        SystemState.machines3[0].M3State = MachineState.State.idle;
                        SystemState.machines3[0].checkRebootMachine3();
                    }
                }
                else if (SystemState.machines3[1].M3State == MachineState.State.blocked)
                {
                    // put que from batch in buffer 4
                    while (SystemState.machines3[1].batch.Count != 0)
                    {
                        double transfer = SystemState.machines3[1].batch.Dequeue();
                        SystemState.machines4[machine4Index].buffer.Enqueue(transfer);
                    }

                    if (SystemState.machines4[machine4Index].M4State == MachineState.State.idle)
                    {
                        if (SystemState.machines4[machine4Index].inkCounter == 200 + SystemState.machines4[machine4Index].deviation)
                        {
                            SystemState.machines4[machine4Index].ScheduleM4NewInk();
                            SystemState.machines4[machine4Index].M4State = MachineState.State.broken;
                        }
                        else
                        {
                            double startTimefromQ = SystemState.machines4[machine4Index].buffer.Dequeue();
                            SystemState.machines4[machine4Index].ScheduleDvdM4Finished(startTimefromQ);
                            SystemState.machines4[machine4Index].M4State = MachineState.State.busy;
                        }
                    }

                    if (SystemState.machines3[1].M3State != MachineState.State.busy)
                    {
                        SystemState.machines3[1].M3State = MachineState.State.idle;
                        SystemState.machines3[1].checkRebootMachine3();
                    }
                }
            }
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
