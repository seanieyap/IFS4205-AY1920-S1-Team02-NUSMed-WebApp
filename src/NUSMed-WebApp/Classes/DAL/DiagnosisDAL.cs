using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.DAL
{
    public class DiagnosisDAL : DAL
    {
        public DiagnosisDAL() : base() { }

        #region Retrievals
        /// <summary>
        /// Retrieve all the diagnoses information of a specific patient
        /// </summary>
        public List<PatientDiagnosis> RetrieveAllAccounts(string patientNRIC)
        {
            List<PatientDiagnosis> result = new List<PatientDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.name_first, a.name_last, 
                    pd.diagnosis_code, pd.start, pd.end, 
                    d.diagnosis_description_short, d.category_title 
                    FROM patient_diagnosis pd 
                    INNER JOIN diagnosis d ON pd.diagnosis_code = d.diagnosis_Code
                    INNER JOIN account a ON a.nric = pd.therapist_nric
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

                            PatientDiagnosis patientDiagnosis = new PatientDiagnosis
                            {
                                patientNRIC = patientNRIC,
                                therapist = therapist,
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
       
        #endregion
    }
}