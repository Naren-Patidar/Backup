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
using System.IO;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services;
using Tesco.Framework.UITesting.Constants;
using System.Globalization;
using Tesco.Framework.UITesting.Services.ClubcardService;


namespace Tesco.Framework.UITesting.Test.Common
{
    class MyLatestStatement : Base
    {
        #region Constructor
       
 Generic objGeneric = null;
 public MyLatestStatement(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(objhelper);
        }

        public void VerifyPointsDetails(string clubcardNumber)
        {
            try
            {

                CustomLogs.LogMessage("Verifying Options And Benefits Text on Chirstmas Saver Page started ", TraceEventType.Start);
                CustomerServiceAdaptor customerServiceAdpator = new CustomerServiceAdaptor();
                long customerID = customerServiceAdpator.GetCustomerID(clubcardNumber, CountrySetting.culture);
                ClubcardServiceAdapter client = new ClubcardServiceAdapter();
                List<string> offers = client.GetOffersForCustomer(customerID, CountrySetting.culture);
                Dictionary<string, string> pointsSummaryInfo = client.GetPointsSummary(customerID, offers[1], CountrySetting.culture);
                 

            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }

        }
       
        #endregion
    }
}
