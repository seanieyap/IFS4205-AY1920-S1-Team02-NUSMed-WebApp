using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace NUSMed_WebApp.Classes.BLL
{
    public class AccountBLL
    {
        private readonly AccountDAL accountDAL = new AccountDAL();
        private readonly LogAccountBLL logBLL = new LogAccountBLL();
        public AccountBLL()
        {
        }

        public void Login(string nric, string role)
        {
            Guid guid = Guid.NewGuid();
            DateTime now = DateTime.Now;

            accountDAL.UpdateFullLogin(nric, now.AddSeconds(-1));

            FormsAuthenticationTicket formsAuthenticationTicket = new FormsAuthenticationTicket(
                version: 1,
                name: nric,
                issueDate: now,
                expiration: now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                isPersistent: false,
                userData: role + ";" + guid);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsAuthenticationTicket));
            cookie.HttpOnly = true;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Expires = formsAuthenticationTicket.Expiration;

            HttpContext.Current.Cache.Insert(nric, guid, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(FormsAuthentication.Timeout.TotalMinutes));
            HttpContext.Current.Response.Cookies.Add(cookie);

            logBLL.LogEvent(nric, "Login", "Into role, " + role + ".");
        }
        public string LoginDevice(string nric, string role)
        {
            JWTBLL jwtBll = new JWTBLL();

            string jwt = jwtBll.GetJWT(nric, role);

            if (role.Equals("11"))
            {
                role = "Multiple";
            }
            else if (role.Equals("10"))
            {
                role = "Patient";
            }
            else if (role.Equals("01"))
            {
                role = "Therapist";
            }

            logBLL.LogEvent(nric, "Device Login", "Into role, " + role + ".");

            return jwt;

            /*Guid guid = Guid.NewGuid();

            HttpContext.Current.Cache.Insert(guid.ToString(), nric, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(FormsAuthentication.Timeout.TotalMinutes));

            new LogAccountDAL().Insert(nric, nric, "Device Login", "Using role, " + role + ".");

            return guid.ToString();*/
        }

        public void Logout()
        {
            HttpContext.Current.Cache.Remove(GetNRIC());
            FormsAuthentication.SignOut();
        }
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public bool IsValid(string nric, string deviceID)
        {
            return accountDAL.IsValid(nric, deviceID);
        }
        public bool IsValid(string nric, string tokenID, string deviceID)
        {
            return accountDAL.IsValid(nric, tokenID, deviceID);
        }

        // Serves as authentication
        public Account GetStatus(string nric, string password, string deviceID, string tokenID)
        {
            string salt = accountDAL.RetrieveSalt(nric);
            HashSalt hashSalt = GenerateSaltedHash(salt, password);

            Account account = accountDAL.RetrieveStatus(nric, hashSalt.Hash, deviceID, tokenID);
            Lockout(nric, account);

            return accountDAL.RetrieveStatus(nric, hashSalt.Hash, deviceID, tokenID);
        }
        public Account GetStatus(string nric, string password, string deviceID)
        {
            string salt = accountDAL.RetrieveSalt(nric);
            HashSalt hashSalt = GenerateSaltedHash(salt, password);

            Account account = accountDAL.RetrieveStatus(nric, hashSalt.Hash, deviceID);
            Lockout(nric, account);

            return account;
        }
        public Account GetStatus(string nric, string password)
        {
            string salt = accountDAL.RetrieveSalt(nric);
            HashSalt hashSalt = GenerateSaltedHash(salt, password);

            Account account = accountDAL.RetrieveStatus(nric, hashSalt.Hash);
            Lockout(nric, account);

            return account;
        }
        public Account GetStatus(string nric)
        {
            Account account = accountDAL.RetrieveStatus(nric);
            Lockout(nric, account);

            return account;
        }

        public void SetRole(string nric, string role)
        {
            List<string> userData = new List<string>();
            userData.Add(role);
            GenericIdentity genericIdentity = new GenericIdentity(nric, "JWT");
            HttpContext.Current.User = new GenericPrincipal(genericIdentity, userData.ToArray());
        }

        private void Lockout(string nric, Account account)
        {
            if (string.IsNullOrEmpty(account.nric) || account.status == 0)
            {
                if (HttpContext.Current == null || HttpContext.Current.Cache[nric + "_LoginAttempt"] == null)
                {
                    HttpContext.Current.Cache.Insert(nric + "_LoginAttempt", 1, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                    logBLL.LogEvent(nric, "Account Login Failed Attempt", "Attempt 1.");
                }
                else
                {
                    int count = Convert.ToInt16(HttpContext.Current.Cache[nric + "_LoginAttempt"]);

                    if (count >= 3)
                    {
                        accountDAL.UpdateStatusDisable(nric);
                        HttpContext.Current.Cache.Remove(nric + "_LoginAttempt");
                        logBLL.LogEvent(nric, "Account Disabled", "Due to 3 failed attempts within 5 mins.");
                    }
                    else
                    {
                        HttpContext.Current.Cache.Insert(nric + "_LoginAttempt", count + 1, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);

                        logBLL.LogEvent(nric, "Account Login Failed Attempt", "Attempt " + count + 1 + ".");
                    }
                }
            }
            else
            {
                HttpContext.Current.Cache.Remove(nric + "_LoginAttempt");
            }
        }

        public DateTime GetCreateTime(string nric)
        {
            if ((IsPatient() && nric.Equals(GetNRIC())) ||
                IsTherapist() || IsResearcher())
            {
                return accountDAL.RetrieveCreateTime(nric);
            }

            return DateTime.Now;
        }
        #region Requires Authenticated Account
        public Account GetStatus()
        {
            if (IsAuthenticated())
            {
                return accountDAL.RetrieveStatus(GetNRIC());
            }

            return null;
        }
        public static string GetNRIC()
        {
            if (IsAuthenticated())
            {
                return HttpContext.Current.User.Identity.Name.ToUpper();
            }

            return null;
        }
        public static string GetRole()
        {
            if (IsAuthenticated())
            {
                if (HttpContext.Current.User.IsInRole("Multiple"))
                {
                    return "Not Selected";
                }
                else if (HttpContext.Current.User.IsInRole("Patient"))
                {
                    return "Patient";
                }
                else if (HttpContext.Current.User.IsInRole("Therapist"))
                {
                    return "Therapist";
                }
                else if (HttpContext.Current.User.IsInRole("Researcher"))
                {
                    return "Researcher";
                }
                else if (HttpContext.Current.User.IsInRole("Administrator"))
                {
                    return "Administrator";
                }
            }

            return null;
        }
        public static bool IsMultiple()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Multiple");

            return false;
        }
        public static bool IsPatient()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Patient");

            return false;
        }
        public static bool IsTherapist()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Therapist");

            return false;
        }
        public static bool IsResearcher()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Researcher");

            return false;
        }
        public static bool IsAdministrator()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Administrator");

            return false;
        }
        public static bool HasMultipleRole()
        {
            if (IsAuthenticated())
            {
                Account account = new AccountDAL().RetrieveAccountRoles(GetNRIC());

                if (account.roles.Count == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public Account GetAccount()
        {
            if (IsAuthenticated() && !IsMultiple())
            {
                Account result = accountDAL.Retrieve(GetNRIC());
                logBLL.LogEvent(GetNRIC(), "View Account Information", "Self.");
                return result;
            }

            return null;
        }
        public Account GetPersonalInformation(string nric)
        {
            if (IsAdministrator())
            {
                Account result = accountDAL.RetrievePersonalInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Personal Information", "Action On: " + nric + ".");
                return result;
            }

            return null;
        }
        public Account GetContactInformation(string nric)
        {
            if (IsAdministrator())
            {
                Account result = accountDAL.RetrieveContactInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Contact Information", "Action On: " + nric + ".");
                return result;
            }

            return null;
        }
        public Account GetPatientInformation(string nric)
        {
            if (IsPatient() || IsAdministrator())
            {
                Account result = accountDAL.RetrievePatientInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Patient Information", "Action On: " + nric + ".");

                return result;
            }

            return null;
        }
        public Account GetTherapistInformation(string nric)
        {
            if (IsTherapist() || IsAdministrator())
            {
                Account result = accountDAL.RetrieveTherapistInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Therapist Information", "Action On: " + nric + ".");
                return result;
            }

            return null;
        }
        public Account GetResearcherInformation(string nric)
        {
            if (IsResearcher() || IsAdministrator())
            {
                Account result = accountDAL.RetrieveReseearcherInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Researcher Information", "Action On: " + nric + ".");

                return result;
            }

            return null;
        }
        public Account GetStatusInformation(string nric)
        {
            if (IsAdministrator())
            {
                Account result = accountDAL.RetrieveStatusInformation(nric);
                logBLL.LogEvent(GetNRIC(), "View Status Information", "Action on: " + nric + ".");
                return result;
            }

            return null;
        }
        public List<Account> GetTherapists(string patientNRIC, string term)
        {
            if (IsAdministrator())
            {
                List<Account> result = accountDAL.RetrieveTherapists(patientNRIC, term);
                logBLL.LogEvent(GetNRIC(), "View List of Therapists", "Action on: " + patientNRIC + ", using Term: \"" + term + "\".");
                return result;
            }

            return null;
        }
        public List<Account> GetEmergencyTherapists(string nric)
        {
            if (IsAdministrator())
            {
                List<Account> result = accountDAL.RetrieveEmergencyTherapists(nric);
                logBLL.LogEvent(GetNRIC(), "View Emergency Therapists", "Action on: " + nric + ".");
                return result;
            }

            return null;
        }
        public void ChangePassword(string password)
        {
            if (IsAuthenticated())
            {
                string nric = GetNRIC();
                HashSalt hashSalt = GenerateSaltedHash(password);
                accountDAL.UpdatePassword(nric, hashSalt.Hash, hashSalt.Salt);
                Logout();

                logBLL.LogEvent(GetNRIC(), "Change Password", "Self.");
            }
        }
        public void UpdateContactDetails(string address, string addressPostalCode, string email, string contactNumber)
        {
            if (IsAuthenticated())
            {
                accountDAL.UpdateContactDetails(GetNRIC(), address, addressPostalCode, email, contactNumber);
                logBLL.LogEvent(GetNRIC(), "Update Contact Information", "Self.");
            }
        }
        public void UpdatePatientDetails(string nokName, string nokContact)
        {
            if (IsAuthenticated())
            {
                accountDAL.UpdatePatientDetails(GetNRIC(), nokName, nokContact);
                logBLL.LogEvent(GetNRIC(), "Update Patient Information", "Self.");
            }
        }
        #endregion

        #region Requires Admin Account
        public void Register(string nric, string password, string associatedTokenID, string firstName, string lastName, string countryOfBirth,
            string nationality, string sex, string gender, string MaritalStatus, string address, string addressPostalCode, string email,
            string contactNumber, DateTime dateOfBirth, List<string> roles)
        {
            if (!IsAdministrator())
                return;

            HashSalt hashSalt = GenerateSaltedHash(password);

            Account account = new Account
            {
                nric = nric,
                salt = hashSalt.Salt,
                hash = hashSalt.Hash,
                associatedTokenID = associatedTokenID,
                firstName = firstName,
                lastName = lastName,
                countryOfBirth = countryOfBirth,
                nationality = nationality,
                sex = sex,
                gender = gender,
                maritalStatus = MaritalStatus,
                address = address,
                addressPostalCode = addressPostalCode,
                email = email,
                contactNumber = contactNumber,
                dateOfBirth = dateOfBirth
            };

            if (string.IsNullOrEmpty(associatedTokenID))
                account.status = 2;
            else
                account.status = 1;

            accountDAL.Insert(account);
            logBLL.LogEvent(GetNRIC(), "Register Account", "Subject NRIC: " + nric + ".");

            if (roles.Contains("Patient"))
                RoleEnablePatient(account.nric);
            if (roles.Contains("Therapist"))
                RoleEnableTherapist(account.nric);
            if (roles.Contains("Researcher"))
                RoleEnableResearcher(account.nric);
            if (roles.Contains("Administrator"))
                RoleEnableAdmin(account.nric);
        }

        public bool IsRegistered(string nric)
        {
            if (IsAdministrator())
                return accountDAL.IsRegistered(nric);

            return false;
        }
        public List<Account> GetAllAccounts(string term)
        {
            if (IsAdministrator())
            {
                List<Account> result = accountDAL.RetrieveAllAccounts(term, GetNRIC());
                logBLL.LogEvent(GetNRIC(), "Get All Accounts", "Term: \"" + term + "\".");
                return result;
            }

            return null;
        }
        //public void DeleteAccount(string nric)
        //{
        //    if (IsAdministrator() && !nric.Equals(GetNRIC()))
        //    {
        //        new RecordBLL().DeleteRecords(nric);
        //        accountDAL.Delete(nric);

        //        logBLL.LogEvent(GetNRIC(), "Delete Account", "Action on: " + nric + ".");
        //    }
        //}
        public void UpdateTherapistDetails(string nric, string jobTitle, string department)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Update Therapist Information", "Action on: " + nric + ".");

                accountDAL.UpdateTherapistDetails(nric, jobTitle, department);
            }
        }
        public void UpdateResearcherDetails(string nric, string jobTitle, string department)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Update Researcher Information", "Action on: " + nric + ".");

                accountDAL.UpdateResearcherDetails(nric, jobTitle, department);
            }
        }
        public void AddEmergencyTherapist(string nric, string therapistNRIC)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Add Emergency Therapist", "Action on: " + nric + ", Therapist added: " + therapistNRIC + ".");

                accountDAL.InsertEmergencyTherapist(nric, therapistNRIC);
            }
        }
        public void RemoveEmergencyTherapist(string nric, string therapistNRIC)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Remove Emergency Therapist", "Action on: " + nric + ", Therapist removed: " + therapistNRIC + ".");

                accountDAL.DeleteEmergencyTherapist(nric, therapistNRIC);
            }
        }
        #region Status
        public void StatusDisable(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Status Disable", "Action on: " + nric + ".");

                accountDAL.UpdateStatusDisable(nric);
            }
        }
        public void StatusEnable(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                logBLL.LogEvent(GetNRIC(), "Status Enable", "Action on: " + nric + ".");

                accountDAL.UpdateStatusEnable(nric);
            }
        }
        //public void StatusEnableWithoutMFA(string nric)
        //{
        //    if (IsAdministrator() && !nric.Equals(GetNRIC()))
        //    {
        //        logBLL.LogEvent(GetNRIC(), "Status Enable Without MFA", "Action on: " + nric + ".");

        //        accountDAL.UpdateStatusEnableWithoutMFA(nric);
        //    }
        //}
        #endregion

        #region Roles
        #region Enable
        public void RoleEnablePatient(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdatePatientEnable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Enable Patient", "Action on: " + nric + ".");
            }
        }
        public void RoleEnableTherapist(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateTherapistEnable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Enable Therapist", "Action on: " + nric + ".");
            }
        }
        public void RoleEnableResearcher(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateResearcherEnable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Enable Researcher", "Action on: " + nric + ".");
            }
        }
        public void RoleEnableAdmin(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateAdminEnable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Enable Admin", "Action on: " + nric + ".");
            }
        }
        #endregion
        #region Disable
        public void RoleDisablePatient(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdatePatientDisable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Disable Patient", "Action on: " + nric + ".");
            }
        }
        public void RoleDisableTherapist(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateTherapistDisable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Disable Therapist", "Action on: " + nric + ".");
            }
        }
        public void RoleDisableResearcher(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateResearcherDisable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Disable Researcher", "Action on: " + nric + ".");
            }
        }
        public void RoleDisableAdmin(string nric)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateAdminDisable(nric);
                logBLL.LogEvent(GetNRIC(), "Role Disable Admin", "Action on: " + nric + ".");
            }
        }
        #endregion
        #endregion

        #region MFA
        public void MFATokenIDUpdate(string nric, string tokenID)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateMFATokenID(nric, tokenID);
                logBLL.LogEvent(GetNRIC(), "MFA Token ID Update", "Action on: " + nric + ".");
            }
        }
        public void MFADeviceIDUpdate(string nric, string deviceID)
        {
            if (IsAdministrator() && !nric.Equals(GetNRIC()))
            {
                accountDAL.UpdateMFADeviceID(nric, deviceID);
                logBLL.LogEvent(GetNRIC(), "MFA Device ID Update", "Action on: " + nric + ".");
            }
        }
        public void MFADeviceIDUpdateFromPhone(string nric, string tokenID, string deviceID)
        {
            accountDAL.UpdateMFADeviceIDFromPhone(nric, tokenID, deviceID);
            logBLL.LogEvent(GetNRIC(), "MFA Device ID Update From Phone", "Action on: " + nric + ".");
        }
        #endregion
        #endregion

        #region HashSalt
        private class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }

        private static HashSalt GenerateSaltedHash(string password)
        {
            byte[] saltBytes = new byte[64];
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA512);
            string hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        private static HashSalt GenerateSaltedHash(string salt, string password)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA512);
            string hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        #endregion

        #region Validators
        public static bool IsNRICValid(string nric)
        {
            if (string.IsNullOrEmpty(nric))
            {
                return false;
            }

            return nric.Length == 9;
        }
        private static bool TryParseDoB(string doB, ref DateTime dateOfBirth)
        {
            return DateTime.TryParseExact(doB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
        }
        public static bool IsDateOfBirthValid(string doB, ref DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(doB))
            {
                return false;
            }

            if (!TryParseDoB(doB, ref dateOfBirth))
                return false;
            else if (dateOfBirth > DateTime.Now)
                return false;
            else
                return true;
        }
        public static bool IsEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsAddressPostalCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            foreach (char c in code)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return code.Length == 6;
        }
        public static bool IsContactNumber(string contactNumber)
        {
            if (string.IsNullOrEmpty(contactNumber))
            {
                return false;
            }

            foreach (char c in contactNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return contactNumber.Length == 8;
        }
        public static bool IsPasswordValid(string password, string passwordConfirm)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm))
            {
                return false;
            }
            if (!password.Equals(passwordConfirm))
            {
                return false;
            }

            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return regex.IsMatch(password);
        }
        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return regex.IsMatch(password);
        }

        public static bool IsTokenIDValid(string tokenID)
        {
            if (string.IsNullOrEmpty(tokenID))
            {
                return false;
            }

            if (tokenID.Length != 24)
            {
                return false;
            }

            return true;
        }
        public static bool IsDeviceIDValid(string deviceID)
        {
            if (string.IsNullOrEmpty(deviceID))
            {
                return false;
            }

            if (deviceID.Length != 36)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}