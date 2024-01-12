using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LoanManagement.Util
{
    static class DBPropertyUtil
    {
        public static string GetPropertyString()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;

            return connectionString;

        }
    }
}
