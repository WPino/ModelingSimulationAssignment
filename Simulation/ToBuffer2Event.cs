using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    class ToBuffer2Event : Event
    {
        public ToBuffer2Event()
        {
            // method to find time at which this event happens
            // and add to EventList.eventList
        }

        public override Event Add(Event newEvent)
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
    }
}
