using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.DAL
{
    public class RecordLogDAL : LogDAL
    {
        public RecordLogDAL() : base() { }

        #region Inserts
        /// <summary>
        /// Insert Log
        /// </summary>        
        public void Insert(string creatorNRIC, int recordID, string action, string description)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (creator_nric, record_id, action, description)
                    VALUES (@creatorNRIC, @recordID, @action, @description);";

                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@recordID", recordID);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@description", description);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

    }
}