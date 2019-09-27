using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class RecordDiagnosis
    {
        public Therapist therapist { get; set; }
        public Diagnosis diagnosis { get; set; }
    }
}