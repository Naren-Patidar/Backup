using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintVouchersAtKiosk
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox2.Checked)
            {
                CheckBox1.Checked = false;
            }

        }
        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                CheckBox2.Checked = false;
            }
       
        }

        protected void Mes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMonth = Mes.SelectedValue.ToString();
            string strYear = Ano.SelectedValue.ToString();
            DateTime dt;
            string strDt;
            if (strMonth != "0" && strYear != "0")
            {
                if (Mes.SelectedValue.ToString().Length == 1)
                    strMonth = "0" + Mes.SelectedValue;

                strDt = "01/" + strMonth +"/" + strYear;
                //DateTime temp = DateTime.Parse("01/01/2009");
                //DateTime.ParseExact(ds.Tables[0].Rows[i][5].ToString(), "dd/MM/yy",null);
                dt = DateTime.ParseExact(strDt, "dd/MM/yyyy", null);
                if (dt <= DateTime.ParseExact("01/11/2011", "dd/MM/yyyy", null))
                {
                    PlaceHolder1.Visible = false;
                    CheckBox1.Checked = false;
                    CheckBox2.Checked = false;
                }
                else
                {
                    PlaceHolder1.Visible = true;
                }
            }

        }

        protected void Ano_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMonth = Mes.SelectedValue.ToString();
            string strYear = Ano.SelectedValue.ToString();
            DateTime dt;
            string strDt;
            if (strMonth != "0" && strYear != "0")
            {
                if (Mes.SelectedValue.ToString().Length == 1)
                    strMonth = "0" + Mes.SelectedValue;

                strDt = "01/" + strMonth + "/" + strYear;
                //DateTime temp = DateTime.Parse("01/01/2009");
                //DateTime.ParseExact(ds.Tables[0].Rows[i][5].ToString(), "dd/MM/yy",null);
                dt = DateTime.ParseExact(strDt, "dd/MM/yyyy", null);
                if (dt <= DateTime.ParseExact("01/11/2011", "dd/MM/yyyy", null))
                {
                    PlaceHolder1.Visible = false;
                    CheckBox1.Checked = false;
                    CheckBox2.Checked = false;
                }
                else
                {
                    PlaceHolder1.Visible = true;
                }
            }
        }
    }
}