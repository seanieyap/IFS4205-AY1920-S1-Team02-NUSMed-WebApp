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
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()) &&
                therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
            {
                return therapistDAL.RetrievePatientInformation(patientNRIC, AccountBLL.GetNRIC());
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
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                return therapistDAL.RetrievePatient(patientNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }

        public Entity.Patient GetPatientPermissions(string patientNRIC)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                return therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }

        public void SubmitRequest(string patientNRIC, short permission)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                therapistDAL.InsertRecordTypeRequest(patientNRIC, AccountBLL.GetNRIC(), permission);
            }
        }
        public void UpdateRequest(string patientNRIC, short permission)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                therapistDAL.UpdateRecordTypeRequest(patientNRIC, AccountBLL.GetNRIC(), permission);
            }
        }
        public void RescindPermissions(string patientNRIC)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                therapistDAL.UpdateRecordTypeRescind(patientNRIC, AccountBLL.GetNRIC());
            }
        }

        public List<PatientDiagnosis> GetPatientDiagnoses(string patientNRIC)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()) &&
                therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
            {
                return therapistDAL.RetrievePatientDiagnoses(patientNRIC, AccountBLL.GetNRIC());
            }

            return null;
        }

        public List<Diagnosis> GetDiagnoses(string term, string patientNRIC, List<PatientDiagnosis> patientDiagnoses)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()) &&
                therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
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

        public List<Diagnosis> GetDiagnoses(string term, string patientNRIC, List<RecordDiagnosis> recordDiagnoses)
        {
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()) &&
                therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
            {
                List<Diagnosis> diagnoses = therapistDAL.RetrieveDiagnoses(term);
                List<Diagnosis> result = new List<Diagnosis>();

                foreach (Diagnosis diagnosis in diagnoses)
                {
                    if (!recordDiagnoses.Any(x => x.diagnosis.code.Equals(diagnosis.code)))
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
            if (AccountBLL.IsTherapist() &&
                !patientNRIC.Equals(AccountBLL.GetNRIC()) &&
                    therapistDAL.RetrievePatientPermission(patientNRIC, AccountBLL.GetNRIC()).approvedTime != null)
            {
                therapistDAL.InsertPatientDiagnosis(patientNRIC, AccountBLL.GetNRIC(), code);
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

        public bool AddNote(Note note)
        {
            if (AccountBLL.IsTherapist())
            {
                note.therapist.nric = AccountBLL.GetNRIC();
                note.creator.nric = AccountBLL.GetNRIC();

                // check if every record is valid
                RecordBLL recordBLL = new RecordBLL();

                foreach (Record record in note.records)
                {
                    Entity.Patient patient = GetPatientPermissions(record.patientNRIC);

                    if (patient.approvedTime == null || !recordBLL.VerifyRecord(record))
                    {
                        return false;
                    }
                }

                therapistDAL.InsertNote(note);
                foreach (Record record in note.records)
                {
                    therapistDAL.InsertNoteRecord(note, record);
                }

                return true;
            }
            return false;
        }

        public List<Note> GetNotes(string term)
        {
            if (AccountBLL.IsTherapist())
            {
                List<Note> notes = therapistDAL.RetrieveNotes(term, AccountBLL.GetNRIC());

                //foreach (Entity.Patient patient in patients)
                //{
                //    if (patient.approvedTime == null)
                //    {
                //        patient.firstName = string.Empty;
                //        patient.lastName = string.Empty;
                //    }
                //}
                return notes;
            }

            return null;
        }
    }
}