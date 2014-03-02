using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// random comment

namespace LinkedList 
{
    public abstract class Event
    {
        private int time;

        private Event next = null;
        private Event previous = null;


        public Event() { }
        public Event(int time)
        {
            this.time = time;
        }
        public int Time
        {
            get { return this.time; }
            set { this.time = value; }
        }
        public Event Next
        {
            get { return this.next; }
            set { next = value; }
        }
        public Event Prev
        {
            get { return this.previous; }
            set { previous = value; }
        }

        public int CompareTo(Event e)
        {
            if (time < e.time)
                return 1;
            else if (time == e.time)
                return 0;
            else
                return -1;
        }

        public bool Equals(Event e)
        {
            if (this.time == e.time)
                return true;
            else
                return false;
        }


        public abstract Event Add(Event newEvent);
            
    }
}
