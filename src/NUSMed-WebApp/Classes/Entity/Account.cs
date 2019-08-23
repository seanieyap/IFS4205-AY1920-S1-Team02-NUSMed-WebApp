using System;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]    public class Account
    {
        public string nric { get; set; }
        public string salt { get; set; }
        public string hash { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string countryOfBirth { get; set; }
        public string nationality { get; set; }
        public string martialStatus { get; set; }
        public string sex { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string addressPostalCode { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime? last1FALogin { get; set; } = null;
        public DateTime createTime { get; set; }
        public DateTime? lastFullLogin { get; set; } = null;

        // 0 = disabled / lock, 
        // 1 = active with MFA,
        // 2 = active, excluded from MFA.
        public int status { get; set; } = 0;

        #region MFA 
        // Token ID, NFC chip
        public string associatedTokenID { get; set; } = null;
        // Device reading token ID, generated Uid from android app
        public string associatedDeviceID { get; set; } = null;
        #endregion

        #region Roles
        // 0 = disabled
        // 1 = enabled
        public int patientStatus { get; set; } = 0;
        public int therapistStatus { get; set; } = 0;
        public int researcherStatus { get; set; } = 0;
        public int adminStatus { get; set; } = 0;
        #endregion

        public string accountStatus
        {
            get
            {
                if (status == 0)
                    return "Disabled/Lock";
                else if (status == 1)
                    return "Enabled";
                else if (status == 2)
                    return "Enabled, Omitted from MFA";
                else
                    return "Unknown";
            }
        }

        public string MFATokenStatus
        {
            get
            {
                if (string.IsNullOrEmpty(associatedTokenID))
                    return "Not Registered";
                else
                    return "Active";
            }
        }

        public string MFADeviceStatus
        {
            get
            {
                if (string.IsNullOrEmpty(associatedDeviceID))
                    return "Not Registered, Awaiting Sync";
                else 
                    return "Active";
            }
        }

        public int age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - dateOfBirth.Date.Year;
                if (dateOfBirth.Date > today.AddYears(-age))
                    age--;
                return age;
            }
        }
    }
}