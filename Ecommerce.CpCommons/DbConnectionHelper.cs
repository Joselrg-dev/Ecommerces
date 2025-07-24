using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpCommons
{
    public class DbConnectionHelper
    {
        public static SqlConnection GetConnection() =>
             new SqlConnection(ConfigurationManager.ConnectionStrings["EcommerceDB"].ConnectionString);
    }
}

