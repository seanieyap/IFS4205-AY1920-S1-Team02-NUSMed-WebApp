using MySql.Data.MySqlClient;
using System.Configuration;

namespace NUSMed_WebApp.Classes.DAL
{
    public class DALLog
    {
        protected MySqlConnection connection;

        public DALLog()
        {
            connection = new MySqlConnection(Get().ConnectionString);
        }

        public static MySqlConnectionStringBuilder Get()
        {
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder(ConfigurationManager.AppSettings["ConnectionStringLogging"].ToString());

            return mscsb;
        }
    }
}