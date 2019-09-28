using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class RecordBLL
    {
        private readonly RecordDAL recordDAL = new RecordDAL();

        public List<Record> GetRecords()
        {
            if (AccountBLL.IsPatient())
            {
                return recordDAL.RetrieveRecords(AccountBLL.GetNRIC());
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

                return result;
            }

            return null;
        }

        public Record GetRecord(int id)
        {
            if (AccountBLL.IsPatient())
            {
                return recordDAL.RetrieveRecord(AccountBLL.GetNRIC(), id);
            }
            else if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(id, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (patient.hasPermissionsApproved(record))
                {
                    record.permited = true;
                    return record;
                }
            }

            return null;
        }

        public List<RecordDiagnosis> GetRecordDiagnoses(int id)
        {
            if (AccountBLL.IsPatient())
            {
                return recordDAL.RetrieveRecordDiagnoses(id, AccountBLL.GetNRIC());
            }
            else if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(id, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (patient.hasPermissionsApproved(record))
                {
                    return recordDAL.RetrieveRecordDiagnoses(id, record.patientNRIC, AccountBLL.GetNRIC());
                }
            }

            return null;
        }

        public void AddRecordDiagnosis(string patientNRIC, int id, string code)
        {
            if (AccountBLL.IsTherapist())
            {
                Record record = recordDAL.RetrieveRecord(id, AccountBLL.GetNRIC());
                Entity.Patient patient = new TherapistBLL().GetPatient(record.patientNRIC);

                if (record.patientNRIC.Equals(patientNRIC) && patient.hasPermissionsApproved(record))
                {
                    recordDAL.InsertRecordDiagnosis(AccountBLL.GetNRIC(), id, code);
                }
            }
        }

        public void SubmitRecordContent(Record record)
        {
            if (AccountBLL.IsPatient() && record.patientNRIC.Equals(AccountBLL.GetNRIC()))
            {
                if (record.type.isContent)
                {
                    recordDAL.InsertContent(record, AccountBLL.GetNRIC());
                }
                else if (!record.type.isContent)
                {
                    record.fileChecksum = record.GetMD5HashFromFile();

                    recordDAL.InsertFile(record, AccountBLL.GetNRIC());
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
                }
                else if (!record.type.isContent)
                {
                    record.fileChecksum = record.GetMD5HashFromFile();

                    recordDAL.InsertFile(record, AccountBLL.GetNRIC());
                }
            }
        }

        public void UpdateRecordEnable(int recordID)
        {
            if (AccountBLL.IsPatient())
            {
                recordDAL.UpdateRecordEnable(recordID, AccountBLL.GetNRIC());
            }
        }
        public void UpdateRecordDisable(int recordID)
        {
            if (AccountBLL.IsPatient())
            {
                recordDAL.UpdateRecordDisable(recordID, AccountBLL.GetNRIC());
            }
        }
        public void UpdateRecordTherapistDefault(int recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.DeleteRecordPermission(recordID, therapistNRIC);
                }
            }
        }

        public void UpdateRecordTherapistAllow(int recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.InsertRecordPermissionAllow(recordID, therapistNRIC);
                }
            }
        }
        public void UpdateRecordTherapistDisallow(int recordID, string therapistNRIC)
        {
            if (AccountBLL.IsPatient())
            {
                if (recordDAL.RetrieveRecordOwner(AccountBLL.GetNRIC(), recordID))
                {
                    recordDAL.InsertRecordPermissionDisallow(recordID, therapistNRIC);
                }
            }
        }

        public void DeleteRecords(string nric)
        {
            if (AccountBLL.IsAdministrator())
            {
                List<Record> records = recordDAL.RetrieveAssociatedRecords(nric);

                foreach (Record record in records)
                {
                    // delete all record diagnosis first
                    recordDAL.DeleteRecordDiagnosis(record.id);

                    // delete all permissions
                    recordDAL.DeleteRecordPermission(record.id);

                    // delete record
                    recordDAL.DeleteRecord(record.id);
                }
            }
        }
    }
}