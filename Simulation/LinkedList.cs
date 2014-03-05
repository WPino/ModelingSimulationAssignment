using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation 
{
    public class LinkedList
    {
        private Event headEvent = null;
        private Event endEvent = null;
        private int length = 0;

        public Event HeadEvent
        {
            get { return headEvent; }
        }
        public Event EndEvent
        {
            get { return endEvent; }
        }

        public int Length
        {
            get { return length; }
        }


        public void Add(Event e)
        {
            if (IsEmpty())
            {
               
                headEvent = e;
                endEvent = e;
                length++;
            }
            else
            {
                headEvent = headEvent.Add(e);
                if (e.Next == null)
                {
                    endEvent = e;
                    endEvent.Next = headEvent;
                    headEvent.Prev = endEvent;
                }
                length++;
            }

        }

        public void ReadFromHead()
        {
            Event head = headEvent;

            if(IsEmpty())
            {
                Console.WriteLine("no items in list");
            }
            else
            {
                Event tmp = headEvent;
                Console.WriteLine(" ======== LINKED LIST ========");
                for(int i = 0; i < length; i++)
                {
                    tmp.PrintDetails();
                    tmp = tmp.Next;
                }
                Console.WriteLine(" =============================");
                
            }
        }

        public void ReadFromRear()
        {
            Event end = endEvent;
            if (IsEmpty())
            {
                Console.WriteLine("no items in list");
            }
            else
            {
                Event tmp = endEvent;
                Console.WriteLine(" ======== LINKED LIST ========");
                for (int i = 0; i < length; i++)
			    {
			        tmp.PrintDetails();
                    tmp = tmp.Prev;
                }
                Console.WriteLine(" =============================");
            }
        }

        public int GetLength()
        {
            return length;
        }

        public bool IsEmpty()
        {
            if (headEvent == null)
                return true;
            else return false;
        }

        public bool Contains(Event e)
        {
            // override : overloaod Equals to check for time and event type
            Event tmp = headEvent;
            for (int i = 0; i < length; i++)
            {
                if (tmp.Equals(e))
                {
                    return true;
                }
                tmp = tmp.Next;
            }

            return false;
        }

        // we want to delete the first node
        public Event Remove()
        {
            Event temp = headEvent;
            headEvent = headEvent.Next;
            headEvent.Prev = endEvent;
            endEvent.Next = headEvent;

            length--;
            return temp;
        }
    }
}
