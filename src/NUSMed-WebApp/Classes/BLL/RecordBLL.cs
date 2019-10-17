using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System.Collections.Generic;

namespace NUSMed_WebApp.Classes.BLL
{
    public class RecordBLL
    {
        private readonly RecordDAL recordDAL = new RecordDAL();
        private readonly LogRecordBLL logRecordBLL = new LogRecordBLL();

        public List<Record> GetRecords()
        {
            if (AccountBLL.IsPatient())
            {
                List<Record> result = recordDAL.RetrieveRecords(AccountBLL.GetNRIC());
                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Records", "Self.");
                return result;
            }

            return null;
        }
        public List<Record> GetRecords(string patientNRIC)
        {
            if (AccountBLL.IsTherapist())
            {
                List<Record> records = recordDAL.RetrieveRecords(patientNRIC, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(patientNRIC);

                List<Record> result = new List<Record>();
                foreach (Record record in records)
                {
                    if (!patient.hasPermissionsApproved(record))
                    {
                        Record newRecord = new Record();
                        newRecord.id = record.id;
                        newRecord.title = record.title;
                        newRecord.type = record.type;
                        newRecord.status = record.status;
                        newRecord.recordPermissionStatus = record.recordPermissionStatus;
                        result.Add(newRecord);
                    }
                    else
                    {
                        record.permited = true;
                        result.Add(record);
                    }
                }

                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Records", "Action on: " + patientNRIC + ".");
                return result;
            }

            return null;
        }
        public List<Record> GetRecords(string patientNRIC, long noteID)
        {
            if (AccountBLL.IsTherapist())
            {
                List<Record> records = recordDAL.RetrieveRecords(noteID, patientNRIC, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(patientNRIC);

                List<Record> result = new List<Record>();
                foreach (Record record in records)
                {
                    if (!patient.hasPermissionsApproved(record))
                    {
                        Record newRecord = new Record();
                        newRecord.id = record.id;
                        newRecord.title = record.title;
                        newRecord.type = record.type;
                        newRecord.status = record.status;
                        newRecord.recordPermissionStatus = record.recordPermissionStatus;
                        result.Add(newRecord);
                    }
                    else
                    {
                        record.permited = true;
                        result.Add(record);
                    }
                }

                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Records of Note", "Action on: " + patientNRIC + " , Note ID: " + noteID + ".");
                return result;
            }

            return null;
        }

        public Record GetRecord(long recordID)
        {
            if (AccountBLL.IsPatient())
            {
                return recordDAL.RetrieveRecord(AccountBLL.GetNRIC(), recordID);
            }
            else if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(recordID, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (patient.hasPermissionsApproved(record))
                {
                    record.permited = true;

                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Record", "Record ID: " + recordID + ".");

                    return record;
                }
            }

            return null;
        }

        public List<RecordDiagnosis> GetRecordDiagnoses(long recordID)
        {
            if (AccountBLL.IsPatient())
            {
                return recordDAL.RetrieveRecordDiagnoses(recordID, AccountBLL.GetNRIC());
            }
            else if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(recordID, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (patient.hasPermissionsApproved(record))
                {
                    List<RecordDiagnosis> result = recordDAL.RetrieveRecordDiagnoses(recordID, record.patientNRIC, AccountBLL.GetNRIC());
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Record Diagnoses", "Record ID: " + recordID + ".");
                    return result;
                }
            }

            return null;
        }

        public void AddRecordDiagnosis(string patientNRIC, long recordID, string code)
        {
            if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(recordID, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (record.patientNRIC.Equals(patientNRIC) && patient.hasPermissionsApproved(record))
                {
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Insert Record Diagnoses", "Action on: " + patientNRIC + ", Record ID: " + recordID + ", Diagnosis Code: " + code + ".");

                    recordDAL.InsertRecordDiagnosis(AccountBLL.GetNRIC(), recordID, code);
                }
            }
        }

        public void AddRecord(Record record)
        {
            if (AccountBLL.IsPatient() && record.patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                if (record.type.isContent)
                {
                    recordDAL.InsertContent(record, AccountBLL.GetNRIC());
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Insert Record", "Action on: " + record.patientNRIC + ", Record ID: " + record.id + ".");
                }
                else if (!record.type.isContent)
                {
                    record.fileChecksum = record.GetMD5HashFromFile();

                    recordDAL.InsertFile(record, AccountBLL.GetNRIC());
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Insert Record", "Action on: " + record.patientNRIC + ", Record ID: " + record.id + ".");
                }
            }
            else if (AccountBLL.IsTherapist())
            {
                Entity.Patient patient = new TherapistBLL().GetPatientPermissions(record.patientNRIC);

                if (patient.permissionApproved == 0 || ((patient.permissionApproved & record.type.permissionFlag) == 0) ||
                    AccountBLL.GetNRIC().Equals(record.patientNRIC))
                {
                    return;
                }

                if (record.type.isContent)
                {
                    recordDAL.InsertContent(record, AccountBLL.GetNRIC());
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Insert Record", "Action on: " + record.patientNRIC + ", Record ID: " + record.id + ".");
                }
                else if (!record.type.isContent)
                {
                    record.fileChecksum = record.GetMD5HashFromFile();

                    recordDAL.InsertFile(record, AccountBLL.GetNRIC());
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Insert Record", "Action on: " + record.patientNRIC + ", Record ID: " + record.id + ".");
                }
            }
        }

        public void UpdateRecordEnable(long recordID)
        {
            if (AccountBLL.IsPatient())
            {
                recordDAL.UpdateRecordEnable(recordID, AccountBLL.GetNRIC());
                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Update Record Status Enable", "Record ID: " + recordID + ".");
            }
        }
        public void UpdateRecordDisable(long recordID)
        {
            if (AccountBLL.IsPatient())
            {
                recordDAL.UpdateRecordDisable(recordID, AccountBLL.GetNRIC());
                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Update Record Status Disable", "Record ID: " + recordID + ".");
            }
        }
        public void UpdateRecordTherapistDefault(long recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.DeleteRecordPermission(recordID, therapistNRIC);
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Update Record Status Default", "Record ID: " + recordID + ".");
                }
            }
        }

        public void UpdateRecordTherapistAllow(long recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.InsertRecordPermissionAllow(recordID, therapistNRIC);
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Update Record Fine Grain Permission Allow", "Action on: " + therapistNRIC + ", Record ID: " + recordID + ".");
                }
            }
        }
        public void UpdateRecordTherapistDisallow(long recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.InsertRecordPermissionDisallow(recordID, therapistNRIC);
                    logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "Update Record Fine Grain Permission Disallow", "Action on: " + therapistNRIC + ", Record ID: " + recordID + ".");
                }
            }
        }

        //public void DeleteRecords(string nric)
        //{
        //    if (AccountBLL.IsAdministrator())
        //    {
        //        List<Record> records = recordDAL.RetrieveAssociatedRecords(nric);

        //        foreach (Record record in records)
        //        {
        //            // delete all record diagnosis first
        //            recordDAL.DeleteRecordDiagnosis(record.id);

        //            // delete all permissions
        //            recordDAL.DeleteRecordPermission(record.id);

        //            // delete record
        //            recordDAL.DeleteRecord(record.id);
        //        }
        //    }
        //}

        public bool VerifyRecord(Record record)
        {
            if (AccountBLL.IsTherapist() && recordDAL.RetrieveRecordExists(record.id, record.patientNRIC))
            {
                return true;
            }

            return false;
        }
    }
}