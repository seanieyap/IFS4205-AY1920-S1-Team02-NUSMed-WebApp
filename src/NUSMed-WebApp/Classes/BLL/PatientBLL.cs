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
        
        public List<Entity.Therapist> GetCurrentTherapists(string term)
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrieveCurrentTherapists(term, AccountBLL.GetNRIC());
            }

            return null;
        }
        public List<Entity.Therapist> GetCurrentTherapistsFineGrain(string term, int recordID)
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrieveCurrentTherapistsFineGrain(term, recordID, AccountBLL.GetNRIC());
            }

            return null;
        }

        public List<Entity.Therapist> GetDisallowedTherapists(int recordID, string term)
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrievePermissionsDisallow(recordID, term, AccountBLL.GetNRIC());
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
        public void ApproveRequest(string therapistNRIC, short permission)
        {
            if (AccountBLL.IsPatient())
            {
                patientDAL.UpdateRequestApprove(AccountBLL.GetNRIC(), therapistNRIC, permission);
            }
        }
        public void RevokePermissions(string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                patientDAL.UpdateRequestRevoke(AccountBLL.GetNRIC(), therapistNRIC);
            }
        }

        public List<PatientDiagnosis> GetDiagnoses()
        {
            if (AccountBLL.IsPatient())
            {
                return patientDAL.RetrievePatientDiagnoses(AccountBLL.GetNRIC());
            }

            return null;
        }
        #endregion
    }
}