using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// NEW OVERALL COMMENT

namespace LinkedList 
{
    public class Event
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


        // only checking on time!!!
        public int CompareTo(Event e)
        {
            if (time < e.time)
                return 1;
            else if (time == e.time)
                return 1;
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

        public Event Add(Event newEvent)
        {
            if (this.CompareTo(newEvent) < 0)
            {
                newEvent.Next = this;
                if (this.Prev != null)
                {
                    this.Prev.Next = newEvent;
                    newEvent.Prev = this.Prev;
                }
                this.Prev = newEvent;

                return newEvent;
            }
            else
            {
                if (this.Next != null)
                {
                    this.Next.Add(newEvent);
                }
                else
                {
                    this.Next = newEvent;
                    newEvent.Prev = this;
                }
                return this;
            }
        }

        public virtual void PrintDetails()
        {
            Console.WriteLine("Generic base class event");
        }
    }
}
