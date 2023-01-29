using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class LogConsole:ILogger
    {
        public void Init()
        {

        }
        //cant create global date time variable
        static string Date()
        {
            DateTime dateTime= DateTime.Now;
            string date = $"{dateTime.Year}/{dateTime.Month}/{dateTime.Day}-{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}";
            return date;
        }
        void PrintMessage(string msg,string logLevel, Exception exce)
        {
            if(exce == null)
            {
                Console.WriteLine($"{Date()}-{logLevel}-{msg}");
            }
            else
            { 
                Console.WriteLine($"{Date()}-{logLevel}-{msg}-{exce.Message}");
            }
        }
        public void LogEvent(string msg)
        {
            PrintMessage(msg, "Event", null);
        }
        public void LogError(string msg)
        {
            PrintMessage(msg, "Error", null);
        }
        public void LogException(string msg, Exception exce)
        {
            PrintMessage(msg, "Exception", exce);
        }
        public void LogCheckHoseKeeping()
        {

        }
    }
}
