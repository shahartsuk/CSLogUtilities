using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class LogFile : ILogger
    {
        static string _FileName = "log";
            //ConfigurationManager.AppSettings["fileName"];
        static int _MaxSize = 5000000;
        static int _Counter = 0;
        static string Ending = ".txt";
        static string OriginFileName= "log";
        //cant create global date time variable
        static string Date()
        {
            DateTime dateTime = DateTime.Now;
            string date = $"{dateTime.Year}/{dateTime.Month}/{dateTime.Day}-{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}";
            return date;
        } 
        //create the file log
        public void Init()
        {
            LogCheckHoseKeeping();
        }
        public void WriteToFile(string msg,string logLevel, Exception exce)
        {
            using (StreamWriter file = new StreamWriter(_FileName))
            {
                if (exce == null)
                {
                    file.WriteLine($"{logLevel} - {Date()}-{msg}");
                }
                else
                {
                    file.WriteLine($"{Date()}-{msg}-{logLevel}:{exce.Message}");
                }
            }
                
        }
        public void LogEvent(string msg)
        {
            WriteToFile(msg, "Event", null);
        }
        public void LogError(string msg)
        {
            WriteToFile(msg, "Error", null);
        }
        public void LogException(string msg, Exception exce)
        {
            WriteToFile(msg, "Exception", exce);
        }
        //cheking if the file log already exists and his size
        public void LogCheckHoseKeeping()
        {
            //get file info check if exists in while so i can change his name
            FileInfo fileInfo= new FileInfo($"{OriginFileName}{Ending}");
            while (fileInfo.Exists)
            {
                if (fileInfo.Length >= _MaxSize)
                {
                    //if the size is too big create new file with bigger number
                    _Counter++;
                    _FileName = $"{OriginFileName}{_Counter}{Ending}";
                    fileInfo = new FileInfo(_FileName);
                }
                else return;
            }
        }
    }
}
