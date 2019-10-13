using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.DAL
{
    public class DataDAL : DAL
    {
        public DataDAL() : base() { }

        /// <summary>
        /// Create a Data Table for storing the required data for anonymization
        /// </summary>
        public DataTable RetrieveColumns()
        {
            DataTable result = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT account.marital_status AS marital_status, account.gender AS gender, account.date_of_birth AS dob, 
                    account.address_postal_code AS postal, account.sex AS sex, record.id AS record_id, record.create_time AS record_created_time
                    FROM account RIGHT JOIN record ON account.nric = record.patient_nric WHERE EXISTS (SELECT 1 FROM account_patient WHERE account_patient.nric = account.nric);";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        result.Load(reader);

                    }
                }
            }

            return result;
        }

        public void ClearAnonymizedTable()
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SET SQL_SAFE_UPDATES = 0;
                            DELETE FROM records_anonymized;
                            SET SQL_SAFE_UPDATES = 1;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertIntoAnonymizedTable(DataTable dt)
        {
            StringBuilder entireStringBuilder = new StringBuilder();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                // to change to record type
                foreach (DataRow row in dt.Rows)
                {
                    StringBuilder sb = new StringBuilder("INSERT INTO records_anonymized(marital_status, gender, sex, age, postal, record_create_date, record_id) VALUES (");
                    string formatString = "'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6});";
                    string marital_status = row["Marital Status"].ToString();
                    string gender = row["Gender"].ToString();
                    string sex = row["Sex"].ToString();
                    string age = row["Age"].ToString();
                    string postal = row["Postal"].ToString();
                    string createdDate = row["Created Date"].ToString();
                    string recordId = row["Record ID"].ToString();
                    sb.AppendFormat(formatString, marital_status, gender, sex, age, postal, createdDate, recordId);
                    entireStringBuilder.Append(sb.ToString());
                }
                cmd.CommandText = entireStringBuilder.ToString();
                using (cmd.Connection = connection)
                {
                    cmd.CommandTimeout = 180;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update the generalization_table in db.
        /// Pre-condition: A row should already exist in the table.
        /// Post-condition: There should only be a row in the table.
        /// </summary>
        public void UpdateGeneralizationLevel(Dictionary<string, int> generlizationLevel)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE generalization_level 
                            SET marital_status = @maritalStatus, gender = @gender,
                            sex = @sex, postal = @postal, age = @age, record_create_date = @recordCreateDate
                            WHERE id = 1;";
                int maritalStatusLevel = 0;
                int genderLevel = 0;
                int sexLevel = 0;
                int postalLevel = 0;
                int ageLevel = 0;
                int recordCreationDateLevel = 0;

                foreach (KeyValuePair<string, int> entry in generlizationLevel)
                {
                    string quasiIdentifier = entry.Key;
                    int level = entry.Value;

                    if (string.Equals(quasiIdentifier, "Age"))
                    {
                        ageLevel = level;
                    }
                    else if (string.Equals(quasiIdentifier, "Sex"))
                    {
                        sexLevel = level;
                    }
                    else if (string.Equals(quasiIdentifier, "Gender"))
                    {
                        genderLevel = level;
                    }
                    else if (string.Equals(quasiIdentifier, "Marital Status"))
                    {
                        maritalStatusLevel = level;
                    }
                    else if (string.Equals(quasiIdentifier, "Postal"))
                    {
                        postalLevel = level;
                    }
                    else if (string.Equals(quasiIdentifier, "Record Creation Date"))
                    {
                        recordCreationDateLevel = level;
                    }
                }
                cmd.Parameters.AddWithValue("@maritalStatus", maritalStatusLevel);
                cmd.Parameters.AddWithValue("@gender", genderLevel);
                cmd.Parameters.AddWithValue("@sex", sexLevel);
                cmd.Parameters.AddWithValue("@age", ageLevel);
                cmd.Parameters.AddWithValue("@postal", postalLevel);
                cmd.Parameters.AddWithValue("@recordCreateDate", recordCreationDateLevel);
                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieve all the anonymised records
        /// </summary>
        //public List<PatientAnonymised> RetrieveAnonymised()
        //{
        //  List<PatientAnonymised> result = new List<PatientAnonymised>();

        //  using (MySqlCommand cmd = new MySqlCommand())
        //  {
        //    cmd.CommandText = @"SELECT ra.marital_status, ra.gender, ra.sex, ra.age, ra.postal
        //      FROM records_anonymized ra
        //      INNER JOIN record r ON ra.record_id = r.id
        //      GROUP BY r.patient_nric
        //            LIMIT 100;";

        //    using (cmd.Connection = connection)
        //    {
        //      cmd.Connection.Open();
        //      cmd.ExecuteNonQuery();

        //      using (MySqlDataReader reader = cmd.ExecuteReader())
        //      {
        //        while (reader.Read())
        //        {
        //          //Record record = new Record
        //          //{
        //          //    patientNRIC = Convert.ToString(reader["patient_nric"]),
        //          //    content = Convert.ToString(reader["content"]),
        //          //    fileExtension = Convert.ToString(reader["file_extension"]),
        //          //};
        //          //record.type = RecordType.Get(Convert.ToString(reader["type"]));

        //          PatientAnonymised recordAnonymised = new PatientAnonymised
        //          {
        //            maritalStatus = Convert.ToString(reader["marital_status"]),
        //            gender = Convert.ToString(reader["gender"]),
        //            sex = Convert.ToString(reader["sex"]),
        //            age = Convert.ToString(reader["age"]),
        //            postal = Convert.ToString(reader["postal"]),
        //            //createDate = Convert.ToString(reader["record_create_date"])
        //          };

        //          result.Add(recordAnonymised);
        //        }

        //      }
        //    }
        //  }

        //  return result;
        //}

        public GeneralizedSetting RetrieveGeneralizationLevel()
        {
            GeneralizedSetting generalizedSetting = new GeneralizedSetting();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT marital_status, gender, sex, postal, age, record_create_date FROM generalization_level;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            generalizedSetting.maritalStatus = Convert.ToInt32(reader["marital_status"]);
                            generalizedSetting.gender = Convert.ToInt32(reader["gender"]);
                            generalizedSetting.sex = Convert.ToInt32(reader["sex"]);
                            generalizedSetting.age = Convert.ToInt32(reader["age"]);
                            generalizedSetting.postal = Convert.ToInt32(reader["postal"]);
                            generalizedSetting.recordCreationDate = Convert.ToInt32(reader["record_create_date"]);
                        }
                    }
                }
            }
            return generalizedSetting;
        }

        //public List<PatientAnonymised> RetrievePatients(string query, List<Tuple<string, string>> paraList)
        //{
        //  List<PatientAnonymised> patientsList = new List<PatientAnonymised>();

        //  using (MySqlCommand cmd = new MySqlCommand())
        //  {
        //    cmd.CommandText = query;

        //    foreach (Tuple<string, string> para in paraList)
        //    {
        //      cmd.Parameters.AddWithValue(para.Item1, para.Item2);
        //    }

        //    using (cmd.Connection = connection)
        //    {
        //      cmd.Connection.Open();
        //      cmd.ExecuteNonQuery();

        //      using (MySqlDataReader reader = cmd.ExecuteReader())
        //      {
        //        while (reader.Read())
        //        {
        //          PatientAnonymised patientAnonymised = new PatientAnonymised();
        //          patientAnonymised.maritalStatus = Convert.ToString(reader["marital_status"]);
        //          patientAnonymised.gender = Convert.ToString(reader["gender"]);
        //          patientAnonymised.sex = Convert.ToString(reader["sex"]);
        //          patientAnonymised.age = Convert.ToString(reader["age"]);
        //          patientAnonymised.postal = Convert.ToString(reader["postal"]);

        //          patientsList.Add(patientAnonymised);
        //        }
        //      }
        //    }
        //  }
        //  return patientsList;
        //}

        public List<PatientAnonymised> RetrievePatients(string query)
        {
            List<PatientAnonymised> patientsList = new List<PatientAnonymised>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = query;

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatientAnonymised patientAnonymised = new PatientAnonymised
                            {
                                maritalStatus = Convert.ToString(reader["marital_status"]),
                                gender = Convert.ToString(reader["gender"]),
                                sex = Convert.ToString(reader["sex"]),
                                age = Convert.ToString(reader["age"]),
                                postal = Convert.ToString(reader["postal"]),
                                recordIDs = Convert.ToString(reader["record_ids"])
                            };

                            patientsList.Add(patientAnonymised);
                        }
                    }
                }
            }
            return patientsList;
        }

        /// <summary>
        /// Retrieve all the diagnoses information of a specific patient
        /// </summary>
        public List<PatientDiagnosis> RetrievePatientDiagnoses(IEnumerable<Tuple<string, long>> recordIDsParameterized)
        {
            if (recordIDsParameterized.Count() == 0)
            {
                return null;
            }

            List<PatientDiagnosis> result = new List<PatientDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @" ";

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT pd.diagnosis_code, pd.start, pd.end, 
                    d.diagnosis_description_short, d.category_title 
                    FROM patient_diagnosis pd 
                    INNER JOIN diagnosis d ON pd.diagnosis_code = d.diagnosis_Code
					WHERE pd.patient_nric = 
                        (SELECT r.patient_nric 
						    FROM records_anonymized ra 
                            INNER JOIN record r ON ra.record_id = r.id
                            WHERE ");
                stringBuilder.Append(string.Join(" OR ", recordIDsParameterized.Select(r => " ra.record_id = " + r.Item1)));
                stringBuilder.Append(@" GROUP BY r.patient_nric LIMIT 1);");

                cmd.CommandText = stringBuilder.ToString();

                foreach (Tuple<string, long> r in recordIDsParameterized)
                {
                    cmd.Parameters.AddWithValue(r.Item1, r.Item2);
                }

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Diagnosis diagnosis = new Diagnosis
                            {
                                code = Convert.ToString(reader["diagnosis_code"]),
                                descriptionShort = Convert.ToString(reader["diagnosis_description_short"]),
                                categoryTitle = Convert.ToString(reader["category_title"])
                            };

                            PatientDiagnosis patientDiagnosis = new PatientDiagnosis
                            {
                                therapist = new Entity.Therapist(),
                                diagnosis = diagnosis,
                                start = Convert.ToDateTime(reader["start"]),
                            };
                            patientDiagnosis.end = reader["end"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["end"]);

                            result.Add(patientDiagnosis);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Records information via a list of record IDs
        /// </summary>
        public List<Record> RetrieveRecords(IEnumerable<Tuple<string, long>> recordIDsParameterized)
        {
            if (recordIDsParameterized.Count() == 0)
            {
                return null;
            }

            List<Record> result = new List<Record>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT r.id, r.description, r.type, r.content, r.title, 
                    r.create_time, r.file_extension
                    FROM record r
                    INNER JOIN records_anonymized ra ON ra.record_id = r.id 
                    INNER JOIN account a ON a.nric = r.creator_nric
                    WHERE ");
                stringBuilder.Append(string.Join(" OR ", recordIDsParameterized.Select(r => " r.id = " + r.Item1)));
                stringBuilder.Append(@";");

                cmd.CommandText = stringBuilder.ToString();

                foreach (Tuple<string, long> r in recordIDsParameterized)
                {
                    cmd.Parameters.AddWithValue(r.Item1, r.Item2);
                }

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
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
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
        /// Retrieve Records information with specific id owned by specific patient
        /// </summary>
        public Record RetrieveRecord(long recordID)
        {
            Record result = new Record();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT r.id, r.patient_nric, r.description, r.type, r.content, r.title, 
                    r.file_name, r.file_checksum, r.file_extension, r.file_size, r.create_time
                    FROM record r
                    INNER JOIN account a ON a.nric = r.creator_nric
                    WHERE r.id = @id 
                    ORDER BY r.create_time DESC;";

                cmd.Parameters.AddWithValue("@id", recordID);

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
                                description = Convert.ToString(reader["description"]),
                                type = RecordType.Get(Convert.ToString(reader["type"])),
                                content = Convert.ToString(reader["content"]),
                                title = Convert.ToString(reader["title"]),
                                fileName = Convert.ToString(reader["file_name"]),
                                fileChecksum = Convert.ToString(reader["file_checksum"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),
                                createTime = Convert.ToDateTime(reader["create_time"])
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
        /// Retrieve all the diagnoses attributed to a specific record
        /// </summary>
        public List<RecordDiagnosis> RetrieveRecordDiagnoses(long recordID)
        {
            List<RecordDiagnosis> result = new List<RecordDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short, d.category_title
                    FROM record_diagnosis rd 
                    INNER JOIN record r ON r.id = rd.record_id
                    INNER JOIN account_patient ap ON ap.nric = r.patient_nric
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    INNER JOIN account a ON a.nric = rd.creator_nric
                    WHERE r.id = @recordID
                    ORDER BY rd.create_time DESC;";

                cmd.Parameters.AddWithValue("@recordID", recordID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Diagnosis diagnosis = new Diagnosis
                            {
                                code = Convert.ToString(reader["diagnosis_code"]),
                                descriptionShort = Convert.ToString(reader["diagnosis_description_short"]),
                                categoryTitle = Convert.ToString(reader["category_title"])
                            };

                            RecordDiagnosis recordDiagnosis = new RecordDiagnosis
                            {
                                therapist = new Entity.Therapist(),
                                diagnosis = diagnosis,
                            };

                            result.Add(recordDiagnosis);
                        }
                    }
                }
            }

            return result;
        }

        public DataTable RetrieveDiagnoses()
        {
            DataTable diagnosesTable = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short 
                    FROM diagnosis d
                    INNER JOIN patient_diagnosis pd ON pd.diagnosis_code = d.diagnosis_code
                    INNER JOIN account_patient ap ON ap.nric = pd.patient_nric
                    INNER JOIN record r ON r.patient_nric = pd.patient_nric
                    INNER JOIN records_anonymized ra ON ra.record_id = r.id 
                    GROUP BY d.diagnosis_code
                    ORDER BY diagnosis_code;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    mySqlDataAdapter.Fill(diagnosesTable);
                }
            }
            return diagnosesTable;
        }

        public DataTable RetrieveRecordDiagnoses()
        {
            DataTable diagnosesTable = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short, d.category_title 
                    FROM records_anonymized ra
                    INNER JOIN record r ON r.id = ra.record_id
                    INNER JOIN record_diagnosis rd ON rd.record_id = r.id
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    ORDER BY d.diagnosis_code;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    mySqlDataAdapter.Fill(diagnosesTable);
                }
            }
            return diagnosesTable;
        }

        public DataTable RetrievePostal()
        {
            DataTable postalCodeTable = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT postal FROM records_anonymized ORDER BY postal ASC;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    mySqlDataAdapter.Fill(postalCodeTable);
                }
            }
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = postalCodeTable.Columns["postal"];
            postalCodeTable.PrimaryKey = keyColumns;
            return postalCodeTable;
        }

        public DataTable RetrieveCreationDate()
        {
            DataTable recordCreationDateTable = new DataTable();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT record_create_date FROM records_anonymized ORDER BY record_create_date ASC;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    mySqlDataAdapter.Fill(recordCreationDateTable);
                }
            }
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = recordCreationDateTable.Columns["record_create_date"];
            recordCreationDateTable.PrimaryKey = keyColumns;
            return recordCreationDateTable;
        }
    }
}
