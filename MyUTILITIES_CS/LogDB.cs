using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyUTILITIES_CS
{
    public class LogDB:ILogger
    {
        Queue<LogItem> logQueue = new Queue<LogItem>();
        Task Queuetask;
        Task CheckHoseKeepingTask;
        LogItem logItem;
        public bool Stop = false;

        static string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Northwind;Data Source=SHAHAR\\SQLEXPRESS01";

        public void AddToDB(LogItem item)
        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = "insert into LogDB (Message,LogLevel,Date) values(@msg,@logLevel,GETDATE())";
                if (item.exception != null)
                {
                    queryString = "insert into LogDB values(@msg,@logLevel,GETDATE(),@exce)";
                }
                

                connection.Open();

                // Adapter
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if(item.exception != null)
                    {
                        command.Parameters.AddWithValue("@exce", item.exception.Message);
                    }
                    command.Parameters.AddWithValue("@msg", item.Message);
                    command.ExecuteNonQuery();
                }
            }
        }
        static void RunNonQuery(string sqlQuery)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Adapter
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    //Reader
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Init()
        {
            Queuetask = Task.Run(() =>
            {
                while (!Stop)
                {
                    if (logQueue.Count > 0)
                    {
                        LogItem item = logQueue.Dequeue();
                        AddToDB(item);
                    }
                    Thread.Sleep(1000);
                }
            });
            //Checks if there is a list of logs and if there is no add to database
            string queryString = "if not exists (Select * From LogDB) begin \r\nCREATE TABLE LogDB (int id identity, Message nvarchar(100) NOT NULL,\r\n LogLevel nvarchar(40) NOT NULL, Date Date NOT NULL,Exception nvarchar(80)) \r\nend;";
            RunNonQuery(queryString);
            LogCheckHoseKeeping();
        }
        public void LogEvent(string msg)
        {
            logItem = new LogItem { Message = $"Event - {msg}" };
            logQueue.Enqueue(logItem);
        }
        public void LogError(string msg)
        {
            logItem = new LogItem { Message = $"Event - {msg}" };
            logQueue.Enqueue(logItem);
        }
        public void LogException(string msg, Exception exce)
        {
            logItem = new LogItem { Message = $"Exception - {msg}", exception = exce };
            logQueue.Enqueue(logItem);
        }
        public void LogCheckHoseKeeping()
        {
            //Deletes all logs entered more than 3 months ago 
            string queryString = "if exists (Select * from LogDB where Date < DATEADD(month,-3, GETDATE())) begin DELETE FROM LogDB WHERE Date < DATEADD(month, -3, GETDATE()) end";
            RunNonQuery(queryString);
        }
    }
}
