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

            // using the "fake" planning method
            this.Time = PlanWhenEventFinished();

            // adding to the event list.
            EventList.eventList.Add(this);

            // method to find time at which this event happens
            // and add to EventList.eventList
	    }


        // I am not using the proper data but random number (just for experimentations)
        public int PlanWhenEventFinished()
        {
            Random rand = new Random();
            double finished = rand.NextDouble();
            return (int)(finished * 100);
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
