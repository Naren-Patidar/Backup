using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Generic;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.VoucherandCouponDetails;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Xml;
using System.Xml.Serialization;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class PDFGenerator : IPDFGenerator
    {
        IConfigurationProvider _configProvider;
        private ILoggingService _logger;
        AccountDetails _customerAccountDetails;
        Dictionary<Type, Func<PdfDocument>> _pdfHandlers = new Dictionary<Type, Func<PdfDocument>>();

        dynamic _template = null;
        dynamic _lstModel = null;
        dynamic _lstCModel = null;
        XPen borderPen;
        double borderWidth = 4.5;
        XGraphicsState state;
        string dateformat = String.Empty;
        string fontName = String.Empty;
        string sConfiguredClubcard, imgPath, fontLoadPath, strIsAlphaCodeRequired, culture;
        string strClubcardVouchers, strBonusvoucher, strTopup;

        #region Constructors

        public PDFGenerator(IConfigurationProvider configProvider, ILoggingService logger)
        {
            this._configProvider = configProvider;
            this._logger = logger;
            this.dateformat = _configProvider.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);
            this.fontName = _configProvider.GetStringAppSetting(AppConfigEnum.FontName);

            this._pdfHandlers.Add(typeof(CouponDetails), () => this.GetCouponsDoc());
            this._pdfHandlers.Add(typeof(VoucherDetails), () => this.GetVouchersDoc());
            this._pdfHandlers.Add(typeof(Token), () => this.GetBoostAtTescoDoc());
            this._pdfHandlers.Add(typeof(VoucherandCouponDetails), () => this.GetVoucherandCoupon());
            this._pdfHandlers.Add(typeof(string), () => this.GetClubcardDoc());
        }

        #endregion Constructors

        private PdfDocument GetPDFCouponsAndVouchersDocument(VoucherandCouponDetails VoucherandCouponDetails, AccountDetails customerAccountDetails, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            //couponTemplate = template;
            InitialPrivateVariables(template);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            int widc = 1200, widv = 590;
            //page.Width = wid;
            XGraphics gfx = null;
            //XGraphics.FromPdfPage(page);
            int x1 = 10;
            int y1 = 10;
            bool isNewRequired = false;
            int vc = 0;
            try
            {
                //  _logdata.CaptureData("Unusedvoucherdetails", VoucherandCouponDetails.VoucherDetails);
                if (VoucherandCouponDetails.VoucherDetails != null)
                {
                    page.Width = widv;
                    gfx = XGraphics.FromPdfPage(page);
                    isNewRequired = true;
                    List<VoucherDetails> list = this.GetFormattedDetails(VoucherandCouponDetails.VoucherDetails, customerAccountDetails);
                    //      _logdata.CaptureData("formatted voucher details", list);
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

                        //this.putPDF(gfx,v.VoucherNumberToPrint, v.AlphaCode, v.ExpiryDate, v.Value, v.pCustFirstname, v.VoucherType, v.pCustMiddlename, v.pCustLastname, v.pCustCardnumber, v.aCustFirstname, v.aCustMiddlename, v.aCustLastname, v.aCustCardnumber, x1, y1, path, document);
                        // string culture = AppConfiguration.Settings[AppConfigEnum.CultureDefaultloc];
                        _logdata.RecordStep(string.Format("Culture: {0}, ", template.Culture));
                        if (!string.IsNullOrEmpty(template.Culture) && template.Culture.ToUpper() == "PL-PL")
                        {
                            this.AddVouchersToPdfPL(gfx, v as VoucherDetails, x1, y1, template);
                            _logdata.RecordStep("Vouchers added successfully to PDF document for PL");
                        }
                        else if (!string.IsNullOrEmpty(template.Culture) && template.Culture.ToUpper() == "EN-MY" || template.Culture.ToUpper() == "MS-MY")
                        {
                            this.AddVouchersToPdfMY(gfx, v as VoucherDetails, x1, y1, template);
                            _logdata.RecordStep("vouchers added successfully to PDF document for MY");
                        }
                        else
                            this.AddVouchersToPdf(gfx, v as VoucherDetails, x1, y1, template);
                        _logdata.RecordStep("vouchers added successfully to PDF document");
                        y1 += 0x109;
                    }
                }

                #region For adding Coupons
                //    _logdata.CaptureData("Coupon Object", VoucherandCouponDetails.CouponDetails);
                if (VoucherandCouponDetails.CouponDetails != null)
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


                    List<CouponDetails> couponsList = GetFormattedDetails(VoucherandCouponDetails.CouponDetails, customerAccountDetails);
                    //    _logdata.CaptureData("formatted coupon details", couponsList);
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

                        //this.putPDFCoupons(gfx, v.BarcodeNumber, v.ImageName, v.pCustFirstname, v.pCustMiddlename, v.pCustLastname, v.pCustCardnumber, v.aCustFirstname, v.aCustMiddlename, v.aCustLastname, v.aCustCardnumber, x1, y1, vc);
                        this.AddCouponsToPdf(gfx, v, x1, y1, template); //, vc);
                        _logdata.RecordStep("Coupons added successfully to PDF document");
                        if (vc % 2 == 0)
                        {
                            //y1 += 0x109;
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
                _logdata.RecordStep("Vouchers and coupons successfully added to the document.");
                document.Close();
                _logger.Submit(_logdata);

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in adding the vopucher/coupon details to the document", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
            return document;
        }

        private PdfDocument GetPDFDocument<T>(List<T> list, AccountDetails customerAccountDetails, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            this.InitialPrivateVariables(template);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = null;
            int x1 = 10, y1 = 10, vc = 0;
            try
            {
                _logdata.CaptureData("List of details to add for document", list);
                //   _logdata.CaptureData("Customer details", customerAccountDetails);
                if (list != null)
                {
                    page.Width = template.DocumentWidth;
                    gfx = XGraphics.FromPdfPage(page);
                    x1 = template.Left;
                    y1 = template.Top;
                    vc = 0;
                    list = this.GetFormattedDetails<T>(list, customerAccountDetails);
                    //   _logdata.CaptureData("List of formatted details", list);
                    foreach (T v in list)
                    {
                        if (vc == template.ItemPerPage)
                        {
                            _logdata.RecordStep("adding new page to the document");
                            page = document.AddPage();
                            page.Width = template.DocumentWidth;
                            gfx = XGraphics.FromPdfPage(page);
                            x1 = 30;
                            y1 = 10;
                            vc = 1;
                        }
                        else
                        {
                            vc++;
                        }
                        if (typeof(T).Equals(typeof(CouponDetails)))
                        {
                            this.AddCouponsToPdf(gfx, v as CouponDetails, x1, y1, template);
                            _logdata.RecordStep("Coupons added successfully to the document");
                            y1 += (vc % 2 == 0) ? 270 : 0;
                            x1 = (vc % 2 == 0) ? 30 : 610;
                        }
                        else if (typeof(T).Equals(typeof(VoucherDetails)))
                        {
                            if (!string.IsNullOrEmpty(template.Culture) && template.Culture.ToUpper() == "PL-PL")
                            {
                                this.AddVouchersToPdfPL(gfx, v as VoucherDetails, x1, y1, template);
                                _logdata.RecordStep("Vouchers added successfully to the document for PL");
                            }
                            else if (!string.IsNullOrEmpty(template.Culture) && (template.Culture.ToUpper() == "EN-MY" || template.Culture.ToUpper() == "MS-MY"))
                            {
                                this.AddVouchersToPdfMY(gfx, v as VoucherDetails, x1, y1, template);
                                _logdata.RecordStep("Vouchers added successfully to the document for MY");
                            }
                            else
                            {
                                this.AddVouchersToPdf(gfx, v as VoucherDetails, x1, y1, template);
                                _logdata.RecordStep("Vouchers added successfully to the document");
                            }
                            y1 += 0x109;
                        }
                    }
                }
                _logdata.RecordStep("adding new page to the document");
                _logger.Submit(_logdata);
                document.Close();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in adding the details to the document", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
            return document;
        }

        private PdfDocument GetNewClubcardPDFDocument(string clubcardId, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {
                int x1 = 30, y1 = 10, wid = 590;
                string clubcardID = clubcardId.TryParse<string>();
                //_logdata.BlackLists.Add(clubcardId);
                string strFormatedClubcardId = string.Empty;
                strFormatedClubcardId = GeneralUtility.FormateClubcardNumber(clubcardID);

                string sClubcardID = string.Empty;
                sClubcardID = clubcardID.Substring(5);
                sClubcardID = sConfiguredClubcard + sClubcardID;
                string strTitleClubcard = template.lblstrTitleClubcard;

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

                this.imageDraw(@"" + imgPath + "PrintatHomeClubcard.jpg", gfx, x1, y1);
                this.imageDraw(ean1, gfx, x1 + 32, y1 + 110);
                gfx.DrawString(strFormatedClubcardId, font_val, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 160), XStringFormats.Default);
                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                gfx.DrawString(strFormatedClubcardId, font_val, XBrushes.Black, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 25), (double)(y1 + 138)));
                //*************************************************************
                _logdata.RecordStep("all the details added successfully to the document.");
                _logger.Submit(_logdata);
                return document;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in adding the new clubcard details to the document.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
        }

        private List<T> GetFormattedDetails<T>(List<T> dataList, AccountDetails customerDetails)
        {
            List<T> newVoucherDetailsList = new List<T>();
            LogData _logdata = new LogData();
            try
            {

                if (dataList == null)
                {
                    return newVoucherDetailsList;
                }

                if (typeof(T).Equals(typeof(VoucherDetails)))
                {
                    List<VoucherDetails> voucherDetailsList = dataList as List<VoucherDetails>;
                    newVoucherDetailsList = (from voucherDetails in voucherDetailsList
                                             select new VoucherDetails
                                             {
                                                 VoucherNumber = voucherDetails.VoucherNumber,
                                                 AlphaCode = voucherDetails.AlphaCode,
                                                 ExpiryDate = voucherDetails.ExpiryDate,
                                                 Value = voucherDetails.Value,
                                                 VoucherType = voucherDetails.VoucherType,
                                                 VoucherNumberToPrint = voucherDetails.VoucherNumberToPrint,
                                                 PrimaryCustomerFirstname = customerDetails.PrimaryCustomerName1,
                                                 PrimaryCustomerMiddlename = customerDetails.PrimaryCustomerName2,
                                                 PrimaryCustomerLastname = customerDetails.PrimaryCustomerName3,
                                                 PrimaryCustomerCardnumber = customerDetails.PrimaryClubcardID,
                                                 AssociateCustomerFirstname = customerDetails.AssociateCustName1,
                                                 AssociateCustomerMiddlename = customerDetails.AssociateCustName2,
                                                 AssociateCustomerLastname = customerDetails.AssociateCustName3,
                                                 AssociateCustomerCardnumber = customerDetails.AssociateClubcardID
                                             }).ToList() as List<T>;
                    //    _logdata.CaptureData("formatted voucher details", newVoucherDetailsList);

                }

                else if (typeof(T).Equals(typeof(CouponDetails)))
                {
                    List<CouponDetails> couponDetailsList = dataList as List<CouponDetails>;
                    newVoucherDetailsList = (from coupon in couponDetailsList
                                             select new CouponDetails
                                             {
                                                 BarcodeNumber = coupon.BarcodeNumber,
                                                 ImageName = coupon.FullImageName,
                                                 PrimaryCustomerFirstName = customerDetails.PrimaryCustomerName1,
                                                 PrimaryCustomerMiddleName = customerDetails.PrimaryCustomerName2,
                                                 PrimaryCustomerLastName = customerDetails.PrimaryCustomerName3,
                                                 PrimaryClubcardnumber = customerDetails.PrimaryClubcardID,
                                                 AssociateCustomerFirstName = customerDetails.AssociateCustName1,
                                                 AssociateCustomerMiddleName = customerDetails.AssociateCustName2,
                                                 AssociateCustomerLastName = customerDetails.AssociateCustName3,
                                                 AssociateClubcardNumber = customerDetails.AssociateClubcardID
                                             }).ToList() as List<T>;
                    //    _logdata.CaptureData("formatted coupon details", newVoucherDetailsList);

                }
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator BC while getting formatted the coupon/voucher details", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
            return newVoucherDetailsList;
        }

        private void AddCouponsToPdf(XGraphics gfx, CouponDetails v, int x1, int y1, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {

                string pCustCardNo = v.PrimaryClubcardnumber.ToString();
                string aCustCardNo = v.AssociateClubcardNumber.ToString();
                string pCustFName = " ", pCustMName = " ", pCustLName = " ", aCustFName = " ", aCustMName = " ", aCustLName = " ";
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                if (!Convert.ToBoolean(template.IsHideCustomerName))
                {
                    pCustFName = v.PrimaryCustomerFirstName;
                    pCustMName = v.PrimaryCustomerMiddleName;
                    pCustLName = v.PrimaryCustomerLastName;
                    aCustFName = v.AssociateCustomerFirstName;
                    aCustMName = v.AssociateCustomerMiddleName;
                    aCustLName = v.AssociateCustomerLastName;
                }
                _logdata.RecordStep("Customer name added pdf by checking the conditon of if customer should present on the coupon or not");
                XFont font_val = new XFont(template.FontName, 9.0, XFontStyle.Regular, options);
                XFont font_CardNo = new XFont(template.FontName, 8.0, XFontStyle.Bold, options);
                XFont font_name = new XFont(template.FontName, 6.0, XFontStyle.Bold, options);
                XFont font_number = new XFont(template.FontName, 6.0, XFontStyle.Regular, options);

                string imagePath = HttpContext.Current.Server.MapPath(_configProvider.GetStringAppSetting(AppConfigEnum.CouponImageFolder)) + "/" + v.ImageName;
                _logdata.RecordStep(string.Format("imagePath: {0}", imagePath));
                this.imageDraw(imagePath, gfx, x1, y1);

                if (!string.IsNullOrEmpty(v.BarcodeNumber))
                {
                    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                    BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                    System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(312, 50);
                    ean1 = (System.Drawing.Bitmap)b.Encode(type, v.BarcodeNumber, System.Drawing.Color.Black, System.Drawing.Color.White, 312, 50);
                    //**************** START Vertical barcode/text ******************************
                    //Print barcode
                    gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 215), (double)(y1 + 258)));
                    this.imageDraw(ean1, gfx, x1 + 215, y1 + 258);
                    gfx.RotateAtTransform(90, new XPoint((double)(x1 + 215), (double)(y1 + 258)));
                    //Print barcode number
                    gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 265), (double)(y1 + 205)));
                    gfx.DrawString(v.BarcodeNumber, font_CardNo, XBrushes.Black, (double)(x1 + 265), (double)(y1 + 205), XStringFormats.Default);
                    gfx.RotateAtTransform(90, new XPoint((double)(x1 + 265), (double)(y1 + 205)));
                    //**************** END Vertical barcode/text *******************************
                }
                //******************* START Customer details ************************
                CustomerDisplayName customerName = new CustomerDisplayName();
                GeneralUtility name = new GeneralUtility();
                customerName = SetCustomerNameFields(pCustFName, pCustMName, pCustLName, "PrimCust", string.Empty);
                string pCustName = name.GetCustomerDisplayName(customerName, "COUPON");

                string aCustName = string.Empty;
                customerName = SetCustomerNameFields(aCustFName, aCustMName, aCustLName, "SecCust", pCustLName);
                aCustName = name.GetCustomerDisplayName(customerName, "COUPON");

                bool isCardsSame = false, isNamesSame = false;
                //*******************************************
                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)), font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 225), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName, font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 225), XStringFormats.Default);
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
                        gfx.DrawString((aCustName.Length > 23) ? (aCustName.Substring(0, 24)) : aCustName, font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 239), XStringFormats.Default);
                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName, font_name, XBrushes.Black, (double)(x1 + 98), (double)(y1 + 231), XStringFormats.Default);
                        isNamesSame = true;
                    }
                    gfx.DrawString(MasknFormatClubcard(aCustCardNo, true, 'X'), font_number, XBrushes.Black, (double)(x1 + 98), (double)(y1 + ((isCardsSame && !isNamesSame) ? 239 : 245)), XStringFormats.Default);
                }
                //******************* END Customer details ************************
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {

                throw GeneralUtility.GetCustomException("Failed in pdfgeneratorBC while adding coupons to pdf", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }

        }

        private void AddVouchersToPdf(XGraphics gfx, VoucherDetails v, int x1, int y1, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate.ToString(template.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

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
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }


                string pCustCardNo = v.PrimaryCustomerCardnumber.ToString();
                //_logdata.BlackLists.Add(v.PrimaryCustomerCardnumber.ToString());
                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();
                //_logdata.BlackLists.Add(v.AssociateCustomerCardnumber.ToString());
                string strCustomerPrinted = template.lblstrCustomerPrinted;

                string strClubcardVoucher = template.lblstrClubcardVouchers;

                string strFromTescoBank = template.lblstrFromTescoBank;

                string strClubcardWinner = template.lblstrClubcardWinner;

                string strTitleVoucher = template.lblstrTitleVoucher;

                string strClubcardChristmas = template.lblstrClubcardChristmas;
                string strBonusVouchers = template.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = template.lblstrSaverTop_UpVoucher;
                string strOnlineCode = template.lblstrOnlineCode;
                string strValidUntil = template.lblstrValidUntil;
                string isDecimalDisabled = template.isDisableDecimal;
                _logdata.RecordStep(string.Format("isDecimalDisabled: {0}", isDecimalDisabled));
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

                XFont txtFont = new XFont(template.FontName, 6.0, XFontStyle.Regular, options);
                XFont font_cc = new XFont(template.FontName, 7.0, XFontStyle.Bold, options);
                XFont font_TB = new XFont(template.FontName, 9.0, XFontStyle.Bold, options);
                XFont font_VertText = new XFont(template.FontName, 8.0, XFontStyle.Bold, options);
                _logdata.RecordStep(string.Format("VoucherType: {0}", Vtype));
                if (Vtype == "1")   //Clubcard voucher
                {
                    _logdata.RecordStep(string.Format("imgPath: {0}", imgPath));
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", gfx, x1, y1);

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

                    List<string> sTescoBankVoucher = new List<string>(_configProvider.GetStringAppSetting(AppConfigEnum.BankVoucherType).Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {
                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);


                    }
                    else    //Xmas bonus vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }

                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }

                //******************* Customer details ************************
                CustomerDisplayName customerName = new CustomerDisplayName();
                customerName = SetCustomerNameFields(pCustFName, pCustMName, pCustLName, "PrimCust", string.Empty);
               
                GeneralUtility name = new GeneralUtility();
                string pCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");

                string aCustName = string.Empty;
                customerName = SetCustomerNameFields(aCustFName, aCustMName, aCustLName, "SecCust", pCustLName);
                aCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
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
                            gfx.DrawString((aCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
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
                string strCurrencySymbol = template.lblstrCurrencySymbol;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrCurrencySymbol").ToString();
                string strCurrencyAllignment = template.lblstrCurrencyAllignment;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrCurrencyAllignment").ToString();

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = template.lblstrCurrencyDecimalSymbol;
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

                    strCurrencySymbol = template.lblstrCurrencySymbol;
                    strCurrencyAllignment = template.lblstrCurrencyAllignment;
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
                        //gfx.DrawString(a2, font_val1, XBrushes.Black, (double)(x1 + 82), (double)(y1 + 0x67), XStringFormats.Default);

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

                this.imageDraw(ean1, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                //**********************************************
                _logdata.RecordStep("adding vouchers to pdf done successfully.");
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator BC while adding vouchers to pdf", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
        }

        private void AddVouchersToPdfMY(XGraphics gfx, VoucherDetails v, int x1, int y1, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate.ToString(template.DateFormat);
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

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
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }

                string pCustCardNo = v.PrimaryCustomerCardnumber.ToString();

                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();

                string strCustomerPrinted = template.lblstrCustomerPrinted;
                string strClubcardVoucher = template.lblstrClubcardVouchers;
                string strFromTescoBank = template.lblstrFromTescoBank;
                string strClubcardWinner = template.lblstrClubcardWinner;
                string strTitleVoucher = template.lblstrTitleVoucher;
                string strClubcardChristmas = template.lblstrClubcardChristmas;
                string strBonusVouchers = template.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = template.lblstrSaverTop_UpVoucher;
                string strOnlineCode = template.lblstrOnlineCode;
                string strValidUntil = template.lblstrValidUntil;
                string isDecimalDisabled = template.isDisableDecimal;
                _logdata.RecordStep(string.Format("isDecimalDisabled{0}", isDecimalDisabled));
                //\
                //Clubcard Christmas
                //Bonus Voucher
                //Clubcard Christmas
                //Saver Top-Up Voucher
                //Online Code
                //Valid Until 

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, EAN, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                //Different font sizes for different text

                XFont txtFont = new XFont("Verdana", 6.0, XFontStyle.Regular, options);
                XFont font_cc = new XFont("Verdana", 7.0, XFontStyle.Bold, options);
                XFont font_TB = new XFont("Verdana", 9.0, XFontStyle.Bold, options);
                XFont font_VertText = new XFont("Verdana", 8.0, XFontStyle.Bold, options);
                _logdata.RecordStep(string.Format("Vouchertype{0}", Vtype));
                if (Vtype == "1")   //Clubcard voucher
                {
                    _logdata.RecordStep(string.Format("imgPath{0}", imgPath));
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", gfx, x1, y1);

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

                    List<string> sTescoBankVoucher = new List<string>(_configProvider.GetStringAppSetting(AppConfigEnum.BankVoucherType).Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {
                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);
                    }
                    else    //Xmas bonus vouchers
                    {
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }

                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }

                //******************* Customer details ************************
                CustomerDisplayName customerName = new CustomerDisplayName();
                GeneralUtility name = new GeneralUtility();
                customerName = SetCustomerNameFields(pCustFName, pCustMName, pCustLName, "PrimCust", string.Empty);

                string pCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");
                //If associate customer doesn't have surname, add main customer's surname
                string aCustName = string.Empty;
                customerName = SetCustomerNameFields(aCustFName, aCustMName, aCustLName, "SecCust", pCustLName);
                aCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
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
                            gfx.DrawString((aCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }

                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
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
                string strCurrencySymbol = template.lblstrCurrencySymbol;
                string strCurrencyAllignment = template.lblstrCurrencyAllignment;

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = template.lblstrCurrencyDecimalSymbol;
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
                    strCurrencySymbol = template.lblstrCurrencySymbol;
                    strCurrencyAllignment = template.lblstrCurrencyAllignment;
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

                this.imageDraw(ean1, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                _logger.Submit(_logdata);
                //**********************************************
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("MY|Failed in pdfGeneratorBC while adding vouchers to pdf.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
        }

        private void AddVouchersToPdfPL(XGraphics gfx, VoucherDetails v, int x1, int y1, PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {
                string EAN = v.VoucherNumberToPrint;
                string Alp = v.AlphaCode;
                string exp = v.ExpiryDate.ToString(template.DateFormat);
                string val = v.Value;
                string Vtype = v.VoucherType;

                string pCustFName = string.Empty;
                string pCustMName = string.Empty;
                string pCustLName = string.Empty;
                string aCustFName = string.Empty;
                string aCustMName = string.Empty;
                string aCustLName = string.Empty;

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

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
                    pCustFName = v.PrimaryCustomerFirstname;
                    pCustMName = v.PrimaryCustomerMiddlename;
                    pCustLName = v.PrimaryCustomerLastname;
                    aCustFName = v.AssociateCustomerFirstname;
                    aCustMName = v.AssociateCustomerMiddlename;
                    aCustLName = v.AssociateCustomerLastname;
                }


                string pCustCardNo = v.PrimaryCustomerCardnumber.ToString();
                string aCustCardNo = v.AssociateCustomerCardnumber.ToString();
                string strCustomerPrinted = template.lblstrCustomerPrinted;
                string strClubcardVoucher = template.lblstrClubcardVouchers;
                string strFromTescoBank = template.lblstrFromTescoBank;
                string strClubcardWinner = template.lblstrClubcardWinner;
                string strTitleVoucher = template.lblstrTitleVoucher;
                string strClubcardChristmas = template.lblstrClubcardChristmas;
                string strBonusVouchers = template.lblstrBonusvoucher;
                string strSaverTop_UpVoucher = template.lblstrSaverTop_UpVoucher;
                string strOnlineCode = template.lblstrOnlineCode;
                string strValidUntil = template.lblstrValidUntil;
                string isDecimalDisabled = template.isDisableDecimal;
                _logdata.RecordStep(string.Format("isDecimalDisabled{0}", isDecimalDisabled));
                //\
                //Clubcard Christmas
                //Bonus Voucher
                //Clubcard Christmas
                //Saver Top-Up Voucher
                //Online Code
                //Valid Until 

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
                _logdata.RecordStep(string.Format("Vtype{0}", Vtype));
                if (Vtype == "1")   //Clubcard voucher
                {
                    this.imageDraw(@"" + imgPath + "Vouchers_Reward.jpg", gfx, x1, y1);
                    _logdata.RecordStep(string.Format("imgPath{0}", imgPath));
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

                    List<string> sTescoBankVoucher = new List<string>(_configProvider.GetStringAppSetting(AppConfigEnum.BankVoucherType).Split(','));

                    if (sTescoBankVoucher.Contains(EAN.Substring(2, 2))) // Tesco Bank Vouchers
                    {
                        gfx.DrawString(strFromTescoBank, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 132), XStringFormats.Default);
                    }
                }
                else if (Vtype == "4")   //Bonus voucher
                {

                    if (EAN.Substring(4, 3) == "051" || EAN.Substring(4, 3) == "150")  //Amex vouchers
                    {
                        _logdata.RecordStep("Voucher Type - Amex Bonus Voucher");
                        _logdata.RecordStep(string.Format("imgPath{0}", imgPath));
                        this.imageDraw(@"" + imgPath + "Vouchers_AMX.jpg", gfx, x1, y1);

                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardWinner, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strTitleVoucher, font_TB, XBrushes.Black, (double)(x1 + 40), (double)(y1 + 179), XStringFormats.Default);
                    }
                    else    //Xmas bonus vouchers
                    {
                        _logdata.RecordStep("Voucher Type - Xmas Bonus Voucher");
                        _logdata.RecordStep(string.Format("imgPath{0}", imgPath));
                        this.imageDraw(@"" + imgPath + "Vouchers_Bonus.jpg", gfx, x1, y1);
                        gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                        gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 27), (double)(y1 + 167), XStringFormats.Default);
                        gfx.DrawString(strBonusVouchers, font_TB, XBrushes.Black, (double)(x1 + 34), (double)(y1 + 179), XStringFormats.Default);
                    }
                    gfx.DrawString(strBonusvoucher, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                else if (Vtype == "5")  //Top up voucher
                {
                    _logdata.RecordStep("Voucher Type - Top up voucher");

                    _logdata.RecordStep(string.Format("imgPath{0}", imgPath));
                    this.imageDraw(@"" + imgPath + "Vouchers_TopUp.jpg", gfx, x1, y1);
                    gfx.DrawString(strCustomerPrinted, font_TB, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 155), XStringFormats.Default);
                    gfx.DrawString(strClubcardChristmas, font_TB, XBrushes.Black, (double)(x1 + 25), (double)(y1 + 167), XStringFormats.Default);
                    gfx.DrawString(strSaverTop_UpVoucher, font_TB, XBrushes.Black, (double)(x1 + 20), (double)(y1 + 179), XStringFormats.Default);
                    gfx.DrawString(strTopup, txtFont, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 200), XStringFormats.Default);
                }
                //******************* Customer details ************************
                CustomerDisplayName customerName = new CustomerDisplayName();
                GeneralUtility name = new GeneralUtility();

                string pCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");
                customerName = SetCustomerNameFields(pCustFName, pCustMName, pCustLName, "PrimCust", string.Empty);
               
               
                string aCustName = string.Empty;
                customerName = SetCustomerNameFields(aCustFName, aCustMName, aCustLName, "SecCust", pCustLName);
                aCustName = name.GetCustomerDisplayName(customerName, "VOUCHER");

                bool isCardsSame = false;
                bool isNamesSame = false;
                //*******************************************

                //To print primary customer name
                if (pCustName.Length > 23)
                {
                    gfx.DrawString((pCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
                }
                else
                {
                    gfx.DrawString(pCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 130), XStringFormats.Default);
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
                            gfx.DrawString((aCustName.Substring(0, 24)), font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        else
                        {
                            gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 150), XStringFormats.Default);
                        }
                        isNamesSame = true;
                    }
                    else if ((pCustName.ToUpper() != aCustName.ToUpper()) && !isCardsSame)
                    {
                        gfx.DrawString(aCustName, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 140), XStringFormats.Default);
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
                string strCurrencySymbol = template.lblstrCurrencySymbol;
                string strCurrencyAllignment = template.lblstrCurrencyAllignment;

                if (aa == strCurrencySymbol + "0.50")
                {
                    aa = "50p";
                    gfx.DrawString(aa, font_val, XBrushes.Black, (double)(x1 + 30), (double)(y1 + 110), XStringFormats.Default);
                }
                else
                {
                    string strDecimalSymbol = ".";
                    string strCurrencyDecimalSymbol = template.lblstrCurrencyDecimalSymbol;
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
                        a1 = aa.Substring(0, a);                        // a2 = aa.Substring(a, 3);
                        // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                        a3 = aa.Substring(a, 1);
                        a5 = aa.Substring(a, 3);
                        a2 = a5.Replace(strCurrencyDecimalSymbol, "");
                    }

                    strCurrencySymbol = template.lblstrCurrencySymbol;
                    strCurrencyAllignment = template.lblstrCurrencyAllignment;
                    _logdata.RecordStep(string.Format("strCurrencyAllignment{0}", strCurrencyAllignment));
                    if (strCurrencyAllignment == "RIGHT")
                    {
                        // Added to display comma and currency symbol below the currency - PL CR - Legal Requirement
                        a4 = strCurrencySymbol;
                    }
                    else
                    {
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

                this.imageDraw(ean1, gfx, x1 + 270, y1 + 150);
                XFont font = new XFont("Verdana", 10.0, XFontStyle.Regular, options);
                gfx.DrawString(EAN, font, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 200), XStringFormats.Default);

                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.DrawString(EAN, font_VertText, XBrushes.Black, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 270), (double)(y1 + 145)));
                //**********************************************
                _logger.Submit(_logdata);

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("PL|Failed in pdfGeneratorBC while Adding Vouchers to pdf", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        private PdfDocument GetPDFDocument(List<Token> list, BoostBackgroundTemplate template)
        {
            PdfDocument document = new PdfDocument();
            LogData _logdata = new LogData();
            try
            {
                _logdata.RecordStep("setting up the template ht,wd,left,top pixels");
                PdfPage page = document.AddPage();
                XGraphics gfx = null;
                int x1 = 10, y1 = 10, vc = 0;
                if (list != null)
                {
                    page.Width = 590;//template.DocumentWidth;
                    gfx = XGraphics.FromPdfPage(page);
                    x1 = 10;//template.Left;
                    y1 = 10;//template.Top;
                    vc = 0;
                    list = this.GetFormattedDocumentForBoost(list);
                    foreach (Token v in list)
                    {
                        if (vc == 3)
                        {
                            page = document.AddPage();
                            page.Width = 590;
                            gfx = XGraphics.FromPdfPage(page);
                            x1 = 10;
                            y1 = 10;
                            vc = 1;
                        }
                        else
                        {
                            vc++;
                        }

                        this.putPDFExchanges(gfx, v as Token, x1, y1, template);
                        y1 += 0x109;

                    }
                }
                _logger.Submit(_logdata);
                document.Close();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator BC while getting pdf Document", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }

            return document;
        }

        private List<Token> GetFormattedDocumentForBoost(List<Token> list)
        {
            List<Token> tokenDetailsList = list as List<Token>;
            LogData _logdata = new LogData();
            try
            {

                //  _logdata.CaptureData("list of boost details before formatting", list);
                _logdata.RecordStep("Formatting of the boost template");
                var template = this._template as BoostBackgroundTemplate;

                string strCurrencySymbol = template.lblstrCurrencySymbol;



                tokenDetailsList = (from token in tokenDetailsList
                                    select new Token
                                    {
                                        TokenID = token.TokenID,
                                        SupplierTokenCode = token.SupplierTokenCode,
                                        ValidUntil = token.ValidUntil,
                                        ValidUntilFormated = Convert.ToDateTime(token.ValidUntil).ToString(dateformat),
                                        ProductTokenValue = token.ProductTokenValue,
                                        ProductTokenValuewithcurrency = String.Format("{0}{1}", strCurrencySymbol, token.TokenValue),
                                        QualifyingSpend = token.QualifyingSpend,
                                        Includes = token.Includes,
                                        Excludes = token.Excludes,
                                        TermsAndConditions = token.TermsAndConditions
                                    }).ToList() as List<Token>;
                _logdata.CaptureData("After formatting the boost document", tokenDetailsList);
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in formatting the boost document", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }

            return tokenDetailsList;
        }

        private void putPDFExchanges(XGraphics gfx, Token v, int x1, int y1, BoostBackgroundTemplate template)
        {
            LogData _logData = new LogData();
            try
            {
                sConfiguredClubcard = template.ReplaceClubcardPrefix;// ConfigurationManager.AppSettings["ReplaceClubcardPrefix"].ToString();
                imgPath = template.PrintBGImagePath;// ConfigurationManager.AppSettings["PrintBGImagePath"].ToString();
                fontLoadPath = template.FontPath;// ConfigurationManager.AppSettings["FontPath"].ToString();
                strIsAlphaCodeRequired = template.IsAlphaCodeRequired;// ConfigurationManager.AppSettings["IsAlphaCodeRequired"].ToString();
                culture = template.CultureDefaultloc;// ConfigurationManager.AppSettings["CultureDefaultloc"];

                string strToken = template.lblstrToken;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrToken").ToString();

                string strClubcardBoostatTesco = template.lblstrClubcardBoostatTesco;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrClubcardBoostatTesco").ToString();
                string strCustomerPrintedClubcardBoostToken = template.lblstrCustomerPrintedClubcardBoostToken;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrCustomerPrintedClubcardBoostToken").ToString();
                string strOFF = template.lblstrOFF;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrOFF").ToString();
                string strWhenYouSpend = template.lblstrWhenYouSpend;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrWhenYouSpend").ToString();
                string strOrMoreOn = template.lblstrOrMoreOn;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrOrMoreOn").ToString();
                string strInaSingleTransaction = template.lblstrInaSingleTransaction;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrInaSingleTransaction").ToString();
                string strValidUntil = template.lblstrValidUntil;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrValidUntil").ToString();

                string strLine1 = template.lblstrLine1;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine1").ToString();
                string strLine2 = template.lblstrLine2;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine2").ToString();
                string strLine3 = template.lblstrLine3;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine3").ToString();
                string strLine4 = template.lblstrLine4;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine4").ToString();
                string strLine5 = template.lblstrLine5;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine5").ToString();
                string strLine6 = template.lblstrLine6;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine6").ToString();
                string strLine7 = template.lblstrLine7;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine7").ToString();
                string strLine8 = template.lblstrLine8;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine8").ToString();
                string strLine9 = template.lblstrLine9;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine9").ToString();
                string strLine10 = template.lblstrLine10;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine10").ToString();
                string strLine11 = template.lblstrLine11;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine11").ToString();
                string strLine12 = template.lblstrLine12;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine12").ToString();
                string strLine13 = template.lblstrLine13;//Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrLine13").ToString();

                string strSerialNumber = template.lblstrSerialNumber;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrSerialNumber").ToString();
                string strDatePrinted = template.lblstrDatePrinted;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrDatePrinted").ToString();
                string strDateFormat = template.lblstrDateFormat;// Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrDateFormat").ToString();

                PdfDocument document = new PdfDocument
                {
                    Info = { Title = strToken }
                };

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;

                System.Drawing.Bitmap ean1 = new System.Drawing.Bitmap(325, 50);
                ean1 = (System.Drawing.Bitmap)b.Encode(type, v.SupplierTokenCode, System.Drawing.Color.Black, System.Drawing.Color.White, 325, 50);

                XFont font_cc = new XFont("Verdana", 7.0, XFontStyle.Bold);
                XFont font_alp = new XFont("Verdana", 7.0, XFontStyle.Regular);
                XFont font_val = new XFont("Verdana", 9.0, XFontStyle.Regular);
                XFont font_val9bold = new XFont("Verdana", 9.0, XFontStyle.Bold);
                XFont font_alp_large = new XFont("Verdana", 13.0, XFontStyle.Bold);
                XFont font_small = new XFont("Verdana", 6.5, XFontStyle.Regular);

                this.imageDraw(@"" + imgPath + "Exchange_Tokens.jpg", gfx, x1, y1);

                //***********************
                gfx.DrawString(strClubcardBoostatTesco, font_alp_large, XBrushes.Black, (double)(x1 + 55), (double)(y1 + 90), XStringFormats.Default);
                gfx.DrawString(strCustomerPrintedClubcardBoostToken, font_val, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 105), XStringFormats.Default);
                gfx.DrawString(v.ProductTokenValuewithcurrency + " " + strOFF, font_val9bold, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 115), XStringFormats.Default);
                gfx.DrawString(strWhenYouSpend + v.QualifyingSpend + strOrMoreOn, font_alp, XBrushes.Black, (double)(x1 + 100), (double)(y1 + 125), XStringFormats.Default);

                //Based on the length of "Includes" align the text center.
                if (v.Includes.Length > 50)
                {
                    string includesStr = string.Empty;
                    string includesStr1 = string.Empty;
                    string[] upprBnd = v.Includes.Split(' ');

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
                else if (v.Includes.Length >= 40 && v.Includes.Length <= 50)
                {
                    gfx.DrawString(v.Includes, font_cc, XBrushes.Black, (double)(x1 + 50), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (v.Includes.Length >= 30 && v.Includes.Length <= 40)
                {
                    gfx.DrawString(v.Includes, font_cc, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (v.Includes.Length >= 20 && v.Includes.Length <= 30)
                {
                    gfx.DrawString(v.Includes, font_cc, XBrushes.Black, (double)(x1 + 110), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (v.Includes.Length >= 10 && v.Includes.Length <= 20)
                {
                    gfx.DrawString(v.Includes, font_cc, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }
                else if (v.Includes.Length <= 10)
                {
                    gfx.DrawString(v.Includes, font_cc, XBrushes.Black, (double)(x1 + 135), (double)(y1 + 135), XStringFormats.Default);
                    gfx.DrawString(strInaSingleTransaction, font_alp, XBrushes.Black, (double)(x1 + 120), (double)(y1 + 145), XStringFormats.Default);
                }

                //Based on the length of "Excludes" align the text center.
                if (v.Excludes.Length >= 50)
                {
                    int yAxis = 155;
                    int i = 0;
                    StringBuilder excludesStr;

                    while (i <= v.Excludes.Length)
                    {
                        int exBrkPoint = 58;

                        if (exBrkPoint + i < v.Excludes.Length)
                        {
                            exBrkPoint = v.Excludes.IndexOf(" ", exBrkPoint + i);
                        }
                        else
                        {
                            exBrkPoint = v.Excludes.Length;
                        }

                        if (exBrkPoint == -1)
                        {
                            exBrkPoint = v.Excludes.Length;
                        }

                        excludesStr = new StringBuilder();

                        for (int j = i; j <= (exBrkPoint - 1); j++)
                        {
                            //excludesStr = excludesStr + Excludes[j];
                            excludesStr.Append(v.Excludes[j]);

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
                else if (v.Excludes.Length >= 30 && v.Excludes.Length <= 50)
                {
                    gfx.DrawString(v.Excludes, font_small, XBrushes.Black, (double)(x1 + 80), (double)(y1 + 162), XStringFormats.Default);
                }
                else if (v.Excludes.Length <= 30)
                {
                    gfx.DrawString(v.Excludes, font_small, XBrushes.Black, (double)(x1 + 100), (double)(y1 + 162), XStringFormats.Default);
                }

                gfx.DrawString(strValidUntil + v.ValidUntilFormated, font_cc, XBrushes.Black, (double)(x1 + 110), (double)(y1 + 192), XStringFormats.Default);
                this.imageDraw(ean1, gfx, x1 + 40, y1 + 200);
                XFont font = new XFont("Verdana", 9.0, XFontStyle.Regular);
                gfx.DrawString(v.SupplierTokenCode, font, XBrushes.Black, (double)(x1 + 88), (double)(y1 + 248), XStringFormats.Default);
                //**************** Vertical text ******************************
                gfx.RotateAtTransform(-90, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                gfx.DrawString(v.SupplierTokenCode, font_val, XBrushes.Black, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                gfx.RotateAtTransform(90, new XPoint((double)(x1 + 300), (double)(y1 + 170)));
                //*************************************************************
                //Terms & Conditions
                XFont font_tc = new XFont("Verdana", 6.0, XFontStyle.Regular);
                gfx.DrawString(strLine1, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 100), XStringFormats.Default);
                gfx.DrawString(strLine2, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 108), XStringFormats.Default);
                gfx.DrawString(strLine3, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 116), XStringFormats.Default);
                gfx.DrawString(strLine4 + v.ProductTokenValuewithcurrency + strLine5, font_tc, XBrushes.Black, (double)(x1 + 320), (double)(y1 + 124), XStringFormats.Default);
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
                gfx.DrawString(strSerialNumber + v.TokenID, font_exp01, XBrushes.Black, (double)(x1 + 318), (double)(y1 + 240), XStringFormats.Default);
                //Date printed
                gfx.DrawString(strDatePrinted + DateTime.Now.ToString(dateformat), font_exp01, XBrushes.Black, (double)(x1 + 410), (double)(y1 + 240), XStringFormats.Default);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator  -putPDFExchanges", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

        private void imageDraw(string gif, XGraphics gfx, int x, int y)
        {
            if (File.Exists(gif))
            {
                XImage image = XImage.FromFile(gif);
                gfx.DrawImage(image, x, y);
            }
        }
        //same method is duplicated.
        private void imageDraw(Image gif, XGraphics gfx, int x, int y)
        {
            XImage image = gif;
            gfx.DrawImage(image, x, y);
        }

        //I haven't find any references for this menthod:Laxmi.
        private void AddDocumentToResponse(PdfSharp.Pdf.PdfDocument document)
        {
            LogData _logdata = new LogData();

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
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator while adding Document to response", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
        }

        //I haven't find any references for this menthod:Laxmi.

        public void AddDocumentToResponse(PdfSharp.Pdf.PdfDocument document, BoostBackgroundTemplate template)
        {
            LogData _logdata = new LogData();

            try
            {
                // var browserInformation = HttpContext.Current.Request.Browser;
                string genPathFileName = template.lblstrTokenFileName;
                //************************ Save PDF document to memory stream  ****************
                MemoryStream stream = new MemoryStream();
                document.Save(stream, false);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("Content-Length", stream.Length.ToString());
                //HttpContext.Current.Response.AppendHeader("cache-control", browserInformation.Browser == "IE" ? "private" : "no-cache");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + genPathFileName);
                HttpContext.Current.Response.BinaryWrite(stream.ToArray());
                HttpContext.Current.Response.Flush();
                stream.Dispose();
                stream.Close();
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator while adding Document to response", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata }
                            });
            }
        }

        private void InitialPrivateVariables(PdfBackgroundTemplate template)
        {
            LogData _logdata = new LogData();
            try
            {
                _logdata.RecordStep("This method is for just initial private variables of the template");
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
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("failed in intial private variables of the template", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
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
            string middleGroup = string.Empty;
            string maskedClubCard = string.Empty;
            LogData _logdata = new LogData();
            try
            {
                _logdata.RecordStep("Masking of the clubcard");
                if (clubcardNumber.Trim().Length > 15)
                {
                    middleGroup = clubcardNumber.Substring(6, 8);
                    StringBuilder MG = new StringBuilder(clubcardNumber);
                    MG.Replace(middleGroup, " XXXX XXXX ");

                    //return the formatted card number
                    maskedClubCard = MG.ToString();
                }
                else
                {
                    //if clubcard number found lesser than 16 digits the unformatted clubcard number is returned
                    maskedClubCard = clubcardNumber;
                }
                _logger.Submit(_logdata);
                return maskedClubCard;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in masking of the clubcard", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }
        #region static methods
        public static string GetDecimalTrimmedCurrencyVal(string currencyVal)
        {
            string formattedVal = currencyVal;
            formattedVal = (currencyVal.Contains(",") ? currencyVal.TrimEnd('0').TrimEnd(',') : currencyVal.Contains(".") ? currencyVal.TrimEnd('0').TrimEnd('.') : formattedVal);
            formattedVal = formattedVal.Contains(".") ? currencyVal : formattedVal.Contains(",") ? currencyVal : formattedVal;

            return formattedVal;
        }


        private static string GetXMLFromObject(object o)
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
                //ToDO: Add some logging action here.
                throw ex;
            }
            finally
            {
                sw.Close();
                tw.Close();
            }
        }
        #endregion static methods

        #region IPDFGenerator Members

        public MemoryStream GetPDFDocumentStream<T, S>(List<T> list, S template, AccountDetails customerAccountDetails)
        {
            MemoryStream stream = new MemoryStream();
            LogData _logdata = new LogData();
            try
            {
                this._template = template;
                this._lstModel = list;
                this._customerAccountDetails = customerAccountDetails;
                _logdata.CaptureData("we are passing specific type list to the dictonary", list);

                if (list.Count > 0)
                {
                    PdfDocument document = this._pdfHandlers[typeof(T)]();
                    document.Save(stream, false);
                }
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenerator in getting PDF Document Stream", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return stream;
        }

        public MemoryStream GetCouponsAndVouchersDocument(VoucherandCouponDetails list, AccountDetails customerAccountDetails, PdfBackgroundTemplate resourceValues)
        {
            MemoryStream stream = new MemoryStream();
            LogData _logdata = new LogData();
            try
            {
                this._template = resourceValues;
                this._lstModel = list;
                this._customerAccountDetails = customerAccountDetails;
                //VoucherandCouponDetails list has sensitive info
                //  _logdata.CaptureData("we are passing vouchers and coupon list to the dictonary", list);
                if (list != null)
                {
                    PdfDocument document = this._pdfHandlers[typeof(VoucherandCouponDetails)]();
                    document.Save(stream, false);
                }
                _logger.Submit(_logdata);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in pdfGenertor BC while getting Coupons and Vouchers Document", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return stream;
        }

        #endregion IPDFGenerator Members

        #region Landing Methods

        private PdfDocument GetBoostAtTescoDoc()
        {
            return this.GetPDFDocument(this._lstModel as List<Token>, this._template as BoostBackgroundTemplate);
        }

        private PdfDocument GetVoucherandCoupon()
        {
            return this.GetPDFCouponsAndVouchersDocument
                (
                this._lstModel as VoucherandCouponDetails,
                this._customerAccountDetails,
                this._template as PdfBackgroundTemplate
                );
        }

        private PdfDocument GetCouponsDoc()
        {
            return this.GetPDFDocument<CouponDetails>(
                            this._lstModel as List<CouponDetails>,
                            this._customerAccountDetails,
                            this._template as PdfBackgroundTemplate);
        }

        private PdfDocument GetVouchersDoc()
        {
            return this.GetPDFDocument<VoucherDetails>(
                            this._lstModel as List<VoucherDetails>,
                            this._customerAccountDetails,
                            this._template as PdfBackgroundTemplate);
        }

        private PdfDocument GetClubcardDoc()
        {

            return this.GetNewClubcardPDFDocument(
                            (this._lstModel[0]),
                            this._template as PdfBackgroundTemplate);
        }

        #endregion Landing Methods

        private CustomerDisplayName SetCustomerNameFields(string firstName, string middleName, string lastName, string customerType, string PrimCustLName)
        {
            CustomerDisplayName customerName = new CustomerDisplayName();

            customerName.Name1 = string.IsNullOrEmpty(firstName) ? string.Empty : firstName;
            customerName.Name2 = string.IsNullOrEmpty(middleName) ? string.Empty : middleName;
            if (customerType.Equals("PrimCust"))
            {
                customerName.Name3 = string.IsNullOrEmpty(lastName) ? string.Empty : lastName;

            }
            else if (customerType.Equals("SecCust"))
            {
                customerName.Name3 = string.IsNullOrEmpty(lastName) ? PrimCustLName : lastName;
            }
            customerName.TitleEnglish = string.Empty;
            return customerName;
        }
    }
}
