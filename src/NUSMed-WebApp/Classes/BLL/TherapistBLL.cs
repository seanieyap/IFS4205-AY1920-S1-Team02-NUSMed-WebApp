using NUSMed_WebApp.Classes.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class TherapistBLL
    {
        private readonly TherapistDAL therapistDAL = new TherapistDAL();

        public Entity.Patient GetPatientInformation(string patientNRIC)
        {
            if (AccountBLL.IsTherapist())
            {
                //if (therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
                    //{
                    return therapistDAL.RetrievePatientInformation(patientNRIC, AccountBLL.GetNRIC());
                //}
            }

            return null;
        }
        public List<Entity.Patient> GetUnrequestedPatients(string term)
        {
            if (AccountBLL.IsTherapist())
            {
                List<Entity.Patient> accountsAllPatients = therapistDAL.RetrieveAllPatients(term);
                List<Entity.Patient> accountsCurrentPatients = therapistDAL.RetrieveCurrentPatientsDelimited(AccountBLL.GetNRIC());

                // remove current user
                accountsAllPatients.RemoveAll(x => x.nric.Equals(AccountBLL.GetNRIC(), StringComparison.InvariantCultureIgnoreCase));

                foreach (Entity.Patient patients in accountsAllPatients)
                {
                    if (!accountsCurrentPatients.Any(x => x.nric.Equals(patients.nric)))
                    {
                        patients.acceptNewRequest = true;
                    }
                }

                return accountsAllPatients;
            }

            return null;
        }
        public List<Entity.Patient> GetCurrentPatients(string term)
        {
            if (AccountBLL.IsTherapist())
            {
                return therapistDAL.RetrieveCurrentPatients(term, AccountBLL.GetNRIC());
            }

            return null;
        }
        public Entity.Patient GetPatient(string patientNRIC)
        {
            if (AccountBLL.IsTherapist())
            {
                return therapistDAL.RetrievePatient(patientNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }

        public Entity.Patient GetPatientPermissions(string patientNRIC)
        {
            if (AccountBLL.IsTherapist())
            {
                return therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }
        public void SubmitRequest(string patientNRIC, short permission)
        {
            if (AccountBLL.IsTherapist())
            {
                therapistDAL.InsertRecordTypeRequest(patientNRIC, AccountBLL.GetNRIC(), permission);
            }
        }
        public void UpdateRequest(string patientNRIC, short permission)
        {
            if (AccountBLL.IsTherapist())
            {
                therapistDAL.UpdateRecordTypeRequest(patientNRIC, AccountBLL.GetNRIC(), permission);
            }
        }
    }
}