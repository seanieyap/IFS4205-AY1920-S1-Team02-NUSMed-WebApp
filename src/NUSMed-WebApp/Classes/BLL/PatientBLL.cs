using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace NUSMed_WebApp.Classes.BLL
{
    public class PatientBLL
    {
        private readonly PatientDAL patientDAL = new PatientDAL();

        #region Requires Patient Account
        //public List<Entity.Patient> GetUnrequestedPatients(string term)
        //{
        //    if (AccountBLL.IsPatient())
        //    {
        //        List<Entity.Patient> accountsAllPatients = patientDAL.RetrieveAllPatients(term);
        //        List<Entity.Patient> accountsCurrentPatients = patientDAL.RetrieveCurrentPatientsDelimited(AccountBLL.GetNRIC());

        //        // remove current user
        //        accountsAllPatients.RemoveAll(x => x.nric == AccountBLL.GetNRIC());

        //        foreach (Entity.Patient patients in accountsAllPatients)
        //        {
        //            if (!accountsCurrentPatients.Any(x => x.nric.Equals(patients.nric)))
        //            {
        //                patients.acceptNewRequest = true;
        //            }
        //        }

        //        return accountsAllPatients;
        //    }

        //    return null;
        //}
        public List<Entity.Therapist> GetCurrentTherapists(string term)
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrieveCurrentTherapists(term, AccountBLL.GetNRIC());
            }

            return null;
        }
        public Entity.Therapist GetTherapistPermission(string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrieveTherapistPermission(therapistNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }
        public void RequestApprove(string therapistNRIC, short permission)
        {
            if (AccountBLL.IsPatient())
            {
                patientDAL.UpdateRequestApprove(AccountBLL.GetNRIC(), therapistNRIC, permission);
            }
        }

        #endregion
    }
}