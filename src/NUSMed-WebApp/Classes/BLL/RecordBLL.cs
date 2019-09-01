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

        public bool DeleteRecord(string nric)
        {
            //if (IsAuthenticated())
            //{
            // retrieve associated records
            List<Record> records = recordDAL.RetrieveAssociated(nric);

            foreach (Record record in records)
            {
                // delete all record diagnosis first
                recordDAL.DeleteRecordDiagnosis(record.id);
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