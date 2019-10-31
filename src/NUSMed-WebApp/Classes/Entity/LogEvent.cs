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
                return _creatorNRIC.ToUpper();
            }
            set
            {
                _creatorNRIC = value;
            }
        }
        private string _subjectNRIC;
        public string subjectNRIC
        {
            get
            {
                return _subjectNRIC.ToUpper();
            }
            set
            {
                _subjectNRIC = value;
            }
        }
        private string _action;
        public string action
        {
            get
            {
                return _action.ToUpper();
            }
            set
            {
                _action = value;
            }
        }
        private string _description;
        public string description
        {
            get
            {
                return _description.ToUpper();
            }
            set
            {
                _description = value;
            }
        }


        public DateTime createTime { get; set; }
    }
}