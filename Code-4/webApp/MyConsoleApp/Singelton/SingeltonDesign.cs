using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyConsoleApp.Singelton
{
    public class SingeltonDesign
    {
        //-- Private constructor
        //--Static instance

        private SingeltonDesign()
        {
        }

        private static SingeltonDesign instance = null;

        public static SingeltonDesign getObject()
        {
            if (instance == null)
            {
                instance = new SingeltonDesign(); 
            }
            return instance; 
        }
 
    }
}
