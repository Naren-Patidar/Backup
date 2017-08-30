using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationTrigger.Entities
{
    public class TriggerResponse
    {
        public string Environment { get; set; }
        public string Country { get; set; }
        public string Category { get; set; }
        public string BatchFile { get; set; }
        public bool IsSuccess { get; set; }
        public Exception Error { get; set; }
    }
}
