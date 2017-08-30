namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Common
{
    public class PdfBackgroundTemplate
    {
        public string ReplaceClubcardPrefix { get; set; }
        public string PrintBGImagePath { get; set; }
        public string FontPath { get; set; }
        public string IsAlphaCodeRequired { get; set; }

        public string lblstrClubcardVouchers { get; set; }
        public string CultureDefaultloc { get; set; }
        public string IsHideCustomerName { get; set; }

        public string lblstrBonusvoucher { get; set; }
        public string lblstrTopup { get; set; }


        public string lblClubcardVoucher_Culture { get; set; }
        public string lblstrBonusvoucher_Culture { get; set; }
        public string lblstrTopup_Culture { get; set; }
        public string lblstrCouponsFileName { get; set; }
        public string lblstrCustomerPrinted { get; set; }
        public string lblstrClubcardVoucher { get; set; }
        public string lblstrFromTescoBank { get; set; }


        public string lblstrClubcardWinner { get; set; }
        public string lblstrTitleVoucher { get; set; }
        public string lblstrClubcardChristmas { get; set; }
        public string lblstrBonusVouchers { get; set; }
        public string lblstrSaverTop_UpVoucher { get; set; }
        public string lblstrOnlineCode { get; set; }
        public string lblstrValidUntil { get; set; }


        public string lblstrCurrencySymbol { get; set; }
        public string lblstrCurrencyAllignment { get; set; }
        public string lblstrCurrencyDecimalSymbol { get; set; }
        public string lblstrTitleCoupon { get; set; }
        public string lblstrVouchersAndCouponsFileName { get; set; }

		public string lblstrTitleClubcard { get; set; }         
		public string isDisableDecimal { get; set; }

        public string Culture { get; set; }
        public string FontName { get; set; }
        public string DateFormat { get; set; }
        public int ItemPerPage { get; set; }
        public int DocumentWidth { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }
}
