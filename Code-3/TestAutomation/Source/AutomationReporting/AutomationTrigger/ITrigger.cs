using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using AutomationTrigger.Entities;

namespace AutomationTrigger
{
    [ServiceContract]
    public interface ITrigger
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "run/{environment}/{country}/{category}")]
        TriggerResponse Run(string environment, string country, string category);
    }
}
