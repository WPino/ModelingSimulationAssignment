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

        //public Event Add(Event newEvent)
        //{
        //    if (this.Time - newEvent.Time >= 0)
        //    {
        //        newEvent.Next = this;
        //        if (this.Prev != null)
        //        {
        //            this.Prev.Next = newEvent;
        //            newEvent.Prev = this.Prev;
        //        }
        //        this.Prev = newEvent;

        //        return newEvent;
        //    }
        //    else
        //    {
        //        if (this.Next != null)
        //        {
        //            this.Next.Add(newEvent);
        //        }
        //        else
        //        {
        //            this.Next = newEvent;
        //            newEvent.Prev = this;
        //        }
        //        return this;
        //    }
        //}

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

        protected double randomExpDist(int lambda)
        {
            Random R = new Random();
            double y = R.NextDouble();
            double x = (Math.Log(1 - y)) * (-lambda);
            return x;
        }
    }
}
