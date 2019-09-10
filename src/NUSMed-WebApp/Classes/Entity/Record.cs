using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Record
    {
        public int id { get; set; }
        public string patientNRIC { get; set; }
        public string creatorNRIC { get; set; }
        public string creatorFirstName { get; set; }
        public string creatorLastName { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
        public string digitalSignature { get; set; }
        public string remarks { get; set; }
        public string fileType { get; set; }
        public string path { get; set; }
        public string content { get; set; }
        public string extension { get; set; }

        public RecordType type { get; set; }

        #region Validation Helpers
        public bool IsContentValid()
        {
            if (!type.isContent)
                return false;

            return type.IsContentValid(content);
        }
        public bool IsFileValid()
        {
            if (!type.isContent)
                return false;

            return type.IsFileValid(extension);
        }
        #endregion
    }
}