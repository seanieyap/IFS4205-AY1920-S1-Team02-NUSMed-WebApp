using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.DAL
{
    public class TherapistDAL : DAL
    {
        public TherapistDAL() : base() { }
        /// <summary>
        /// Retrieve all Accounts who are patients
        /// </summary>
        public List<Entity.Patient> RetrieveAllPatients(string term)
        {
            List<Entity.Patient> result = new List<Entity.Patient>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    WHERE a.`nric` LIKE @term AND a.status > 0 AND ap.status = 1
                    ORDER BY nric
                    LIMIT 50;";

                cmd.Parameters.AddWithValue("@term", "%" + term + "%");

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity.Patient patient = new Entity.Patient
                            {
                                nric = Convert.ToString(reader["nric"])
                            };

                            result.Add(patient);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Retrieve all of therapist's existing patients
        /// </summary>
        public List<Entity.Patient> RetrieveCurrentPatientsDelimited(string nric)
        {
            List<Entity.Patient> result = new List<Entity.Patient>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT patient_nric
                    FROM record_type_permission
                    WHERE therapist_nric = @nric
                    ORDER BY create_time;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity.Patient patient = new Entity.Patient
                            {
                                nric = Convert.ToString(reader["patient_nric"])
                            };

                            result.Add(patient);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Retrieve all of therapist's existing patients
        /// </summary>
        public List<Entity.Patient> RetrieveCurrentPatients(string term, string nric)
        {
            List<Entity.Patient> result = new List<Entity.Patient>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, 
                    rtp.permission_unapproved, rtp.request_time, 
                    rtp.permission_approved, rtp.approved_time
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.patient_nric = a.nric
                    WHERE rtp.therapist_nric = @nric AND a.nric LIKE @term
                    ORDER BY rtp.create_time;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@term", "%" + term + "%");

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity.Patient patient = new Entity.Patient
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"]),
                            };
                            patient.requestTime = reader["request_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["request_time"]);
                            patient.approvedTime = reader["approved_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["approved_time"]);

                            result.Add(patient);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Retrieve a specific patient's permissions
        /// </summary>
        public Entity.Patient RetrievePatientPermission(string patientNRIC, string therapistNRIC)
        {
            Entity.Patient result = new Entity.Patient();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT patient_nric, permission_unapproved,
                    request_time, permission_approved, approved_time
                    FROM record_type_permission
                    WHERE therapist_nric = @therapistNRIC AND patient_nric = @patientNRIC;";

                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Entity.Patient patient = new Entity.Patient
                            {
                                nric = Convert.ToString(reader["patient_nric"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"])
                            };
                            patient.requestTime = reader["request_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["request_time"]);
                            patient.approvedTime = reader["approved_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["approved_time"]);

                            result = patient;
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Insert a Request for Permissions relationship between Therapist and Patient
        /// </summary>
        public void InsertRecordTypeRequest(string patientNRIC, string therapistNRIC, short permission)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO record_type_permission
                    (patient_nric, therapist_nric, permission_unapproved, request_time)
                    VALUES (@patientNRIC, @therapistNRIC, @permissionUnapproved, @request_time);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@permissionUnapproved", permission);
                cmd.Parameters.AddWithValue("@request_time", DateTime.Now);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Update a Request for Permissions relationship between Therapist and Patient
        /// </summary>
        public void UpdateRecordTypeRequest(string patientNRIC, string therapistNRIC, short permission)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record_type_permission
                    SET permission_unapproved = @permissionUnapproved
                    WHERE patient_nric = @patientNRIC AND therapist_nric = @therapistNRIC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@permissionUnapproved", permission);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}