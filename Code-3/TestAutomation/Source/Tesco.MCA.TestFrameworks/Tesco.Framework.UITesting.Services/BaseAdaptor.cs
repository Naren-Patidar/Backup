using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Utilities;


namespace Tesco.Framework.UITesting.Services
{
    public class BaseAdaptor
    {
        ILogger customLogs = null;


        #region Properties
        public ILogger CustomLogs
        {
            get { return customLogs; }
            set { customLogs = value; }
        }
        #endregion

        #region Constructor
        public BaseAdaptor()
        {
            Utilities.InitializeLogger(ref customLogs, AppenderType.UNITTEST);
            
        }
        #endregion
    }
}
