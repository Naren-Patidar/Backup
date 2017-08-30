using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigReader
{
    public interface IAppConfigReader
    {
        string GetConfigValue(string key, string fileName);
    }
}
