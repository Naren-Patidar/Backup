using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elevator
{
    class Program
    {
        static void Main(string[] args)
        {
            //--Uncomment below two lines to execute the elevator program
            //elevator obj = new elevator();
            //obj.getInputFromUser();  
            arrayOperationTogetSum obj = new arrayOperationTogetSum();
            obj.getElement();
            Console.WriteLine("Please enter the floor number to visit.");

        }
    }


    public class elevator
    {
        int iChoice;       
        int iCurrentPossition = 0;

        public void getInputFromUser()
        {
            Console.WriteLine("Please enter the floor number to visit.");
            if (int.TryParse(Console.ReadLine(), out iChoice))
            {
                elevatorAction(iChoice);
            }
            else 
            {
                Console.WriteLine("Please enter valid input (int value)");
                getInputFromUser();
            }
        }
        private void moveUp(int idestinationFloor)
        {
            for (int i = iCurrentPossition; i <= idestinationFloor; i++)
            {
                Console.WriteLine("you are on " + i + " floor");
                if (i == idestinationFloor)
                {

                    Console.WriteLine("have a good day. bye bye");
                    iCurrentPossition = idestinationFloor;
                    idestinationFloor = 0;
                    break; 
                }
            }
            getInputFromUser();

               
        }
        private void moveDown(int idestinationFloor)
        {
            for (int i = iCurrentPossition; i >= idestinationFloor; i--)
            {
                Console.WriteLine("you are on " + i + " floor");
                if (i == idestinationFloor)
                {

                    Console.WriteLine("have a good day. bye bye");
                    iCurrentPossition = idestinationFloor;
                    idestinationFloor = 0;
                    break;
                }
            }
            getInputFromUser();
        }
        private void elevatorAction(int iDestinationFloor)
        {
            if (iDestinationFloor > iCurrentPossition)
            {
                moveUp(iDestinationFloor);
            }
            else if (iDestinationFloor < iCurrentPossition)
            {
                moveDown(iDestinationFloor);
            }           
        }

    }

    public class arrayOperationTogetSum
    {
        //-Objective : write a program to get the element in the array whose left and right side element sum is equal.
        int leftSide_sumOfElements;
        int rightSide_sumOfElements;
        int rootElement;
        int[] arr = { 5, 6, 7, 8, 18 };

        public void getElement()
        {
            leftSide_sumOfElements = arr[0];
            rootElement = arr[1];
            for (int i = 2; i < arr.Length; i++)
            {
                rightSide_sumOfElements = rightSide_sumOfElements + arr[i];
            }

            for (int i = 1; i < arr.Length; i++)
            {

                if (leftSide_sumOfElements == rightSide_sumOfElements)
                {
                    Console.WriteLine("Element from where the sum of element of left and right side are equal : " +rootElement.ToString());
                    Console.ReadLine();
                    break;
                }
                else
                {
                    rightSide_sumOfElements = rightSide_sumOfElements - arr[i + 1];
                    leftSide_sumOfElements = leftSide_sumOfElements + arr[i];
                    rootElement = arr[i + 1];

                    Console.WriteLine("Sum of left side element =" + leftSide_sumOfElements);
                    Console.WriteLine("Root element =" + rootElement);
                    Console.WriteLine("Sum of right side element =" + rightSide_sumOfElements);
                    Console.WriteLine("--------------------------------------");
                    continue;
                }
            }



        }
 

    }

}
