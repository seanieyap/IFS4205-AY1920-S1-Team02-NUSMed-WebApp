using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace NUSMed_WebApp.Classes.BLL
{
    public class AccountBLL
    {
        private readonly AccountDAL accountDAL = new AccountDAL();
        private static RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();

        public AccountBLL()
        {
        }

        public void Login(string nric, string role)
        {
            Guid guid = Guid.NewGuid();
            DateTime now = DateTime.Now;

            new AccountDAL().UpdateFullLogin(nric, now.AddSeconds(-1));

            FormsAuthenticationTicket formsAuthenticationTicket = new FormsAuthenticationTicket(
                version: 1,
                name: nric,
                issueDate: now,
                expiration: now.AddMinutes(HttpContext.Current.Session.Timeout),
                isPersistent: false,
                userData: role + ";" + guid);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsAuthenticationTicket));
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            //cookie.Domain = "";
            // SEAN TODO

            HttpContext.Current.Cache.Insert(nric, guid, null, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout), Cache.NoSlidingExpiration);
            HttpContext.Current.Response.Cookies.Add(cookie);

            new AccountLogDAL().Insert(nric, nric, "Log in using role, " + role + ".", "Nil");
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        // Serves as authentication
        public Account GetStatus(string nric, string password)
        {
            string salt = accountDAL.RetrieveSalt(nric);
            HashSalt hashSalt = GenerateSaltedHash(salt, password);

            return accountDAL.RetrieveStatus(nric, hashSalt.Hash);
        }
        public Account GetStatus(string nric)
        {
            return accountDAL.RetrieveStatus(nric);
        }

        #region Requires Authenticated Account
        public Account GetStatus()
        {
            if (IsAuthenticated())
                return accountDAL.RetrieveStatus(GetNRIC());

            return null;
        }
        public string GetNRIC()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.Identity.Name;

            return null;
        }
        public string GetRole()
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
        public bool IsMultiple()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Multiple");

            return false;
        }
        public bool IsPatient()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Patient");

            return false;
        }
        public bool IsTherapist()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Therapist");

            return false;
        }
        public bool IsResearcher()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Researcher");

            return false;
        }
        public bool IsAdministrator()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.IsInRole("Administrator");

            return false;
        }
        public List<Account> GetAllAccounts(string term)
        {
            if (IsAdministrator())
                return accountDAL.RetrieveAllAccounts(term);

            return null;
        }
        public Account GetAccount(string nric)
        {
            if (IsAuthenticated() && !IsMultiple())
                return accountDAL.Retrieve(nric);

            return null;
        }
        public Account GetPersonalInformation(string nric)
        {
            if (IsAdministrator())
                return accountDAL.RetrievePersonalInformation(nric);

            return null;
        }
        public Account GetContactInformation(string nric)
        {
            if (IsAdministrator())
                return accountDAL.RetrieveContactInformation(nric);

            return null;
        }
        public Account GetPatientInformation(string nric)
        {
            if (IsPatient() || IsAdministrator())
                return accountDAL.RetrievePatientInformation(nric);

            return null;
        }
        public Account GetTherapistInformation(string nric)
        {
            if (IsTherapist() || IsAdministrator())
                return accountDAL.RetrieveTherapistInformation(nric);

            return null;
        }
        public Account GetResearcherInformation(string nric)
        {
            if (IsResearcher() || IsAdministrator())
                return accountDAL.RetrieveReseearcherInformation(nric);

            return null;
        }
        public List<Account> GetTherapists(string patientNRIC, string term)
        {
            if (IsAdministrator())
                return accountDAL.RetrieveTherapists(patientNRIC, term);

            return null;
        }
        public List<Account> GetEmergencyTherapists(string nric)
        {
            if (IsAdministrator())
                return accountDAL.RetrieveEmergencyTherapists(nric);

            return null;
        }
        public void AddEmergencyTherapist(string patientNRIC, string therapistNRIC)
        {
            if (IsAdministrator())
                accountDAL.InsertEmergencyTherapist(patientNRIC, therapistNRIC);

        }
        public void RemoveEmergencyTherapist(string therapistNRIC)
        {
            if (IsAdministrator())
                accountDAL.DeleteEmergencyTherapist(therapistNRIC);
        }
        public void ChangePassword(string password)
        {
            if (!IsAuthenticated())
                return;

            string nric = GetNRIC();
            HashSalt hashSalt = GenerateSaltedHash(password);

            accountDAL.UpdatePassword(nric, hashSalt.Hash, hashSalt.Salt);
        }

        #endregion

        #region Requires Admin Account
        public void Register(string nric, string password, string associatedTokenID, string firstName, string lastName, string countryOfBirth,
            string nationality, string sex, string gender, string martialStatus, string address, string addressPostalCode, string email,
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
                martialStatus = martialStatus,
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

            if (roles.Contains("Patient"))
                RoleEnablePatient(account.nric);
            if (roles.Contains("Therapist"))
                RoleEnableTherapist(account.nric);
            if (roles.Contains("Researcher"))
                RoleEnableResearcher(account.nric);
            if (roles.Contains("Administrator"))
                RoleEnableAdmin(account.nric);
        }

        public void DeleteAccount(string nric)
        {
            if (IsAdministrator())
            {
                new RecordBLL().DeleteRecord(nric);
                accountDAL.Delete(nric);
            }
        }

        #region Status
        public void StatusDisable(string nric)
        {
            if (IsAdministrator())
                accountDAL.UpdateStatusDisable(nric);
        }
        public void StatusEnable(string nric)
        {
            if (IsAdministrator())
                accountDAL.UpdateStatusEnable(nric);
        }
        public void StatusEnableWithoutMFA(string nric)
        {
            if (IsAdministrator())
                accountDAL.UpdateStatusEnableWithoutMFA(nric);
        }
        #endregion

        #region Roles
        #region Enable
        public void RoleEnablePatient(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdatePatientEnable(nric);
        }
        public void RoleEnableTherapist(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateTherapistEnable(nric);
        }
        public void RoleEnableResearcher(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateResearcherEnable(nric);
        }
        public void RoleEnableAdmin(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateAdminEnable(nric);
        }
        #endregion
        #region Disable
        public void RoleDisablePatient(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdatePatientDisable(nric);
        }
        public void RoleDisableTherapist(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateTherapistDisable(nric);
        }
        public void RoleDisableResearcher(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateResearcherDisable(nric);
        }
        public void RoleDisableAdmin(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateAdminDisable(nric);
        }
        #endregion
        #endregion
        #endregion

        #region Status
        public void MFATokenIDUpdate(string nric, string tokenID)
        {
            if (IsAdministrator())
                accountDAL.UpdateMFATokenID(nric, tokenID);
        }
        public void MFADeviceIDUpdate(string nric, string deviceID)
        {
            if (IsAdministrator())
                accountDAL.UpdateMFADeviceID(nric, deviceID);
        }
        #endregion

        public void Update1FALogin(string nric)
        {
            accountDAL.Update1FALogin(nric, DateTime.Now.AddSeconds(-1));
        }
        public bool IsRegistered(string nric)
        {
            if (IsAdministrator())
                return accountDAL.IsRegistered(nric);

            return false;
        }
        public void UpdateContactDetails(string address, string addressPostalCode, string email, string contactNumber)
        {
            accountDAL.UpdateContactDetails(GetNRIC(), address, addressPostalCode, email, contactNumber);
            return;
        }
        public void UpdatePatientDetails(string nokName, string nokContact)
        {
            accountDAL.UpdatePatientDetails(GetNRIC(), nokName, nokContact);
            return;
        }
        public void UpdateTherapistDetails(string nric, string jobTitle, string department)
        {
            if (!IsAdministrator())
                return;

            accountDAL.UpdateTherapistDetails(nric, jobTitle, department);
        }

        public void UpdateResearcherDetails(string nric, string jobTitle, string department)
        {
            if (!IsAdministrator())
                return;

            accountDAL.UpdateResearcherDetails(nric, jobTitle, department);
        }

        //public bool UpdateAccount(string oldUserID, string userID, string domain, string designation, string name, bool active)
        //{
        //    if (IsAuthenticated())
        //    {
        //        accountDAL.Update(oldUserID, userID, domain, designation, name, active);
        //        return true;
        //    }

        //    return false;
        //}

        //public bool DeleteAccount(string userID, string domain)
        //{
        //    if (IsAuthenticated())
        //    {
        //        accountDAL.Delete(userID, domain);
        //        return true;
        //    }

        //    return false;
        //}

        //public bool InsertAccount(string userID, string domain, string designation, string name, bool active)
        //{
        //    if (IsAuthenticated())
        //    {
        //        accountDAL.Insert(userID, domain, designation, name, active);
        //        return true;
        //    }

        //    return false;
        //}

        #region HashSalt
        private class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }

        private static HashSalt GenerateSaltedHash(string password)
        {
            byte[] saltBytes = new byte[64];
            RNGCryptoServiceProvider provider = rNGCryptoServiceProvider;
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

        #region Helpers

        public static string ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        #endregion

        #region Validators
        public static bool IsNRICValid(string nric)
        {
            return nric.Length == 9;
        }
        private static bool TryParseDoB(string doB, ref DateTime dateOfBirth)
        {
            return DateTime.TryParseExact(doB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
        }
        public static bool IsDateOfBirthValid(string doB, ref DateTime dateOfBirth)
        {
            if (!TryParseDoB(doB, ref dateOfBirth))
                return false;
            else if (dateOfBirth > DateTime.Now)
                return false;
            else
                return true;
        }
        public static bool IsEmailAddress(string email)
        {
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
            foreach (char c in code)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return code.Length == 6;
        }
        public static bool IsContactNumber(string contactNumber)
        {
            foreach (char c in contactNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return contactNumber.Length == 8;
        }
        public static bool IsPassword(string password, string passwordConfirm)
        {
            if (!password.Equals(passwordConfirm))
                return false;

            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return regex.IsMatch(password);
        }
        public static bool IsTokenValid(string tokenID)
        {
            // TODO
            return true;
        }
        #endregion
    }
}