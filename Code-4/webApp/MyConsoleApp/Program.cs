using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyConsoleApp
{
    class Program
    {
        public delegate void delmethod();
        public class testMethods
        {
            public static void display()
            {
                Console.WriteLine("Hello");

            }
            public static void show()
            {
                Console.WriteLine("Hi");  

            }
            public void print()
            {
                Console.WriteLine("Say thanks");

            }
        }
        
        static void Main(string[] args)
        {
            //delmethod del = new delmethod(testMethods.display);
            //del();
            //testMethods obj = new testMethods();
            //delmethod d2 = obj.print;
            //delmethod d3 = testMethods.show;

            //d3();
            //d2();
            //Console.ReadLine();
            int k = 1;
            try
            {
                
                while (1 == 1)
                {
                    k++;
                }
            }
            catch (Exception ex)
            { }
            for (int j = 3; j <= 1; j++)
            {

                k = k + j;
            }


            Singelton.SingeltonDesign obj;
            Singelton.SingeltonDesign obj2;
            obj = Singelton.SingeltonDesign.getObject();
            obj2 = Singelton.SingeltonDesign.getObject();
            if (obj.Equals(obj2))
            { }

         
  
           
        }      

    }

    public partial class PartialTestClass
    {
        public void test1()
        {

        }
    }

    public partial class PartialTestClass
    {
        public void test3()
        {

        }
    }
}
