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
                return recordDAL.Retrieve(AccountBLL.GetNRIC());

            return null;
        }
        public Record GetRecordFileInformation(int id)
        {
            if (AccountBLL.IsPatient())
                return recordDAL.RetrieveFileInformation(AccountBLL.GetNRIC(), id);

            return null;
        }

        public void SubmitRecordContent(Record record)
        {
            if (AccountBLL.IsPatient())
            {
                string nric = AccountBLL.GetNRIC();

                if (record.type.isContent)
                {
                    recordDAL.InsertContent(record, nric, nric);
                }
            }
        }
        public void SubmitRecordFile(Record record)
        {
            if (AccountBLL.IsPatient())
            {
                string nric = AccountBLL.GetNRIC();

                if (!record.type.isContent)
                {
                    record.fileChecksum = GetMD5HashFromFile(record.fullpath);
                    recordDAL.InsertFile(record, nric, nric);
                }
            }
        }

        public void DeleteRecords(string nric)
        {
            if (AccountBLL.IsAdministrator())
            {
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
            }
        }

        public static string GetFileDirectoryNameHash()
        {
            if (AccountBLL.IsPatient())
            {
                SHA256 Sha256 = SHA256.Create();
                byte[] hashValue1 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(AccountBLL.GetNRIC()));
                byte[] hashValue2 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(new AccountBLL().GetCreateTime().ToString()));
                byte[] concat = new byte[hashValue1.Length + hashValue2.Length];
                Buffer.BlockCopy(hashValue1, 0, concat, 0, hashValue1.Length);
                Buffer.BlockCopy(hashValue2, 0, concat, hashValue1.Length, hashValue2.Length);
                byte[] hash = Sha256.ComputeHash(concat);

                string result = string.Empty;                                    
                foreach (byte b in hash)
                {
                    result += string.Format("{0:x2}", b);
                }
                return result;
            }

            return null;
        }

        public static string GetFileServerPath()
        {
            return ConfigurationManager.AppSettings["fileServerPath"].ToString();
        }

        public static string GetMD5HashFromFile(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    //return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }
    }
}