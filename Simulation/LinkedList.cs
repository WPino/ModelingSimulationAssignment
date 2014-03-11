using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation 
{
    public class LinkedList
    {
        /*
        private Event headEvent = null;
        private Event current = null;
        private Event prev = null;
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

        public Event Current
        {
            get { return current; }
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
                length++;
            }
            else
            {
                bool lastEvent = false;
                Event temp = headEvent;
                while (e.Time - temp.Time >= 0)
                {
                    if (temp.Next != null)
                    {
                        temp = temp.Next;
                    }
                    else
                    {
                        temp.Next = e;
                        e.Prev = temp;
                        lastEvent = true;
                        break;
                    }
                }

                if (lastEvent != true)
                {
                    if (temp == headEvent)
                    {
                        headEvent = e;
                        e.Next = temp;
                        temp.Prev = e;
                    }
                    else
                    {
                        if (temp.Prev != null)
                        {
                            temp.Prev.Next = e;
                            e.Next = temp;
                            temp.Prev = e;
                        }
                    }
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
                Console.WriteLine(" ======== LINKED LIST ========");
                head.PrintDetails();
                
                while (head.Next != null)
                {
                    head = head.Next;
                    head.PrintDetails();
                }
                Console.WriteLine(" =============================");
                Console.WriteLine();
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

        public Event Remove()
        {
            Event temp = headEvent;
            headEvent = headEvent.Next;
            headEvent.Prev = null;


            length--;
            return temp;
        }
        */ 
    }
    
}
