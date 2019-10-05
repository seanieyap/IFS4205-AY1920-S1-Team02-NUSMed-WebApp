using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
  [Serializable]
  public class GeneralizedSetting
  {
    public int maritalStatus { get; set; } = -1;
    public int gender { get; set; } = -1;
    public int sex { get; set; } = -1;
    public int postal { get; set; } = -1; 
    public int age { get; set; } = -1;
    public int recordCreationDate { get; set; } = -1;

  }
}
