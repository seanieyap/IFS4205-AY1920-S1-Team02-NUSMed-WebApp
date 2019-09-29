using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

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
        public string description { get; set; }
        public DateTime createTime { get; set; }

        #region Validation Helpers
        public bool IsTitleValid()
        {
            if (string.IsNullOrEmpty(title) && title.Length <= 45)
                return true;

            return false;
        }
        public bool IsDescriptionValid()
        {
            if (string.IsNullOrEmpty(description) && description.Length <= 120)
                return true;

            return false;
        }
        #endregion
    }
}