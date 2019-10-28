using System;
using System.Web.Security.AntiXss;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Diagnosis
    {
        private string _code;
        public string code
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_code, true);
            }
            set
            {
                _code = value;
            }
        }
        private string _descriptionShort;
        public string descriptionShort
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_descriptionShort, true);
            }
            set
            {
                _descriptionShort = value;
            }
        }
        private string _descriptionLong;
        public string descriptionLong
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_descriptionLong, true);
            }
            set
            {
                _descriptionLong = value;
            }
        }
        private string _categoryTitle;
        public string categoryTitle
        {
            get
            {
                return AntiXssEncoder.HtmlEncode(_categoryTitle, true);
            }
            set
            {
                _categoryTitle = value;
            }
        }

        public bool patientHas { get; set; } = false;
    }
}