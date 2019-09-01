using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.DAL
{
    public class RecordDAL : DAL
    {
        public RecordDAL() : base() { }

        /// <summary>
        /// Retrieve all Records registered in the database of particular nric
        /// </summary>
        public List<Record> RetrieveAssociated(string nric)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT id    
                    FROM record 
                    WHERE patient_nric = @patientNRIC OR creator_nric = @creatorNRIC";

                cmd.Parameters.AddWithValue("@patientNRIC", nric);
                cmd.Parameters.AddWithValue("@creatorNRIC", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Record record = new Record
                            {
                                id = Convert.ToInt32(reader["id"])
                            };

                            result.Add(record);
                        }
                    }
                }
            }

            return result;
        }


        #region Deletions
        public void DeleteRecord(int id)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM record_permission
                        WHERE record_id = @recordID;
                    DELETE FROM record
                        WHERE record_id = @recordID;";

                cmd.Parameters.AddWithValue("@recordID", id);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecordDiagnosis(int id)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"DELETE FROM record_diagnosis 
                        WHERE record_id = @recordID;";

                cmd.Parameters.AddWithValue("@recordID", id);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecordPermission(int id)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"DELETE FROM record_permission 
                        WHERE record_id = @recordID;";

                cmd.Parameters.AddWithValue("@recordID", id);

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