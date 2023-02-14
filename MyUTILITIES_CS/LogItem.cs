using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class LogItem
    {
        public string Message { get; set; }
        public string LogLocation { get; set; }
        public Exception exception { get; set; }
        private string _Date()
        {
            DateTime dateTime = DateTime.Now;
            string date = $"{dateTime.Year}/{dateTime.Month}/{dateTime.Day}-{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}";
            return date;
        }
        public string Date { get { return _Date(); } }
    }
}
