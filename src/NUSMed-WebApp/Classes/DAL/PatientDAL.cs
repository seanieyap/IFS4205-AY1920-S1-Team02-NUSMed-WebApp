using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.DAL
{
    public class PatientDAL : DAL
    {
        public PatientDAL() : base() { }

        #region Retrievals
        /// <summary>
        /// Retrieve all of therapist's existing patients
        /// </summary>
        public List<Entity.Therapist> RetrieveCurrentTherapists(string term, string patientNRIC)
        {
            List<Entity.Therapist> result = new List<Entity.Therapist>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, 
                    at.department, at.job_title,
                    rtp.permission_unapproved, rtp.request_time, rtp.is_emergency,
                    rtp.permission_approved, rtp.approved_time
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.therapist_nric = a.nric
                    INNER JOIN account_therapist at ON rtp.therapist_nric = at.nric
                    WHERE rtp.patient_nric = @patientNRIC AND (a.name_first LIKE @term OR a.name_last LIKE @term)
                    ORDER BY rtp.create_time DESC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
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
                                isEmergency = Convert.ToBoolean(reader["is_emergency"])
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
        /// Retrieve all of therapist's existing patients
        /// </summary>
        public List<Entity.Therapist> RetrieveCurrentTherapistsFineGrain(string term, long recordID, string patientNRIC)
        {
            List<Entity.Therapist> result = new List<Entity.Therapist>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, 
                    at.department, at.job_title,
                    rtp.permission_unapproved, rtp.request_time, rtp.permission_approved, rtp.approved_time, rtp.is_emergency,
                    rp.status as record_permission_status
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.therapist_nric = a.nric
                    INNER JOIN account_therapist at ON rtp.therapist_nric = at.nric
                    LEFT JOIN record_permission rp ON rp.therapist_nric = rtp.therapist_nric AND rp.record_id = @recordID
                    WHERE rtp.patient_nric = @patientNRIC AND (a.name_first LIKE @term OR a.name_last LIKE @term)
                    ORDER BY rtp.create_time DESC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@term", "%" + term + "%");
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
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                therapistDepartment = Convert.ToString(reader["department"]),
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"])
                            };

                            therapist.recordPermissionStatus = reader["record_permission_status"] == DBNull.Value ? null :
                               (short?)Convert.ToInt16(reader["record_permission_status"]);

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
        /// Retrieve Therapists with access to specific record
        /// </summary>
        public List<Entity.Therapist> RetrievePermissionsDisallow(int recordID, string term, string patientNRIC)
        {
            List<Entity.Therapist> result = new List<Entity.Therapist>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, at.job_title, at.department
                    FROM record_permission rp
                    INNER JOIN account_therapist at ON at.nric = rp.therapist_nric
                    INNER JOIN account a ON a.nric = at.nric
                    INNER JOIN record r ON r.id = rp.record_id
                    WHERE rp.record_id = @recordID AND r.patient_nric = @patientNRIC AND
                    a.status > 0 AND at.status > 0 AND (a.name_first LIKE @term OR a.name_last LIKE @term)
                    ORDER BY rp.create_time DESC;";

                cmd.Parameters.AddWithValue("@term", "%" + term + "%");
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
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                therapistDepartment = Convert.ToString(reader["department"]),
                            };
                            result.Add(therapist);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve all the diagnoses information of a specific patient
        /// </summary>
        public List<PatientDiagnosis> RetrievePatientDiagnoses(string patientNRIC)
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
                    WHERE pd.patient_nric = @patientNRIC
                    ORDER BY pd.end DESC, pd.start DESC;";

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

        #region Updates
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
        /// Approve a Request for Permissions of a therapist
        /// </summary>
        public void UpdateRequestApprove(string patientNRIC, string therapistNRIC, short permission)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record_type_permission
                    SET permission_approved = @permissionApproved, 
                    permission_unapproved = 0,
                    approved_time = NOW(),
                    request_time = NULL,
                    is_emergency = false
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

        /// <summary>
        /// Update Permissions of a Therapist to the start state, revoke permissions.
        /// </summary>
        public void UpdateRequestRevoke(string patientNRIC, string therapistNRIC)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE record_type_permission
                    SET permission_approved = 0, 
                    permission_unapproved = 0,
                    approved_time = NULL,
                    request_time = NULL,
                    is_emergency = false
                    WHERE patient_nric = @patientNRIC AND therapist_nric = @therapistNRIC;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
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