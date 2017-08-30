<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponDetails.aspx.cs" Inherits="PrintVouchersAtKiosk.CouponDetails" Culture="auto" UICulture="auto"
    meta:resourcekey="PageResource2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="CSS/kiosk-styles.css" type="text/css" media="screen, projector"/>
    <script type="text/javascript" language="javascript">
        window.external.showKeyboard(0);
        window.external.setTimeout(60);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="wrapper">
        <div id="header">
            <div id="navigation">
                <asp:Image ID="imgTescoClubcardLogo" runat="server" AlternateText="Tesco Clubcard Logo"
                    Width="234px" Height="24px" meta:resourcekey="imgTescoClubcardLogoResource1" />
                <em>
                    <asp:Label ID="lblScanYourClubcardHeaderText" runat="server" Text="Print out your Clubcard Vouchers"
                        meta:resourcekey="lblScanYourClubcardHeaderTextResource1"></asp:Label></em></div>
            <div id="breadcrumbs" runat="server">
                <asp:Image ID="imgbreadcrumbs" runat="server" AlternateText="img BreadCrumbs" Width="1152px"
                    Height="45px" meta:resourcekey="imgbreadcrumbsResource1" /></div>
        </div>
        <div id="body_wrapper">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="contentCoupon">
                        <div style="clear: both; height: 20px;">
                        </div>
                        <div>
                            <table style="height: 560px" border="0">
                                <tr>
                                    <td valign="top">
                                        <asp:GridView ID="gvCoupons" runat="server" EnableModelValidation="True" CellPadding="10"
                                            CellSpacing="5" BorderColor="White" BorderWidth="3px" ForeColor="#333333" Font-Size="20px"
                                            OnRowDataBound="gvCoupons_RowDataBound" OnRowCommand="gvCoupons_RowCommand"
                                            AllowPaging="True" PageSize="5" OnPageIndexChanging="gvCoupons_PageIndexChanging"
                                            Width="1160px" meta:resourcekey="gvCouponsResource1">
                                            <AlternatingRowStyle BackColor="White" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="chkSelect" meta:resourcekey="TemplateFieldResource1">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" meta:resourcekey="chkSelectResource1" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHSelect" runat="server" />
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource2">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="litSelectCoupon" runat="server" Text="Select Coupon" 
                                                            meta:resourcekey="litSelectCouponResource1"></asp:Label><br />
                                                             <asp:LinkButton ID="lnkHSelect" runat="server"  OnClick="lnkSelAllCoupons_Click"  CssClass="inputbluesquare" meta:resourcekey="lnkHSelectResource1"></asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSelect" runat="server" CssClass="inputbluesquare" meta:resourcekey="lnkSelectResource1"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="135px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="#507CD1" Font-Size="22px" />
                                            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerSettings Visible="False" />
                                        </asp:GridView>
                                    </td>
                                    <td valign="middle" align="right" style="width: 250px;padding-left:1%;">
                                        <table style="text-align: center">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlPageUp" CssClass="greybtnsmall inactive" runat="server" meta:resourcekey="pnlPageUpResource1">
                                                        <asp:LinkButton ID="lnkPageUp" runat="server" OnClick="lnkPageUp_Click" Enabled="False"
                                                            meta:resourcekey="lnkPageUpResource1">
                                                        <span class="lessmore"></span>
                                                        </asp:LinkButton>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgSmallArrowDown" runat="server" Width="79px" Height="75px" AlternateText="imgSmallArrowDown"
                                                        meta:resourcekey="imgSmallArrowDownResource1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTotalPage" runat="server" CssClass="msg" meta:resourcekey="lblTotalPageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgSmallArrow" runat="server" Width="79px" Height="75px" AlternateText="imgSmallArrow"
                                                        meta:resourcekey="imgSmallArrowResource1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlPageDown" CssClass="greybtnsmall" runat="server" meta:resourcekey="pnlPageDownResource1">
                                                        <asp:LinkButton ID="lnkPageDown" runat="server" OnClick="lnkPageDown_Click" meta:resourcekey="lnkPageDownResource1">
                                                        <span class="lessmore"> </span>
                                                        </asp:LinkButton>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="Vouchermsgdiv">
                            <%--<asp:Label ID="lblMsg1" runat="server" CssClass="Vouchermsg" Text="You have selected "
                                meta:resourcekey="lblMsg1Resource1"></asp:Label>
                            <asp:Label ID="lblSelCoupon" runat="server" CssClass="Vouchermsg" meta:resourcekey="lblSelCouponResource1"></asp:Label>
                            <asp:Label ID="lblMsg2" runat="server" CssClass="Vouchermsg" meta:resourcekey="lblMsg2Resource1"></asp:Label>--%>
                        </div>
                    </div>
                    <br /><br />
                    <div class="back">
                        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" meta:resourcekey="lnkBackResource1"><div>BACK</div></asp:LinkButton>
                    </div>
                    <div class="greybtn">
                        <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" meta:resourcekey="lnkCancelResource1"><span class="cancelStart">CANCEL</span><span class="cancelStartagain">and start again</span></asp:LinkButton>
                    </div>
                    <div class="greybtn">
                        <asp:LinkButton ID="lnkTerms" runat="server" Text="&lt;span class=&quot;terms&quot;&gt;TERMS &amp;amp;&lt;br /&gt;CONDITIONS&lt;/span&gt;"
                            meta:resourcekey="lnkTermsResource1" onclick="lnkTerms_Click"></asp:LinkButton>
                    </div>
                    <div class="greybtn">
                        <asp:LinkButton ID="lnkChooseVoucher" runat="server" OnClick="lnkChooseVoucher_Click"
                            meta:resourcekey="lnkChooseVoucherResource1">
                        <span class="allVouchers"><br /></span>
                        </asp:LinkButton>
                    </div>
                    <asp:Panel ID="pnlPrint" runat="server" CssClass="confirm inactive" meta:resourcekey="pnlPrintResource1">
                        <asp:LinkButton ID="lnkPrint" runat="server" OnClick="btnPrint_Click" meta:resourcekey="lnkPrintResource1"> <span class="twolines" style="font-weight:bold">PRINT SELECTED COUPONS</span> </asp:LinkButton>
                    </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
