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
using System.Diagnostics;
using Tesco.Framework.UITesting.Constants;
using Tesco.Framework.UITesting.Services;
using System.IO;
using System.Threading;

namespace Tesco.Framework.UITesting.Test.Common
{
    class BoostAtTesco :Base 
    {
        
        #region PROPERTIES
        string isPresent = string.Empty;
        Generic objGeneric = null;
        #endregion 

        #region Constructor
        public BoostAtTesco(AutomationHelper objhelper)
        {
            this.ObjAutomationHelper = objhelper;
            objGeneric = new Generic(ObjAutomationHelper);
        }

        #endregion
        #region Text Validation 
        public void TextValidation(string pageName)
        {
            string errorMessage = string.Empty;
            try
            {
                Driver = ObjAutomationHelper.WebDriver;
                string resxFile = Path.Combine(SanityConfiguration.MessageDataDirectory, SanityConfiguration.ResourceFiles.BOOST_RESOURCE);
                errorMessage = objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLEXCHANGEON, ControlKeys.BOOSTTEXTEXCHANGEON, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLMANAGECARDDESCRIPTION, ControlKeys.BOOSTTEXTLBLMANAGECARDDESCRIPTION, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLMANAGECARDDESCRIPTION2, ControlKeys.BOOSTTEXTLBLMANAGECARDDESCRIPTION2, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLMANAGECARDDESCRIPTION3, ControlKeys.BOOSTTEXTLBLMANAGECARDDESCRIPTION3, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLEXCHANGESFORM, ControlKeys.BOOSTTEXTLBLEXCHANGESFORM, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLINFOMESSAGE, ControlKeys.BOOSTTEXTLBLINFOMESSAGE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLUSEONLINE, ControlKeys.BOOSTTEXTLBLUSEONLINE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_BOOSTCOPYTOKENSLIST, ControlKeys.BOOSTTEXTBOOSTONLINELIST, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLPURCHASEDONLINE, ControlKeys.BOOSTTEXTLBLPURCHASEDONLINE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLCODESUSEDONCE, ControlKeys.BOOSTTEXTLBLCODESUSEDONCE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLSTOREEXCCODES, ControlKeys.BOOSTTEXTLBLSTOREEXCCODES, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORBOOSTMSGSLCLMSG1, ControlKeys.BOOST_DIVFORBOOSTMSGSLCLMSG1, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORBOOSTMSGSLCLMSG2, ControlKeys.BOOST_DIVFORBOOSTMSGSLCLMSG2, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORBOOSTMSGSLCLMSG3, ControlKeys.BOOST_DIVFORBOOSTMSGSLCLMSG3, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORBOOSTMSGSLCLMSG4, ControlKeys.BOOST_DIVFORBOOSTMSGSLCLMSG4, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSPRINT, ControlKeys.BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSPRINT, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORREWARDANDTOKENOLPRINTTOKENS, ControlKeys.BOOST_DIVFORREWARDANDTOKENOLPRINTTOKENS, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSTORE, ControlKeys.BOOST_DIVFORREWARDANDTOKENOLEXCTOKENSTORE, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLSHWERRORMSG, ControlKeys.BOOSTTEXTLBLSHWERRORMSG, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLIWANTTO, ControlKeys.BOOSTTEXTLBLIWANTTO, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_PNLFORINFO, ControlKeys.BOOSTTEXTPNLFORINFO, resxFile, pageName);
                errorMessage = errorMessage + objGeneric.VerifyText_Contains(ValidationKey.BOOST_LBLBOOSTINFO3, ControlKeys.BOOSTTEXTLBLBOOSTINFO3, resxFile, pageName);


                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Assert.Fail(errorMessage);
                }
            }
            catch (Exception ex)
            {
                CustomLogs.LogException(ex);
                ScreenShotDetails.TakeScreenShot(Driver, ex);
                Assert.Fail(ex.InnerException == null ? ex.Message : ex.InnerException.ToString());
                Driver.Quit();
            }
        }   
       
        #endregion
    }
}
