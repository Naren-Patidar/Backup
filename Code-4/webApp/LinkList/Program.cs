using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkList
{
    class Program
    {
        static void Main(string[] args)
        {
            linkList lnklist = new linkList();
            lnklist.printNodes();
            Console.WriteLine();

            lnklist.AddAtLast(12);
            lnklist.AddAtLast("John");
            lnklist.AddAtLast("Peter");
            lnklist.AddAtLast(34);
            lnklist.printNodes();
            Console.WriteLine();

            lnklist.AddAtFirst(55);
            lnklist.printNodes();
            Console.WriteLine();
            Console.ReadKey();
        }
    }

    public class Node
    {
        public Node Next;
        public object data;
    }
    public class linkList
    {
        private Node Head;
        private Node Current;
        public int count;

        public linkList()
        {
            Head = new Node();
            Current = Head;
        }

        public void AddAtLast(object data)
        {
            Node newNode = new Node();
            newNode.data = data;
            Current.Next = newNode;
            Current = newNode; 
        }

        public void AddAtFirst(object data)
        {
            Node newNode = new Node();
            newNode.data = data;
            newNode.Next = Head.Next;
            Head.Next = newNode;           
        }

        public void printNodes()
        {
            Node c = Head;
            while (c.Next != null)
            {
                c = c.Next;
                Console.Write("<");
                Console.Write(c.data.ToString());
                Console.Write(">");
               
            }
        }

    }
}
