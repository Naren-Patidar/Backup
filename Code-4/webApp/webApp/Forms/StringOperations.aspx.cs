using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webApp.Forms
{
    public partial class StringOperations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            litOutput.Text = funReverseMyString(txtInput.Text);   
        }

        public static string funReverseMyString(string str)
        {
            string strRev = string.Empty;
            string strFinal = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ')
                {
                    strRev = str[i] + strRev;
                }
                else
                {
                    strRev = strRev + " ";
                    strFinal = strFinal + strRev;
                    strRev = ""; 
                }
                
            }

            if (strRev != string.Empty)
            {
                strFinal = strFinal + strRev;
            }

            return strFinal; 

        }

        protected void btnReversestring_Click(object sender, EventArgs e)
        {
            Response.Write(ReverseTheString("Narendra")); 
        }
        private string ReverseTheString(string s)
        {
            string strRev=string.Empty;  
            if (s.Length > 0)
            {
                int i = 0;
                while (i < s.Length)
                {
                    strRev =  s[i] + strRev ;
                    i++;
                }


            }
            return strRev;

        }

        protected void btnAddDigits_Click(object sender, EventArgs e)
        {
            litResultSumofNumbers.Text = SumTheDigits(txtEnterNumber.Text).ToString();   
        }
        private int SumTheDigits(string no)
        {
            int sum = 0;
            if (no.Length > 0)
            {
                int i = 0;
                
                while (i < no.Length)
                {
                    sum = sum + Convert.ToInt32(no[i].ToString()); 

                    i++;
                }
            }
            return sum; 
            
        }

        protected void btnFIndFactorials_Click(object sender, EventArgs e)
        {
            litResultSumofNumbers.Text = findFactorials(Convert.ToInt32(txtEnterNumber.Text));
        }
        private string findFactorials(int s)
        {
            string strResult = string.Empty;
            if (s > 0)
            {
                
                for (int i = 1; i <= s; i++)
                {
                    if (s % i == 0)
                    {
                        strResult = strResult + i.ToString() + ", "; 
                    }

                }
            }
            return strResult;

        }
    }
}