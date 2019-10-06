using System;
using System.Collections.Generic;
using System.Globalization;

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
                    return _nric;
                }
                return _nric.ToUpper();
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
                    return _firstName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_firstName.ToLower());
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
                    return _lastName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_lastName.ToLower());
            }
            set
            {
                _lastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        public string countryOfBirth { get; set; }
        public string nationality { get; set; }
        private string _maritalStatus;
        public string maritalStatus
        {
            get
            {
                if (string.IsNullOrEmpty(_maritalStatus))
                {
                    return _maritalStatus;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_maritalStatus.ToLower());
            }
            set
            {
                _maritalStatus = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        private string _sex;
        public string sex
        {
            get
            {
                if (string.IsNullOrEmpty(_sex))
                {
                    return _sex;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_sex.ToLower());
            }
            set
            {
                _sex = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _gender;
        public string gender
        {
            get
            {
                if (string.IsNullOrEmpty(_gender))
                {
                    return _gender;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_gender.ToLower());
            }
            set
            {
                _gender = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        private string _address;
        public string address
        {
            get
            {
                if (string.IsNullOrEmpty(_address))
                {
                    return _address;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_address.ToLower());
            }
            set
            {
                _address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        public string addressPostalCode { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public DateTime dateOfBirth { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? lastFullLogin { get; set; } = null;

        //public string accountStatus
        //{
        //    get
        //    {
        //        if (status == 0)
        //            return "Disabled/Lock";
        //        else if (status == 1)
        //            return "Enabled";
        //        else if (status == 2)
        //            return "Enabled, Omitted from MFA";
        //        else
        //            return "Unknown";
        //    }
        //}

        //public int age
        //{
        //    get
        //    {
        //        DateTime today = DateTime.Today;
        //        int age = today.Year - dateOfBirth.Date.Year;
        //        if (dateOfBirth.Date > today.AddYears(-age))
        //            age--;
        //        return age;
        //    }
        //}

        #region Patient
        private string _nokName;
        public string nokName
        {
            get
            {
                if (string.IsNullOrEmpty(_nokName))
                {
                    return _nokName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_nokName.ToLower());
            }
            set
            {
                _nokName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        public string nokContact { get; set; }
        #endregion
        #region Therapist
        private string _therapistJobTitle;
        public string therapistJobTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_therapistJobTitle))
                {
                    return _therapistJobTitle;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_therapistJobTitle.ToLower());
            }
            set
            {
                _therapistJobTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        public string therapistDepartment { get; set; }
        #endregion
        #region Researcher
        private string _researcherJobTitle;
        public string researcherJobTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_researcherJobTitle))
                {
                    return _therapistJobTitle;
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