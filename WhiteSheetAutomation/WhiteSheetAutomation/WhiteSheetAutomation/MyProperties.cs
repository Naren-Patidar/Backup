using System;
using System.Collections.Generic;
using System.Text;

namespace WhiteSheetAutomation
{   [Serializable]
    class MyProperties
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ApplicationConfig { get; set; }
        public string Server { get; set; }
        public string MyDatabase { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PDWS { get; set; }
        public string MDWS { get; set; }

       

    }
}
