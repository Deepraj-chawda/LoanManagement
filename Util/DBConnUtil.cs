using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Util
{
    static class DBConnUtil
    {
        private static SqlConnection connection;

        public static SqlConnection getDBConn()
        {
            // Ensure the connection is initialized
            if (connection == null)
            {
                string connectionString = DBPropertyUtil.GetPropertyString();

                if (connectionString != null)
                {

                    try
                    {
                        // Open the connection
                        connection = new SqlConnection(connectionString);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error connecting to the database: {ex.Message}");
                        connection = null;
                    }
                }
            }

            return connection;
        }
    }
}
