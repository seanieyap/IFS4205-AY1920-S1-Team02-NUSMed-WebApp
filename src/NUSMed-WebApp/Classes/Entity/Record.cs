using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class Record
    {
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public long id { get; set; }
        public string patientNRIC { get; set; }
        public string creatorNRIC { get; set; }
        private string _creatorFirstName;
        public string creatorFirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_creatorFirstName))
                {
                    return _creatorFirstName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_creatorFirstName.ToLower());
            }
            set
            {
                _creatorFirstName = value;
            }
        }
        private string _creatorLastName;
        public string creatorLastName
        {
            get
            {
                if (string.IsNullOrEmpty(_creatorLastName))
                {
                    return _creatorLastName;
                }
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_creatorLastName.ToLower());
            }
            set
            {
                _creatorLastName = value;
            }
        }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime createTime { get; set; }
        public string fileName { get; set; }
        public string fileNameHash
        {
            get
            {
                SHA256 Sha256 = SHA256.Create();
                byte[] hashValue1 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(fileName));
                byte[] hashValue2 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(createTime.ToString("yyyy-MM-dd HH:mm:ss")));
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
        }
        public string fileChecksum { get; set; }
        public string fileExtension { get; set; }
        public int? fileSize { get; set; }
        public string fileSizeMegabytes
        {
            get
            {
                int counter = 0;
                decimal number = (decimal)fileSize;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1} {1}", number, suffixes[counter]);
            }
        }
        public string fileType
        {
            get
            {
                if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    return "Image";
                }
                else if (fileExtension == ".txt")
                {
                    return "Timeseries";
                }
                else if (fileExtension == ".mp4")
                {
                    return "Video";
                }

                return string.Empty;
            }
        }

        public string content { get; set; }
        public bool isEmergency { get; set; } = false;
        public RecordType type { get; set; }
        public short status { get; set; } = 0;
        public short? recordPermissionStatus { get; set; }
        public bool permited { get; set; } = false;

        public string fullpath
        {
            get
            {
                return GetFileServerPath() + "\\" + GetFileDirectoryNameHash() + "\\" + fileNameHash;
            }
        }

        public bool IsFileSafe()
        {
            string test = GetMD5HashFromFile();
            if (fileNameHash.Contains("\\") || !File.Exists(fullpath)
                || !GetMD5HashFromFile().Equals(fileChecksum)
                || new FileInfo(fullpath).Length != (int)fileSize)
            {
                return false;
            }

            return true;
        }

        public string GetFileDirectoryNameHash()
        {
            if ((AccountBLL.IsPatient() && AccountBLL.GetNRIC().Equals(patientNRIC))
                || (AccountBLL.IsTherapist()))
            {

                // todo permission check for therapist
                SHA256 Sha256 = SHA256.Create();
                byte[] hashValue1 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(patientNRIC));
                byte[] hashValue2 = Sha256.ComputeHash(Encoding.ASCII.GetBytes(new AccountBLL().GetCreateTime(patientNRIC).ToString("yyyy-MM-dd HH:mm:ss")));

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

        public string GetMD5HashFromFile()
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(fullpath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
                }
            }
        }

        public string GetFileServerPath()
        {
            return ConfigurationManager.AppSettings["fileServerPath"].ToString();
        }

        #region Validation Helpers
        public bool IsTitleValid()
        {
            if (string.IsNullOrEmpty(title) && title.Length <= 45)
                return true;

            return false;
        }
        public bool IsDescriptionValid()
        {
            if (string.IsNullOrEmpty(description) && description.Length <= 120)
                return true;

            return false;
        }
        public bool IsContentValid()
        {
            if (!type.isContent)
                return false;

            return type.IsContentValid(content);
        }
        public bool IsFileValid()
        {
            if (type.isContent)
                return false;

            return type.IsFileValid(fileExtension, fileSize);
        }
        #endregion
    }
}