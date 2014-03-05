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



        public DvdM1FinishedEvent(int index)
	    {
            machine1Index = index;
            

            
            // method calculating the time when the event will occur
            this.Time = CalculateEventTime();
            
            // adding event to the linkedlist
            EventList.eventList.Add(this);
            
	    }


        // I am not using the proper data but random number (just for experimentations)
        public int PlanWhenEventFinished()
        {

            // dont really know how this random number Generator works but it seems to do the job;
            Random rand = new Random();
            int finished = rand.Next(Math.Abs(Guid.NewGuid().GetHashCode()) % 100);
            return finished;
        }

        // calculate when new Event of type DvdFinishedEvent will happen
        public override int CalculateEventTime()
        {
            // read from processing times (exponential distribution)
            // for now we use the random number from the fake method
            return PlanWhenEventFinished();
        }


        public override void HandleEvent()
        {
            // check if one of the machine is idle
            
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
