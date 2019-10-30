using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Security.AntiXss;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Account
    {
        private string _nric;
        public string nric
        {
            get
            {
                if (string.IsNullOrEmpty(_nric))
                {
                    return string.Empty;
                }
                return AntiXssEncoder.HtmlEncode(_nric.ToUpper(), true);
            }
            set
            {
                _nric = value.ToUpper();
            }
        }
        public string salt { get; set; }
        public string hash { get; set; }
        private string _firstName;
        public string firstName
        {
            get
            {
                if (string.IsNullOrEmpty(_firstName))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AntiXssEncoder.HtmlEncode(_firstName.ToLower(), true));
            }
            set
            {
                _firstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _lastName;
        public string lastName
        {
            get
            {
                if (string.IsNullOrEmpty(_lastName))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AntiXssEncoder.HtmlEncode(_lastName.ToLower(), true));
            }
            set
            {
                _lastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _countryOfBirth;
        public string countryOfBirth
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_countryOfBirth, true);
            }
            set
            {
                _countryOfBirth = value;
            }
        }

        private string _nationality;
        public string nationality
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_nationality, true);
            }
            set
            {
                _nationality = value;
            }
        }

        private string _maritalStatus;
        public string maritalStatus
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_maritalStatus, true);
            }
            set
            {
                _maritalStatus = value;
            }
        }

        private string _sex;
        public string sex
        {
            get
            { 
                return AntiXssEncoder.HtmlEncode(_sex, true);
            }
            set
            {
                _sex = value;
            }
        }
        private string _gender;
        public string gender
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_gender, true);
            }
            set
            {
                _gender = value;
            }
        }
        private string _address;
        public string address
        {
            get
            {
                if (string.IsNullOrEmpty(_address))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AntiXssEncoder.HtmlEncode(_address, true).ToLower());
            }
            set
            {
                _address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        private string _addressPostalCode;
        public string addressPostalCode
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_addressPostalCode, true);
            }
            set
            {
                _addressPostalCode = value.ToLower();
            }
        }


        private string _email;
        public string email
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_email, true);
            }
            set
            {
                _email = value.ToLower();
            }
        }

        private string _contactNumber;
        public string contactNumber
    {
            get
            {
                return AntiXssEncoder.HtmlEncode(_contactNumber, true);
            }
            set
            {
                _contactNumber = value.ToLower();
            }
        }

        public DateTime dateOfBirth { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? lastFullLogin { get; set; } = null;

        #region Patient
        private string _nokName;
        public string nokName
        {
            get
            {
                if (string.IsNullOrEmpty(_nokName))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AntiXssEncoder.HtmlEncode(_nokName, true).ToLower());
            }
            set
            {
                _nokName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _nokContact;
        public string nokContact
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_nokContact, true);
            }
            set
            {
                _nokContact = value.ToLower();
            }
        }

        #endregion
        #region Therapist
        private string _therapistJobTitle;
        public string therapistJobTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_therapistJobTitle))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AntiXssEncoder.HtmlEncode(_therapistJobTitle, true).ToLower());
            }
            set
            {
                _therapistJobTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _therapistDepartment;
        public string therapistDepartment
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_therapistDepartment, true);
            }
            set
            {
                _therapistDepartment = value;
            }
        }

        #endregion
        #region Researcher
        private string _researcherJobTitle;
        public string researcherJobTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_researcherJobTitle))
                {
                    return string.Empty;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_researcherJobTitle.ToLower());
            }
            set
            {
                _researcherJobTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        public string researcherDepartment { get; set; }
        #endregion

        // 0 = disabled / lock, 
        // 1 = active with MFA,
        // 2 = active, excluded from MFA.
        public short status { get; set; } = 0;

        #region MFA 
        // Token ID, NFC chip
        public string associatedTokenID { get; set; } = null;
        // Device reading token ID, generated Uid from android app
        public string associatedDeviceID { get; set; } = null;
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

        #endregion

        #region Roles
        // 0 = disabled
        // 1 = enabled
        public int patientStatus { get; set; } = 0;
        public int therapistStatus { get; set; } = 0;
        public int researcherStatus { get; set; } = 0;
        public int adminStatus { get; set; } = 0;
        public List<string> roles
        {
            get
            {
                List<string> roles = new List<string>();

                if (patientStatus == 1)
                    roles.Add("Patient");
                if (therapistStatus == 1)
                    roles.Add("Therapist");
                if (researcherStatus == 1)
                    roles.Add("Researcher");
                if (adminStatus == 1)
                    roles.Add("Administrator");

                return roles;
            }
        }
        #endregion

    }
}