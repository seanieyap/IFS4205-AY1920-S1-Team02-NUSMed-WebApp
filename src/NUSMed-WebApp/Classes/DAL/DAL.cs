using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.DAL
{
    public class DAL
    {
        protected MySqlConnection connection;

        protected DAL()
        {
            connection = new MySqlConnection(Get().ConnectionString);
        }

        public static MySqlConnectionStringBuilder Get()
        {
            // GET from 64 Bit registry
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder();
            mscsb.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

            return mscsb;
        }

    }
}