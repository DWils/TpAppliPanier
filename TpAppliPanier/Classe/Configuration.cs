using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TpAppliPanier.Classe
{
    public class Configuration
    {
        public static SqlConnection Connection = new SqlConnection(@"Data Source=(LocalDb)\coursSql;Integrated Security=True");
    }
}
