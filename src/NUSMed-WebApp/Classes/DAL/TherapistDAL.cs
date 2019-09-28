using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;
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
                    rtp.permission_unapproved, rtp.request_time, rtp.permission_approved, 
                    rtp.approved_time, rtp.is_emergency
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.patient_nric = a.nric
                    WHERE rtp.therapist_nric = @nric AND a.nric LIKE @term
                    ORDER BY rtp.create_time DESC;";

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
                                isEmergency = Convert.ToBoolean(reader["is_emergency"])
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
        /// Retrieve therapist's existing patient
        /// </summary>
        public Entity.Patient RetrievePatient(string patientNRIC, string therapistNRIC)
        {
            Entity.Patient result = new Entity.Patient();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT a.nric, a.name_first, a.name_last, 
                    rtp.permission_unapproved, rtp.request_time, rtp.is_emergency,
                    rtp.permission_approved, rtp.approved_time
                    FROM record_type_permission rtp
                    INNER JOIN account a ON rtp.patient_nric = a.nric
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
                            Entity.Patient patient = new Entity.Patient
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                permissionUnapproved = Convert.ToInt16(reader["permission_unapproved"]),
                                permissionApproved = Convert.ToInt16(reader["permission_approved"]),
                                isEmergency = Convert.ToBoolean(reader["is_emergency"])
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
        /// Retrieve a specific patient's permissions
        /// </summary>
        public Entity.Patient RetrievePatientInformation(string patientNRIC, string therapistNRIC)
        {
            Entity.Patient result = new Entity.Patient();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.birth_country, a.nationality, a.sex, a.gender,
                    a.marital_status, a.name_last, a.address, a.address_postal_code, a.email, a.contact_number, a.create_time,
                    a.last_full_login, a.date_of_birth,
                    ap.nok_name, ap.nok_contact_number
                    FROM record_type_permission rtp
                    INNER JOIN account a ON a.nric = rtp.patient_nric 
                    INNER JOIN account_patient ap ON ap.nric = rtp.patient_nric
                    WHERE rtp.therapist_nric = @therapistNRIC AND rtp.patient_nric = @patientNRIC
                    AND a.status > 0 AND ap.status = 1;";

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
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                countryOfBirth = Convert.ToString(reader["birth_country"]),
                                sex = Convert.ToString(reader["sex"]),
                                gender = Convert.ToString(reader["gender"]),
                                dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                nationality = Convert.ToString(reader["nationality"]),
                                maritalStatus = Convert.ToString(reader["marital_status"]),
                                email = Convert.ToString(reader["email"]),
                                address = Convert.ToString(reader["address"]),
                                addressPostalCode = Convert.ToString(reader["address_postal_code"]),
                                contactNumber = Convert.ToString(reader["contact_number"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                nokName = Convert.ToString(reader["nok_name"]),
                                nokContact = Convert.ToString(reader["nok_contact_number"]),
                            };

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
        /// Insert a Diagnosis to a Patient
        /// </summary>
        public void InsertPatientDiagnosis(string patientNRIC, string therapistNRIC, string code)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO patient_diagnosis (patient_nric, diagnosis_code, therapist_nric)
                    VALUES (@patientNRIC, @code, @therapistNRIC);";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@code", code);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update a Patient's diagnosis's end datetime
        /// </summary>
        public void UpdatePatientDiagnosis(string patientNRIC, string therapistNRIC, string code)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"Update patient_diagnosis 
                    SET end = NOW()
                    WHERE patient_nric = @patientNRIC AND therapist_NRIC = @therapistNRIC AND 
                    diagnosis_code = @code AND end IS NULL;";

                cmd.Parameters.AddWithValue("@patientNRIC", patientNRIC);
                cmd.Parameters.AddWithValue("@therapistNRIC", therapistNRIC);
                cmd.Parameters.AddWithValue("@code", code);

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

        /// <summary>
        /// Retrieve all the diagnoses information of a specific record
        /// </summary>
        public List<RecordDiagnosis> RetrieveRecordDiagnoses(string patientNRIC, int recordID)
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
        /// Retrieve all the diagnoses information of a specific patient
        /// </summary>
        public List<PatientDiagnosis> RetrievePatientDiagnoses(string patientNRIC, string therapistNRIC)
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
                    INNER JOIN record_type_permission rtp ON rtp.patient_nric = pd.patient_nric
                    WHERE rtp.patient_nric = @patientNRIC AND rtp.therapist_nric = @therapistNRIC AND rtp.approved_time IS NOT NULL
                    ORDER BY pd.start DESC;";

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

        /// <summary>
        /// Retrieve list of diagnosis based on a keyword
        /// </summary>
        public List<Diagnosis> RetrieveDiagnoses(string term)
        {
            List<Diagnosis> result = new List<Diagnosis>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT diagnosis_code, diagnosis_description_short, category_title
                    FROM diagnosis 
                    WHERE diagnosis_code LIKE @term OR diagnosis_description_long = @term OR category_title LIKE @term
                    ORDER BY diagnosis_code desc LIMIT 25;";

                cmd.Parameters.AddWithValue("@term", "%" + term + "%");

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

                            result.Add(diagnosis);
                        }
                    }
                }
            }

            return result;
        }

    }
}