using System;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class PatientDiagnosis
    {
        public Therapist therapist { get; set; }
        public Diagnosis diagnosis { get; set; }
        public DateTime start { get; set; }
        public DateTime? end { get; set; }
    }
}