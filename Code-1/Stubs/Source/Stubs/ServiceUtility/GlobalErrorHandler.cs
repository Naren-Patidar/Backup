using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Configuration;


namespace ServiceUtility
{
    public class GlobalErrorHandler : IErrorHandler
    {
        private ILoggingService _logger = new LoggingService();

        public bool HandleError(Exception error) 
        {
           
            string strNlogConfigPath = System.Web.Configuration.WebConfigurationManager.AppSettings.Get("NLogConfigPath");
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strNlogConfigPath), true);
            LogData _logData = new LogData();
            _logData.CaptureData("Error", error);
            _logger.Submit(_logData);
            return true;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            
            FaultCode fc = new FaultCode("NewFaultCode");
            MessageVersion ver = OperationContext.Current.IncomingMessageVersion;
            fault = Message.CreateMessage(ver, fc, "Original Exception is handled! this is customized exception!", "Out Going Server Response");
        }
    }
}
