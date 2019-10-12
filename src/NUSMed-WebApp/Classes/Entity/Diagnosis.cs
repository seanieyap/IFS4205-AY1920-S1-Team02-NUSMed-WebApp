using System;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Diagnosis
    {
        public string code { get; set; }
        public string descriptionShort { get; set; }
        public string descriptionLong { get; set; }
        public string categoryTitle { get; set; }
        public bool patientHas { get; set; } = false;
    }
}