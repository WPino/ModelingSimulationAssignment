using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList 
{
    class M1BreaksEvent : Event
    {
        public M1BreaksEvent()
        {
            // method to find time at which this event happens
            // and add to EventList.eventList
        }

        
        // time for a breakDown to be passed into constructor
        // not yet using the exponential distribution.
        private int TimeForBreakDown()
        {
            return 8 * 60 * 60;
        }
    }
}
