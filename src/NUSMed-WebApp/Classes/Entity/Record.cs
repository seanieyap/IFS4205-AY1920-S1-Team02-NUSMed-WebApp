using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    public class Record
    {
        public int id { get; set; }
        public string patientNRIC { get; set; }
        public string creatorNRIC { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string createTime { get; set; }
        public string digitalSignature { get; set; }
        public string remarks { get; set; }
        public string fileTYpe { get; set; }
        public string path { get; set; }

    }
}