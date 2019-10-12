using System;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class LogEvent
    {
        public long id { get; set; }
        private string _creatorNRIC;
        public string creatorNRIC
        {
            get
            {
                if (string.IsNullOrEmpty(_creatorNRIC))
                {
                    return _creatorNRIC;
                }
                return _creatorNRIC.ToUpper();
            }
            set
            {
                _creatorNRIC = value.ToUpper();
            }
        }
        private string _subjectNRIC;
        public string subjectNRIC
        {
            get
            {
                if (string.IsNullOrEmpty(_subjectNRIC))
                {
                    return _subjectNRIC;
                }
                return _subjectNRIC.ToUpper();
            }
            set
            {
                _subjectNRIC = value.ToUpper();
            }
        }
        public string action { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
    }
}