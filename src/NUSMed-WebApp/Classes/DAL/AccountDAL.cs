using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.DAL
{
    public class AccountDAL : DAL
    {
        public AccountDAL() : base() { }

        /// <summary>
        /// Check if NRIC exists in the database
        /// </summary>
        public bool IsRegistered(string nric)
        {
            bool result = false;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT EXISTS(SELECT * FROM account a WHERE a.nric = @nric) as result;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToBoolean(reader["result"]);
                        }
                    }
                }
            }

            return result;
        }

        #region Retrievals
        /// <summary>
        /// Retrieve all Accounts registered in the database search by term, except own account
        /// </summary>
        public List<Account> RetrieveAllAccounts(string term, string nric)
        {
            List<Account> result = new List<Account>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric
                    FROM account a 
                    WHERE a.`nric` LIKE @term AND a.nric != @nric
                    ORDER BY nric
                    LIMIT 50;";

                cmd.Parameters.AddWithValue("@term", "%" + term + "%");
                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                            };

                            result.Add(account);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Account registered in the database
        /// </summary>
        public Account Retrieve(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.`nric`, a.`name_first`, a.`birth_country`, a.`nationality`, a.`sex`, a.`gender`,
                    a.`marital_status`, a.`name_last`, a.`address`, a.`address_postal_code`, a.`email`, a.`contact_number`, a.`create_time`,
                    a.`last_full_login`, a.`date_of_birth`, a.`status`, a.`associated_token_id`, a.`associated_device_id`, 
                    ap.nok_name, ap.nok_contact_number,
                    at.job_title as therapist_job_title, at.department as therapist_department,
                    ar.job_title as researcher_job_title, ar.department as researcher_department,
                    ap.`status` as patient_status, at.`status` as therapist_status, ar.`status` as researcher_status, aa.`status` as admin_status     
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    INNER JOIN account_therapist at ON a.nric = at.nric
                    INNER JOIN account_researcher ar ON a.nric = ar.nric
                    INNER JOIN account_admin aa ON a.nric = aa.nric
                    WHERE a.`nric` = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
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
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                                nokName = Convert.ToString(reader["nok_name"]),
                                nokContact = Convert.ToString(reader["nok_contact_number"]),
                                therapistJobTitle = Convert.ToString(reader["therapist_job_title"]),
                                therapistDepartment = Convert.ToString(reader["therapist_department"]),
                                researcherJobTitle = Convert.ToString(reader["researcher_job_title"]),
                                researcherDepartment = Convert.ToString(reader["researcher_department"]),
                            };

                            result.lastFullLogin = reader["last_full_login"] == DBNull.Value ? null :
                                   (DateTime?)Convert.ToDateTime(reader["last_full_login"]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Personal Information of Account registered in the database
        /// </summary>
        public Account RetrievePersonalInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT nric, name_first, name_last, birth_country, nationality, sex, gender,
                    marital_status, date_of_birth
                    FROM account
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                countryOfBirth = Convert.ToString(reader["birth_country"]),
                                sex = Convert.ToString(reader["sex"]),
                                gender = Convert.ToString(reader["gender"]),
                                dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                nationality = Convert.ToString(reader["nationality"]),
                                maritalStatus = Convert.ToString(reader["marital_status"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Contact Information of Account registered in the database
        /// </summary>
        public Account RetrieveContactInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT nric, address, address_postal_code, email, contact_number
                    FROM account
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                email = Convert.ToString(reader["email"]),
                                address = Convert.ToString(reader["address"]),
                                addressPostalCode = Convert.ToString(reader["address_postal_code"]),
                                contactNumber = Convert.ToString(reader["contact_number"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Patient Information of specific Account
        /// </summary>
        public Account RetrievePatientInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT nok_name, nok_contact_number
                    FROM account_patient 
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                nokName = Convert.ToString(reader["nok_name"]),
                                nokContact = Convert.ToString(reader["nok_contact_number"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Therapist Details of specific Account
        /// </summary>
        public Account RetrieveTherapistInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT job_title, department
                    FROM account_therapist 
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                therapistDepartment = Convert.ToString(reader["department"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve Researcher Details of specific Account
        /// </summary>
        public Account RetrieveReseearcherInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT job_title, department
                    FROM account_researcher 
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                researcherJobTitle = Convert.ToString(reader["job_title"]),
                                researcherDepartment = Convert.ToString(reader["department"])
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve all Therapists registered in the database search by term
        /// </summary>
        public List<Account> RetrieveTherapists(string patientNRIC, string term)
        {
            List<Account> result = new List<Account>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, at.job_title, at.department
                    FROM account a
                    INNER JOIN account_therapist at ON a.nric = at.nric 
                    WHERE a.status > 0 AND at.status = 1 AND a.nric != @patientNRIC AND a.nric LIKE @term
                    AND a.nric NOT IN (
                        SELECT aa.nric
                        FROM patient_emergency pe
                        INNER JOIN account_therapist att ON att.nric = pe.therapist_nric
                        INNER JOIN account aa ON aa.nric = pe.therapist_nric
                        WHERE pe.patient_nric = @patientNRIC)
                    ORDER BY nric
                    LIMIT 25;";

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
                            Account account = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                therapistDepartment = Convert.ToString(reader["department"])
                            };
                            result.Add(account);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve emergency Therapists of a Patient registered in the database
        /// </summary>
        public List<Account> RetrieveEmergencyTherapists(string nric)
        {
            List<Account> result = new List<Account>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.nric, a.name_first, a.name_last, at.job_title, at.department
                    FROM patient_emergency pe
                    INNER JOIN account_therapist at ON at.nric = pe.therapist_nric
                    INNER JOIN account a ON a.nric = pe.therapist_nric
                    WHERE a.status > 0 AND at.status = 1 AND pe.patient_nric = @nric
                    ORDER BY a.nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                therapistJobTitle = Convert.ToString(reader["job_title"]),
                                therapistDepartment = Convert.ToString(reader["department"])
                            };
                            result.Add(account);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve salt of specific account registered in the database
        /// </summary>
        public string RetrieveSalt(string nric)
        {
            string result = "1234567812345678";

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT salt
                    FROM account
                    WHERE nric = @nric";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToString(reader["salt"]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve status of specific account registered in the database
        /// </summary>
        public Account RetrieveStatus(string nric, string hash)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.status as status, 
                    ap.status as patient_status, at.status as therapist_status, 
                    ar.status as researcher_status, aa.status as admin_status
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    INNER JOIN account_therapist at ON a.nric = at.nric
                    INNER JOIN account_researcher ar ON a.nric = ar.nric
                    INNER JOIN account_admin aa ON a.nric = aa.nric
                    WHERE a.nric = @nric AND hash = @hash;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@hash", hash);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                status = Convert.ToInt32(reader["status"]),
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                            };
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve status of specific account registered in the database
        /// </summary>
        public Account RetrieveStatus(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.status as status, a.last_full_login, 
                    a.associated_token_id, a.associated_device_id,
                    ap.status as patient_status, at.status as therapist_status, 
                    ar.status as researcher_status, aa.status as admin_status
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    INNER JOIN account_therapist at ON a.nric = at.nric
                    INNER JOIN account_researcher ar ON a.nric = ar.nric
                    INNER JOIN account_admin aa ON a.nric = aa.nric
                    WHERE a.nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                status = Convert.ToInt32(reader["status"]),
                                associatedTokenID = Convert.ToString(reader["associated_token_id"]),
                                associatedDeviceID = Convert.ToString(reader["associated_device_id"]),
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                            };

                            result.lastFullLogin = reader["last_full_login"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["last_full_login"]);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve status of specific account registered in the database
        /// </summary>
        public DateTime RetrieveCreateTime(string nric)
        {
            DateTime result = new DateTime();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT create_time
                    FROM account 
                    WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime status = Convert.ToDateTime(reader["create_time"]);

                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Retrieve status of specific account registered in the database
        /// </summary>
        public Account RetrieveStatusInformation(string nric)
        {
            Account result = new Account();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.status as status, a.last_full_login, a.create_time, a.associated_token_id, a.associated_device_id,
                    ap.status as patient_status, at.status as therapist_status, ar.status as researcher_status, aa.status as admin_status
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    INNER JOIN account_therapist at ON a.nric = at.nric
                    INNER JOIN account_researcher ar ON a.nric = ar.nric
                    INNER JOIN account_admin aa ON a.nric = aa.nric
                    WHERE a.nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Account
                            {
                                status = Convert.ToInt32(reader["status"]),
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                associatedTokenID = Convert.ToString(reader["associated_token_id"]),
                                associatedDeviceID = Convert.ToString(reader["associated_device_id"])
                            };
                            result.lastFullLogin = reader["last_full_login"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["last_full_login"]);
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region Deletions
        /// <summary>
        /// Delete all database records related to specific Account
        /// </summary>
        public void Delete(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM patient_diagnosis 
                        WHERE patient_nric = @nric;
                    DELETE FROM patient_emergency
                        WHERE patient_nric = @nric OR therapist_nric = @nric;
                    DELETE FROM record_type_permission
                        WHERE patient_nric = @nric OR therapist_nric = @nric;
                    DELETE FROM record
                        WHERE patient_nric = @nric OR creator_nric = @nric;

                    DELETE FROM account_patient 
                        WHERE nric = @nric;
                    DELETE FROM account_therapist 
                        WHERE nric = @nric;
                    DELETE FROM account_researcher
                        WHERE nric = @nric;
                    DELETE FROM account_admin
                        WHERE nric = @nric;
                    DELETE FROM account 
                        WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete a Emergency relationship between Patient and Therapist
        /// </summary>
        public void DeleteEmergencyTherapist(string patientNRIC, string therapistNRIC)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"DELETE FROM patient_emergency 
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

        #region Inserts
        /// <summary>
        /// Register Account
        /// </summary>
        public void Insert(Account account)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO `account`
                    (`nric`, `salt`, `hash`, `associated_token_id`, `name_first`, `name_last`, `birth_country`, `nationality`, 
                    `marital_status`, `sex`, `gender`, `address`, `address_postal_code`, `email`, `contact_number`, `date_of_birth`, `status`)
                    VALUES
                    (@nric, @salt, @hash, @associated_token_id, @firstName, @lastName, @birthCountry, @nationality, @MaritalStatus, @sex, 
                    @gender, @address, @addressPostalCode, @email, @contactNumber, @dateOfBirth, @status);

                    INSERT INTO `account_patient` (`nric`)
                        VALUES (@nric);
                    INSERT INTO `account_therapist` (`nric`)
                        VALUES (@nric);
                    INSERT INTO `account_researcher` (`nric`)
                        VALUES (@nric);
                    INSERT INTO `account_admin` (`nric`)
                        VALUES (@nric);";

                cmd.Parameters.AddWithValue("@nric", account.nric);
                cmd.Parameters.AddWithValue("@salt", account.salt);
                cmd.Parameters.AddWithValue("@hash", account.hash);
                cmd.Parameters.AddWithValue("@firstName", account.firstName);
                cmd.Parameters.AddWithValue("@lastName", account.lastName);
                cmd.Parameters.AddWithValue("@birthCountry", account.countryOfBirth);
                cmd.Parameters.AddWithValue("@nationality", account.nationality);
                cmd.Parameters.AddWithValue("@MaritalStatus", account.maritalStatus);
                cmd.Parameters.AddWithValue("@sex", account.sex);
                cmd.Parameters.AddWithValue("@gender", account.gender);
                cmd.Parameters.AddWithValue("@address", account.address);
                cmd.Parameters.AddWithValue("@addressPostalCode", account.addressPostalCode);
                cmd.Parameters.AddWithValue("@email", account.email);
                cmd.Parameters.AddWithValue("@contactNumber", account.contactNumber);
                cmd.Parameters.AddWithValue("@dateOfBirth", account.dateOfBirth);
                cmd.Parameters.AddWithValue("@associated_token_id", account.associatedTokenID);
                cmd.Parameters.AddWithValue("@status", account.status);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Insert a Emergency relationship between Therapist and Patient
        /// </summary>
        public void InsertEmergencyTherapist(string patientNRIC, string therapistNRIC)
        {
            if (patientNRIC == therapistNRIC)
                return;

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO patient_emergency
                    (patient_nric, therapist_nric)
                    VALUES (@patientNRIC, @therapistNRIC);";

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

        #region Updates
        public void UpdatePassword(string nric, string hash, string salt)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET salt = @salt, hash = @hash
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@hash", hash);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateContactDetails(string nric, string address, string addressPostalCode, string email, string contactNumber)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET address = @address, address_postal_code = @addressPostalCode, 
                            email = @email, contact_number = @contactNumber
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@addressPostalCode", addressPostalCode);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@contactNumber", contactNumber);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePatientDetails(string nric, string nokName, string nokContact)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_patient 
                            SET nok_name = @nokName, nok_contact_number = @nokContactNumber
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@nokName", nokName);
                cmd.Parameters.AddWithValue("@nokContactNumber", nokContact);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTherapistDetails(string nric, string jobTitle, string department)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_therapist 
                            SET job_title = @jobTitle, department = @department
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@jobTitle", jobTitle);
                cmd.Parameters.AddWithValue("@department", department);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateResearcherDetails(string nric, string jobTitle, string department)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_researcher
                            SET job_title = @jobTitle, department = @department
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@jobTitle", jobTitle);
                cmd.Parameters.AddWithValue("@department", department);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #region Status
        public void UpdateStatusDisable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateStatusEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateStatusEnableWithoutMFA(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 2);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Role
        public void UpdatePatientEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_patient 
                            SET status = @status
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTherapistEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_therapist 
                            SET status = @status
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateResearcherEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_researcher 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateAdminEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_admin 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 1);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdatePatientDisable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_patient 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTherapistDisable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_therapist 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateResearcherDisable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_researcher 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateAdminDisable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_admin 
                            SET status = @status
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@status", 0);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region MFA        
        public void UpdateMFATokenID(string nric, string tokenID)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET associated_token_id = @associatedTokenID
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@associatedTokenID", tokenID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateMFADeviceID(string nric, string deviceID)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET associated_device_id = @associatedDeviceID
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@associatedDeviceID", deviceID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateMFADeviceIDFromPhone(string tokenID, string deviceID)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET associated_device_id = @associatedDeviceID
                            WHERE associated_token_id = @associatedTokenID;";

                cmd.Parameters.AddWithValue("@associatedTokenID", tokenID);
                cmd.Parameters.AddWithValue("@associatedDeviceID", deviceID);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        public void UpdateFullLogin(string nric, DateTime dateTime)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET last_full_login = @dateTime
                            WHERE nric = @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@dateTime", dateTime);

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