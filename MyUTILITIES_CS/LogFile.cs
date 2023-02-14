using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class LogFile : ILogger
    {
        Queue<LogItem> logQueue = new Queue<LogItem>();
        Task Queuetask;
        Task CheckHoseKeepingTask;
        LogItem logItem;
        string _FileName = "log";
            //ConfigurationManager.AppSettings["fileName"];
        int _MaxSize = 5000000;
        int _Counter = 0;
        string Ending = ".txt";
        string OriginFileName= "log";
        bool Stop = false;
        bool CheckStop = false;
        //cant create global date time variable
        //create the file log
        public void Init()
        {
            CheckHoseKeepingTask = Task.Run(() =>
            {
                while (!CheckStop)
                {
                    LogCheckHoseKeeping();

                    Thread.Sleep(1000 * 60 * 5); //5 min
                }
            });
            Queuetask = Task.Run(() =>
            {
                while (!Stop)
                {

                    LogItem item = logQueue.Dequeue();
                    WriteToFile(item);

                    Thread.Sleep(1000);
                }
            });

            
        }
        public void WriteToFile(LogItem item)
        {
            using (StreamWriter file = new StreamWriter(_FileName))
            {
                if (item.exception == null)
                {
                    file.WriteLine($"{item.Date} - {item.Message}");
                }
                else
                {
                    file.WriteLine($"{item.Date} - {item.Message}:{item.exception.Message}");
                }
            }
                
        }
        public void LogEvent(string msg)
        {
            logItem = new LogItem { Message = $"Event - {msg}"};
            logQueue.Enqueue(logItem);
        }
        public void LogError(string msg)
        {
            logItem = new LogItem { Message = $"Error - {msg}"};
            logQueue.Enqueue(logItem);
        }
        public void LogException(string msg, Exception exce)
        {
            logItem = new LogItem { Message = $"Exception - {msg}", exception = exce};
            logQueue.Enqueue(logItem);
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
