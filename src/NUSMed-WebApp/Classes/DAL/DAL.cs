﻿using MySql.Data.MySqlClient;
using System.Configuration;

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
            MySqlConnectionStringBuilder mscsb = new MySqlConnectionStringBuilder(ConfigurationManager.AppSettings["ConnectionString"].ToString());

            return mscsb;
        }
    }
}