using System;
using System.Collections.Generic;
using System.Web.Security.AntiXss;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Note
    {
        public long id { get; set; }
        public Patient patient { get; set; } = new Patient();
        public Therapist therapist { get; set; } = new Therapist();
        public Therapist creator { get; set; } = new Therapist();
        public List<Record> records { get; set; } = new List<Record>();
        private string _title;
        public string title
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_title, true);
            }
            set
            {
                _title = value;
            }
        }

        private string _content;
        public string content
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_content, true);
            }
            set
            {
                _content = value;
            }
        }

        public DateTime createTime { get; set; }

        #region Validation Helpers
        public bool IsTitleValid()
        {
            if (!string.IsNullOrEmpty(title) && title.Length <= 75)
                return true;

            return false;
        }
        public bool IsContentValid()
        {
            if (!string.IsNullOrEmpty(content))
                return true;

            return false;
        }
        #endregion
    }
}