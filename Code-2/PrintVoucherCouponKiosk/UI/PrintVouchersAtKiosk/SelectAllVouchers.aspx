<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectAllVouchers.aspx.cs"
    Inherits="PrintVouchersAtKiosk.SelectAllVouchers" Culture="auto" UICulture="auto"
    meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector">
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" AlternateText="img Tesco Clubcard Logo" runat="server"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em></div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgBreadCrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgBreadCrumbsResource1" /></div>
        </div>
        <div id="body_wrapper">
            <div>
                <table cellpadding="0" cellspacing="0" class="style1" border="0">
                    <tr>
                        <td colspan="4">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 50%">
                            <div class="msgVoucherSummary">
                                <asp:Literal ID="lityouhave" runat="server" Text="You have&nbsp" meta:resourcekey="lityouhaveResource1"></asp:Literal>
                                <asp:Literal ID="litVoucherCount" runat="server" Text=" 14 " meta:resourcekey="litVoucherCountResource1"></asp:Literal>
                                <asp:Literal ID="litClubcardVouchersWorth" runat="server" Text="Clubcard vouchers worth&nbsp"
                                    meta:resourcekey="litClubcardVouchersWorthResource1"></asp:Literal>
                                <asp:Literal ID="litVoucherValue" runat="server" Text=" 241.50 " meta:resourcekey="litVoucherValueResource1"></asp:Literal>
                                <asp:Label ID="litVoucherErrorMessage" runat="server"  Visible="false" meta:resourcekey="litClubcardVoucherError"></asp:Label>
                            </div>
                        </td>
                        <td rowspan="3" style="margin-top: 50px;" align="right">
                            <asp:Image ID="imgVoucher" runat="server" meta:resourcekey="imgVoucherResource1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="greybtn">
                            <asp:LinkButton ID="lnkChooseVouchers" runat="server" OnClick="lnkChooseVouchers_Click"
                                meta:resourcekey="lnkChooseVouchersResource1" style="margin-right:0px">
                                <span class="allvouchers">CHOOSE<br />VOUCHERS</span>
                            </asp:LinkButton>
                        </td>
                        <td style="padding-top: 5px; padding-right: 23px; font-weight: bold; font-size: 22px;">
                            or
                        </td>
                        <td class="confirmVoucherSummary" style="vertical-align:top;padding-top:8px;">
                            <asp:LinkButton ID="lnkPrint" runat="server" OnClick="btnPrint_Click" meta:resourcekey="lnkPrintResource1"> <span class="twolines" style="font-weight:bold">PRINT ALL <br /> VOUCHERS</span> </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="body_wrapperVoucherSummary1">
            <hr class="dottedline" />
        </div>
        <div id="body_wrapperVoucherSummary2">
            <div>
                <table cellpadding="0" cellspacing="0" class="style1" border="0">
                    <tr>
                        <td colspan="4">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 50%">
                            <div class="msgVoucherSummary">
                                <asp:Literal ID="litYouhavecoupons" runat="server" Text="You have&nbsp" meta:resourcekey="litYouhavecouponsResource1"></asp:Literal>
                                <asp:Literal ID="litCouponCount" runat="server" Text=" 14 " meta:resourcekey="litCouponCountResource1"></asp:Literal>
                                <asp:Literal ID="litClubcardCoupons" runat="server" Text="Clubcard Coupons" meta:resourcekey="litClubcardCouponsResource1"></asp:Literal>
                                <asp:Label ID="litCouponErrorMessage" runat="server" Text="CouponServiceDown" Visible="false" meta:resourcekey="litClubcardCouponError"></asp:Label>
                            </div>
                        </td>
                        <td rowspan="3" style="margin-top: 50px;" align="right">
                            <asp:Image ID="imgCoupon" runat="server" meta:resourcekey="imgCouponResource1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="greybtn" style="margin-right:0px">
                            <asp:LinkButton ID="lnkChooseCoupons" runat="server" OnClick="lnkChooseCoupons_Click"
                                meta:resourcekey="lnkChooseCouponsResource1" style="margin-right:0px"> 
                                <span class="allvouchers">CHOOSE<br />COUPONS</span>
                            </asp:LinkButton>
                        </td>
                        <td style="padding-top: 5px; padding-right: 23px; font-weight: bold; font-size: 22px;">
                            or
                        </td>
                        <td class="confirmVoucherSummary"  style="vertical-align:top;padding-top:8px;">
                            <asp:LinkButton ID="lnkPrintAllCoupons" runat="server" 
                                meta:resourcekey="lnkPrintAllCouponsResource1" 
                                onclick="lnkPrintCoupons_Click"> <span class="twolines" style="font-weight:bold">PRINT ALL <br /> COUPONS</span> </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="VoucherSummaryBackCancel">
                <div class="buttons" style="width: 1170px">
                    <%--<div class="back">
                        <asp:LinkButton ID="lnkBack" runat="server" meta:resourcekey="lnkBackResource1" onclick="lnkBack_Click" ><div>Back</div></asp:LinkButton>
                    </div>--%>
                    <div class="greybtn">
                        <asp:LinkButton ID="lnkCancel" runat="server" meta:resourcekey="lnkCancelResource1" onclick="lnkCancel_Click"><span class="cancelStart">CANCEL</span><span class="cancelStartagain">and start again</span></asp:LinkButton>
                    </div>
                    <div class="greybtn">
                        <asp:LinkButton ID="lnkTerms" runat="server" Text="&lt;span class=&quot;terms&quot;&gt;TERMS &amp;amp;&lt;br /&gt;CONDITIONS&lt;/span&gt;"
                            meta:resourcekey="lnkTermsResource1" onclick="lnkTerms_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
