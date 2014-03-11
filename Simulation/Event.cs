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

        protected double randomExpDist(int lambda)
        {
            double y = SystemState.R.NextDouble();
            double x = (Math.Log(1 - y)) * (-lambda);
            return x;
        }

        private static double BoxMuller()
        {
            bool uselast = true;
            double next_gaussian = 0.0;
            
            if (uselast)
            {
                uselast = false;
                return next_gaussian;
            }
            else
            {
                double v1, v2, s;
                do
                {
                    v1 = 2.0 * SystemState.R.NextDouble() - 1.0;
                    v2 = 2.0 * SystemState.R.NextDouble() - 1.0;
                    s = v1 * v1 + v2 * v2;
                } while (s >= 1.0 || s == 0);

                s = System.Math.Sqrt((-2.0 * System.Math.Log(s)) / s);

                next_gaussian = v2 * s;
                uselast = true;
                return v1 * s;
            }
        }

        public static double randomNormDist(double mean, double standard_deviation)
        {
            return mean + BoxMuller() * standard_deviation;
        }
    }
}
