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
        /// Retrieve Records information owned by specific patient
        /// </summary>
        public List<Record> Retrieve(string patientNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.id, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.create_time, r.is_emergency, r.file_extension,
                    a.name_first as creator_name_first, a.name_last as creator_name_last
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    WHERE patient_nric = @patientNRIC
                    ORDER BY r.create_time DESC;";

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
                                title = Convert.ToString(reader["title"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                fileExtension = Convert.ToString(reader["file_extension"])
                            };
                            result.Add(record);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Record's associated file information owned by specific patient NRIC
        /// </summary>
        public Record RetrieveFileInformation(string patientNRIC, int id)
        {
            Record result = new Record();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT r.id, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.file_name, r.file_extension, r.file_checksum, r.file_size, r.create_time, r.is_emergency,
                    a.name_first as creator_name_first, a.name_last as creator_name_last
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    WHERE patient_nric = @patientNRIC AND r.id = @id;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@id", id);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Record record = new Record
                            {
                                id = Convert.ToInt32(reader["id"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),
                                fileName = Convert.ToString(reader["file_name"]),
                                fileSize = Convert.ToInt32(reader["file_size"]),
                                fileChecksum = Convert.ToString(reader["file_checksum"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"])
                            };
                            result = record;
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
        /// <summary>
        /// Insert new record
        /// </summary>
        public void InsertContent(Record record, string patientNRIC, string creatorNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (patient_nric, creator_nric, title, description, type, content)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type, @content);";

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
        /// <summary>
        /// Insert new record
        /// </summary>
        public void InsertFile(Record record, string patientNRIC, string creatorNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (patient_nric, creator_nric, title, description, type,
                    file_name, file_extension, file_size, file_checksum, create_time)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type,
                    @fileName, @fileExtension, @fileSize, @fileChecksum, @createTime);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@title", record.title);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@type", record.type.name);

                cmd.Parameters.AddWithValue("@fileName", record.fileName);
                cmd.Parameters.AddWithValue("@fileExtension", record.fileExtension);
                cmd.Parameters.AddWithValue("@fileSize", record.fileSize);
                cmd.Parameters.AddWithValue("@fileChecksum", record.fileChecksum);
                cmd.Parameters.AddWithValue("@createTime", record.createTime);

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