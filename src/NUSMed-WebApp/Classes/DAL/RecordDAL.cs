using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
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
                                id = Convert.ToInt64(reader["id"]),
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
                                id = Convert.ToInt64(reader["id"]),
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
        public Record RetrieveRecord(long recordID, string therapistNRIC)
        {
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

                cmd.Parameters.AddWithValue("@id", recordID);
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
                                id = Convert.ToInt64(reader["id"]),
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),
                                fileName = Convert.ToString(reader["file_name"]),
                                //fileSize = Convert.ToInt32(reader["file_size"]),
                                fileChecksum = Convert.ToString(reader["file_checksum"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                status = Convert.ToInt16(reader["record_status"])
                            };
                            record.fileSize = reader["file_size"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["file_size"]);

                            record.recordPermissionStatus = reader["record_permission_status"] == DBNull.Value ? null : (short?)Convert.ToInt16(reader["record_permission_status"]);
                            result = record;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Records information attached to a note
        /// </summary>
        public List<Record> RetrieveRecords(long noteID, string patientNRIC, string therapistNRIC)
        {
            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.id, r.patient_nric, r.creator_nric, r.description, r.type, r.content, r.title, 
                    r.create_time, r.is_emergency, r.file_extension, r.status as record_status,
                    a.name_first as creator_name_first, a.name_last as creator_name_last,
                    rp.status as record_permission_status
                    FROM medical_note mn 
					INNER JOIN medical_note_record mnr ON mnr.medical_note_id = mn.id
					INNER JOIN record r ON r.id = mnr.record_id
                    INNER JOIN account a ON a.nric = r.creator_nric
                    LEFT JOIN record_permission rp ON rp.record_id = r.id AND rp.therapist_nric = @therapistNRIC
                    WHERE mn.therapist_nric = @therapistNRIC AND mn.patient_nric = @patientNRIC AND mnr.medical_note_id = @noteID
                    ORDER BY r.create_time DESC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@noteID", noteID);

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
                                id = Convert.ToInt64(reader["id"]),
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
                    WHERE r.patient_nric = @patientNRIC AND r.id = @id;";

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
                                id = Convert.ToInt64(reader["id"]),
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),
                                fileName = Convert.ToString(reader["file_name"]),
                                fileChecksum = Convert.ToString(reader["file_checksum"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                creatorFirstName = Convert.ToString(reader["creator_name_first"]),
                                creatorLastName = Convert.ToString(reader["creator_name_last"]),
                                status = Convert.ToInt16(reader["status"])
                            };
                            record.fileSize = reader["file_size"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["file_size"]);

                            result = record;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Records information with specific id owned by specific patient
        /// </summary>
        public bool RetrieveRecordExists(long id, string patientNRIC)
        {
            bool result = new bool();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT EXISTS 
	                (SELECT id
	                FROM record
	                WHERE id = @id AND patient_nric = @patientNRIC) 
                    as result;";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);

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


        /// <summary>
        /// Retrieve all the diagnoses attributed to a specific record
        /// </summary>
        public List<RecordDiagnosis> RetrieveRecordDiagnoses(int recordID, string patientNRIC)
        {
            List<RecordDiagnosis> result = new List<RecordDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short, d.category_title, 
                    a.name_first, a.name_last
                    FROM record_diagnosis rd 
                    INNER JOIN record r ON r.id = rd.record_id
                    INNER JOIN account_patient ap ON ap.nric = r.patient_nric
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    INNER JOIN account a ON a.nric = rd.creator_nric
                    WHERE r.patient_nric = @patientNRIC AND r.id = @recordID 
                    ORDER BY rd.create_time DESC;";

                cmd.Parameters.AddWithValue("@recordID", recordID);
                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity.Therapist therapist = new Entity.Therapist
                            {
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"])
                            };

                            Diagnosis diagnosis = new Diagnosis
                            {
                                code = Convert.ToString(reader["diagnosis_code"]),
                                descriptionShort = Convert.ToString(reader["diagnosis_description_short"]),
                                categoryTitle = Convert.ToString(reader["category_title"])
                            };

                            RecordDiagnosis recordDiagnosis = new RecordDiagnosis
                            {
                                therapist = therapist,
                                diagnosis = diagnosis,
                            };

                            result.Add(recordDiagnosis);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Retrieve all the diagnoses attributed to a specific record of a specific patient
        /// </summary>
        public List<RecordDiagnosis> RetrieveRecordDiagnoses(int recordID, string patientNRIC, string therapistNRIC)
        {
            List<RecordDiagnosis> result = new List<RecordDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short, d.category_title, 
                    a.name_first, a.name_last
                    FROM record_diagnosis rd 
                    INNER JOIN record r ON r.id = rd.record_id
                    INNER JOIN account_patient ap ON ap.nric = r.patient_nric 
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    INNER JOIN account a ON a.nric = rd.creator_nric
                    INNER JOIN record_type_permission rtp ON rtp.patient_nric = ap.nric 
                    WHERE rtp.patient_nric = @patientNRIC AND rtp.therapist_nric = @therapistNRIC AND r.id = @recordID
                    ORDER BY rd.create_time DESC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity.Therapist therapist = new Entity.Therapist
                            {
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"])
                            };

                            Diagnosis diagnosis = new Diagnosis
                            {
                                code = Convert.ToString(reader["diagnosis_code"]),
                                descriptionShort = Convert.ToString(reader["diagnosis_description_short"]),
                                categoryTitle = Convert.ToString(reader["category_title"])
                            };

                            RecordDiagnosis recordDiagnosis = new RecordDiagnosis
                            {
                                therapist = therapist,
                                diagnosis = diagnosis,
                            };

                            result.Add(recordDiagnosis);
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Insert a Record Diagnosis
        /// </summary>
        public void InsertRecordDiagnosis(string therapistNRIC, int id, string code)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record_diagnosis (record_id, diagnosis_code, creator_nric)
                    VALUES (@id, @code, @creatorNRIC);";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@creatorNRIC", therapistNRIC);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
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
                                id = Convert.ToInt64(reader["id"]),
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
                                id = Convert.ToInt64(reader["id"])
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

        #region Updates
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
        #endregion

        #region Insertions
        /// <summary>
        /// Insert new record
        /// </summary>
        public void InsertContent(Record record, string creatorNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (patient_nric, creator_nric, title, description, type, content, is_emergency)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type, @content, @isEmergency);";

                cmd.Parameters.AddWithValue("@patientNRIC", record.patientNRIC);
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
                    record.id = cmd.LastInsertedId;
                }
            }
        }
        /// <summary>
        /// Insert new record
        /// </summary>
        public void InsertFile(Record record, string creatorNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record
                    (patient_nric, creator_nric, title, description, type,
                    file_name, file_extension, file_size, file_checksum, create_time, is_emergency)
                    VALUES
                    (@patientNRIC, @creatorNRIC, @title, @description, @type,
                    @fileName, @fileExtension, @fileSize, @fileChecksum, @createTime, @isEmergency);";

                cmd.Parameters.AddWithValue("@patientNRIC", record.patientNRIC);
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
                    record.id = cmd.LastInsertedId;
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
        #endregion

        #region Deletions
        public void DeleteRecord(long id)
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
        public void DeleteRecordDiagnosis(long id)
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
        public void DeleteRecordPermission(long recordID)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"DELETE FROM record_permission 
                        WHERE record_id = @recordID;";

                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteRecordPermission(long recordID, string therapistNRIC)
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

        #endregion
    }
}