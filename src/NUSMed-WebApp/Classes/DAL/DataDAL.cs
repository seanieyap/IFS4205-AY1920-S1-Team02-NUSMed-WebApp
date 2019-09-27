using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public void InsertIntoAnonymizedTable(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                // to change to record type
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "INSERT INTO records_anonymized(marital_status, gender, sex, age, postal, record_create_date, record_id) VALUES (@maritalStatus, @gender, @sex, @age, @postal, @recordCreateDate, @recordId);";
                    string marital_status = row["Marital Status"].ToString();
                    string gender = row["Gender"].ToString();
                    string sex = row["Sex"].ToString();
                    string age = row["Age"].ToString();
                    string postal = row["Postal"].ToString();
                    string createdDate = row["Created Date"].ToString();
                    string recordId = row["Record ID"].ToString();
                    cmd.Parameters.AddWithValue("@maritalStatus", marital_status);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@sex", sex);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@postal", postal);
                    cmd.Parameters.AddWithValue("@recordCreateDate", createdDate);
                    cmd.Parameters.AddWithValue("@recordId", recordId);
                    using (cmd.Connection = connection)
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
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
                cmd.CommandText = @"UPDATE generalization_level SET marital_status = @maritalStatus, gender = @gender,
                          sex = @sex, postal = @postal, age = @age, record_create_date = @recordCreateDate LIMIT 1;";
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
        public List<RecordAnonymised> RetrieveRecords()
        {
            List<RecordAnonymised> result = new List<RecordAnonymised>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT r.patient_nric, r.type, r.content, r.file_extension,
                    ra.marital_status, ra.gender, ra.sex, ra.age, ra.postal, ra.record_create_date
                    FROM records_anonymized ra
                    INNER JOIN record r ON r.id = ra.record_id;";

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
                                patientNRIC = Convert.ToString(reader["patient_nric"]),
                                content = Convert.ToString(reader["content"]),
                                fileExtension = Convert.ToString(reader["file_extension"]),

                            };
                            record.type = RecordType.Get(Convert.ToString(reader["type"]));

                            RecordAnonymised recordAnonymised = new RecordAnonymised
                            {
                                maritalStatus = Convert.ToString(reader["marital_status"]),
                                gender = Convert.ToString(reader["gender"]),
                                sex = Convert.ToString(reader["sex"]),
                                age = Convert.ToString(reader["age"]),
                                postal = Convert.ToString(reader["postal"]),
                                createDate = Convert.ToString(reader["record_create_date"])
                            };

                            result.Add(recordAnonymised);
                        }

                    }
                }
            }

            return result;
        }
    }
}
