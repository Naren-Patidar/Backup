using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.ServiceModel;

namespace AutomationTrigger
{
    public class WindowServiceTrigger : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public WindowServiceTrigger()
        {
            ServiceName = "TriggerAutomation";            
        }

        public static void Main()
        {
            ServiceBase.Run(new WindowServiceTrigger());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof(Trigger));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
