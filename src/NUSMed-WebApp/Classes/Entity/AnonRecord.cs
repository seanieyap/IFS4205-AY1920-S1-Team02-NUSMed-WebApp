using System;
using System.Collections.Generic;
using System.Globalization;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class AnonRecord
    {
      public string maritalStatus { get; set; }

      public string gender { get; set; }

      public string sex { get; set; }

      public string age { get; set; }

      public string postal { get; set; }

      public string recordCreationDate { get; set; }
    }
}
