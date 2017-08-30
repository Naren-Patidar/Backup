using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Enums;
using Tesco.Framework.UITesting.Entities;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.UITesting.Helpers.CustomHelper;
using OpenQA.Selenium;
using Tesco.Framework.UITesting.Helpers;
using Tesco.Framework.Common.Logging.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Tesco.Framework.UITesting.Test.Common
{
    class ChristmasSaver : Base
    {
        #region Constructor


        public ChristmasSaver(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
        }

        #endregion
        #region Methods

        //public bool verify_MyVoucherText()
        //{
        //    try
        //    {
        //        Message = ObjAutomationHelper.GetMessageByID(Enums.Messages.MyVoucher);
        //        var expectedLinkName = Message.Title;
        //        var actualPageHeader = Driver.FindElement(By.CssSelector(ObjAutomationHelper.GetControl(ControlKeys.MYVOUCHER_TEXT).Id)).Text;
        //        if (expectedLinkName == actualPageHeader)
        //            CustomLogs.LogInformation("My Voucher Page Verified");
        //        else
        //        {
        //            CustomLogs.LogInformation("My Voucher Page Verified");
        //            Assert.Fail("My Voucher Page Verified");

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CustomLogs.LogException(ex);
        //    }
        //    return true;

        //}
        #endregion
    }

}
