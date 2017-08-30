using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VATTool
{
    public partial class VATTool : Form
    {
        #region Form Level Declaration
        string strCurrentTab = string.Empty;
        VATDataAccess dataAccess = new VATDataAccess();
        #endregion

        public VATTool()
        {
            InitializeComponent();
        }

        #region Events

        #region Form Load
        /// <summary>
        /// Dafault tab is Voucher and the Bincgrid method is called with voucher as input to bind the values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VATTool_Load(object sender, EventArgs e)
        {
            try
            {
                strCurrentTab = "Voucher";
                BindGrid(strCurrentTab, gvVFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region VATtabControl_SelectedIndexChanged
        /// <summary>
        /// When tab is changed, datas to the grid will be loaded based on the current selected tab.
        /// BindGrid metnod is called with selected tab name and grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VATtabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((System.Windows.Forms.TabControl)(sender)).SelectedTab.Text.ToUpper() == "VOUCHER")
                {
                    strCurrentTab = "Voucher";
                    BindGrid(strCurrentTab, gvVFormat);
                }
                else
                {
                    strCurrentTab = "Notification";
                    BindGrid(strCurrentTab, gvNFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Voucher Save
        /// <summary>
        /// Saves entered details to coupon class and coupon line.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtVTillCouponTemplateNoVal.Text))
                {
                    MessageBox.Show("Till Coupon Template Number should not be empty or null. Please enter a valid value");
                }
                else
                {
                    Int64 intCouponClassID = InsertVoucherCouponClassData();

                    string strVValid = InsertLineText(gvVFormat, intCouponClassID);

                    if (strVValid.Trim().Contains("0"))
                    {
                        MessageBox.Show("Invalid Data(s) not saved. There is some invalid data entered, please correct the data.");
                    }
                    else
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Notification Save
        /// Saves entered details to coupon class and coupon line.
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNTillCouponTemplateNoVal.Text))
                {
                    MessageBox.Show("Till Coupon Template Number should not be empty or null. Please enter a valid value");
                }
                else
                {
                    Int64 intCouponClassID = InsertNotificationCouponClassData();

                    string strNValid = InsertLineText(gvNFormat, intCouponClassID);

                    if (strNValid.Trim().Contains("0"))
                    {
                        MessageBox.Show("Invalid Data(s) not saved. There is some invalid data entered, please correct the data.");
                    }
                    else
                    {
                        MessageBox.Show("Data Saved Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Voucher Cancel
        /// <summary>
        /// Reverts back the edited values to the original value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid(strCurrentTab, gvVFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Notification Cancel
        /// Reverts back the edited values to the original value
        private void btnNCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid(strCurrentTab, gvNFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Functions

        #region Default GridView
        /// <summary>
        /// Default grid view to bind the grid on load
        /// </summary>
        private void DefaultGridView(DataGridView defaultGridView)
        {
            DataTable defaultDT = new DataTable();

            // Calling GridViewColumns function with the datatable as input to bind the coulumns to the datatable
            GridViewColumns(defaultDT);

            defaultDT.Rows.Add(" ", "Line 1", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 2", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 3", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 4", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 5", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 6", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 7", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Barcode", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Alpha", " ", " ", " ", " ", " ", " ", " ", " ", " ");
            defaultDT.Rows.Add(" ", "Line 10", " ", " ", " ", " ", " ", " ", " ", " ", " ");

            defaultGridView.AutoGenerateColumns = true;
            defaultGridView.AllowUserToAddRows = false;

            defaultGridView.DataSource = defaultDT;

            // Calling GridViewSettings to set the requires settings of the gridview
            GridViewSettings(defaultGridView);

        }
        #endregion

        # region Grid View Setting
        /// <summary>
        /// // Calling GridViewSettings to set the requires settings of the gridview
        /// </summary>
        /// <param name="gridViewSettings">Gridview name for which settings has to be appliedS</param>
        private void GridViewSettings(DataGridView gridViewSettings)
        {
            gridViewSettings.Width = 790;
            gridViewSettings.Height = 245;

            //gridViewSettings.Columns["TillCouponLineID"].Visible = false;
            gridViewSettings.Columns["CouponClassID"].Visible = false;

            gridViewSettings.Columns["Text"].Width = 250;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Text"]).MaxInputLength = 40;
            gridViewSettings.Columns["LineNumber"].Width = 80;
            gridViewSettings.Columns["LineNumber"].ReadOnly = true;
            gridViewSettings.Columns["Unused"].Width = 50;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Unused"]).MaxInputLength = 1;
            gridViewSettings.Columns["Underline"].Width = 60;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Underline"]).MaxInputLength = 1;
            gridViewSettings.Columns["Italic"].Width = 50;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Italic"]).MaxInputLength = 1;
            gridViewSettings.Columns["W/B"].Width = 50;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["W/B"]).MaxInputLength = 1;
            gridViewSettings.Columns["Centered"].Width = 55;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Centered"]).MaxInputLength = 1;
            gridViewSettings.Columns["BCode"].Width = 50;
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["BCode"]).MaxInputLength = 1;
            gridViewSettings.Columns["Height"].Width = 50;
            //gridViewSettings.Columns["Height"].ValueType = Type.GetType("Int32");
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Height"]).MaxInputLength = 1;
            gridViewSettings.Columns["Weight"].Width = 50;
            //gridViewSettings.Columns["Weight"].ValueType = Type.GetType("Int32");
            ((DataGridViewTextBoxColumn)gridViewSettings.Columns["Weight"]).MaxInputLength = 1;
        }
        #endregion

        #region GridView Columns
        /// <summary>
        /// Column names to be in the display is binded to the data table
        /// </summary>
        /// <param name="dtColumn">Accepts datacolumn to which columns with column names had tobe binded</param>
        private void GridViewColumns(DataTable dtColumn)
        {
            //dtColumn.Columns.Add("TillCouponLineID", Type.GetType("System.String"));
            dtColumn.Columns.Add("CouponClassID", Type.GetType("System.String"));
            dtColumn.Columns.Add("LineNumber", Type.GetType("System.String"));
            dtColumn.Columns.Add("Text", Type.GetType("System.String"));
            dtColumn.Columns.Add("Unused", Type.GetType("System.String"));
            dtColumn.Columns.Add("Underline", Type.GetType("System.String"));
            dtColumn.Columns.Add("Italic", Type.GetType("System.String"));
            dtColumn.Columns.Add("W/B", Type.GetType("System.String"));
            dtColumn.Columns.Add("Centered", Type.GetType("System.String"));
            dtColumn.Columns.Add("BCode", Type.GetType("System.String"));
            dtColumn.Columns.Add("Height", Type.GetType("System.String"));
            dtColumn.Columns.Add("Weight", Type.GetType("System.String"));
        }

        #endregion

        #region Insert Voucher Coupon Class Data
        /// <summary>
        /// Assign the values to the coupon class object and calls InsertCouponClass method with coupon class object as input parameter to save the data
        /// </summary>
        /// <returns>IT returns Coupon class id of the inserted data</returns>
        private Int64 InsertVoucherCouponClassData()
        {
            CouponClass objCouponClass = new CouponClass();
            objCouponClass.TriggerNumber = Convert.ToInt16(lblvTriggerNoVal.Text.Trim());
            objCouponClass.StatementNumber = lblVMailingNoValue.Text.Trim();
            objCouponClass.CouponDescription = lblVCouponDescValue.Text.Trim();
            objCouponClass.CouponImageThumbnail = null;
            objCouponClass.CouponImageFull = null;
            objCouponClass.ThumbnailImageName = null;
            objCouponClass.FullImageName = null;
            objCouponClass.RedemptionEndDate = Convert.ToDateTime(lblVREDVal.Text.Trim());
            objCouponClass.IssuanceStartDate = null;
            objCouponClass.IssuanceStartTime = null;
            objCouponClass.IssuanceEndDate = null;
            objCouponClass.IssuanceEndTime = null;
            objCouponClass.IssuanceChannel = lblVIssuanceChannelVal.Text.Trim();
            objCouponClass.RedemptionChannel = lblVRedemptionChannelVal.Text.Trim();
            objCouponClass.MaxRedemptionLimit = Convert.ToInt16(lblVMaxRedemptionLimitVal.Text.Trim());
            objCouponClass.AlphaCode = null;
            objCouponClass.EANBarcode = null;
            objCouponClass.IsGenerateSmartCodes = false;
            objCouponClass.TillCouponTemplateNumber = txtVTillCouponTemplateNoVal.Text.Trim();

            Int64 intCouponClassID = dataAccess.InsertCouponClass(objCouponClass);

            return intCouponClassID;
        }
        #endregion

        #region Insert Notification Coupon Class Data
        /// <summary>
        /// Assign the values to the coupon class object and calls InsertCouponClass method with coupon class object as input parameter to save the data
        /// </summary>
        /// <returns>IT returns Coupon class id of the inserted data</returns>
        private Int64 InsertNotificationCouponClassData()
        {
            CouponClass objCouponClass = new CouponClass();
            objCouponClass.TriggerNumber = Convert.ToInt16(lblNTriggerNoValue.Text.Trim());
            objCouponClass.StatementNumber = lblNMailingNoValue.Text.Trim();
            objCouponClass.CouponDescription = lblNCouponDescVal.Text.Trim();
            objCouponClass.CouponImageThumbnail = null;
            objCouponClass.CouponImageFull = null;
            objCouponClass.ThumbnailImageName = null;
            objCouponClass.FullImageName = null;
            objCouponClass.RedemptionEndDate = Convert.ToDateTime(lblNRedemptionEndDateVal.Text.Trim());
            objCouponClass.IssuanceStartDate = null;
            objCouponClass.IssuanceStartTime = null;
            objCouponClass.IssuanceEndDate = null;
            objCouponClass.IssuanceEndTime = null;
            objCouponClass.IssuanceChannel = lblNIssuanceChannelVal.Text.Trim();
            objCouponClass.RedemptionChannel = lblNRedemptionChannelVal.Text.Trim();
            objCouponClass.MaxRedemptionLimit = Convert.ToInt16(lblNMaxRedemptionLimitVal.Text.Trim());
            objCouponClass.AlphaCode = null;
            objCouponClass.EANBarcode = null;
            objCouponClass.IsGenerateSmartCodes = false;
            objCouponClass.TillCouponTemplateNumber = txtNTillCouponTemplateNoVal.Text.Trim();

            Int64 intCouponClassID = dataAccess.InsertCouponClass(objCouponClass);

            return intCouponClassID;
        }
        #endregion

        #region InsertLineText
        /// <summary>
        /// Forms coupon line object and calls InsertTillCouponLine method with the coupon line object and couponclassid as intout parameters
        /// </summary>
        /// <param name="gvLineTextData">Grid view on the data which coupon line obeject is generated</param>
        /// <param name="intCouponClassID">Coupon class id to save the line text against the couponclassid</param>
        private string InsertLineText(DataGridView gvLineTextData, Int64 intCouponClassID)
        {
            string strValid = string.Empty;
            for (int i = 0; i < gvLineTextData.Rows.Count; i++)
            {
                CouponLineTextInfo objCLT = new CouponLineTextInfo();

                objCLT.LineText = ((DataGridViewCell)gvLineTextData.Rows[i].Cells["Text"]).Value.ToString().Trim();
                objCLT.LineUsed = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["Unused"]).Value.ToString());
                objCLT.UnderLine = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["Underline"]).Value.ToString());
                objCLT.Italic = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["Italic"]).Value.ToString());
                objCLT.WhiteOnBlack = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["W/B"]).Value.ToString());
                objCLT.Center = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["Centered"]).Value.ToString());
                objCLT.Barcode = formatYN(((DataGridViewCell)gvLineTextData.Rows[i].Cells["BCode"]).Value.ToString());
                string strHeight = ((DataGridViewCell)gvLineTextData.Rows[i].Cells["Height"]).Value.ToString().Trim();
                if (!(string.IsNullOrWhiteSpace(strHeight)))
                {
                    objCLT.CharacterWidth = Convert.ToByte(strHeight);
                }
                else
                {
                    objCLT.CharacterWidth = 0;
                }

                string strWeight = ((DataGridViewCell)gvLineTextData.Rows[i].Cells["Weight"]).Value.ToString().Trim();
                if (!(string.IsNullOrWhiteSpace(strWeight)))
                {
                    objCLT.CharacterWeigth = Convert.ToByte(strWeight);
                }
                else
                {
                    objCLT.CharacterWeigth = 0;
                }
                string strLineNumber = ((DataGridViewCell)gvLineTextData.Rows[i].Cells["LineNumber"]).Value.ToString().Trim();

                #region Set Line No
                switch (strLineNumber)
                {
                    case "Line 1":
                        objCLT.LineNumber = "1";
                        break;
                    case "Line 2":
                        objCLT.LineNumber = "2";
                        break;
                    case "Line 3":
                        objCLT.LineNumber = "3";
                        break;
                    case "Line 4":
                        objCLT.LineNumber = "4";
                        break;
                    case "Line 5":
                        objCLT.LineNumber = "5";
                        break;
                    case "Line 6":
                        objCLT.LineNumber = "6";
                        break;
                    case "Line 7":
                        objCLT.LineNumber = "7";
                        break;
                    case "Barcode":
                        objCLT.LineNumber = "8";
                        break;
                    case "Alpha":
                        objCLT.LineNumber = "9";
                        break;
                    case "Line 10":
                        objCLT.LineNumber = "10";
                        break;
                }
                #endregion

                if (objCLT.ValidateCouponLine())
                {
                    dataAccess.InsertTillCouponLine(objCLT, intCouponClassID);
                    strValid = strValid + "1";
                }
                else
                {
                    strValid = strValid + "0";
                }
            }
            return strValid;
        }


        private char formatYN(string strVal)
        {
            char charVal = Convert.ToChar("N");

            if (!(string.IsNullOrWhiteSpace(strVal)))
            {
                charVal = Convert.ToChar(strVal);
            }

            return charVal;
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// It retrives coupon class and coupon line details by calling GetVAT method with mailing number and trigger number and bunds the grid with the retrieved data
        /// </summary>
        /// <param name="selectedTab">Currently selected tab (Voucher/Notification)</param>
        /// <param name="dataGridView">Gridview name on the current tab</param>
        private void BindGrid(string selectedTab, DataGridView dataGridView)
        {
            DataSet dsVAT = new DataSet();
            VATDataAccess dataAccess = new VATDataAccess();

            if (selectedTab == "Voucher")
            {
                dsVAT = dataAccess.GetVAT(lblVMailingNoValue.Text.Trim(), Convert.ToInt32(lblvTriggerNoVal.Text.Trim()));
            }
            else
            {
                dsVAT = dataAccess.GetVAT(lblNMailingNoValue.Text.Trim(), Convert.ToInt32(lblNTriggerNoValue.Text.Trim()));
            }

            if (dsVAT.Tables.Count != null)
            {
                if (dsVAT.Tables[1].Rows.Count > 0)
                {
                    DataTable defaultDT = new DataTable();

                    GridViewColumns(defaultDT);
                    int countRows = dsVAT.Tables[1].Rows.Count, counter = 0;

                    var dAe = dsVAT.Tables[1].AsEnumerable();
                    for (int i = 1; i < 11; i++)
                    {
                        DataRow dRw = defaultDT.NewRow();
                        DataRow dr = null;

                        if (counter < countRows)
                            dr = dsVAT.Tables[1].Rows[counter];

                        string stt, st = "Line " + i.ToString();
                        switch (i)
                        {
                            case 8: stt = "Barcode"; break;
                            case 9: stt = "Alpha"; break;
                            default: stt = "Line " + i.ToString(); break;
                        }
                        if (dAe.Where(c => c.Field<string>("LineNumber") == st).Count() > 0)
                        {
                            dRw["CouponClassId"] = dr["CouponClassId"];

                            dRw["LineNumber"] = stt;
                            dRw["Text"] = dr["Text"];
                            dRw["Unused"] = dr["Unused"];
                            dRw["UnderLine"] = dr["UnderLine"];
                            dRw["Italic"] = dr["Italic"];
                            dRw["W/B"] = dr["W/B"];
                            dRw["Centered"] = dr["Centered"];
                            dRw["BCode"] = dr["BCode"];
                            dRw["Height"] = dr["Height"];
                            dRw["Weight"] = dr["Weight"];
                            counter++;
                        }
                        else
                        {
                            dRw["CouponClassId"] = dsVAT.Tables[0].Rows[0]["CouponClassId"];
                            dRw["LineNumber"] = stt;
                            dRw["Text"] = "";
                            dRw["Unused"] = "";
                            dRw["UnderLine"] = "";
                            dRw["Italic"] = "";
                            dRw["W/B"] = "";
                            dRw["Centered"] = "";
                            dRw["BCode"] = "";
                            dRw["Height"] = "";
                            dRw["Weight"] = "";
                        }

                        defaultDT.Rows.Add(dRw);
                    }


                    //foreach (DataRow dr in dsVAT.Tables[1].Rows)
                    //{
                    //    DataRow dRw = defaultDT.NewRow();

                    //    dRw["CouponClassId"] = dr["CouponClassId"];
                    //    dRw["LineNumber"] = dr["LineNumber"];
                    //    dRw["Text"] = dr["Text"];
                    //    dRw["Unused"] = dr["Unused"];
                    //    dRw["UnderLine"] = dr["UnderLine"];
                    //    dRw["Italic"] = dr["Italic"];
                    //    dRw["W/B"] = dr["W/B"];
                    //    dRw["Centered"] = dr["Centered"];
                    //    dRw["BCode"] = dr["BCode"];
                    //    dRw["Height"] = dr["Height"];
                    //    dRw["Weight"] = dr["Weight"];

                    //    defaultDT.Rows.Add(dRw);
                    //}

                    dataGridView.AutoGenerateColumns = true;
                    dataGridView.AllowUserToAddRows = false;
                    dataGridView.DataSource = defaultDT;

                    GridViewSettings(dataGridView);
                }
                else
                {
                    DefaultGridView(dataGridView);
                }
            }
            else
            {
                DefaultGridView(dataGridView);
            }
        }
        #endregion

        #endregion

    }
}
