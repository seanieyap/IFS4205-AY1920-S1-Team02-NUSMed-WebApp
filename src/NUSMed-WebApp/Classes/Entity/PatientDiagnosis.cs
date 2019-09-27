using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class PatientDiagnosis
    {
        public string patientNRIC { get; set; }
        public Therapist therapist { get; set; }
        public Diagnosis diagnosis { get; set; }
        public DateTime start { get; set; }
        public DateTime? end { get; set; }
    }
}