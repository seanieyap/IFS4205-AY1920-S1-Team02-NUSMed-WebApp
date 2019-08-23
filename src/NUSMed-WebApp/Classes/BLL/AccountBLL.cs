using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
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

        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public void Login(string nric, string[] roles)
        {
            FormsAuthenticationTicket formsAuthenticationTicket = new FormsAuthenticationTicket(
                version: 1,
                name: nric,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddSeconds(HttpContext.Current.Session.Timeout),
                isPersistent: false,
                userData: string.Join("|", roles));

            string encryptedTicket = FormsAuthentication.Encrypt(formsAuthenticationTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            HttpContext.Current.Response.Cookies.Add(cookie);
            //FormsAuthentication.SetAuthCookie(nric, false);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        #region Requires Authenticated Account
        public string GetNRIC()
        {
            if (IsAuthenticated())
                return HttpContext.Current.User.Identity.Name;

            return null;
        }
        public List<Account> GetAllAccounts()
        {
            //if (IsAuthenticated())
            return accountDAL.RetrieveAllAccounts();

            //return null;
        }
        public List<Account> GetAllAccounts(string term)
        {
            //if (IsAuthenticated())
            return accountDAL.RetrieveAllAccounts(term);

            //return null;
        }
        #endregion

        #region Requires Admin Account
        public void Register(string nric, string password, string associatedTokenID, string firstName, string lastName, string countryOfBirth, 
            string nationality, string sex, string gender, string martialStatus, string address, string addressPostalCode, string email, 
            string contactNumber, DateTime dateOfBirth, List<string> roles)
        {
            HashSalt hashSalt = GenerateSaltedHash(64, password);

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

        public bool DeleteAccount(string nric)
        {
            //if (IsAuthenticated())
            //{
            accountDAL.Delete(nric);
            return true;
            //}

            //return false;
        }

        #region Status
        public void StatusDisable(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateStatusDisable(nric);
        }
        public void StatusEnable(string nric)
        {
            //if (IsAuthenticated())
            accountDAL.UpdateStatusEnable(nric);
        }
        public void StatusEnableWithoutMFA(string nric)
        {
            //if (IsAuthenticated())
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


        //public List<Account> GetAllAccounts()
        //{
        //    if (IsAuthenticated())
        //        return accountDAL.RetrieveAllAccounts();

        //    return null;
        //}

        public bool IsRegistered(string nric)
        {
            return accountDAL.IsRegistered(nric);
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
        public class HashSalt
        {
            public string Hash { get; set; }
            public string Salt { get; set; }
        }

        public static HashSalt GenerateSaltedHash(int size, string password)
        {
            byte[] saltBytes = new byte[size];
            RNGCryptoServiceProvider provider = rNGCryptoServiceProvider;
            provider.GetNonZeroBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA512);
            string hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        #endregion

        #region Helpers
        public static bool IsNRICValid(string tokenID)
        {
            // TODO
            return true;
        }

        private static bool TryParseDoB(string doB, ref DateTime dateOfBirth)
        {
            return DateTime.TryParseExact(doB, "dd/M/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
        }
        public static bool IsDateOfBirthValid(string doB, ref DateTime dateOfBirth)
        {
            if (TryParseDoB(doB, ref dateOfBirth))
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