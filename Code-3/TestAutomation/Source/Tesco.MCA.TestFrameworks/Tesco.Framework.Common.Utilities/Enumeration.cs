using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities
{
    public enum RequestMethod
    {
        GET = 0,
        POST = 1
    }

    public enum AppenderType
    {
        UNITTEST = 0,
        HTMLREPORT = 1,
        APITEST = 2,
        ACCOUNTDETAILSSUITE = 3,
        JOINTESTSUITE = 4,
        HOMESECURITYTESTSUITE = 5,
        ACTIVATIONSUITE = 6,
        COUPONTESTSUITE = 7,
        VOUCHERTESTSUITE = 8,
        MYPOINTTESTSUITE = 9,
        MYLATESTSTATEMENTSUITE = 10,
        CHRISTMASSAVERSUITE = 11,
        CONTACTPREFERNECESUITE = 12,
        PERSONALDETAILSSUITE = 13,
        ORDERAREPLACEMENTSUITE = 14,
        HOMETESTSUITE
    }
}
