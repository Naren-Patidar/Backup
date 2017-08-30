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
    
    using System.Xml.Serialization;
    using System.Xml;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
    using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
    using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

    public class GenerateVouchersPDFAdapter : BaseGenerateVoucherPDFAdapter,IGeneratePDFForMLS, IGeneratePDFForExchange, IGeneratePDFForCoupons, IGeneratePDFForVouchers, IGeneratePdfForClubcard
    {
        string dateformat = AppConfiguration.Settings[AppConfigEnum.DisplayDateFormat];
        string fontName = AppConfiguration.Settings[AppConfigEnum.FontName];
       
        public GenerateVouchersPDFAdapter()
        {
        }

        private new void InitialPrivateVariables(CouponBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.InitialPrivateVariables", "Context: " + GeneralUtility.GetXMLFromObject(template)); 
           
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
                trace.NoteException(ex, "Context: " + GeneralUtility.GetXMLFromObject(template));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

        public void EndBox(XGraphics gfx)
        {
            gfx.Restore(this.state);
        }

        public void imageDraw(Image gif, PdfDocument doc, XGraphics gfx, int x, int y)
        {
            XImage image = gif;
            gfx.DrawImage(image, x, y);
        }

        public void imageDraw(string gif, PdfDocument doc, XGraphics gfx, int x, int y)
        {
            XImage image = XImage.FromFile(gif);
            gfx.DrawImage(image, x, y);
        }

        /// <summary>
        /// For Vouchers
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="EAN"></param>
        /// <param name="Alp"></param>
        /// <param name="exp"></param>
        /// <param name="val"></param>
        /// <param name="pCustFName"></param>
        /// <param name="Vtype"></param>
        /// <param name="pCustMName"></param>
        /// <param name="pCustLName"></param>
        /// <param name="pCustCardNo"></param>
        /// <param name="aCustFName"></param>
        /// <param name="aCustMName"></param>
        /// <param name="aCustLName"></param>
        /// <param name="aCustCardNo"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        private void AddVouchersToPdf(XGraphics gfx, VoucherDetails v, int x1, int y1)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.AddVouchersToPdf", "Context: " + GeneralUtility.GetXMLFromObject(v));
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate;
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                if (Convert.ToBoolean(couponTemplate.IsHideCustomerName))
                {
                    pCustFName = " ";
                    pCustMName = " ";
                    pCustLName = " ";
                    aCustFName = " ";
                    aCustMName = " ";
                    aCustLName = " ";
                }
                else
                {
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }

                
                string pCustCardNo =  v.PrimaryCustomerCardnumber.ToString();
                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();

                string strCustomerPrinted = couponTemplate.lblstrCustomerPrinted; 
                string strClubcardVoucher = couponTemplate.lblstrClubcardVouchers; 

                string strFromTescoBank = couponTemplate.lblstrFromTescoBank; 
                string strClubcardWinner = couponTemplate.lblstrClubcardWinner; 

                string strTitleVoucher = couponTemplate.lblstrTitleVoucher;

                string strClubcardChristmas = couponTemplate.lblstrClubcardChristmas;
                string strBonusVouchers = couponTemplate.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = couponTemplate.lblstrSaverTop_UpVoucher;
                string strOnlineCode = couponTemplate.lblstrOnlineCode; 
                string strValidUntil = couponTemplate.lblstrValidUntil; 
                string isDecimalDisabled = couponTemplate.isDisableDecimal;

                //\
                //Clubcard Christmas
                //Bonus Voucher
                //Clubcard Christmas
                //Saver Top-Up Voucher
                //Online Code
                //Valid Until 

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strTitleVoucher }
                };

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                //Different font sizes for different text
               
                XFont txtFont = new XFont(fontName, 6.0, XFontStyle.Regular, options);
                XFont font_cc = new XFont(fontName, 7.0, XFontStyle.Bold, options);
                XFont font_TB = new XFont(fontName, 9.0, XFontStyle.Bold, options);
                XFont font_VertText = new XFont(fontName, 8.0, XFontStyle.Bold, options);

                if (Vtype == "1")   //Clubcard voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", document, gfx, x1, y1);

                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardVoucher, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 167), XStringFormats.Default);


                    gfx.DrawString(strClubcardVouchers, txtFont, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 200), XStringFormats.Default);

                    /// <summary>
                    /// Change Details  : For ROI Tesco Bank voucher type is 52/53. 
                    ///                   In the Code it was hardcoded as 22. Due to this "From Tesco Bank" was not getting printer in the voucher.
                    /// Modified By     : Swaraj Patra
                    /// Change Date     : 11th Sept 2014
                    /// Team Name       : HSC_41_DigitalClubcard_MCA_VIKINGS
                    /// Country         : ROI
                    /// Solution        : As a part of this change, we change this Voucher type as a configurable variable.
                    /// User Store No.  : 334
                    /// </summary>

                    List<string> sTescoBankVoucher = new List<string>(AppConfiguration.Settings[AppConfigEnum.BankVoucherType].Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {
                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", document, gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);


                    }
                    else    //Xmas bonus vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", document, gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }

                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", document, gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }

                //******************* Customer details ************************
                string pCustName = string.IsNullOrEmpty(pCustFName) ? "" : pCustFName.Substring(0, 1) + pCustMName + " " + pCustLName;

                //If associate customer doesn't have surname, add main customer's surname
                string aCustName = string.Empty;

                if (string.IsNullOrEmpty(aCustLName))
                {
                    aCustName = string.IsNullOrEmpty(aCustFName)? "" : aCustFName.Substring(0, 1) + aCustMName + " " + pCustLName;
                }
                else
                {
                    aCustName = string.IsNullOrEmpty(aCustFName) ? "" : aCustFName.Substring(0, 1) + aCustMName + " " + aCustLName;
                }

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }

                //To print primary customer card number
                if (aCustCardNo == "0")
                {
                    gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                }
                else
                {
                    if (pCustCardNo.Substring((pCustCardNo.Length - 16), 16) != aCustCardNo.Substring((aCustCardNo.Length - 16), 16))
                    {
                        gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isCardsSame = true;
                    }

                    //To print associate customer name and card number
                    if ((pCustName.ToUpper() != aCustName.ToUpper()) && isCardsSame)
                    {
                        if (aCustName.Length > 23)
                        {
                            gfx.DrawString((aCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isNamesSame = true;
                    }

                    if (isCardsSame && isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if (isCardsSame && !isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if ((!isCardsSame && !isNamesSame) || (!isCardsSame && isNamesSame))
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                    }
                }
                if (strIsAlphaCodeRequired.Equals("1"))
                {
                    gfx.DrawString(strOnlineCode, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 170), XStringFormats.Default);
                    gfx.DrawString(Alp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 180), XStringFormats.Default);
                }
                gfx.DrawString(strValidUntil + " " + " " + exp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 190), XStringFormats.Default);

                string aa = val;
                XFont font_val = new XFont("Verdana", 45.0, XFontStyle.Bold, options);

                //Release1.8 - If the voucher value is "£0.50", then make it as "50p"
                string strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                string strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = couponTemplate.lblstrCurrencyDecimalSymbol;
                    aa = aa.Replace(strDecimalSymbol, strCurrencyDecimalSymbol);
                    string a1 = string.Empty;
                    string a2 = string.Empty;

                    if (isDecimalDisabled.ToLower() == "true")
                    {
                        aa = GetDecimalTrimmedCurrencyVal(aa);
                        a1 = aa;

                    }
                    else
                    {
                        int a = aa.LastIndexOf(strCurrencyDecimalSymbol);
                        a1 = aa.Substring(0, a);
                        a2 = aa.Substring(a, 3);

                    }

                     strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                     strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;
                    if (strCurrencyAllignment == "RIGHT")
                    {
                        a2 = a2 + strCurrencySymbol;

                    }
                    else
                    {
                        a1 = strCurrencySymbol + a1;

                    }

                    gfx.DrawString(a1, font_val, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 120), XStringFormats.Default);
                    XFont font_val1 = new XFont("Verdana", 21.0, XFontStyle.Bold, options);


                    if (a1.Length == 3)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 115), (double)(y1 + 0x67), XStringFormats.Default);
                    }

                    else if (a1.Length == 4)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 146), (double)(y1 + 0x67), XStringFormats.Default);
                    }
                    else if (a1.Length > 4)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 175), (double)(y1 + 0x67), XStringFormats.Default);
                    }
                    else
                    {
                        if (a1.Length > 0)
                        {
                            if (a1.Length == 1)
                            {
                                gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 0x67), XStringFormats.Default);
                            }
                            else
                            {
                                gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 0x67), XStringFormats.Default);
                            }
                        }
                        else
                        {
                            gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 0x67), XStringFormats.Default);
                        }

                    }
                }

                this.imageDraw(ean1, document, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                //**********************************************
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context : " + GeneralUtility.GetXMLFromObject(v));
                throw; 
            }
            finally
            {
                trace.Dispose();  
            }
        }
		
        private void AddVouchersToPdfMY(XGraphics gfx, VoucherDetails v, int x1, int y1)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.AddVouchersToPdf", "Context: " + GeneralUtility.GetXMLFromObject(v));
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate;
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                if (Convert.ToBoolean(couponTemplate.IsHideCustomerName))
                {
                    pCustFName = " ";
                    pCustMName = " ";
                    pCustLName = " ";
                    aCustFName = " ";
                    aCustMName = " ";
                    aCustLName = " ";
                }
                else
                {
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }


                string pCustCardNo = v.PrimaryCustomerCardnumber.ToString();
                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();

                string strCustomerPrinted = couponTemplate.lblstrCustomerPrinted; 
                string strClubcardVoucher = couponTemplate.lblstrClubcardVouchers; 

                string strFromTescoBank = couponTemplate.lblstrFromTescoBank; 
                string strClubcardWinner = couponTemplate.lblstrClubcardWinner; 

                string strTitleVoucher = couponTemplate.lblstrTitleVoucher;

                string strClubcardChristmas = couponTemplate.lblstrClubcardChristmas;
                string strBonusVouchers = couponTemplate.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = couponTemplate.lblstrSaverTop_UpVoucher;
                string strOnlineCode = couponTemplate.lblstrOnlineCode; 
                string strValidUntil = couponTemplate.lblstrValidUntil; 
                string isDecimalDisabled = couponTemplate.isDisableDecimal;

                //\
                //Clubcard Christmas
                //Bonus Voucher
                //Clubcard Christmas
                //Saver Top-Up Voucher
                //Online Code
                //Valid Until 

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strTitleVoucher }
                };

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                //Different font sizes for different text

                XFont txtFont = new XFont("Verdana", 6.0, XFontStyle.Regular, options);
                XFont font_cc = new XFont("Verdana", 7.0, XFontStyle.Bold, options);
                XFont font_TB = new XFont("Verdana", 9.0, XFontStyle.Bold, options);
                XFont font_VertText = new XFont("Verdana", 8.0, XFontStyle.Bold, options);

                if (Vtype == "1")   //Clubcard voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", document, gfx, x1, y1);

                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardVoucher, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 167), XStringFormats.Default);


                    gfx.DrawString(strClubcardVouchers, txtFont, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 200), XStringFormats.Default);

                    /// <summary>
                    /// Change Details  : For ROI Tesco Bank voucher type is 52/53. 
                    ///                   In the Code it was hardcoded as 22. Due to this "From Tesco Bank" was not getting printer in the voucher.
                    /// Modified By     : Swaraj Patra
                    /// Change Date     : 11th Sept 2014
                    /// Team Name       : HSC_41_DigitalClubcard_MCA_VIKINGS
                    /// Country         : ROI
                    /// Solution        : As a part of this change, we change this Voucher type as a configurable variable.
                    /// User Store No.  : 334
                    /// </summary>

                    List<string> sTescoBankVoucher = new List<string>(AppConfiguration.Settings[AppConfigEnum.BankVoucherType].Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {
                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", document, gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);


                    }
                    else    //Xmas bonus vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", document, gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }

                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", document, gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }

                //******************* Customer details ************************
                string pCustName = string.IsNullOrEmpty(pCustFName) ? "" : pCustFName.Substring(0, 1) + pCustMName + " " + pCustLName;

                //If associate customer doesn't have surname, add main customer's surname
                string aCustName = string.Empty;

                if (string.IsNullOrEmpty(aCustLName))
                {
                    aCustName = string.IsNullOrEmpty(aCustFName) ? "" : aCustFName.Substring(0, 1) + aCustMName + " " + pCustLName;
                }
                else
                {
                    aCustName = string.IsNullOrEmpty(aCustFName) ? "" : aCustFName.Substring(0, 1) + aCustMName + " " + aCustLName;
                }

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }

                //To print primary customer card number
                if (aCustCardNo == "0")
                {
                    gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                }
                else
                {
                    if (pCustCardNo.Substring((pCustCardNo.Length - 16), 16) != aCustCardNo.Substring((aCustCardNo.Length - 16), 16))
                    {
                        gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isCardsSame = true;
                    }

                    //To print associate customer name and card number
                    if ((pCustName.ToUpper() != aCustName.ToUpper()) && isCardsSame)
                    {
                        if (aCustName.Length > 23)
                        {
                            gfx.DrawString((aCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isNamesSame = true;
                    }

                    if (isCardsSame && isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if (isCardsSame && !isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if ((!isCardsSame && !isNamesSame) || (!isCardsSame && isNamesSame))
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                    }
                }
                if (strIsAlphaCodeRequired.Equals("1"))
                {
                    gfx.DrawString(strOnlineCode, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 170), XStringFormats.Default);
                    gfx.DrawString(Alp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 180), XStringFormats.Default);
                }
                gfx.DrawString(strValidUntil + " " + " " + exp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 190), XStringFormats.Default);

                string aa = val;
                XFont font_val = new XFont("Verdana", 31.0, XFontStyle.Bold, options);

                //Release1.8 - If the voucher value is "£0.50", then make it as "50p"
                string strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                string strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = couponTemplate.lblstrCurrencyDecimalSymbol;
                    aa = aa.Replace(strDecimalSymbol, strCurrencyDecimalSymbol);
                    string a1 = string.Empty;
                    string a2 = string.Empty;

                    if (isDecimalDisabled.ToLower() == "true")
                    {
                        aa = GetDecimalTrimmedCurrencyVal(aa);
                        a1 = aa;

                    }
                    else
                    {
                        int a = aa.LastIndexOf(strCurrencyDecimalSymbol);
                        a1 = aa.Substring(0, a);
                        a2 = aa.Substring(a, 3);

                    }

                    strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                    strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;
                    if (strCurrencyAllignment == "RIGHT")
                    {
                        a2 = a2 + strCurrencySymbol;

                    }
                    else
                    {
                        a1 = strCurrencySymbol + a1;

                    }

                    gfx.DrawString(a1, font_val, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 120), XStringFormats.Default);
                    XFont font_val1 = new XFont("Verdana", 21.7, XFontStyle.Bold, options);


                    if (a1.Length == 3)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 96), (double)(y1 + 0x78), XStringFormats.Default);
                    }

                    else if (a1.Length == 4)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 116), (double)(y1 + 0x78), XStringFormats.Default);
                    }
                    else if (a1.Length == 5)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 0x79), XStringFormats.Default);
                    }
                    else if (a1.Length > 5)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 160), (double)(y1 + 0x79), XStringFormats.Default);
                    }
                    else
                    {
                        if (a1.Length > 0)
                        {
                            if (a1.Length == 1)
                            {
                                gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 0x67), XStringFormats.Default);
                            }
                            else
                            {
                                gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 0x67), XStringFormats.Default);
                            }
                        }
                        else
                        {
                            gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 0x67), XStringFormats.Default);
                        }

                    }
                }

                this.imageDraw(ean1, document, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                //**********************************************
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context : " + GeneralUtility.GetXMLFromObject(v));
                throw;
            }
            finally
            {
                trace.Dispose();
            }
        }

        private void AddVouchersToPdfPL(XGraphics gfx, VoucherDetails v, int x1, int y1)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.AddVouchersToPdf", "Context: " + GeneralUtility.GetXMLFromObject(v));
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate;
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                if (Convert.ToBoolean(couponTemplate.IsHideCustomerName))
                {
                    pCustFName = " ";
                    pCustMName = " ";
                    pCustLName = " ";
                    aCustFName = " ";
                    aCustMName = " ";
                    aCustLName = " ";
                }
                else
                {
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }


                string pCustCardNo = v.PrimaryCustomerCardnumber.ToString();
                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();

                string strCustomerPrinted = couponTemplate.lblstrCustomerPrinted; 
                string strClubcardVoucher = couponTemplate.lblstrClubcardVouchers; 

                string strFromTescoBank = couponTemplate.lblstrFromTescoBank; 
                string strClubcardWinner = couponTemplate.lblstrClubcardWinner; 

                string strTitleVoucher = couponTemplate.lblstrTitleVoucher;

                string strClubcardChristmas = couponTemplate.lblstrClubcardChristmas;
                string strBonusVouchers = couponTemplate.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = couponTemplate.lblstrSaverTop_UpVoucher;
                string strOnlineCode = couponTemplate.lblstrOnlineCode; 
                string strValidUntil = couponTemplate.lblstrValidUntil; 
                string isDecimalDisabled = couponTemplate.isDisableDecimal;

                //\
                //Clubcard Christmas
                //Bonus Voucher
                //Clubcard Christmas
                //Saver Top-Up Voucher
                //Online Code
                //Valid Until 

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strTitleVoucher }
                };

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                //Different font sizes for different text

                XFont txtFont = new XFont("Verdana", 6.0, XFontStyle.Regular, options);
                XFont font_cc = new XFont("Verdana", 7.0, XFontStyle.Bold, options);
                XFont font_cctext = new XFont("Verdana", 7.0, XFontStyle.Regular, options);
                XFont font_TB = new XFont("Verdana", 9.0, XFontStyle.Bold, options);
                XFont font_VertText = new XFont("Verdana", 8.0, XFontStyle.Bold, options);

                if (Vtype == "1")   //Clubcard voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", document, gfx, x1, y1);

                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardVoucher, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 167), XStringFormats.Default);


                    gfx.DrawString(strClubcardVouchers, txtFont, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 200), XStringFormats.Default);

                    /// <summary>
                    /// Change Details  : For ROI Tesco Bank voucher type is 52/53. 
                    ///                   In the Code it was hardcoded as 22. Due to this "From Tesco Bank" was not getting printer in the voucher.
                    /// Modified By     : Swaraj Patra
                    /// Change Date     : 11th Sept 2014
                    /// Team Name       : HSC_41_DigitalClubcard_MCA_VIKINGS
                    /// Country         : ROI
                    /// Solution        : As a part of this change, we change this Voucher type as a configurable variable.
                    /// User Store No.  : 334
                    /// </summary>

                    List<string> sTescoBankVoucher = new List<string>(AppConfiguration.Settings[AppConfigEnum.BankVoucherType].Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {
                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", document, gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);


                    }
                    else    //Xmas bonus vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", document, gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }

                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", document, gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }

                //******************* Customer details ************************
                string pCustName = string.IsNullOrEmpty(pCustFName) ? "" : pCustFName.Substring(0, 1) + pCustMName + " " + pCustLName;

                //If associate customer doesn't have surname, add main customer's surname
                string aCustName = string.Empty;

                if (string.IsNullOrEmpty(aCustLName))
                {
                    aCustName = string.IsNullOrEmpty(aCustFName) ? "" : aCustFName.Substring(0, 1) + aCustMName + " " + pCustLName;
                }
                else
                {
                    aCustName = string.IsNullOrEmpty(aCustFName) ? "" : aCustFName.Substring(0, 1) + aCustMName + " " + aCustLName;
                }

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }

                //To print primary customer card number
                if (aCustCardNo == "0")
                {
                    gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                }
                else
                {
                    if (pCustCardNo.Substring((pCustCardNo.Length - 16), 16) != aCustCardNo.Substring((aCustCardNo.Length - 16), 16))
                    {
                        gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isCardsSame = true;
                    }

                    //To print associate customer name and card number
                    if ((pCustName.ToUpper() != aCustName.ToUpper()) && isCardsSame)
                    {
                        if (aCustName.Length > 23)
                        {
                            gfx.DrawString((aCustName.Substring(0, 24)).ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName.ToUpper(), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
                        isNamesSame = true;
                    }

                    if (isCardsSame && isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if (isCardsSame && !isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 160), XStringFormats.Default);
                    }
                    else if ((!isCardsSame && !isNamesSame) || (!isCardsSame && isNamesSame))
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                    }
                }
                if (strIsAlphaCodeRequired.Equals("1"))
                {
                    gfx.DrawString(strOnlineCode, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 170), XStringFormats.Default);
                    gfx.DrawString(Alp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 180), XStringFormats.Default);
                }
                gfx.DrawString(strValidUntil + " " + " " + exp, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 190), XStringFormats.Default);

                string aa = val;
                XFont font_val = new XFont("Verdana", 60.0, XFontStyle.Bold, options);

                //Release1.8 - If the voucher value is "£0.50", then make it as "50p"
                string strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                string strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = couponTemplate.lblstrCurrencyDecimalSymbol;
                    aa = aa.Replace(strDecimalSymbol, strCurrencyDecimalSymbol);
                    string a1 = string.Empty;
                    string a2 = string.Empty;
                    // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                    //a3 will be used for comma
                    //a4 will be used for currency symbol
                    string a3 = string.Empty;
                    string a4 = string.Empty;
                    string a5 = string.Empty;
                    if (isDecimalDisabled.ToLower() == "true")
                    {
                        aa = GetDecimalTrimmedCurrencyVal(aa);
                        a1 = aa;

                    }
                    else
                    {
                        int a = aa.LastIndexOf(strCurrencyDecimalSymbol);
                        a1 = aa.Substring(0, a);
                        // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                        a3 = aa.Substring(a, 1);
                        a5 = aa.Substring(a, 3);
                        a2 = a5.Replace(strCurrencyDecimalSymbol, "");

                    }

                    strCurrencySymbol = couponTemplate.lblstrCurrencySymbol;
                    strCurrencyAllignment = couponTemplate.lblstrCurrencyAllignment;
                    if (strCurrencyAllignment == "RIGHT")
                    {
                        // Commented to display comma and currency symbol below the currency - PL CR - Legal Requirement

                        // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                        a4 = strCurrencySymbol;

                    }
                    else
                    {
                        // Commented to display comma and currency symbol below the currency - PL CR - Legal Requirement

                        // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                        a4 = strCurrencySymbol;

                    }

                    gfx.DrawString(a1, font_val, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 120), XStringFormats.Default);
                    XFont font_val1 = new XFont("Verdana", 35.0, XFontStyle.Bold, options);
                    // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                    XFont font_val2 = new XFont("Verdana", 25.0, XFontStyle.Bold, options);
                    XFont font_val3 = new XFont("Verdana", 28.0, XFontStyle.Bold, options);

                    if (a1.Length == 3)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 115), (double)(y1 + 0x67), XStringFormats.Default);
                    }

                    else if (a1.Length == 4)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 146), (double)(y1 + 0x67), XStringFormats.Default);
                    }
                    else if (a1.Length > 4)
                    {
                        gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 175), (double)(y1 + 0x67), XStringFormats.Default);
                    }
                    else
                    {
                        if (a1.Length > 0)
                        {
                            if (a1.Length == 1)
                            {
                                gfx.DrawString(a2, font_val3, XBrushes.Black, (double)(x1 + 76), (double)(y1 + 96), XStringFormats.Default);
                                gfx.DrawString(a3, font_val1, XBrushes.Black, (double)(x1 + 68), (double)(y1 + 120), XStringFormats.Default);
                                gfx.DrawString(a4, font_val2, XBrushes.Black, (double)(x1 + 86), (double)(y1 + 120), XStringFormats.Default);
                            }
                            else
                            {
                                gfx.DrawString(a2, font_val3, XBrushes.Black, (double)(x1 + 104), (double)(y1 + 96), XStringFormats.Default);
                                gfx.DrawString(a3, font_val1, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 120), XStringFormats.Default);
                                gfx.DrawString(a4, font_val2, XBrushes.Black, (double)(x1 + 114), (double)(y1 + 120), XStringFormats.Default);
                            }
                        }
                        else
                        {
                            gfx.DrawString(a2, font_val3, XBrushes.Black, (double)(x1 + 86), (double)(y1 + 94), XStringFormats.Default);
                            gfx.DrawString(a3, font_val1, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 120), XStringFormats.Default);
                            gfx.DrawString(a4, font_val2, XBrushes.Black, (double)(x1 + 96), (double)(y1 + 120), XStringFormats.Default);
                        }

                    }
                }

                this.imageDraw(ean1, document, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                //**********************************************
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context : " + GeneralUtility.GetXMLFromObject(v));
                throw;
            }
            finally
            {
                trace.Dispose();
            }
        }

        private void AddCouponsToPdf(XGraphics gfx, CouponDetails v, int x1, int y1,CouponBackgroundTemplate template) //, int couponNo)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.AddCouponsToPdf", "Context: " + GeneralUtility.GetXMLFromObject(v));
            string EAN = v.BarcodeNumber;
            string imageName = v.ImageName;
            string pCustCardNo = v.PrimaryClubcardnumber.ToString();
            string aCustCardNo = v.AssociateClubcardNumber.ToString();
            string pCustFName = string.Empty;
            string pCustMName = string.Empty;
            string pCustLName = string.Empty;
            string aCustFName = string.Empty;
            string aCustMName = string.Empty;
            string aCustLName = string.Empty;

            XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

            try
            {

                if (Convert.ToBoolean(template.IsHideCustomerName))
                {
                    pCustFName = " ";
                    pCustMName = " ";
                    pCustLName = " ";
                    aCustFName = " ";
                    aCustMName = " ";
                    aCustLName = " ";
                }
                else
                {
                    pCustFName = v.PrimaryCustomerFirstName;
                    pCustMName = v.PrimaryCustomerMiddleName;
                    pCustLName = v.PrimaryCustomerLastName;
                    aCustFName = v.AssociateCustomerFirstName;
                    aCustMName = v.AssociateCustomerMiddleName;
                    aCustLName = v.AssociateCustomerLastName;
                }

                string strTitleCoupon = couponTemplate.lblstrTitleCoupon;

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strTitleCoupon }
                };

                XFont font_val = new XFont(fontName, 9.0, XFontStyle.Regular, options);
                XFont font_CardNo = new XFont(fontName, 8.0, XFontStyle.Bold, options);
                XFont font_name = new XFont(fontName, 6.0, XFontStyle.Bold, options);
                XFont font_number = new XFont(fontName, 6.0, XFontStyle.Regular, options);

                string imagePath = @"" + imgPath + "/CouponImages/" + imageName;
                this.imageDraw(imagePath, document, gfx, x1, y1);

                if (!string.IsNullOrEmpty(EAN))
                {
                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                    BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                    System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(312, 50);
                    ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 312, 50);

                    //**************** START Vertical barcode/text ******************************
                    //Print barcode
                    gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 215), (double)(y1 + 258)));
                    this.imageDraw(ean1, document, gfx, x1 + 215, y1 + 258);
                    gfx.RotateAtTransform(90, new XPoint((double)(x1 + 215), (double)(y1 + 258)));
                    //Print barcode number
                    gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 265), (double)(y1 + 205)));
                    gfx.DrawString(EAN, font_CardNo, XBrushes.Black, (double)(x1 + 265), (double)(y1 + 205), XStringFormats.Default);
                    gfx.RotateAtTransform(90, new XPoint((double)(x1 + 265), (double)(y1 + 205)));
                    //**************** END Vertical barcode/text *******************************
                }
                //******************* START Customer details ************************

                string pCustName = pCustFName.Substring(0, 1) + pCustMName + " " + pCustLName;

                //If associate customer doesn't have surname, add main customer's surname
                string aCustName = string.Empty;

                //Srini - Check this logic once
                if (string.IsNullOrEmpty(aCustLName))
                {
                    aCustName = aCustFName.Substring(0, 1) + aCustMName + " " + pCustLName;
                }
                else
                {
                    aCustName = aCustFName.Substring(0, 1) + aCustMName + " " + aCustLName;
                }

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)).ToUpper(), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 225), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName.ToUpper(), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 225), XStringFormats.Default);
                }

                //To print primary customer card number
                if (aCustCardNo == "0")
                {
                    gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 231), XStringFormats.Default);
                }
                else
                {
                    if (pCustCardNo.Substring((pCustCardNo.Length - 16), 16) != aCustCardNo.Substring((aCustCardNo.Length - 16), 16))
                    {
                        gfx.DrawString(MasknFormatClubcard(pCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 231), XStringFormats.Default);
                        isCardsSame = true;
                    }

                    //To print associate customer name and card number
                    if ((pCustName.ToUpper() != aCustName.ToUpper()) && isCardsSame)
                    {
                        if (aCustName.Length > 23)
                        {
                            gfx.DrawString((aCustName.Substring(0, 24)).ToUpper(), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 239), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName.ToUpper(), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 239), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName.ToUpper(), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 231), XStringFormats.Default);
                        isNamesSame = true;
                    }

                    if (isCardsSame && isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 245), XStringFormats.Default);
                    }
                    else if (isCardsSame && !isNamesSame)
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 239), XStringFormats.Default);
                    }
                    else if ((!isCardsSame && !isNamesSame) || (!isCardsSame && isNamesSame))
                    {
                        gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 245), XStringFormats.Default);
                    }
                }
                //******************* END Customer details ************************
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context: " + GeneralUtility.GetXMLFromObject(v));
                throw; 
            }
            finally
            {
                trace.Dispose();  
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
        private string MasknFormatClubcard(string clubcardNumber, bool isMaskReq, char maskChar)
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

        private List<VoucherDetails> GetCustomerDetailsAmendedVoucherDetails(List<VoucherDetails> voucherDetailsList, AccountDetails customerDetails)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetCustomerDetailsAmendedVoucherDetails", "Context:" + GeneralUtility.GetXMLFromObject(voucherDetailsList) + GeneralUtility.GetXMLFromObject(customerDetails) );
            string strCurrencySymbol = couponTemplate.lblstrCurrencySymbol; 

            try
            {
                List<VoucherDetails> newVoucherDetailsList = new List<VoucherDetails>();

                if (voucherDetailsList == null) 
                {
                    return newVoucherDetailsList;
                }

                foreach (VoucherDetails voucherDetails in voucherDetailsList)
                {
                    VoucherDetails newVoucherDetails = new VoucherDetails
                    {

                        VoucherNumber = voucherDetails.VoucherNumber,
                        AlphaCode = voucherDetails.AlphaCode, 
                        ExpiryDate = Convert.ToDateTime(voucherDetails.ExpiryDate).ToString(dateformat),//Convert.ToDateTime(_ds.Tables[0].Rows[_rowSelected][5].ToString()).ToString("dd/MM/yyyy"),

                        Value =  voucherDetails.Value, 

                        VoucherType = voucherDetails.VoucherType, 
                        VoucherNumberToPrint = voucherDetails.VoucherNumberToPrint, 

                        //New fields added for CR
                        PrimaryCustomerFirstname = customerDetails.PrimaryCustomerName1, 
                        PrimaryCustomerMiddlename = customerDetails.PrimaryCustomerName2, 
                        PrimaryCustomerLastname = customerDetails.PrimaryCustomerName3, 
                        PrimaryCustomerCardnumber = customerDetails.PrimaryClubcardID, 
                        AssociateCustomerFirstname = customerDetails.AssociateCustName1,
                        AssociateCustomerMiddlename = customerDetails.AssociateCustName2, 
                        AssociateCustomerLastname = customerDetails.AssociateCustName3, 
                        AssociateCustomerCardnumber = customerDetails.AssociateClubcardID 
                    };
                    newVoucherDetailsList.Add(newVoucherDetails);
                }

                return newVoucherDetailsList;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(voucherDetailsList) + GeneralUtility.GetXMLFromObject(customerDetails));
                throw; 
            }
            finally
            {
                trace.Dispose();
            }

        }

        private List<CouponDetails> GetCustomerDetailsAmendedCouponDetails(List<CouponDetails> couponDetailsList, AccountDetails customerDetails)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetCustomerDetailsAmendedCouponDetails", "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(customerDetails));
            List<CouponDetails> newCoupons = new List<CouponDetails>();
            try
            {
                if (couponDetailsList == null) 
                {
                    return newCoupons;
                }

                foreach (CouponDetails coupon in couponDetailsList)//   (DataRow dr in _dsCoupon.Tables[0].Rows)
                {
                    CouponDetails newCoupon = new CouponDetails
                    {
                        BarcodeNumber = coupon.BarcodeNumber, 
                        ImageName = coupon.FullImageName,

                        //Customer details
                        PrimaryCustomerFirstName = customerDetails.PrimaryCustomerName1, 
                        PrimaryCustomerMiddleName = customerDetails.PrimaryCustomerName2, 
                        PrimaryCustomerLastName = customerDetails.PrimaryCustomerName3, 
                        PrimaryClubcardnumber = customerDetails.PrimaryClubcardID, 
                        AssociateCustomerFirstName = customerDetails.AssociateCustName1, 
                        AssociateCustomerMiddleName = customerDetails.AssociateCustName2, 
                        AssociateCustomerLastName = customerDetails.AssociateCustName3, 
                        AssociateClubcardNumber = customerDetails.AssociateClubcardID 
                    };

                    newCoupons.Add(newCoupon);
                }
                return newCoupons;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(customerDetails));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }

        }

        #region IGeneratePDFForMLS Members

        /// <summary>
        /// To Generate the PDF which contain both Coupons and Vouchers
        /// </summary>
        /// <param name="dsVouchers"> Set of vouchers List</param>
        /// <param name="dsCoupons">Set of Coupons List</param>
        /// <param name="custDetails">Customer Details</param>
        /// <param name="path"></param>
        /// <returns></returns>
        public PdfDocument GetCouponsAndVouchersDocument(List<VoucherDetails> voucherdetailsList, List<CouponDetails> couponDetailsList, AccountDetails customerAccountDetails, CouponBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetCouponsAndVouchersDocument", "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(voucherdetailsList));
            couponTemplate = template;
            InitialPrivateVariables(template);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            int widc = 1200, widv = 590;
            XGraphics gfx = null;
            int x1 = 10;
            int y1 = 10;
            bool isNewRequired = false;
            int vc = 0;

            #region For adding Vouchers

            try
            {
                if (voucherdetailsList != null)
                {
                    page.Width = widv;
                    gfx = XGraphics.FromPdfPage(page);
                    isNewRequired = true;
                    List<VoucherDetails> list = GetCustomerDetailsAmendedVoucherDetails(voucherdetailsList, customerAccountDetails);

                    foreach (VoucherDetails v in list)
                    {
                        if (vc == 3)
                        {
                            page = document.AddPage();
                            page.Width = widv;
                            gfx = XGraphics.FromPdfPage(page);
                            x1 = 10;
                            y1 = 10;
                            vc = 1;
                        }
                        else
                        {
                            vc++;
                        }

                        string culture = AppConfiguration.Settings[AppConfigEnum.CultureDefaultloc];
                        if (culture.ToUpper() == "PL-PL")
                        {
                            this.AddVouchersToPdfPL(gfx, v, x1, y1);
                        }
                        else if (culture.ToUpper() == "EN-MY" || culture.ToUpper() == "MS-MY")
	                    {
	                        this.AddVouchersToPdfMY(gfx, v, x1, y1);
	                    }
                        else
                            this.AddVouchersToPdf(gfx, v, x1, y1);
                        y1 += 0x109;
                    }
                }
            #endregion

                #region For adding Coupons
                if (couponDetailsList != null)
                {
                    if (isNewRequired)
                    {
                        page = document.AddPage();
                        page.Width = widc;
                        gfx = XGraphics.FromPdfPage(page);
                    }
                    else
                    {
                        page.Width = widc;
                        gfx = XGraphics.FromPdfPage(page);
                    }

                    x1 = 30;
                    y1 = 10;
                    vc = 0;

                    List<CouponDetails> couponsList = GetCustomerDetailsAmendedCouponDetails(couponDetailsList, customerAccountDetails);

                    foreach (CouponDetails v in couponsList)
                    {
                        if (vc == 6)
                        {
                            page = document.AddPage();
                            page.Width = widc;
                            gfx = XGraphics.FromPdfPage(page);
                            x1 = 30;
                            y1 = 10;
                            vc = 1;
                        }
                        else
                        {
                            vc++;
                        }

                        this.AddCouponsToPdf(gfx, v, x1, y1,template); //, vc);

                        if (vc % 2 == 0)
                        {
                            y1 += 270;
                            x1 = 30;
                        }
                        else
                        {
                            x1 = 610;
                        }
                    }
                }
                #endregion
                document.Close();

                return document;

            }
            catch (Exception exp)
            {
                trace.NoteException(exp, "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(voucherdetailsList));
                throw;
            }
            finally
            {
                trace.Dispose();
            }

        }


        #endregion

        public PdfDocument GetExchangeDocument(List<Token> lstTokenDetails, BoostBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetExchangeDocument", "Context:" + GeneralUtility.GetXMLFromObject(template) + GeneralUtility.GetXMLFromObject(lstTokenDetails));
            try
            {
                int x1 = 10;
                int y1 = 10;
                int wid = 590;
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Width = wid;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                int vc = 0;
                List<TokenDetails> list = getTokens(lstTokenDetails, template);

                foreach (TokenDetails v in list)
                {
                    if (vc == 3)
                    {
                        page = document.AddPage();
                        page.Width = wid;
                        gfx = XGraphics.FromPdfPage(page);
                        x1 = 10;
                        y1 = 10;
                        vc = 1;
                    }
                    else
                    {
                        vc++;
                    }

                    this.putPDFExchanges(gfx, v.TokenId, v.TokenCode, v.ExpiryDate, v.Value, v.QualifySpend, v.Includes, v.Excludes, v.TermsAndCondition, x1, y1, template);
                    y1 += 0x109;
                }

                return document;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(template) + GeneralUtility.GetXMLFromObject(lstTokenDetails));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

        public void putPDFExchanges(XGraphics gfx, string tokenId, string EAN, string exp, string val, string QualifySpend, string Includes, string Excludes, string TermsAndCondition, int x1, int y1, BoostBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.putPDFExchanges", "Context:" + GeneralUtility.GetXMLFromObject(template));
            try
            {
                sConfiguredClubcard = template.ReplaceClubcardPrefix;
                imgPath = template.PrintBGImagePath;
                fontLoadPath = template.FontPath;
                strIsAlphaCodeRequired = template.IsAlphaCodeRequired;
                culture = template.CultureDefaultloc;

                string strToken = template.lblstrToken;

                string strClubcardBoostatTesco = template.lblstrClubcardBoostatTesco;
                string strCustomerPrintedClubcardBoostToken = template.lblstrCustomerPrintedClubcardBoostToken;
                string strOFF = template.lblstrOFF;
                string strWhenYouSpend = template.lblstrWhenYouSpend;
                string strOrMoreOn = template.lblstrOrMoreOn;
                string strInaSingleTransaction = template.lblstrInaSingleTransaction;
                string strValidUntil = template.lblstrValidUntil;

                string strLine1 = template.lblstrLine1;
                string strLine2 = template.lblstrLine2;
                string strLine3 = template.lblstrLine3;
                string strLine4 = template.lblstrLine4;
                string strLine5 = template.lblstrLine5;
                string strLine6 = template.lblstrLine6;
                string strLine7 = template.lblstrLine7;
                string strLine8 = template.lblstrLine8;
                string strLine9 = template.lblstrLine9;
                string strLine10 = template.lblstrLine10;
                string strLine11 = template.lblstrLine11;
                string strLine12 = template.lblstrLine12;
                string strLine13 = template.lblstrLine13;

                string strSerialNumber = template.lblstrSerialNumber;
                string strDatePrinted = template.lblstrDatePrinted;
                string strDateFormat = template.lblstrDateFormat;

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strToken }
                };

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                XFont font_cc = new XFont("Verdana", 7.0, XFontStyle.Bold);
                XFont font_alp = new XFont("Verdana", 7.0, XFontStyle.Regular);
                XFont font_val = new XFont("Verdana", 9.0, XFontStyle.Regular);
                XFont font_val9bold = new XFont("Verdana", 9.0, XFontStyle.Bold);
                XFont font_alp_large = new XFont("Verdana", 13.0, XFontStyle.Bold);
                XFont font_small = new XFont("Verdana", 6.5, XFontStyle.Regular);

                this.imageDraw(@"" + imgPath + "Exchange_Tokens.jpg", document, gfx, x1, y1);

                //***********************
                gfx.DrawString(strClubcardBoostatTesco, font_alp_large, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 90), XStringFormats.Default);
                gfx.DrawString(strCustomerPrintedClubcardBoostToken, font_val, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 105), XStringFormats.Default);
                gfx.DrawString(val + " " + strOFF, font_val9bold, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 115), XStringFormats.Default);
                gfx.DrawString(strWhenYouSpend + QualifySpend + strOrMoreOn, font_alp, XBrushes.Black, (double)(x1 + 100), (double)(y1 + 125), XStringFormats.Default);

                //Based on the length of "Includes" align the text center.
                if (Includes.Length > 50)
                {
                    string includesStr = string.Empty;
                    string includesStr1 = string.Empty;
                    string[] upprBnd = Includes.Split(' ');

                    if (upprBnd.GetUpperBound(0) > 5)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            includesStr = includesStr + upprBnd[i] + " ";
                        }

                        for (int j = 6; j <= upprBnd.GetUpperBound(0); j++)
                        {
                            includesStr1 = includesStr1 + " " + upprBnd[j];
                        }
                    }

                    gfx.DrawString(includesStr, font_cc, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(includesStr1, font_cc, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 145), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 155), XStringFormats.Default);
                }
                else if (Includes.Length >= 40 && Includes.Length <= 50)
                {
                    gfx.DrawString(Includes, font_cc, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (Includes.Length >= 30 && Includes.Length <= 40)
                {
                    gfx.DrawString(Includes, font_cc, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (Includes.Length >= 20 && Includes.Length <= 30)
                {
                    gfx.DrawString(Includes, font_cc, XBrushes.Black, (double)(x1 + 110), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (Includes.Length >= 10 && Includes.Length <= 20)
                {
                    gfx.DrawString(Includes, font_cc, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (Includes.Length <= 10)
                {
                    gfx.DrawString(Includes, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }

                //Based on the length of "Excludes" align the text center.
                if (Excludes.Length >= 50)
                {
                    int yAxis = 155;
                    int i = 0;
                    StringBuilder excludesStr;

                    while (i <= Excludes.Length)
                    {
                        int exBrkPoint = 58;

                        if (exBrkPoint + i < Excludes.Length)
                        {
                            exBrkPoint = Excludes.IndexOf(" ", exBrkPoint + i);
                        }
                        else
                        {
                            exBrkPoint = Excludes.Length;
                        }

                        if (exBrkPoint == -1)
                        {
                            exBrkPoint = Excludes.Length;
                        }

                        excludesStr = new StringBuilder();

                        for (int j = i; j <= (exBrkPoint - 1); j++)
                        {
                            excludesStr.Append(Excludes[j]);

                            if (j >= exBrkPoint)
                            {
                                break;
                            }
                        }

                        i = exBrkPoint + 1;
                        yAxis = yAxis + 7;

                        gfx.DrawString(excludesStr.ToString(), font_small, XBrushes.Black, (double)(x1 + 40), (double)(y1 + yAxis), XStringFormats.Default);
                    }
                }
                else if (Excludes.Length >= 30 && Excludes.Length <= 50)
                {
                    gfx.DrawString(Excludes, font_small, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 162), XStringFormats.Default);
                }
                else if (Excludes.Length <= 30)
                {
                    gfx.DrawString(Excludes, font_small, XBrushes.Black, (double)(x1 + 100), (double)(y1 + 162), XStringFormats.Default);
                }

                gfx.DrawString(strValidUntil + exp, font_cc, XBrushes.Black, (double)(x1 + 110), (double)(y1 + 192), XStringFormats.Default);
                this.imageDraw(ean1, document, gfx, x1 + 40, y1 + 200);
                XFont font = new XFont("Verdana", 9.0, XFontStyle.Regular);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 88), (double)(y1 + 248), XStringFormats.Default);
                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                gfx.DrawString(EAN, font_val, XBrushes.Black, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                //*************************************************************
                //Terms & Conditions
                XFont font_tc = new XFont("Verdana", 6.0, XFontStyle.Regular);
                gfx.DrawString(strLine1, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 100), XStringFormats.Default);
                gfx.DrawString(strLine2, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 108), XStringFormats.Default);
                gfx.DrawString(strLine3, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 116), XStringFormats.Default);
                gfx.DrawString(strLine4 + val + strLine5, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 124), XStringFormats.Default);
                gfx.DrawString(strLine6, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 132), XStringFormats.Default);
                gfx.DrawString(strLine7, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 140), XStringFormats.Default);
                gfx.DrawString(strLine8, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 148), XStringFormats.Default);
                gfx.DrawString(strLine9, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 156), XStringFormats.Default);
                gfx.DrawString(strLine10, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 164), XStringFormats.Default);
                gfx.DrawString(strLine11, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 172), XStringFormats.Default);
                gfx.DrawString(strLine12, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 180), XStringFormats.Default);
                gfx.DrawString(strLine13, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 188), XStringFormats.Default);

                XFont font_exp01 = new XFont("Verdana", 5.5, XFontStyle.Bold);
                //Serial number
                gfx.DrawString(strSerialNumber + tokenId, font_exp01, XBrushes.Black, (double)(x1 + 318), (double)(y1 + 240), XStringFormats.Default);
                //Date printed


                gfx.DrawString(strDatePrinted + DateTime.Now.ToString(strDateFormat), font_exp01, XBrushes.Black, (double)(x1 + 410), (double)(y1 + 240), XStringFormats.Default);
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(template));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

        public PdfDocument GetCouponsDocument(List<CouponDetails> couponDetailsList, AccountDetails customerAccountDetails, CouponBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetCouponsDocument", "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(customerAccountDetails));
            try
            {
                int x1 = 30;
                int y1 = 10;
                int wid = 1200;
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Width = wid;
                couponTemplate = template;
                InitialPrivateVariables(template);
                XGraphics gfx = XGraphics.FromPdfPage(page);
                int vc = 0;
                List<CouponDetails> list = GetCustomerDetailsAmendedCouponDetails(couponDetailsList, customerAccountDetails);

                foreach (CouponDetails v in list)
                {
                    if (vc == 6)
                    {
                        page = document.AddPage();
                        page.Width = wid;
                        gfx = XGraphics.FromPdfPage(page);
                        x1 = 30;
                        y1 = 10;
                        vc = 1;
                    }
                    else
                    {
                        vc++;
                    }

                    this.AddCouponsToPdf(gfx, v, x1, y1, template);

                    if (vc % 2 == 0)
                    {
                        y1 += 270;
                        x1 = 30;
                    }
                    else
                    {
                        x1 = 610;
                    }
                }
                document.Close();

                return document;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(couponDetailsList) + GeneralUtility.GetXMLFromObject(customerAccountDetails));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

        public void PrintVouchersDocument(List<VoucherDetails> voucherDetails, AccountDetails customerAccountDetails, CouponBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetVouchersDocument", "Context:" + GeneralUtility.GetXMLFromObject(customerAccountDetails) + GeneralUtility.GetXMLFromObject(template));
            try
            {
                int x1 = 10;
                int y1 = 10;
                int wid = 590;
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Width = wid;

                couponTemplate = template;
                InitialPrivateVariables(template);

                XGraphics gfx = XGraphics.FromPdfPage(page);
                int vc = 0;
                List<VoucherDetails> list = GetCustomerDetailsAmendedVoucherDetails(voucherDetails, customerAccountDetails);

                foreach (VoucherDetails v in list)
                {
                    if (vc == 3)
                    {
                        page = document.AddPage();
                        page.Width = wid;
                        gfx = XGraphics.FromPdfPage(page);
                        x1 = 10;
                        y1 = 10;
                        vc = 1;
                    }
                    else
                    {
                        vc++;
                    }

                    string culture = AppConfiguration.Settings[AppConfigEnum.CultureDefaultloc];
                    if (culture.ToUpper() == "PL-PL")
                    {
                        this.AddVouchersToPdfPL(gfx, v, x1, y1);
                    }
                    else if (culture.ToUpper() == "EN-MY" || culture.ToUpper() == "MS-MY")
                    {
                        this.AddVouchersToPdfMY(gfx, v, x1, y1);
                    }
                    else
                        this.AddVouchersToPdf(gfx, v, x1, y1);

                    y1 += 0x109;
                }

                document.Close();

                AddDocumentToResponse(document);

            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(customerAccountDetails) + GeneralUtility.GetXMLFromObject(template));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

        public static List<TokenDetails> getTokens(List<Token> _lstToken, BoostBackgroundTemplate template)
        {
            string dateformat = AppConfiguration.Settings[AppConfigEnum.DisplayDateFormat];
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.getTokens", "Context:" + GeneralUtility.GetXMLFromObject(_lstToken) + GeneralUtility.GetXMLFromObject(template));
            try
            {
                
                string strCurrencySymbol = template.lblstrCurrencySymbol.ToString();

                List<TokenDetails> list = new List<TokenDetails>();
                if (_lstToken.Count != 0)
                {

                    foreach (Token token in _lstToken)
                    {
                        TokenDetails g__initLocal0 = new TokenDetails
                        {
                            TokenId = token.TokenID,
                            TokenCode = token.SupplierTokenCode,
                            ExpiryDate = Convert.ToDateTime(token.ValidUntil).ToString(dateformat),
                            Value = strCurrencySymbol + token.ProductTokenValue,
                            QualifySpend = token.QualifyingSpend,
                            Includes = token.Includes,
                            Excludes = token.Excludes,
                            TermsAndCondition = token.TermsAndConditions
                        };
                        list.Add(g__initLocal0);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(_lstToken) + GeneralUtility.GetXMLFromObject(template));
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
        }

                
        /// <summary>
        /// To generate the PDF for temperory Clubcard.
        /// </summary>
        /// <param name="clubcardID"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public PdfDocument GetNewClubcardDocument(long iClubcardId, CouponBackgroundTemplate template)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.GetNewClubcardDocument", "Context:" + GeneralUtility.GetXMLFromObject(template));
            try
            {
                int x1 = 30;
                int y1 = 10;
                int wid = 590;
                string clubcardID = iClubcardId.ToString();

                string strFormatedClubcardId = string.Empty;
                strFormatedClubcardId = Common.Utilities.GeneralUtility.FormateClubcardNumber(clubcardID);     
           
                string sClubcardID = string.Empty;
                sClubcardID = clubcardID.Substring(5);
                sClubcardID = sConfiguredClubcard + sClubcardID;
                string strTitleClubcard = template.lblstrTitleClubcard;

                couponTemplate = template;
                InitialPrivateVariables(template);

                PdfDocument document = new PdfDocument()
                {
                    Info = { Title = strTitleClubcard }
                };

                PdfPage page = document.AddPage();
                page.Width = wid;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                XFont font_val = new XFont("Verdana", 8.0, XFontStyle.Regular);
                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(285, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, sClubcardID, System.Drawing.Color.Black, System.Drawing.Color.White, 285, 50);

                this.imageDraw(@"" + imgPath + "PrintatHomeClubcard.jpg", document, gfx, x1, y1);
                this.imageDraw(ean1, document, gfx, x1 + 32, y1 + 110);
                gfx.DrawString(strFormatedClubcardId, font_val, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 160), XStringFormats.Default);
                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                gfx.DrawString(strFormatedClubcardId, font_val, XBrushes.Black, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                //*************************************************************

                return document;

            }
            catch (Exception ex)
            {
                trace.NoteException(ex, "Context:" + GeneralUtility.GetXMLFromObject(template));
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
        private void AddDocumentToResponse(PdfSharp.Pdf.PdfDocument document)
        {
            MCATrace trace = new MCATrace("Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services.GenerateVouchersPDFAdapter.AddDocumentToResponse", string.Empty);

            try
            {
                string genPathFileName = "Vouchers.pdf";

                //************************ Save PDF document to memory stream  ****************
                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Length", stream.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + genPathFileName);
                HttpContext.Current.Response.BinaryWrite(stream.ToArray());
                HttpContext.Current.Response.Flush();
                stream.Dispose();
                stream.Close();
            }
            catch (Exception ex)
            {
                trace.NoteException(ex, string.Empty);
                throw;
            }
            finally
            {
                trace.Dispose();  
            }
 
        }
    }
}
