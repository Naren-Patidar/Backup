using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
namespace NGCReportingServiceLayer
{
   
    [ServiceContract]
    public interface INGCReportingService
    {

       [OperationContract]
        DataSet GetRegistrationReport(string startdate, string enddate, int week, string period);
       [OperationContract]
       DataSet GetPromotionalReport(string startdate, string enddate, int week, string period);
       [OperationContract]
       string ScheduleReport(string ScheduleXml);

       [OperationContract]
       string TerminateSchedule(string ScheduleXml);

        [OperationContract]
       DataSet GetPointsEarnedReport(string startdate, string enddate, int weeknumber, string periodnumber);

       [OperationContract]
       DataSet PopulateWeekData();

       [OperationContract]
        DataSet PopulatePeriodData();

       [OperationContract]
       DataSet GetCustomerLoadReport(string startdate, string enddate, int week, string period);
    }


   
}
