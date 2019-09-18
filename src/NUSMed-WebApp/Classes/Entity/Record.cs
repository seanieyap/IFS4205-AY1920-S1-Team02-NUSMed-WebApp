using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private string _creatorFirstName;
        public string creatorFirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_creatorFirstName))
                {
                    return _creatorFirstName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_creatorFirstName.ToLower());
            }
            set
            {
                _creatorFirstName = value;
            }
        }
        private string _creatorLastName;
        public string creatorLastName
        {
            get
            {
                if (string.IsNullOrEmpty(_creatorLastName))
                {
                    return _creatorLastName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_creatorLastName.ToLower());
            }
            set
            {
                _creatorLastName = value;
            }
        }

        public string title { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
        public string fileName { get; set; }
        //public string fileDirectoryNameHash { get; set; }
        //public string fileNameHash { get; set; }
        public string fileChecksum { get; set; }
        public string fileExtension { get; set; }
        public int fileSize { get; set; }
        public string content { get; set; }

        public RecordType type { get; set; }

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
        public bool IsContentValid()
        {
            if (!type.isContent)
                return false;

            return type.IsContentValid(content);
        }
        public bool IsFileValid()
        {
            if (type.isContent)
                return false;

            return type.IsFileValid(fileExtension, fileSize);
        }
        public string contentAug
        {
            get
            {
                if (type.GetType() == typeof(HeightMeasurement))
                {
                    return "";
                }
                return content;
            }
        }
        #endregion
    }
}