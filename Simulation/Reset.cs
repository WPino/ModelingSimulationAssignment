using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    static class Reset
    {
        public static void ResetNew()
        {   // time
            GeneralTime.MasterTime = 0;
            //SystemState.R = new Random(seed);
            SystemState.totalDVDFinished = 0;
            SystemState.averageThroughputTime = 0;


            // state
            for (int i = 0; i < 4; i++)
            {
                SystemState.machines1[i].busytime = 0;
                SystemState.machines1[i].idletime = 0;
                SystemState.machines1[i].blockedtime = 0;
                SystemState.machines1[i].brokentime = 0;
                SystemState.machines1[i].M1State = MachineState.State.idle;



                if(i < 2)
                {
                    SystemState.machines2[i].busytime = 0;
                    SystemState.machines2[i].idletime = 0;
                    SystemState.machines2[i].brokentime = 0;
                    SystemState.machines2[i].M2State = MachineState.State.idle;
                    SystemState.machines2[i].buffer3InclConveyorContent = 0;
                    

                    SystemState.machines3[i].busytime = 0;
                    SystemState.machines3[i].idletime = 0;
                    SystemState.machines3[i].blockedtime = 0;
                    SystemState.machines3[i].M3State = MachineState.State.idle;
                    SystemState.machines3[i].batch.Clear();
                    SystemState.machines3[i].buffer.Clear();


                    SystemState.machines4[i].busytime = 0;
                    SystemState.machines4[i].idletime = 0;
                    SystemState.machines4[i].brokentime = 0;
                    SystemState.machines4[i].M4State = MachineState.State.idle;
                    SystemState.machines4[i].buffer.Clear();
                    SystemState.machines4[i].deviation = 0;
                    SystemState.machines4[i].inkCounter = 0;
                }
            }
        }
    }
}
