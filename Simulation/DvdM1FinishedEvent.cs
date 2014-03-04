using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    // maybe need a class for all 4 machine ones??
    class DvdM1FinishedEvent : Event
    {
        private int machine1Index;
        private string eventType = "DvdM1FinishedEvent";
        
        public int M1Index
        {
            get { return machine1Index; }
            set
            {
                if (value == 1 || value == 2
                    || value == 3 || value == 4)
                {
                    machine1Index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1{0}", machine1Index));
                }
            }
        }


        public DvdM1FinishedEvent(int index)
	    {
            // the index is used to check from which machine1 (4) is the event schedule
            M1Index = index;

            // set the state of the machine
            AlterMachine1StateToBusy(index);

            // using the "fake" planning method
            this.Time = PlanWhenEventFinished();

            // adding to the event list.


            // FIND A WAY TO LOOK DYNAMICALLY LOOK AT THE STATE OF A MACHINE
            // MAKE THE STATE A PROPERTY OF THE EVENT?
            this.PrintDetails();
            
            EventList.eventList.Add(this);
            Console.WriteLine();
            // method to find time at which this event happens
            // and add to EventList.eventList
	    }


        // I am not using the proper data but random number (just for experimentations)
        public int PlanWhenEventFinished()
        {

            // dont really know how this random number Generator works but it seems to do the job;
            Random rand = new Random();
            int finished = rand.Next(Math.Abs(Guid.NewGuid().GetHashCode()) % 100);
            return finished;
        }

        // assigning the correct index to the machine. Taken from the parameter passed to the object
        public void AlterMachine1StateToBusy(int index)
        {
            switch (index)
            {
                case 1:
                    MachineState.M11State = MachineState.State.busy;
                    break;
                case 2:
                    MachineState.M12State = MachineState.State.busy;
                    break;
                case 3:
                    MachineState.M13State = MachineState.State.busy;
                    break;
                case 4:
                    MachineState.M14State = MachineState.State.busy;
                    break;
                case default:
                    throw new Exception("we do not have that machine");
                    break;
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
