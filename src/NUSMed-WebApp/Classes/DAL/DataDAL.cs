using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace NUSMed_WebApp.Classes.DAL
{
  public class DataDAL : DAL
  {

    public DataDAL() : base() { }

    /// <summary>
    /// Create a Data Table for storing the required data for anonymization
    /// </summary>
    //public DataTable RetrieveColumns()
    //{
    //  string query = "SELECT account.marital_status AS marital_status, account.gender AS gender, account.nric AS nric, account.date_of_birth AS dob, " +
    //    "account.address_postal_code AS postal, account.sex AS sex, record.id AS record_id, record.type AS record_type, record.content AS content, " +
    //    "record.file_name AS file_name, record.file_extension AS file_ext, record.create_time AS record_created_time, record_diagnosis.diagnosis_code " +
    //    "AS diagnosis_code FROM account RIGHT JOIN record ON account.nric = record.patient_nric LEFT JOIN record_diagnosis ON record_diagnosis.record_id " +
    //    "= record.id WHERE EXISTS (SELECT 1 FROM account_patient WHERE account_patient.nric = account.nric)";

    //  if (OpenConnection() == true)
    //  {
    //    MySqlCommand command = new MySqlCommand(query, connection);
    //    DataTable dt = new DataTable();
    //    MySqlDataReader reader = command.ExecuteReader();
    //    dt.Load(reader);
    //    return dt;
    //  }
    //  else
    //  {
    //    throw new Exception("Unable to open connection to the db");
    //  }
    //}

    /// <summary>
    /// Create a Data Table for storing the required data for anonymization
    /// </summary>
    public DataTable RetrieveColumns()
    {
      DataTable result = new DataTable();

      using (MySqlCommand cmd = new MySqlCommand())
      {
        cmd.CommandText = @"SELECT account.marital_status AS marital_status, account.gender AS gender, account.nric AS nric, account.date_of_birth AS dob, 
        account.address_postal_code AS postal, account.sex AS sex, record.id AS record_id, record.type AS record_type, record.content AS content, 
        record.file_name AS file_name, record.file_extension AS file_ext, record.create_time AS record_created_time, record_diagnosis.diagnosis_code 
        AS diagnosis_code FROM account RIGHT JOIN record ON account.nric = record.patient_nric LEFT JOIN record_diagnosis ON record_diagnosis.record_id 
        = record.id WHERE EXISTS (SELECT 1 FROM account_patient WHERE account_patient.nric = account.nric);";

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
          string marital_status = row["Marital Status"].ToString();
          string gender = row["Gender"].ToString();
          string sex = row["Sex"].ToString();
          string age = row["Age"].ToString();
          string postal = row["Postal"].ToString();
          string createdDate = row["Created Date"].ToString();
          string recordId = row["Record ID"].ToString();
          cmd.CommandText = "INSERT INTO records_anonymized(marital_status, gender, sex, age, postal, record_create_date, record_id) VALUES (@maritalStatus, @gender, @sex, @age, @postal, @recordCreateDate, @recordId);";
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
  }
}
