using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList 
{
    class M1BreaksEvent : Event
    {
        private string eventType = "M1BreaksEvent";
        private int machine1Index;

        // same property as in DvdM1FinishedEvent !!! code repetition
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
        
        public M1BreaksEvent(int index)
        {
            // need to know which machine 1 is broken so pass the index when 
            // scheduling a new M1BreaksEvent;

            M1Index = index;
            

            // method to find time at which this event happens
            // and add to EventList.eventList
        }

        
        // time for a breakDown to be passed into constructor
        // not yet using the exponential distribution.
        private int TimeForBreakDown()
        {
            return 8 * 60 * 60;
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
