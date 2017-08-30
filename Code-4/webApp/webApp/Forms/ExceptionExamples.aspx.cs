using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webApp.Forms
{
    public partial class ExceptionExamples : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetErrorTraceWithThrow_Click(object sender, EventArgs e)
        {
            try
            {
                THROW();

            }
            catch (Exception ex)
            {

                litResult.Text = ex.StackTrace;   
            }
        }        

        protected void btnGetErrorTraceWithThrowEX_Click(object sender, EventArgs e)
        {
            try
            {
                THROW_EX();

            }
            catch (Exception ex)
            {

                litResult.Text = ex.StackTrace;
            }

        }

        private void THROW()
        {
            try
            {
                divideByZero();
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        private void THROW_EX()
        {
            try
            {
                divideByZero();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void divideByZero()
        {
            try
            {
                int a = 5;
                int b = 0;
                a = a / b;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      
    }
}