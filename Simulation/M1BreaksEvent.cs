using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation 
{
    class M1BreaksEvent : Event
    {
        private string eventType = "M1BreaksEvent";
        private int machine1Index;

        
         
        public M1BreaksEvent(int index)
        {
            machine1Index = index;
            this.Time = CalculateEventTime();
            EventList.eventList.Add(this);
        }

        public override double CalculateEventTime()
        {
            return GeneralTime.MasterTime + randomExpDist(8*3600);
        }


        public override void HandleEvent()
        {
            SystemState.machines1[machine1Index].ScheduleRepaired();
            SystemState.machines1[machine1Index].M1State = MachineState.State.broken;
        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nfrom  machine1 : {1}\nTime: {2}",
                eventType, machine1Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }
    }
}
