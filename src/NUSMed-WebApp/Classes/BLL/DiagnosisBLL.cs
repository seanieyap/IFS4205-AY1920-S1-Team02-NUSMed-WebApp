using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class DiagnosisBLL
    {
        DiagnosisDAL diagnosisDAL = new DiagnosisDAL();

        public List<PatientDiagnosis> GetDiagnosis()
        {
            if (AccountBLL.IsPatient())
            {
                return diagnosisDAL.RetrieveAllAccounts(AccountBLL.GetNRIC());
            }

            return null;
        }

    }
}