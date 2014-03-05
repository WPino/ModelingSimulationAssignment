using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    // base class
    // we might want to store information about the buffers in those machine classes

    public class Machine
    {
        protected MachineState.State state;
        protected int index;
        protected double[] buffer;


        protected int busyTime;
        protected int idleTime;
        protected int brokenTime;
        protected int stalledTime;
        protected int blockedTime;

        public void IncreaseStateTime(int delta, string state)
        {
            if (state == "busy")
                busyTime += delta;
            if (state == "idle")
                idleTime += delta;
            if (state == "broken")
                brokenTime += delta;
            if (state == "stalled")
                stalledTime += delta;
            if (state == "blocked")
                blockedTime += delta;
        }

        public int busytime { get { return busyTime; } }
        public int idletime { get { return idleTime; } }
        public int brokentime { get { return brokenTime; } }
        public int stalledtime { get { return stalledTime; } }
        public int blockedtime { get { return blockedTime; } }

        public void setBufferSize(int size)
        {
            buffer = new double[size];
        }

        // to be overriden since not all machines hold the same info
        public virtual void MachineDetails()
        {
            Console.WriteLine("base class detail method");
        }
    }

    // derived class
    class Machine1 : Machine 
    {   
        private string type = "machine 1";

        // properties checking state and index
        public MachineState.State M1State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.busy
                    || value == MachineState.State.broken
                    || value == MachineState.State.blocked
                    || value == MachineState.State.idle)
                {
                    state = value;
                }
                else
                {
                    throw new Exception(String.Format("this machine may not have this state:  {0}", value));
                }
            }
        }
        public int M1Index
        {
            get { return index; }
            set
            {
                if (value == 0 || value == 1
                    || value == 2 || value == 3)
                {
                    index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1{0}", index));
                }
            }
        }


        // constructor
        public Machine1(int index)
        {
            this.index = index;
            upTime = 0;
            downTime = 0;
        }


        public void ScheduleBreaksDown()
        {
            M1BreaksEvent breakDown = new M1BreaksEvent(index);
        }

        public void ScheduleRepaired()
        {

        }

        public void ScheduleDvdM1Finished()
        {
            DvdM1FinishedEvent m1Finished = new DvdM1FinishedEvent(index);
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, M1Index, M1State, upTime, downTime);
        }
    }

    // derived class
    class Machine2 : Machine
    {
        private string type = "machine 2";
        public MachineState.State M2State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.blocked)
                {
                    state = value;
                }
                else
                {
                    throw new Exception(String.Format("this machine may not have this state:  {0}", value));
                }
            }
        }
        public int M2Index
        {
            get { return index; }
            set
            {
                if (value == 0 || value == 1)
                {
                    index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1{0}", index));
                }
            }
        }


        public Machine2(int index)
        {
            M2Index = index;
            upTime = 0;
            downTime = 0;
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M2State, upTime, downTime);
        }
    }


    // derived class
    class Machine3 : Machine
    {   
        private string type = "machine 3";
        public MachineState.State M3State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.blocked
                    || value == MachineState.State.stalled)
                {
                    state = value;
                }
                else
                {
                    throw new Exception(String.Format("this machine may not have this state:  {0}", value));
                }
            }
        }
        public int M3Index
        {
            get { return index; }
            set
            {
                if (value == 0 || value == 1)
                {
                    index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1{0}", index));
                }
            }
        }


        public Machine3(int index)
        {
            M3Index = index;
            upTime = 0;
            downTime = 0;
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M3State, upTime, downTime);
        }
    }

    // derived class
    class Machine4 : Machine
    {
        private string type = "machine 4";
        public MachineState.State M4State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.broken)
                {
                    state = value;
                }
                else
                {
                    throw new Exception(String.Format("this machine may not have this state:  {0}", value));
                }
            }
        }
        public int M4Index
        {
            get { return index; }
            set
            {
                if (value == 0 || value == 1)
                {
                    index = value;
                }
                else
                {
                    throw new Exception(String.Format("We do not have a Machine1{0}", index));
                }
            }
        }

        public Machine4(int index)
        {
            M4Index = index;
            upTime = 0;
            downTime = 0;
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M4State, upTime, downTime);
        }
    }
}
