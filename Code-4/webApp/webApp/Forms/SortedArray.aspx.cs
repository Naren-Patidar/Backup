using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webApp.Forms
{
    public partial class SortedArray : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NewSortedList myList1 = new NewSortedList();
            myList1.AddItems(4);
            myList1.AddItems(3);
            myList1.AddItems(12);
            myList1.AddItems(18);
            myList1.AddItems(13);
            myList1.AddItems(5);
            string s = string.Empty;  
            foreach (int i in myList1.myList)
            {

                s = s + i.ToString() + ", "; 
            }
            Response.Write(s);  
  
  
            MySortedList list = new MySortedList();
            list.Add(10);
            list.Add(8);
            list.Add(7);
            list.Add(7);
            list.Add(20);
            list.Add(16);
            list.Add(1);
            foreach (int v in list.Items)
            {
                Console.WriteLine(v);
            }
            Console.ReadLine();

        }
        class NewSortedList
        {
            int[] items = new int[0];
            public int[] myList
            {
                get { return items; }
                set { items = value; }

            }
            public void AddItems(int value)
            {
                int index = Array.BinarySearch(myList, value);
                if (index < 0)
                {
                    index = ~index; 
                }
                int lastIndex = myList.Length;
                int capacity = myList.Length + 1;
                int[] dupArray = new int[capacity];
                Array.Copy(myList, dupArray, myList.Length);
                myList = dupArray;

                //--Adjust elements for its right place
                if (index < lastIndex)
                {
                    Array.Copy(items, index, items, index + 1, lastIndex - index);

                }
                items[index] = value;  
            }
        
        }
        class MySortedList
        {
            int[] items = new int[0];
            public int[] Items
            {
                get { return items; }
                set { items = value; }
            }

            public void Add(int value)
            {
                int index = Array.BinarySearch(items, value);
                if (index < 0)
                    index = ~index;

                //Increase Items Array Size by 1
                int lastIndex = items.Length;
                int capacity = items.Length + 1;
                int[] dupArray = new int[capacity];
                Array.Copy(items, dupArray, items.Length);
                items = dupArray;

                //Adjusting elements to insert element in its right index
                if (index < lastIndex)
                {
                    Array.Copy(items, index, items, index + 1, lastIndex - index);
                }
                items[index] = value;
            }
        }
    }
}