using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace webApp.Forms
{
    public partial class Prime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnFInd_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(txtNumber.Text);
           
            for (int x = 2; x <= i; x++)
            {
                if (i % x == 0)
                {
                    if (i == x)
                    {
                        Response.Write(i.ToString() + " is a prime number");
                        break;

                    }
                    Response.Write(i.ToString() + " is not a prime number");
                    break; 
                }
               

            }

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("0, 1");
            int x, y, z;
            x = 0;
            y = 1;

            for (int i = 1; i <= 10; i++)
            {
                z = x + y;
                sb.Append(", " + z.ToString());
                x = y;
                y = z;
            }

            Response.Write(sb.ToString());


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
             LinkList obj=new LinkList();
             obj.Add("1");
             obj.Add("2");
             obj.Add("3");
             obj.Add("4");
             obj.Add("5");
             litResult.Text = litResult.Text + "First item =" + obj.first().value.ToString();
             litResult.Text =  litResult.Text + "Last item =" + obj.last().value.ToString();
             litResult.Text = litResult.Text + "All items in forward traverse =" + obj.TraverseFarwared().ToString();   
 
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            LinkList obj = new LinkList();
            litResult.Text =  obj.first().ToString();   
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            LinkList obj = new LinkList();
            litResult.Text = obj.last().ToString();   

        }

        protected void btnTraverse_Click(object sender, EventArgs e)
        {
            LinkList obj = new LinkList();
            litResult.Text = obj.TraverseFarwared();  
        }
    }
}