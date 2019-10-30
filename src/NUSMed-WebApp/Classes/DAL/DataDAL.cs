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
    /// UPDATED
    public DataTable RetrieveColumns()
    {
      DataTable result = new DataTable();

      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = @"SELECT account.nric, account.marital_status, account.gender, account.date_of_birth AS dob, account.address_postal_code AS postal, account.sex
                    FROM account INNER JOIN account_patient ON account.nric = account_patient.nric WHERE EXISTS (SELECT 1 FROM record WHERE record.patient_nric = account.nric);";

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
                            DELETE FROM patients_anonymized;
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
          StringBuilder sb = new StringBuilder("INSERT INTO patients_anonymized(marital_status, gender, sex, age, postal, nric, id) VALUES (");
          string formatString = "'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6});";
          string marital_status = row["Marital Status"].ToString();
          string gender = row["Gender"].ToString();
          string sex = row["Sex"].ToString();
          string age = row["Age"].ToString();
          string postal = row["Postal"].ToString();
          string nric = row["Nric"].ToString();
          string patientId = row["Id"].ToString();
          sb.AppendFormat(formatString, marital_status, gender, sex, age, postal, nric, patientId);
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
                            sex = @sex, postal = @postal, age = @age WHERE id = 1;";
        int maritalStatusLevel = 0;
        int genderLevel = 0;
        int sexLevel = 0;
        int postalLevel = 0;
        int ageLevel = 0;

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
        }
        cmd.Parameters.AddWithValue("@maritalStatus", maritalStatusLevel);
        cmd.Parameters.AddWithValue("@gender", genderLevel);
        cmd.Parameters.AddWithValue("@sex", sexLevel);
        cmd.Parameters.AddWithValue("@age", ageLevel);
        cmd.Parameters.AddWithValue("@postal", postalLevel);
        using (cmd.Connection = connection)
        {
          cmd.Connection.Open();
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void ResetGeneralizationLevel()
    {
      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = @"UPDATE generalization_level 
                            SET marital_status = -1, gender = -1,
                            sex = -1, postal = -1, age = -1 WHERE id = 1;";
        using (cmd.Connection = connection)
        {
          cmd.Connection.Open();
          cmd.ExecuteNonQuery();
        }
      }
    }

    public GeneralizedSetting RetrieveGeneralizationLevel()
    {
      GeneralizedSetting generalizedSetting = new GeneralizedSetting();
      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = @"SELECT marital_status, gender, sex, postal, age FROM generalization_level;";

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
            }
          }
        }
      }
      return generalizedSetting;
    }

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
                id = Convert.ToString(reader["id"]),
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

    public DataTable RetrieveAnonPatients(string query)
    {
      DataTable anonPatientsTable = new DataTable();

      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = query;

        using (cmd.Connection = connection)
        {
          cmd.Connection.Open();
          cmd.ExecuteNonQuery();

          MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
          mySqlDataAdapter.Fill(anonPatientsTable);
        }
      }

      return anonPatientsTable;
    }

    /// <summary>
    /// Retrieve all the diagnoses information of a specific patient
    /// </summary>
    public List<PatientDiagnosis> RetrievePatientDiagnoses(string patientId)
    {
      List<PatientDiagnosis> result = new List<PatientDiagnosis>();

      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = @" ";

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(@"SELECT pd.diagnosis_code, d.diagnosis_description_short, d.category_title 
                               FROM patient_diagnosis pd 
                               LEFT JOIN patients_anonymized pa ON pa.nric = pd.patient_nric INNER JOIN
                               diagnosis d ON pd.diagnosis_code = d.diagnosis_Code
					                    WHERE pa.id = ");
        stringBuilder.Append(patientId);
        stringBuilder.Append(" ORDER BY pd.end DESC, pd.start DESC;");

        cmd.CommandText = stringBuilder.ToString();

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
              };

              result.Add(patientDiagnosis);
            }
          }
        }
      }

      if (result.Count == 0)
      {
        return null;
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
        stringBuilder.Append(@"SELECT r.id, r.description, r.type, r.content, r.title, r.file_extension FROM record r
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
                    INNER JOIN patients_anonymized pa ON pa.nric = pd.patient_nric
                    GROUP BY d.diagnosis_code
                    ORDER BY diagnosis_code ASC;";

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
                    FROM patients_anonymized pa
                    INNER JOIN record r ON r.patient_nric = pa.nric
                    INNER JOIN record_diagnosis rd ON rd.record_id = r.id
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    GROUP BY d.diagnosis_code
                    ORDER BY d.diagnosis_code ASC;";

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
        cmd.CommandText = @"SELECT DISTINCT postal FROM patients_anonymized ORDER BY postal ASC;";

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
