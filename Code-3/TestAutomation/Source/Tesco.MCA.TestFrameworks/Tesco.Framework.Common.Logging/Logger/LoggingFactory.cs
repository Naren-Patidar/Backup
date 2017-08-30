using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tesco.Framework.Common.Logging.Logger
{
    public class LoggingFactory
    {
        private static ILogger _logger;

        /* Initialize the instance */
        public static void InitializeLogFactory(ILogger logger)
        {
            _logger = logger;
        }

        /* Fetch the logger object */
        public static ILogger GetLogger() { return _logger; }

    }
}
