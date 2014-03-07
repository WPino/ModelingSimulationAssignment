using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Simulation 
{
    public class Event
    {
        private double time;

        private Event next = null;
        private Event previous = null;


        public Event() { }
        public Event(int time)
        {
            this.time = time;
        }
        public double Time
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

        public bool Equals(Event e)
        {
            if (this.time == e.time)
                return true;
            else
                return false;
        }


        // should be overriden by all derived event class
        public virtual void PrintDetails()
        {
            Console.WriteLine("Generic base class event");
        }

        public virtual double CalculateEventTime()
        {
            return -1;
        }

        // should be overriden by every derived event class
        public virtual void HandleEvent()
        {
            Console.WriteLine("generic event was handled");
        }
    }
}
