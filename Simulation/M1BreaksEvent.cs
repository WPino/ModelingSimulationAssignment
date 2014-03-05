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

        
        // in the constructor we need to schedule a new Repaired event 
        public M1BreaksEvent(int index)
        {
            machine1Index = index;
            this.Time = CalculateEventTime();
            EventList.eventList.Add(this);

        }

        public override void PrintDetails()
        {
            string myState;
            myState = String.Format("Type of event: {0}\nfrom  machine1 : {1}\nTime: {2}",
                eventType, machine1Index, this.Time);
            Console.WriteLine(myState);
            Console.WriteLine();
        }

        public override int CalculateEventTime()
        {
            return GeneralTime.MasterTime + (8 * 60 * 60);
        }

        // when time arrives, state = broken
        public override void HandleEvent()
        {
            base.HandleEvent();
        }
    }
}
