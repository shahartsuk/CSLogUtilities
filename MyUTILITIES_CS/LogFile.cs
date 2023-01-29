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
        private static ILogger Logger;
        private static string _FileName;
        private static int _MaxSize = 5000000;
        private static int _Counter = 0;
        private static string Ending = ".txt";
        private static string OriginFileName="";
        //cant create global date time intiger
        private static string Date()
        {
            string date = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";
            return date ;
        } 
        //create the file log
        public void Init(string fileName)
        {
            if(_Counter == 0)
            {
                _FileName = fileName + Ending;
                OriginFileName = fileName;
            }
            else
            {
                _FileName = Path.GetFileNameWithoutExtension(fileName + Ending) + _Counter.ToString() + Ending;
            }
            
        }
        public void LogEvent(string msg)
        {
            using (StreamWriter file = new StreamWriter(_FileName))
            {
                file.WriteLine($"Event - {Date()}-{msg}");
            }
        }
        public void LogError(string msg)
        {
            using (StreamWriter file = new StreamWriter(_FileName))
            {
                file.WriteLine($"Error - {Date()}-{msg}");
            }
        }
        public void LogException(string msg, Exception exce)
        {
            using (StreamWriter file = new StreamWriter(_FileName))
            {
                file.WriteLine($"Exception - {Date()}-{msg}");
            }
        }
        //cheking if the file log already exists and his size
        public void LogCheckHoseKeeping()
        {
            FileInfo fileInfo= new FileInfo($"{OriginFileName}{Ending}");
            while(fileInfo.Exists)
            {
                fileInfo = new FileInfo($"{OriginFileName}{_Counter}{Ending}");
                if (fileInfo.Length >= _MaxSize)
                {
                    //if the size is too big i create new file with bigger number
                    _Counter++;
                    _FileName = OriginFileName;
                    Init(_FileName);
                }
                else
                {
                    return;
                }
            } 
        }
    }
}
