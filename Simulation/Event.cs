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



        // DISTIBUTIONS
        protected double randomExpDist(double lambda)
        {
            double y = SystemState.R.NextDouble();
            double x = -(Math.Log(y)) * (lambda);
            return x;
        }

        private static double BoxMuller()
        {
            double v1, v2, s;
            do
            {
                v1 = 2.0 * SystemState.R.NextDouble() - 1.0;
                v2 = 2.0 * SystemState.R.NextDouble() - 1.0;
                s = v1 * v1 + v2 * v2;
            }while (s >= 1.0 || s == 0);

            s = System.Math.Sqrt((-2.0 * Math.Log(s)) / s);

            return v1 * s;
        }

        public static double randomNormDist(double mean, double standard_deviation)
        {
            return mean + BoxMuller() * standard_deviation;
        }

        // proctimes DC follows a gamma distribution I believe
        public double GammaDistribution(double shape, double scale)
        {
            bool foundValueToReturn = false;
            
            if(shape > 0 && shape < 1)
            {
                double b = (Math.E + shape) / Math.E;

                while(foundValueToReturn == false)
                {
                    // step 1
                    double u1 = SystemState.R.NextDouble();
                    double p = b * u1;
                    if(p > 1)
                    {
                        // step 3
                        double y = -Math.Log((b - p) / shape);
                        double u2 = SystemState.R.NextDouble();
                        if(u2 <= Math.Pow(y, (shape-1)))
                        {
                            foundValueToReturn = true;
                            return y * scale;
                        }
                    }
                    else
                    {
                        // step 2
                        double y = Math.Pow(p, (1 / shape));
                        double u2 = SystemState.R.NextDouble();
                        if(u2 <= Math.Exp(-y))
                        {
                            foundValueToReturn = true;
                            return y * scale;
                        }
                    }
                }
            }
            
            else
            {
                double a = 1 / (Math.Sqrt(2 * shape - 1));
                double b = shape - Math.Log(4);
                double q = shape + 1 / a;
                double theta = 4.5;
                double d = 1 + Math.Log(theta);

                while (foundValueToReturn == false)
                {
                    //step 1
                    double u1 = SystemState.R.NextDouble();
                    double u2 = SystemState.R.NextDouble();

                    //step 2
                    double v = a * Math.Log(u1 / (1 - u1));
                    double y = shape * Math.Exp(v);
                    double z = u1 * u1 * u2;
                    double w = b + q * v - y;

                    // step 3
                    if (w + d - theta * z >= 0)
                    {
                        foundValueToReturn = true;
                        return y * scale;
                    }
                        //step 4
                    else
                    {
                        if (w >= Math.Log(z))
                        {
                            foundValueToReturn = true;
                            return y * scale;
                        }
                    }
                }
            }
            // make sure in the function call to check if the value is not -1, done
            return -1;
        }
        
    }
}
