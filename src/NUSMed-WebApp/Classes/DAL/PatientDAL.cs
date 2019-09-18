using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.DAL
{
    public class PatientDAL : DAL
    {
        public PatientDAL() : base() { }

        /// <summary>
        /// Retrieve all of therapist's existing patients
        /// </summary>
        public List<Entity.Therapist> RetrieveCurrentTherapists(string term, string nric)
        {
            List<Entity.Therapist> result = new List<Entity.Therapist>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, 
                    at.department, at.job_title,
                    rtp.permission_unapproved, rtp.request_time, 
                    rtp.permission_approved, rtp.approved_time
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.therapist_nric = a.nric
                    INNER JOIN account_therapist at ON rtp.therapist_nric = at.nric
                    WHERE rtp.patient_nric = @nric AND a.nric LIKE @term
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
                            Entity.Therapist therapist = new Entity.Therapist
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                therapistDepartment = Convert.ToString(reader["department"]),
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"]),
                            };
                            therapist.requestTime = reader["request_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["request_time"]);
                            therapist.approvedTime = reader["approved_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["approved_time"]);

                            result.Add(therapist);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Retrieve a specific patient's permissions
        /// </summary>
        public Entity.Therapist RetrieveTherapistPermission(string therapistNRIC, string patientNRIC)
        {
            Entity.Therapist result = new Entity.Therapist();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT rtp.therapist_nric, rtp.permission_unapproved,
                    rtp.request_time, rtp.permission_approved, rtp.approved_time,
                    a.name_first, a.name_last
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.therapist_nric = a.nric
                    WHERE rtp.therapist_nric = @therapistNRIC AND rtp.patient_nric = @patientNRIC;";

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
                            Entity.Therapist therapist = new Entity.Therapist
                            {
                                nric = Convert.ToString(reader["therapist_nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"])
                            };
                            therapist.requestTime = reader["request_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["request_time"]);
                            therapist.approvedTime = reader["approved_time"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["approved_time"]);

                            result = therapist;
                        }
                    }
                }
            }

            return result;
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

        /// <summary>
        /// Update a Request for Permissions relationship between Therapist and Patient
        /// </summary>
        public void UpdateRequestApprove(string patientNRIC, string therapistNRIC, short permission)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record_type_permission
                    SET permission_approved = @permissionApproved, 
                    permission_unapproved = 0, approved_time = NOW() 
                    WHERE patient_nric = @patientNRIC AND therapist_nric = @therapistNRIC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@permissionApproved", permission);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}