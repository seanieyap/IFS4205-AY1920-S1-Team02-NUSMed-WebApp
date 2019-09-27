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
        public List<Entity.Therapist> RetrieveCurrentTherapistsFineGrain(string term, int recordID, string patientNRIC)
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
        /// Retrieve all the diagnoses information of a specific record
        /// </summary>
        public List<RecordDiagnosis> RetrieveRecordDiagnosis(string patientNRIC, int recordID)
        {
            List<RecordDiagnosis> result = new List<RecordDiagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT d.diagnosis_code, d.diagnosis_description_short, d.category_title, 
                    a.name_first, a.name_last
                    FROM record_diagnosis rd 
                    INNER JOIN record r ON r.id = rd.record_id AND r.id = @recordID
                    INNER JOIN account_patient ap ON ap.nric = r.patient_nric AND r.patient_nric = @patientNRIC
                    INNER JOIN diagnosis d ON rd.diagnosis_code = d.diagnosis_Code
                    INNER JOIN account a ON a.nric = rd.creator_nric;";

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
                    permission_unapproved = 0,
                    approved_time = NOW(),
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

    }
}