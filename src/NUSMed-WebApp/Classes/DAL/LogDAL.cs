using MySql.Data.MySqlClient;
using System.Configuration;

namespace NUSMed_WebApp.Classes.DAL
{
    public class LogDAL
    {
        protected MySqlConnection connection;

        protected LogDAL()
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