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
        private bool scheduledFromM2;
        
        
        public ToBuffer3Event(int index, bool fromM2)
        {
            
   
            machine3Index = index;
            scheduledFromM2 = fromM2;

            
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
            // if the queue is empty schedule the event in five minutes
            // if it is not then the event is called from the front, schedule the event on time + diff between prev. and next
            double time;
            if (scheduledFromM2)
            {
                time = GeneralTime.MasterTime + (5 * 60);
                return time;
            }
            else
            {
                double deltaTime = SystemState.machines2[machine3Index].timeDifferencesConveyor.Peek();
                time = GeneralTime.MasterTime + deltaTime;
                return time;
            }
        }

        public override void HandleEvent()
        {

            //the delay is counted twice, but this is trivial

            //Console.WriteLine("SystemState.machines3[{0}].buffer.Count = {1}", machine3Index, SystemState.machines3[machine3Index].buffer.Count);
            //Console.ReadLine();

            if (SystemState.machines3[machine3Index].buffer.Count == SystemState.machines3[machine3Index].bufferSize)
            {
                SystemState.machines2[machine3Index].M2State = MachineState.State.blocked;
            }
            else if (/*scheduledFromM2 || */SystemState.machines2[machine3Index].onConveyor.Count != 0)
            {
                SystemState.machines2[machine3Index].timeDifferencesConveyor.Dequeue();
                //get the starttime of the new dvd
                double startTimeDvd = SystemState.machines2[machine3Index].onConveyor.Dequeue();
                SystemState.machines3[machine3Index].buffer.Enqueue(startTimeDvd);


             
                // if the buffer before machine 3 is full and either of the machines 3 is neither busy or blocked, schedule new M3finished event

                
                if (SystemState.machines3[machine3Index].buffer.Count == SystemState.machines3[machine3Index].bufferSize)
                {
                    if (SystemState.machines3[0].M3State != MachineState.State.blocked &&
                        SystemState.machines3[0].M3State != MachineState.State.busy)
                    {
                        while (SystemState.machines3[machine3Index].buffer.Count != 0)
                        {
                            double transfer = SystemState.machines3[machine3Index].buffer.Dequeue();
                            SystemState.machines3[0].batch.Enqueue(transfer);
                           
                        }

                        SystemState.machines3[0].ScheduleBatchM3Finished();
                        SystemState.machines3[machine3Index].buffer.Clear();
                        SystemState.machines3[0].M3State = MachineState.State.busy;



                        //if M2 was blocked and the buffer before machine 2 was not empty -> schedule new M2 finished event
                        if (SystemState.machines2[machine3Index].M2State == MachineState.State.blocked &&
                            SystemState.machines2[machine3Index].buffer.Count != 0)
                        {
                            double startTimeDvdfromQ = SystemState.machines2[machine3Index].buffer.Dequeue();
                            SystemState.machines2[machine3Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                            SystemState.machines2[machine3Index].M2State = MachineState.State.busy;

                            SystemState.machines2[machine3Index].checkRebootMachines1();
                        }
                    }

                    else if (SystemState.machines3[1].M3State != MachineState.State.blocked &&
                            SystemState.machines3[1].M3State != MachineState.State.busy)
                    {

                        while (SystemState.machines3[machine3Index].buffer.Count != 0)
                        {
                            double transfer = SystemState.machines3[machine3Index].buffer.Dequeue();
                            SystemState.machines3[1].batch.Enqueue(transfer);
                        }


                        SystemState.machines3[1].ScheduleBatchM3Finished();
                        SystemState.machines3[machine3Index].buffer.Clear();
                        SystemState.machines3[1].M3State = MachineState.State.busy;

                        //if M2 was blocked and the buffer before machine 2 was not empty -> schedule new M2 finished event
                        if (SystemState.machines2[machine3Index].M2State == MachineState.State.blocked &&
                            SystemState.machines2[machine3Index].buffer.Count != 0)
                        {
                            double startTimeDvdfromQ = SystemState.machines2[machine3Index].buffer.Dequeue();
                            SystemState.machines2[machine3Index].ScheduleDvdM2Finished(startTimeDvdfromQ);
                            SystemState.machines2[machine3Index].M2State = MachineState.State.busy;

                            SystemState.machines2[machine3Index].checkRebootMachines1();
                        }
                    }
                }
                //if the conveyor is not empty schedule new to buffer 3 event
                if (SystemState.machines2[machine3Index].onConveyor.Count != 0)
                {
                    SystemState.machines3[machine3Index].ScheduleDvdToBuffer3(false);
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
