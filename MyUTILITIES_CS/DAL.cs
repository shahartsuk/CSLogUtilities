using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUTILITIES_CS
{
    internal class DAL
    {
        public static void RunNonQuery(string sqlQuery,string connectionString)
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
    }
}
