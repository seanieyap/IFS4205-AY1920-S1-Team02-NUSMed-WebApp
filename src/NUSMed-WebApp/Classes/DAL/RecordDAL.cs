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

        #region Retrievals
        /// <summary>
        /// Retrieve Records information owned by specific patient
        /// </summary>
        public List<Record> RetrieveRecords(string patientNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, 
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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
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
        /// Retrieve Records information owned by specific patient where therapist has permissions
        /// </summary>
        public List<Record> RetrieveRecords(string patientNRIC, string therapistNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, r.create_time, r.is_emergency, r.file_extension, r.status as record_status,
                    a.name_first as creator_name_first, a.name_last as creator_name_last,
                    rp.status as record_permission_status
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    INNER JOIN record_type_permission rtp ON rtp.patient_nric = r.patient_nric
                    LEFT JOIN record_permission rp ON rp.record_id = r.id AND rp.therapist_nric = @therapistNRIC
                    WHERE rtp.patient_nric = @patientNRIC AND rtp.therapist_nric = @therapistNRIC 
                    ORDER BY r.create_time DESC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);

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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),
                                status = Convert.ToInt16(reader["record_status"])
                            };
                            record.recordPermissionStatus = reader["record_permission_status"] == DBNull.Value ? null : (short?)Convert.ToInt16(reader["record_permission_status"]);

                            result.Add(record);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Records information with specific id owned by specific patient
        /// </summary>
        public Record RetrieveRecord(int id, string therapistNRIC)
        {
            // todo
            Record result = new Record();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.file_name, r.file_extension, r.file_checksum, r.file_size, r.create_time, r.is_emergency, r.file_extension, r.status as record_status,
                    a.name_first as creator_name_first, a.name_last as creator_name_last,
                    rp.status as record_permission_status
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    INNER JOIN record_type_permission rtp ON rtp.patient_nric = r.patient_nric
                    LEFT JOIN record_permission rp ON rp.record_id = r.id AND rp.therapist_nric = @therapistNRIC
                    WHERE r.id = @id AND rtp.therapist_nric = @therapistNRIC
                    ORDER BY r.create_time DESC;";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);

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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
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
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                status = Convert.ToInt16(reader["record_status"])
                            };
                            record.recordPermissionStatus = reader["record_permission_status"] == DBNull.Value ? null : (short?)Convert.ToInt16(reader["record_permission_status"]);
                            result = record;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Record's associated file information owned by specific patient NRIC
        /// </summary>
        public Record RetrieveRecord(string patientNRIC, int id)
        {
            Record result = new Record();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.file_name, r.file_extension, r.file_checksum, r.file_size, r.create_time, r.is_emergency, r.status,
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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
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
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                status = Convert.ToInt16(reader["status"])
                            };
                            result = record;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Record's associated file information owned by specific patient NRIC
        /// </summary>
        public Record RetrieveFileInformation(string patientNRIC, string therapistNRIC, int id)
        {
            Record result = new Record();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.file_name, r.file_extension, r.file_checksum, r.file_size, r.create_time, r.is_emergency,
                    a.name_first as creator_name_first, a.name_last as creator_name_last
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    INNER JOIN record_type_permission rtp ON rtp.patient_nric = r.patient_nric
                    WHERE rtp.patient_nric = @patientNRIC AND rtp.therapist_nric = @therapistNRIC
                    AND r.id = @id;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
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
        public List<Record> RetrieveAssociatedRecords(string patientNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT id    
                    FROM record 
                    WHERE patient_nric = @patientNRIC OR creator_nric = @creatorNRIC";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@creatorNRIC", patientNRIC);

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

        /// <summary>
        /// Retrieve owner of record
        /// </summary>
        public bool RetrieveRecordOwner(string patientNRIC, int recordID)
        {
            bool result = false;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT EXISTS(SELECT patient_nric    
                    FROM record 
                    WHERE patient_nric = @patientNRIC AND ID = @recordID) as result";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToBoolean(reader["result"]);
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        public void UpdateRecordEnable(int recordID, string patientNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record 
                    SET status = 1 
                    WHERE patient_nric = @patientNRIC AND id = @recordID;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateRecordDisable(int recordID, string patientNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record 
                    SET status = 0 
                    WHERE patient_nric = @patientNRIC AND id = @recordID;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteRecordPermission(int recordID, string therapistNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"DELETE FROM record_permission 
                    WHERE record_id = @recordID AND therapist_nric = @therapistNRIC;";

                cmd.Parameters.AddWithValue("@recordID", recordID);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertRecordPermissionDisallow(int recordID, string therapistNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record_permission
                    (record_id, therapist_nric, status)
                    VALUES
                    (@recordID, @therapistNRIC, @status)
                    ON DUPLICATE KEY UPDATE status = 0;";

                cmd.Parameters.AddWithValue("@recordID", recordID);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertRecordPermissionAllow(int recordID, string therapistNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record_permission
                    (record_id, therapist_nric, status)
                    VALUES
                    (@recordID, @therapistNRIC, @status)
                    ON DUPLICATE KEY UPDATE status = 1;";

                cmd.Parameters.AddWithValue("@recordID", recordID);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
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
                    (patient_nric, creator_nric, title, description, type, content, is_emergency)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type, @content, @isEmergency);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@title", record.title);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@content", record.content);
                cmd.Parameters.AddWithValue("@type", record.type.name);
                cmd.Parameters.AddWithValue("@isEmergency", record.isEmergency);

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
                    file_name, file_extension, file_size, file_checksum, create_time, is_emergency)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type,
                    @fileName, @fileExtension, @fileSize, @fileChecksum, @createTime, @isEmergency);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@title", record.title);
                cmd.Parameters.AddWithValue("@description", record.description);
                cmd.Parameters.AddWithValue("@type", record.type.name);

                cmd.Parameters.AddWithValue("@fileName", record.fileName);
                cmd.Parameters.AddWithValue("@fileExtension", record.fileExtension);
                cmd.Parameters.AddWithValue("@fileSize", record.fileSize);
                cmd.Parameters.AddWithValue("@fileChecksum", record.fileChecksum);
                cmd.Parameters.AddWithValue("@createTime", record.createTime.ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@isEmergency", record.isEmergency);

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