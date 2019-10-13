using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System.Collections.Generic;

namespace NUSMed_WebApp.Classes.BLL
{
    public class PatientBLL
    {
        private readonly PatientDAL patientDAL = new PatientDAL();
        private readonly LogAccountBLL logAccountBLL = new LogAccountBLL();
        private readonly LogPermissionBLL logPermissionBLL = new LogPermissionBLL();

        #region Requires Patient Account

        public List<Entity.Therapist> GetCurrentTherapists(string term)
        {
            if (AccountBLL.IsPatient())
            {
                List<Entity.Therapist> result = patientDAL.RetrieveCurrentTherapists(term, AccountBLL.GetNRIC());
                logAccountBLL.LogEvent(AccountBLL.GetNRIC(), "View Current Therapists", "Term: \"" + term + "\".");
                return result;
            }

            return null;
        }
        public List<Entity.Therapist> GetCurrentTherapistsFineGrain(string term, long recordID)
        {
            if (AccountBLL.IsPatient())
            {
                List<Entity.Therapist> result = patientDAL.RetrieveCurrentTherapistsFineGrain(term, recordID, AccountBLL.GetNRIC());
                logPermissionBLL.LogEvent(AccountBLL.GetNRIC(), "View Record Fine Grain Permissions", "Term: \"" + term + "\", Record ID: " + recordID + ".");
                return result;
            }

            return null;
        }

        public List<Entity.Therapist> GetDisallowedTherapists(int recordID, string term)
        {
            if (AccountBLL.IsPatient())
            {
                List<Entity.Therapist> result = patientDAL.RetrievePermissionsDisallow(recordID, term, AccountBLL.GetNRIC());
                logPermissionBLL.LogEvent(AccountBLL.GetNRIC(), "View Disallowed Therapists", "Term: \"" + term + "\", Record ID: " + recordID + ".");
                return result;
            }

            return null;
        }

        public Entity.Therapist GetTherapistPermission(string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                Entity.Therapist result = patientDAL.RetrieveTherapistPermission(therapistNRIC, AccountBLL.GetNRIC());
                logPermissionBLL.LogEvent(AccountBLL.GetNRIC(), "View Therapist Permissions", "Action on: " + therapistNRIC + ".");
                return result;
            }

            return null;
        }
        public void ApproveRequest(string therapistNRIC, short permission)
        {
            if (AccountBLL.IsPatient())
            {
                patientDAL.UpdateRequestApprove(AccountBLL.GetNRIC(), therapistNRIC, permission);
                logPermissionBLL.LogEvent(AccountBLL.GetNRIC(), "Approve Therapist Permissions", "Action on: " + therapistNRIC + ".");
            }
        }
        public void RevokePermissions(string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                patientDAL.UpdateRequestRevoke(AccountBLL.GetNRIC(), therapistNRIC);
                logPermissionBLL.LogEvent(AccountBLL.GetNRIC(), "Revoke Therapist Permissions", "Action on: " + therapistNRIC + ".");
            }
        }

        public List<PatientDiagnosis> GetDiagnoses()
        {
            if (AccountBLL.IsPatient())
            {
                List<PatientDiagnosis> result = patientDAL.RetrievePatientDiagnoses(AccountBLL.GetNRIC());
                logAccountBLL.LogEvent(AccountBLL.GetNRIC(), "View Diagnoses", "Self.");

                return result;
            }

            return null;
        }
        #endregion
    }
}