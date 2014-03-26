using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    //base class
    public class Machine
    {
        protected MachineState.State state;
        protected int index;
        public Queue<double> buffer = new Queue<double>();
        
        protected double busyTime = 0;
        protected double idleTime = 0;
        protected double brokenTime = 0;
        protected double blockedTime = 0;

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

        public void addToBuffer(double startTimeDvd)
        {
            this.buffer.Enqueue(startTimeDvd);
            if (buffer.Count > bufferSize)
            {
                throw new Exception("buffer overflow");
            }
        }

        // to be overriden since not all machines hold the same info
        public virtual void MachineDetails()
        {
            Console.WriteLine("base class detail method");
        }


        public void ReadQueue(Queue<double> queue)
        {
            Console.WriteLine("========");
            for (int i = 0; i < queue.Count; i++)
            {
                Console.WriteLine(queue.Dequeue());
            }
            Console.WriteLine("========");
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

                    state = value;

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

        public void checkRebootMachines1()
        {
            int nr1machine1, nr2machine1;
            if (M2Index == 0)
            {
                nr1machine1 = 0;
                nr2machine1 = 1;
            }
            else
            {
                nr1machine1 = 2;
                nr2machine1 = 3;
            }

            if ((SystemState.machines2[M2Index].buffer.Count < SystemState.machines2[M2Index].bufferSize))
            {
                // if the buffer before machine 2 is full except for one (or except for 2) and one machine is busy (or 2) then do nothing
                if (!(SystemState.machines2[M2Index].buffer.Count == SystemState.machines2[M2Index].bufferSize - 1 &&
                    (SystemState.machines1[nr1machine1].M1State == MachineState.State.busy
                    || SystemState.machines1[nr2machine1].M1State == MachineState.State.busy)))
                {
                    if (!(SystemState.machines2[M2Index].buffer.Count == SystemState.machines2[M2Index].bufferSize - 2 &&
                    (SystemState.machines1[nr1machine1].M1State == MachineState.State.busy
                    && SystemState.machines1[nr2machine1].M1State == MachineState.State.busy)))
                    {
                        if (SystemState.machines1[nr1machine1].M1State != MachineState.State.busy &&
                            SystemState.machines1[nr1machine1].M1State != MachineState.State.broken)
                        {
                            SystemState.machines1[nr1machine1].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                            SystemState.machines1[nr1machine1].M1State = MachineState.State.busy;
                        }
                        if (SystemState.machines1[nr2machine1].M1State != MachineState.State.busy &&
                            SystemState.machines1[nr2machine1].M1State != MachineState.State.broken)
                        {
                            SystemState.machines1[nr2machine1].ScheduleDvdM1Finished(GeneralTime.MasterTime);
                            SystemState.machines1[nr2machine1].M1State = MachineState.State.busy;
                        }
                    }
                }
            }
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
        public Queue<double> batch = new Queue<double>();

        public MachineState.State M3State
        {
            get { return state; }
            set
            {
                if (value == MachineState.State.idle
                    || value == MachineState.State.busy
                    || value == MachineState.State.blocked)
                {

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


        public void ScheduleDvdToBuffer3(double startTime)
        {
            ToBuffer3Event toBuffer3 = new ToBuffer3Event(M3Index, startTime);
        }

        public void ScheduleBatchM3Finished()
        {
            BatchM3FinishedEvent batchFinished = new BatchM3FinishedEvent(M3Index);
        }

        public void checkRebootMachine3()
        {
            if (SystemState.machines3[M3Index].M3State != MachineState.State.blocked &&
                        SystemState.machines3[M3Index].M3State != MachineState.State.busy)
            {
                bool firstSuccesfull = false;

                if (SystemState.machines3[0].buffer.Count == SystemState.machines3[M3Index].bufferSize)
                {
                    firstSuccesfull = true;

                    while (SystemState.machines3[0].buffer.Count != 0)
                    {
                        double transfer = SystemState.machines3[0].buffer.Dequeue();
                        SystemState.machines3[M3Index].batch.Enqueue(transfer);
                    }
                    
                    SystemState.machines3[M3Index].ScheduleBatchM3Finished();
                    SystemState.machines3[M3Index].M3State = MachineState.State.busy;
                    SystemState.machines2[0].buffer3InclConveyorContent = 0;

                    //if M2 was blocked and the buffer before machine 2 was not empty -> schedule new M2 finished event
                    if (SystemState.machines2[0].M2State == MachineState.State.blocked)
                    {
                        if (SystemState.machines2[0].buffer.Count != 0 )
                        {
                            double startTimeDvdfromQ = SystemState.machines2[0].buffer.Dequeue();
                            SystemState.machines2[0].ScheduleDvdM2Finished(startTimeDvdfromQ);
                            SystemState.machines2[0].M2State = MachineState.State.busy;

                            SystemState.machines2[0].checkRebootMachines1();
                        }
                        else
                        {
                            SystemState.machines2[0].M2State = MachineState.State.idle;
                        }
                    }
                }

                if (!firstSuccesfull && SystemState.machines3[1].buffer.Count == SystemState.machines3[M3Index].bufferSize)
                {
                    while (SystemState.machines3[1].buffer.Count != 0)
                    {
                        double transfer = SystemState.machines3[1].buffer.Dequeue();
                        SystemState.machines3[M3Index].batch.Enqueue(transfer);
                    }
                    
                    SystemState.machines3[M3Index].ScheduleBatchM3Finished();
                    SystemState.machines3[M3Index].M3State = MachineState.State.busy;
                    SystemState.machines2[1].buffer3InclConveyorContent = 0;

                    //if M2 was blocked and the buffer before machine 2 was not empty -> schedule new M2 finished event
                    if (SystemState.machines2[1].M2State == MachineState.State.blocked)
                    {
                        if (SystemState.machines2[1].buffer.Count != 0 )
                        {
                            double startTimeDvdfromQ = SystemState.machines2[1].buffer.Dequeue();
                            SystemState.machines2[1].ScheduleDvdM2Finished(startTimeDvdfromQ);
                            SystemState.machines2[1].M2State = MachineState.State.busy;

                            SystemState.machines2[1].checkRebootMachines1();
                        }
                        else
                        {
                            SystemState.machines2[1].M2State = MachineState.State.idle;
                        }
                    }
                }
            }
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
                    MachineState.State oldState = state;

                    double recentStateChange = GeneralTime.MasterTime;
                    double deltaStateChange = recentStateChange - lastStateChange;
                    lastStateChange = recentStateChange;

                    switch (oldState)
                    {
                        case MachineState.State.busy:
                            IncreaseStateTime(deltaStateChange, MachineState.State.busy);
                            break;
                        case MachineState.State.broken:
                            IncreaseStateTime(deltaStateChange, MachineState.State.broken);
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
