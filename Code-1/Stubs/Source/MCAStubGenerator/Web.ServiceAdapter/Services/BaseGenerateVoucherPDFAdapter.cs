namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{

    using PdfSharp.Drawing;
    using PdfSharp.Pdf;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Globalization;
    using System.Resources;
    using System.ServiceModel;
    using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardCouponServices;
    using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.SmartVoucherServices;
    using System.Xml.Serialization;
    using System.Xml;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
    
    public class BaseGenerateVoucherPDFAdapter
    {

        protected XPen borderPen;
        protected double borderWidth = 4.5;
        protected XGraphicsState state;
        protected CouponBackgroundTemplate couponTemplate; 

        protected string sConfiguredClubcard, imgPath, fontLoadPath, strIsAlphaCodeRequired, culture;
        protected string strClubcardVouchers, strBonusvoucher, strTopup;        
     
        protected void InitialPrivateVariables(CouponBackgroundTemplate template)
        {
            try
            {
                this.borderPen = new XPen(XColor.FromArgb(0x5e, 0x76, 0x97), this.borderWidth);
                sConfiguredClubcard = template.ReplaceClubcardPrefix;
                imgPath = template.PrintBGImagePath;
                fontLoadPath = template.FontPath;
                strIsAlphaCodeRequired = template.IsAlphaCodeRequired;
                culture = template.CultureDefaultloc;

                strClubcardVouchers = template.lblstrClubcardVouchers; 
                strBonusvoucher = template.lblstrBonusvoucher; 
                strTopup = template.lblstrTopup; 

                //Voucher Bottom Text Fetch based on Localisation Culture
                strClubcardVouchers = template.lblClubcardVoucher_Culture; 
                strBonusvoucher = template.lblstrBonusvoucher_Culture;
                strTopup = template.lblstrTopup_Culture; 
            }
            catch (Exception ex)
            {
                // Placeholder for logger error 
                throw ex;
            }
        }

        /// <summary>
        /// formats the clubcard number supplied and return it back
        /// masks the clubcard number as per the PCIDSS requirements
        /// </summary>
        /// <param name="cardNumber">clubcard number to be masked</param>
        /// <param name="isMaskReq">if clubcard number is required to be masked</param>
        /// <param name="maskChar">masking character</param>
        /// <returns>formatted clubcard number</returns>
        protected string MasknFormatClubcard(string clubcardNumber, bool isMaskReq, char maskChar)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.MasknFormatClubcard", "Context:clubcardNumber-" + clubcardNumber);
            #region Local variable declaration
            string middleGroup = string.Empty;
            string maskedClubCard = string.Empty;
            #endregion

            try
            {

                if (clubcardNumber.Trim().Length > 15)
                {
                    middleGroup = clubcardNumber.Substring(6, 8);
                    StringBuilder MG = new StringBuilder(clubcardNumber);
                    MG.Replace(middleGroup, " XXXX XXXX ");

                    //return the formatted card number
                    maskedClubCard = MG.ToString();
                }
                //if clubcard number found lesser than 16 digits the unformatted clubcard number is returned
                else
                {
                    maskedClubCard = clubcardNumber;
                }

                return maskedClubCard;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:clubcardNumber-" + clubcardNumber);
                throw;
            }
            finally
            {
                trace.Dispose();
            }
        }

        public static string GetDecimalTrimmedCurrencyVal(string CurrencyVal)
        {
            string formattedVal = CurrencyVal;
            formattedVal = (CurrencyVal.Contains(",") ? CurrencyVal.TrimEnd('0').TrimEnd(',') : CurrencyVal.Contains(".") ? CurrencyVal.TrimEnd('0').TrimEnd('.') : formattedVal);
            formattedVal = formattedVal.Contains(".") ? CurrencyVal : formattedVal.Contains(",") ? CurrencyVal : formattedVal;

            return formattedVal;

        }

        protected static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = new XmlTextWriter(sw);
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(tw, o);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Close();
                tw.Close();
            }
        }
    }
}
