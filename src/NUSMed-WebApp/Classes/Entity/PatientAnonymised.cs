using System;
using System.Collections.Generic;
using System.Web.Security.AntiXss;

namespace NUSMed_WebApp.Classes.Entity
{
  [Serializable]
  public class PatientAnonymised
  {
    public string id { get; set; }
    public string recordIDs { get; set; }
    private string _postal;
    public string postal
    {
      get
      {
        return AntiXssEncoder.HtmlEncode(_postal, true);
      }
      set
      {
        _postal = value;
      }
    }

    private string _age;
    public string age
    {
      get
      {
        return AntiXssEncoder.HtmlEncode(_age, true);
      }
      set
      {
        _age = value;
      }
    }
    private string _sex;
    public string sex
    {
      get
      {
        return AntiXssEncoder.HtmlEncode(_sex, true);
      }
      set
      {
        _sex = value;
      }
    }
    private string _gender;
    public string gender
    {
      get
      {
        return AntiXssEncoder.HtmlEncode(_gender, true);
      }
      set
      {
        _gender = value;
      }
    }
    private string _maritalStatus;
    public string maritalStatus
    {
      get
      {
        return AntiXssEncoder.HtmlEncode(_maritalStatus, true);
      }
      set
      {
        _maritalStatus = value;
      }
    }
  }
}
