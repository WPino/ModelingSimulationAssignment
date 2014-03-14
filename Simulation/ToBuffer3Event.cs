using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    class ToBuffer3Event : Event
    {
        private int machine3Index;
        private string eventType = "ToBuffer3Event";
        double startTimeDvd;
        
        public ToBuffer3Event(int index, double starttime)
        {
            machine3Index = index;
            startTimeDvd = starttime;

            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();

            // adding event to the linkedlist
            //EventList.eventList.Add(this);

            Program.AddNextNode(EventList.eventList, this);

            // method to find time at which this event happens
            // and add to EventList.eventList
        }

        public override double CalculateEventTime()
        {
            double time = GeneralTime.MasterTime + (5 * 60);
            return time;
        }

        public override void HandleEvent()
        {
            SystemState.machines3[machine3Index].buffer.Enqueue(startTimeDvd);

            //if the buffer before machine three is full, look whether either of the machines three is idle with an empty buffer
            // (very inefficient to wait untill the buffer behind it is completely empty....)
            // if so, schedule new M3 finished event, clear the buffer
            if (SystemState.machines3[machine3Index].buffer.Count == SystemState.machines3[machine3Index].bufferSize)
            {
                if (SystemState.machines3[0].M3State == MachineState.State.idle && SystemState.machines4[0].buffer.Count == 0)
                {
                    Queue<double> newBatch = new Queue<double> (SystemState.machines3[machine3Index].buffer); //Should this be a clone?
                    SystemState.machines3[0].ScheduleBatchM3Finished(newBatch);
                    SystemState.machines3[machine3Index].buffer.Clear();
                    SystemState.machines2[machine3Index].buffer3InclConveyorContent = 0;
                    SystemState.machines3[0].M3State = MachineState.State.busy;
                    //if M2 was blocked -> schedule new event from buffer
                    if (SystemState.machines2[machine3Index].M2State == MachineState.State.blocked &&
                        SystemState.machines2[machine3Index].buffer.Count != 0)
                    {
                        double startTimeDvdfromQ = SystemState.machines2[machine3Index].buffer.Dequeue();
                        SystemState.machines2[machine3Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                        SystemState.machines2[machine3Index].M2State = MachineState.State.busy;
                    }

                }
                else if (SystemState.machines3[1].M3State == MachineState.State.idle && SystemState.machines4[1].buffer.Count == 0)
                {
                    Queue<double> newBatch = SystemState.machines3[machine3Index].buffer; //Should this be a clone?
                    SystemState.machines3[1].ScheduleBatchM3Finished(newBatch);
                    SystemState.machines3[machine3Index].buffer.Clear();
                    SystemState.machines2[machine3Index].buffer3InclConveyorContent = 0;
                    SystemState.machines3[1].M3State = MachineState.State.busy;
                    //if M2 was blocked -> schedule new event from buffer
                    if (SystemState.machines2[machine3Index].M2State == MachineState.State.blocked &&
                        SystemState.machines2[machine3Index].buffer.Count != 0)
                    {
                        double startTimeDvdfromQ = SystemState.machines2[machine3Index].buffer.Dequeue();
                        SystemState.machines2[machine3Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                        SystemState.machines2[machine3Index].M2State = MachineState.State.busy;
                    }
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
