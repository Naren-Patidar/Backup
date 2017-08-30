using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.NGC.PosNGCWebService
{
    // NOTE: If you change the interface name "IPosNGCService" here, you must also update the reference to "IPosNGCService" in App.config.
    [ServiceContract]
    public interface IPosNGCService
    {
        [OperationContract]
        string ProcessPosRequest(string requestXml);
        // TODO: Add your service operations here
    }
}
