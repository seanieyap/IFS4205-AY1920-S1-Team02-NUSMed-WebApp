using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.DAL
{
    public class LogAccountDAL : LogDAL
    {
        public LogAccountDAL() : base() { }

        #region Inserts
        /// <summary>
        /// Insert Log
        /// </summary>
        public void Insert(string creatorNRIC, string actionOnNRIC, string action, string description)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO account
                    (creator_nric, action_on_nric, action, description)
                    VALUES (@creatorNRIC, @actionOnNRIC, @action, @description);";

                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@actionOnNRIC", actionOnNRIC);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@description", description);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
        }
        #endregion
    }
}