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

            // adding event to the linkedlist
            EventList.eventList.Add(this);

        }

        public override double CalculateEventTime()
        {
            //again completely arbitrary
            
            return GeneralTime.MasterTime + 7; 
        }

        public override void HandleEvent()
        {
            //update the inkCounter, the total amount of dvds finished and the throughput times
            SystemState.machines4[machine4Index].inkCounter++;
            SystemState.totalDVDFinished++;
            SystemState.throughputTimes.Add(GeneralTime.MasterTime - startTimeDvd); //maybe more efficient way?

            //if there are still dvds in the buffer, schedule a new newink or M4finished event
            if (SystemState.machines4[machine4Index].buffer.Count != 0)
            {
                if (SystemState.machines4[machine4Index].inkCounter == 200 + SystemState.machines4[machine4Index].deviation)
                {
                    SystemState.machines4[machine4Index].ScheduleM4NewInk();
                }
                else
                {
                    double startTimefromQ = SystemState.machines4[machine4Index].buffer.Dequeue();
                    SystemState.machines4[machine4Index].ScheduleDvdM4Finished(startTimefromQ);
                }
            }
            else if (SystemState.machines3[machine4Index].state != MachineState.State.busy)
            {
                SystemState.machines3[machine4Index].state = MachineState.State.idle;

                //if machine 3 is not idle check if either of the buffers are full, if so schedule a new M3batchfinished event
                //also check if the M2 before was blocked and shedule a new event if so.
                if (SystemState.machines3[0].buffer.Count == SystemState.machines3[0].bufferSize)
                {
                    Queue<double> newBatch = new Queue<double>(SystemState.machines3[0].buffer); //Should this be a clone?
                    SystemState.machines3[machine4Index].ScheduleBatchM3Finished(newBatch);
                    SystemState.machines3[0].buffer.Clear();
                    SystemState.machines2[0].buffer3InclConveyorContent = 0;
                    SystemState.machines3[machine4Index].state = MachineState.State.busy;
                    //if M2 was blocked -> schedule new event from buffer
                    if (SystemState.machines2[0].state == MachineState.State.blocked &&
                        SystemState.machines2[0].buffer.Count != 0)
                    {
                        double startTimeDvdfromQ = SystemState.machines2[0].buffer.Dequeue();
                        SystemState.machines2[0].ScheduleDvdM2Finished(startTimeDvdfromQ);
                        SystemState.machines2[0].state = MachineState.State.busy;
                    }

                }
                else if (SystemState.machines3[1].buffer.Count == SystemState.machines3[1].bufferSize)
                {
                    Queue<double> newBatch = new Queue<double>(SystemState.machines3[1].buffer); //Should this be a clone?
                    SystemState.machines3[machine4Index].ScheduleBatchM3Finished(newBatch);
                    SystemState.machines3[1].buffer.Clear();
                    SystemState.machines2[1].buffer3InclConveyorContent = 0;
                    SystemState.machines3[machine4Index].state = MachineState.State.busy;
                    //if M2 was blocked -> schedule new event from buffer
                    if (SystemState.machines2[1].state == MachineState.State.blocked &&
                        SystemState.machines2[1].buffer.Count != 0)
                    {
                        double startTimeDvdfromQ = SystemState.machines2[0].buffer.Dequeue();
                        SystemState.machines2[1].ScheduleDvdM2Finished(startTimeDvdfromQ);
                        SystemState.machines2[1].state = MachineState.State.busy;
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
