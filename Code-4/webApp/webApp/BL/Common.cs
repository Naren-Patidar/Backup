using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webApp.BL
{
    public class Common
    {
        int[] num = { 1, 2, 3, 4, 8, 2, 6, 9 };
        public int getHighestNumberAsInput(int position)
        {
            int a, b;
            b = 0;
            //--Create temp array
            int[] tempArray = new int[position];
            for (int j = 0; j <= position - 1; j++)
            {
                tempArray[j] = num[j];

            }
            for (int i = position; i <= num.Length - 1; i++)
            {
                prepareArray(tempArray, num[i]);
            }
                return 1;
        }

        public int[] prepareArray(int[] arr, int n)
        {
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                int nArrary = arr[i];
                if (n > nArrary)
                {
                    arr[i] = n;
                    n = nArrary;
                    i = -1;
                    continue;
                }

            }
                return arr;
        }
    }
}