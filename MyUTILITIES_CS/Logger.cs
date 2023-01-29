using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class Logger
    {
        static ILogger myLog;
        //public enum ProviderType
        //{
        //    File =1,
        //}
        public Logger(string provider)
        {
            switch (provider)
            {
                case "File":
                    myLog = new LogFile();
                    break;
                case "DB":
                    myLog = new LogDB();
                    break;
                case "Console":
                    myLog = new LogConsole();
                    break;
                default:
                    myLog = new LogNone();
                    break;
            }
        }
    }
}
