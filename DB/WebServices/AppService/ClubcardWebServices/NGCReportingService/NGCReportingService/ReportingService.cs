using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;

namespace Tesco.com.NGCReportingService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class ReportingService : IReportingService
    {
        string connection = string.Empty;
        public ReportingService()
        {
            //connection = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionString"].ToString());
            connection = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        }
    }
}
