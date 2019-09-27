using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class RecordAnonymised
    {
        public Record record { get; set; }
        public string postal { get; set; }
        public string createDate { get; set; }
        public string age { get; set; }
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
    }
}