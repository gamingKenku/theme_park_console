using System;
using System.Collections.Specialized;
using System.Configuration;

namespace theme_park_console
{
    static public class Config
    {
        private readonly static NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public readonly static string Log_Path = appSettings["LogFile"];
        public readonly static string XML_Path = appSettings["XMLPath"];
    }
}
