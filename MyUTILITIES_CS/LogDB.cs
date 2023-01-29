using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    public class LogDB:ILogger
    {
        static string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Northwind;Data Source=SHAHAR\\SQLEXPRESS01";
        public static void AddToDB(string msg,string logLevel, Exception exce)
        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = "insert into LogDB (Message,LogLevel,Date) values(@msg,@logLevel,GETDATE())";
                if (exce != null)
                {
                    queryString = "insert into LogDB values(@msg,@logLevel,GETDATE(),@exce)";
                }
                

                connection.Open();

                // Adapter
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if(exce != null)
                    {
                        command.Parameters.AddWithValue("@exce", exce.Message);
                    }
                    command.Parameters.AddWithValue("@logLevel", logLevel);
                    command.Parameters.AddWithValue("@msg", msg);
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
            //Checks if there is a list of logs and if there is no add to database
            string queryString = "if not exists (Select * From LogDB) begin \r\nCREATE TABLE LogDB (int id identity, Message nvarchar(100) NOT NULL,\r\n LogLevel nvarchar(40) NOT NULL, Date Date NOT NULL,Exception nvarchar(80)) \r\nend;";
            RunNonQuery(queryString);
        }
        public void LogEvent(string msg)
        {
            AddToDB(msg, "Event", null);
        }
        public void LogError(string msg)
        {
            AddToDB(msg, "Error", null);
        }
        public void LogException(string msg, Exception exce)
        {
            AddToDB(msg, "Exception", exce);
        }
        public void LogCheckHoseKeeping()
        {
            //Deletes all logs entered more than 3 months ago 
            string queryString = "if exists (Select * from LogDB where Date < DATEADD(month,-3, GETDATE())) begin DELETE FROM LogDB WHERE Date < DATEADD(month, -3, GETDATE()) end";
            RunNonQuery(queryString);
        }
    }
}
