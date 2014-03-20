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
        public Queue<double> buffer = new Queue<double>(); //nicer way to do this without setting to public?
        


        protected double busyTime = 0;
        protected double idleTime = 0;
        protected double brokenTime = 0;
        protected double blockedTime = 0;


        // does not work
        public void IncreaseStateTime(double delta, MachineState.State state)
        {
            if (state == MachineState.State.busy)
            {
                busyTime += delta;
            }
            if (state == MachineState.State.idle)
            {
                idleTime += delta;
            }
            if (state == MachineState.State.broken)
            {
                brokenTime += delta;
            }
            if (state == MachineState.State.blocked)
            {
                blockedTime += delta;
            }
        }

        public double busytime { get { return busyTime; } }
        public double idletime { get { return idleTime; } }
        public double brokentime { get { return brokenTime; } }
        public double blockedtime { get { return blockedTime; } }

        public int bufferSize { get; set; }

        /*public void setBufferSize(int size)
        {
            buffer = new double[size];
        }*/

        // to be overriden since not all machines hold the same info
        public virtual void MachineDetails()
        {
            Console.WriteLine("base class detail method");
        }
    }

    // derived class
    public class Machine1 : Machine 
    {   
        private string type = "machine 1";
        private double lastStateChange = 0;

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
                    MachineState.State oldstate = state;

                    double recentStateChange = GeneralTime.MasterTime;
                    double deltaStateChange = recentStateChange - lastStateChange;
                    lastStateChange = recentStateChange;

                    switch (oldstate)
                    {
                        case MachineState.State.busy:
                            IncreaseStateTime(deltaStateChange, MachineState.State.busy);
                            break;
                        case MachineState.State.broken:
                            IncreaseStateTime(deltaStateChange, MachineState.State.broken);
                            break;
                        case MachineState.State.blocked:
                            IncreaseStateTime(deltaStateChange, MachineState.State.blocked);
                            break;
                        case MachineState.State.idle:
                            IncreaseStateTime(deltaStateChange, MachineState.State.idle);
                            break;
                        default:
                            break;
                    }
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
            M1Index = index;
            busyTime = 0;
            idleTime = 0;
            brokenTime = 0;
            blockedTime = 0;
        }


        public void ScheduleBreaksDown()
        {
            M1BreaksEvent breakDown = new M1BreaksEvent(M1Index);
        }

        public void ScheduleRepaired()
        {
            M1RepairedEvent repaired = new M1RepairedEvent(M1Index);
        }

        public void ScheduleDvdM1Finished(double startTimeDvd)
        {
            DvdM1FinishedEvent m1Finished = new DvdM1FinishedEvent(M1Index, startTimeDvd);
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, M1Index, M1State, busyTime, idleTime + brokenTime + blockedTime);
        }
    }

    // derived class
    public class Machine2 : Machine
    {
        private string type = "machine 2";
        private double lastStateChange = 0;
        public int buffer3InclConveyorContent { get; set; }

        public MachineState.State M2State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.blocked)
                {
                    //state = value;

                    MachineState.State oldState = state;

                    double recentStateChange = GeneralTime.MasterTime;
                    double deltaStateChange = recentStateChange - lastStateChange;
                    lastStateChange = recentStateChange;

                    switch (oldState)
                    {
                        case MachineState.State.busy:
                            //busyTime += deltaStateChange;
                            IncreaseStateTime(deltaStateChange, MachineState.State.busy);
                            break;
                        case MachineState.State.blocked:
                            //blockedTime += deltaStateChange;
                            IncreaseStateTime(deltaStateChange, MachineState.State.blocked);
                            break;
                        case MachineState.State.idle:
                            IncreaseStateTime(deltaStateChange, MachineState.State.idle);
                            break;
                        default:
                            break;
                    }
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
            busyTime = 0;
            idleTime = 0;
            brokenTime = 0;
            blockedTime = 0;
        }

        public void ScheduleDvdM2Finished(double startTimeDvd)
        {
            DvdM2FinishedEvent m2Finished = new DvdM2FinishedEvent(M2Index, startTimeDvd);
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M2State, busyTime, idleTime + brokenTime + blockedTime);
        }
    }


    // derived class
    public class Machine3 : Machine
    {   
        private string type = "machine 3";
        private double lastStateChange = 0;

        public MachineState.State M3State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.blocked)
                {
                    //state = value;

                    MachineState.State oldState = state;

                    double recentStateChange = GeneralTime.MasterTime;
                    double deltaStateChange = recentStateChange - lastStateChange;
                    lastStateChange = recentStateChange;

                    switch (oldState)
                    {
                        case MachineState.State.busy:
                            IncreaseStateTime(deltaStateChange, MachineState.State.busy);
                            break;
                        case MachineState.State.blocked:
                            IncreaseStateTime(deltaStateChange, MachineState.State.blocked);
                            break;
                        case MachineState.State.idle:
                            IncreaseStateTime(deltaStateChange, MachineState.State.idle);
                            break;
                        default:
                            break;
                    }
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
            busyTime = 0;
            idleTime = 0;
            brokenTime = 0;
            blockedTime = 0;
        }


        public void ScheduleDvdToBuffer3(double startTimeDvd)
        {
            ToBuffer3Event toBuffer3 = new ToBuffer3Event(M3Index, startTimeDvd);
        }

        public void ScheduleBatchM3Finished(Queue<double> batch)
        {
            BatchM3FinishedEvent batchFinished = new BatchM3FinishedEvent(M3Index, batch);
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M3State, busyTime, idleTime + brokenTime + blockedTime);
        }
    }

    // derived class
    public class Machine4 : Machine
    {
        private string type = "machine 4";
        private double lastStateChange = 0;
        public int inkCounter { get; set; }
        public int deviation { get; set; }

        public MachineState.State M4State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.broken)
                {
                    //state = value;

                    MachineState.State oldState = state;

                    double recentStateChange = GeneralTime.MasterTime;
                    double deltaStateChange = recentStateChange - lastStateChange;
                    lastStateChange = recentStateChange;

                    switch (oldState)
                    {
                        case MachineState.State.busy:
                            IncreaseStateTime(deltaStateChange, MachineState.State.busy);
                            break;
                        case MachineState.State.blocked:
                            IncreaseStateTime(deltaStateChange, MachineState.State.blocked);
                            break;
                        case MachineState.State.idle:
                            IncreaseStateTime(deltaStateChange, MachineState.State.idle);
                            break;;
                        default:
                            break;
                    }
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
            busyTime = 0;
            idleTime = 0;
            brokenTime = 0;
            blockedTime = 0;
            inkCounter = 0;

            calculateDeviation();
        }

        public void ScheduleM4NewInk()
        {
            M4NewInkEvent M4NewInk = new M4NewInkEvent(M4Index);
        }

        public void ScheduleDvdM4Finished(double startTimeDvd)
        {
            DvdM4Finished dvdM4Finished = new DvdM4Finished(M4Index, startTimeDvd);
        }

        public void calculateDeviation()
        {
            Random R = new Random();
            int value = R.Next(10);

            switch (value)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    deviation = 0;
                    break;
                case 4:
                case 5:
                    deviation = 1;
                    break;
                case 6:
                case 7:
                    deviation = -1;
                    break;
                case 8:
                    deviation = 2;
                    break;
                default:
                    deviation = -2;
                    break;
            }
        }

        public override void MachineDetails()
        {
            Console.WriteLine("type: {0}\nindex: {1}\nstate: {2}\nupTime: {3}\ndownTime: {4}",
                type, index, M4State, busyTime, idleTime + brokenTime + blockedTime);
        }
    }
}
