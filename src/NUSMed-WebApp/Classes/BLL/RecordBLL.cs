using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.BLL
{
    public class RecordBLL
    {
        private readonly RecordDAL recordDAL = new RecordDAL();


        public List<Record> GetRecords()
        {
            if (AccountBLL.IsPatient())
                return recordDAL.Retrieve(AccountBLL.GetNRIC());

            return null;
        }

        public void SubmitRecord(Record record)
        {
            //if (!AccountBLL.IsPatient())
            //    return;

            string nric = AccountBLL.GetNRIC();
            //             string fileServerPath = ConfigurationManager.AppSettings["fileServerPath"];

            recordDAL.Insert(record, nric, nric);
            //{
            // retrieve associated records

            //}

            //return false;
        }

        public bool DeleteRecords(string nric)
        {
            if (!AccountBLL.IsAdministrator())
                return false;
            //{
            // retrieve associated records
            List<Record> records = recordDAL.RetrieveAssociated(nric);

            foreach (Record record in records)
            {
                // delete all record diagnosis first
                recordDAL.DeleteRecordDiagnosis(record.id);

                // delete all permissions
                recordDAL.DeleteRecordPermission(record.id);

                // delete record
                recordDAL.DeleteRecord(record.id);
            }

            return true;
            //}

            //return false;
        }


    }
}