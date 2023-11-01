using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Back.Context.Connection
{
    public class Connection
    {
        private static IConfiguration configuration;
        public static SqlConnection objConexion;
        private static string conecction;
        public static Queue<string> conecctionName = new Queue<string>();

        public SqlConnection ConnectBD(IConfiguration configuration)
        {
            return new SqlConnection(configuration.GetConnectionString("DevLab"));
        }
    }

}
