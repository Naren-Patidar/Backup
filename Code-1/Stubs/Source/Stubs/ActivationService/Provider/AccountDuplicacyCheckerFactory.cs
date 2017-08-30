using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Data;
using System.Data;
using ServiceUtility;

namespace ActivationService.Provider
{

    public class AccountDuplicacyCheckerFactory
    {
        Helper helper = new Helper();
        private static ILoggingService _logger = new LoggingService();
        public AccountDuplicacyCheck GetInstance(long ClubcardNumber)
        {
            LogData _logData = new LogData();
            AccountDuplicacyCheck rADC = null;
            try
            {
                var custData = helper.SearchByClubcard(ClubcardNumber, AppDomain.CurrentDomain.BaseDirectory);
                if (custData.Item1 != null)
                {
                    switch (custData.Item1)
                    {
                        case "en-GB":
                            rADC = new UKAccountDuplicacyCheck();
                            break;
                        case "cs-CZ":
                            rADC = new CZAccountDuplicacyCheck();
                            break;
                        case "en-MY":
                            rADC = new MYAccountDuplicacyCheck();
                            break;
                        case "pl-PL":
                            rADC = new PLAccountDuplicacyCheck();
                            break;
                        case "hu-HU":
                            rADC = new HUAccountDuplicacyCheck();
                            break;
                        case "sk-SK":
                            rADC = new SKAccountDuplicacyCheck();
                            break;
                        case "th-TH":
                            rADC = new THAccountDuplicacyCheck();
                            break;
                        default:
                            break;
                    }
                }
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw Extensions.GetCustomException("Failed in AccountDuplicacycheckFactory while getting country specific method instance.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            _logData.CaptureData("AccountDuplicacyCheckObject", rADC);
            _logger.Submit(_logData);
            return rADC;
        }
    }
}