using System;
using System.Collections.Generic;

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
        public string title { get; set; }
        public string content { get; set; }
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