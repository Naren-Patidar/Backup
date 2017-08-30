using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webApp.Forms
{
    public class LinkList
    {
        public class Node
        {
            public Node Next = null;
            public object value = null;
            public Node()
            { 
            }
            public Node(object obj)
            {
                value = obj; 
            }
           
        }
        private Node root = null;
        public Node first()
        {
            if (root == null)
                return null;
            else
                return root;
        }
        public Node last()
        {
            Node current=root; 
            if (current == null)
                return null;
            else
            {
                while (current.Next != null)
                {
                    current = current.Next;   
                }
                return current; 
            }
        }
        public void Add(object val)
        {
            Node item = new Node(val);
            if (root == null)
            {
                root = item;
            }
            else
                last().Next = item; 
        }
        public string TraverseFarwared()
        {
            Node item = root;

            if (item == null)
            {  return "list is empty"; }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(item.value.ToString());
                while (item.Next != null)
                {
                    sb.Append(", " + item.Next.value.ToString());
                    item = item.Next;  
                }
                return sb.ToString(); 

            }


        }

    }
}