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
        /// Retrieve all Accounts registered in the database search by term
        /// </summary>
        public List<Account> RetrieveAllAccounts(string term)
        {
            List<Account> result = new List<Account>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.`nric`, a.`name_first`, a.`birth_country`, a.`nationality`, a.`sex`, a.`gender`,
                    a.`martial_status`, a.`name_last`, a.`address`, a.`address_postal_code`, a.`email`, a.`contact_number`, a.`create_time`,
                    a.last_1FA_login, a.`last_full_login`, a.`date_of_birth`, a.`status`, a.`associated_token_id`, a.`associated_device_id`, 
                    ap.`status` as patient_status, at.`status` as therapist_status, ar.`status` as researcher_status, aa.`status` as admin_status     
                    FROM account a 
                    INNER JOIN account_patient ap ON a.nric = ap.nric
                    INNER JOIN account_therapist at ON a.nric = at.nric
                    INNER JOIN account_researcher ar ON a.nric = ar.nric
                    INNER JOIN account_admin aa ON a.nric = aa.nric
                    WHERE a.`nric` LIKE @term
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
                            Account account = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                countryOfBirth = Convert.ToString(reader["birth_country"]),
                                sex = Convert.ToString(reader["sex"]),
                                gender = Convert.ToString(reader["gender"]),
                                dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                nationality = Convert.ToString(reader["nationality"]),
                                martialStatus = Convert.ToString(reader["martial_status"]),
                                email = Convert.ToString(reader["email"]),
                                address = Convert.ToString(reader["address"]),
                                addressPostalCode = Convert.ToString(reader["address_postal_code"]),
                                contactNumber = Convert.ToString(reader["contact_number"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                status = Convert.ToInt32(reader["status"]),
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                                associatedTokenID = Convert.ToString(reader["associated_token_id"]),
                                associatedDeviceID = Convert.ToString(reader["associated_device_id"]),
                            };
                            account.last1FALogin = reader["last_1FA_login"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["last_1FA_login"]);

                            account.lastFullLogin = reader["last_full_login"] == DBNull.Value ? null :
                                (DateTime?)Convert.ToDateTime(reader["last_full_login"]);

                            result.Add(account);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve all Accounts registered in the database search by term
        /// </summary>
        public List<Account> RetrieveAccount(string nric)
        {
            List<Account> result = new List<Account>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT a.`nric`, a.`name_first`, a.`birth_country`, a.`nationality`, a.`sex`, a.`gender`,
                    a.`martial_status`, a.`name_last`, a.`address`, a.`address_postal_code`, a.`email`, a.`contact_number`, a.`create_time`,
                    a.last_1FA_login, a.`last_full_login`, a.`date_of_birth`, a.`status`, a.`associated_token_id`, a.`associated_device_id`, 
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
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                nric = Convert.ToString(reader["nric"]),
                                firstName = Convert.ToString(reader["name_first"]),
                                lastName = Convert.ToString(reader["name_last"]),
                                countryOfBirth = Convert.ToString(reader["birth_country"]),
                                sex = Convert.ToString(reader["sex"]),
                                gender = Convert.ToString(reader["gender"]),
                                dateOfBirth = Convert.ToDateTime(reader["date_of_birth"]),
                                nationality = Convert.ToString(reader["nationality"]),
                                martialStatus = Convert.ToString(reader["martial_status"]),
                                email = Convert.ToString(reader["email"]),
                                address = Convert.ToString(reader["address"]),
                                addressPostalCode = Convert.ToString(reader["address_postal_code"]),
                                contactNumber = Convert.ToString(reader["contact_number"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                                status = Convert.ToInt32(reader["status"]),
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                                associatedTokenID = Convert.ToString(reader["associated_token_id"]),
                                associatedDeviceID = Convert.ToString(reader["associated_device_id"]),
                            };
                            account.last1FALogin = reader["last_1FA_login"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["last_1FA_login"]);

                            account.lastFullLogin = reader["last_full_login"] == DBNull.Value ? null :
                                   (DateTime?)Convert.ToDateTime(reader["last_full_login"]);

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
            string result = "        ";

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
                cmd.CommandText = @"SELECT a.status as status, a.last_1FA_login, a.last_full_login,
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
                                patientStatus = Convert.ToInt32(reader["patient_status"]),
                                therapistStatus = Convert.ToInt32(reader["therapist_status"]),
                                researcherStatus = Convert.ToInt32(reader["researcher_status"]),
                                adminStatus = Convert.ToInt32(reader["admin_status"]),
                            };
                            result.last1FALogin = reader["last_1FA_login"] == DBNull.Value ? null :
                               (DateTime?)Convert.ToDateTime(reader["last_1FA_login"]);

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
        public void Delete(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM patient_diagnosis 
                        WHERE patient_nric = @nric;
                    DELETE FROM patient_permission 
                        WHERE patient_nric = @nric OR therapist_nric = @nric;
                    DELETE FROM patient_emergency 
                        WHERE patient_nric = @nric OR therapist_nric = @nric;
                    DELETE FROM record_type_permission 
                        WHERE patient_nric = @nric OR therapist_nric = @nric;

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
                    `martial_status`, `sex`, `gender`, `address`, `address_postal_code`, `email`, `contact_number`, `date_of_birth`, `status`)
                    VALUES
                    (@nric, @salt, @hash, @associated_token_id, @firstName, @lastName, @birthCountry, @nationality, @martialStatus, @sex, 
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
                cmd.Parameters.AddWithValue("@martialStatus", account.martialStatus);
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
        #endregion

        #region Updates

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
        public void UpdateTherapistEnable(string nric)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account_therapist 
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
        #endregion

        public void Update1FALogin(string nric, DateTime dateTime)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET last_1FA_login = @dateTime
                            WHERE nric= @nric;";

                cmd.Parameters.AddWithValue("@nric", nric);
                cmd.Parameters.AddWithValue("@dateTime", dateTime);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFullLogin(string nric, DateTime dateTime)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"UPDATE account 
                            SET last_full_login = @dateTime
                            WHERE nric= @nric;";

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