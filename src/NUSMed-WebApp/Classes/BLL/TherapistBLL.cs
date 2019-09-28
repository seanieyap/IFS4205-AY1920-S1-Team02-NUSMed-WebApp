using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
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
                if (therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
                {
                    return therapistDAL.RetrievePatientInformation(patientNRIC, AccountBLL.GetNRIC());
                }
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
                List<Entity.Patient> patients = therapistDAL.RetrieveCurrentPatients(term, AccountBLL.GetNRIC());

                foreach (Entity.Patient patient in patients)
                {
                    if (patient.approvedTime == null)
                    {
                        patient.firstName = string.Empty;
                        patient.lastName = string.Empty;
                    }
                }
                return patients;
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
        public List<RecordDiagnosis> GetRecordDiagnoses(int recordID)
        {
            if (AccountBLL.IsTherapist())
            {
                return therapistDAL.RetrieveRecordDiagnoses(AccountBLL.GetNRIC(), recordID);
            }

            return null;
        }
        public List<PatientDiagnosis> GetPatientDiagnoses(string patientNRIC)
        {
            if (AccountBLL.IsTherapist())
            {
                if (therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
                {
                    return therapistDAL.RetrievePatientDiagnoses(patientNRIC, AccountBLL.GetNRIC());
                }
            }

            return null;
        }

        public List<Diagnosis> GetDiagnoses(string term, List<PatientDiagnosis> patientDiagnoses)
        {
            if (AccountBLL.IsTherapist())
            {
                List<Diagnosis> diagnoses = therapistDAL.RetrieveDiagnoses(term);
                List<Diagnosis> result = new List<Diagnosis>();

                foreach (Diagnosis diagnosis in diagnoses)
                {
                    if (!patientDiagnoses.Any(x => x.diagnosis.code.Equals(diagnosis.code) && x.end == null))
                    {
                        result.Add(diagnosis);
                    }
                }
                return result;
            }

            return null;
        }

        public void AddPatientDiagnosis(string patientNRIC, string code)
        {
            if (AccountBLL.IsTherapist())
            {
                if (!patientNRIC.Equals(AccountBLL.GetNRIC()) && 
                    therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
                {
                    therapistDAL.InsertPatientDiagnosis(patientNRIC, AccountBLL.GetNRIC(), code);
                }
            }
        }

        public void UpdatePatientDiagnosis(string patientNRIC, string code)
        {
            if (AccountBLL.IsTherapist())
            {
                if (!patientNRIC.Equals(AccountBLL.GetNRIC()) && 
                    therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
                {
                    therapistDAL.UpdatePatientDiagnosis(patientNRIC, AccountBLL.GetNRIC(), code);
                }
            }
        }

    }
}