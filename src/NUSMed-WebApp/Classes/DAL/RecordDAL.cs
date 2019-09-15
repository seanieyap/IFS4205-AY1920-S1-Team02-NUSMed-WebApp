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
        /// Retrieve Records registered in the database owned by patient NRIC
        /// </summary>
        public List<Record> Retrieve(string patientNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.id, r.creator_nric, r.description, r.type, r.content, r.file_type, r.title, r.path, r.create_time, a.name_first as creator_name_first, a.name_last as creator_name_last
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    WHERE patient_nric = @patientNRIC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);

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
                                id = Convert.ToInt32(reader["id"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                fileType = Convert.ToString(reader["file_type"]),
                                title = Convert.ToString(reader["title"]),
                                path = Convert.ToString(reader["path"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"])
                            };
                            result.Add(record);
                        }
                    }
                }
            }

            return result;
        }

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
        #region Insertions
        public void Insert(Record record, string patientNRIC, string creatorNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (patient_nric, creator_nric, title, description, content, type)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @content, @type);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@title", record.title);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@content", record.content);
                cmd.Parameters.AddWithValue("@type", record.type.name);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

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