using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    // still needs constructor
    class M1RepairedEvent : Event
    {
        private int machine1Index;
        private string eventType = "M1RepairedEvent";

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

        public M1RepairedEvent(int index)
        {
            // check which machine1 was fixed using the index
           M1Index = index;

            // method to find time at which this event happens
            // and add to EventList.eventList
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
