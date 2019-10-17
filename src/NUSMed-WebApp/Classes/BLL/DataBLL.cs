using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Classes.BLL
{
    public class DataBLL
    {
        private readonly DataDAL dataDAL = new DataDAL();
        private readonly LogAccountBLL logAccountBLL = new LogAccountBLL();
        private readonly LogRecordBLL logRecordBLL = new LogRecordBLL();

        public void InsertAnonymizedTableToDb()
        {
            if (AccountBLL.IsAdministrator())
            {
                DataTable dt = dataDAL.RetrieveColumns();
                Anonymizer anonymizer = new Anonymizer();
                Tuple<DataTable, Dictionary<string, int>> anonDtAndGenLevel = anonymizer.anonymize(dt, 3, 0.05);
                DataTable anonymizedDataTable = anonDtAndGenLevel.Item1;
                Dictionary<string, int> genLevel = anonDtAndGenLevel.Item2;
                dataDAL.ClearAnonymizedTable();
                dataDAL.InsertIntoAnonymizedTable(anonymizedDataTable);
                dataDAL.UpdateGeneralizationLevel(genLevel);
            }
        }
        //public List<PatientAnonymised> GetPatients(List<Tuple<string, string>> selectItems)
        //{
        //  if (AccountBLL.IsResearcher())
        //  {
        //    StringBuilder stringBuilder = new StringBuilder();
        //    stringBuilder.Append(@"SELECT ra.record_id, ra.marital_status, ra.gender, ra.sex, ra.age, ra.postal, ra.record_create_date
        //    FROM records_anonymized ra INNER JOIN record r ON ra.record_id = r.id");

        //    List<string> tempList = new List<string>();
        //    List<Tuple<string, string>> paraList = new List<Tuple<string, string>>();

        //    if (selectItems.Any())
        //    {
        //      tempList.Add(" (" + string.Join(" AND ", selectItems.Select(tuple => tuple.Item1 + " = @" + tuple.Item1 + tuple.Item2)) + ")");
        //    }

        //    foreach (Tuple<string, string> selectItem in selectItems)
        //    {
        //      paraList.Add(new Tuple<string, string>("@" + selectItem.Item1 + selectItem.Item2, selectItem.Item2));
        //    }

        //    if (tempList.Count > 0)
        //    {
        //      stringBuilder.Append(" WHERE " + string.Join(" AND ", tempList));
        //    }

        //    stringBuilder.Append(" GROUP BY r.patient_nric;");

        //    string query = stringBuilder.ToString();
        //    return dataDAL.RetrievePatients(query, paraList);
        //  }

        //  return null;
        //}

        /// <summary>
        /// Retrieve patients that fit the filters set
        /// </summary>
        /// <param name="filteredValues">Object containing filtered values</param>
        /// <returns>List of PatientAnonymised</returns>
        public List<PatientAnonymised> GetPatients(FilteredValues filteredValues)
        {
            if (AccountBLL.IsResearcher())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT ra.marital_status, ra.gender, ra.sex, ra.age, ra.postal, ra.record_create_date, 
                    GROUP_CONCAT(DISTINCT ra.record_id SEPARATOR ',') as record_ids
                FROM records_anonymized ra 
                INNER JOIN record r ON ra.record_id = r.id 
                LEFT JOIN record_diagnosis rd ON r.id = rd.record_id 
				LEFT JOIN patient_diagnosis pd ON pd.patient_nric = r.patient_nric ");

                List<Tuple<string, List<string>>> columnsAndValuesList = new List<Tuple<string, List<string>>>();
                if (filteredValues.sex.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.sex", filteredValues.sex));
                }

                if (filteredValues.gender.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.gender", filteredValues.gender));
                }

                if (filteredValues.maritalStatus.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.marital_status", filteredValues.maritalStatus));
                }

                if (filteredValues.postal.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.postal", filteredValues.postal));
                }

                if (filteredValues.diagnoses.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("pd.diagnosis_code", filteredValues.recordDiagnoses));
                }

                if (filteredValues.recordType.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("r.type", filteredValues.recordType));
                }

                if (filteredValues.recordDiagnoses.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("rd.diagnosis_code", filteredValues.recordDiagnoses));
                }

                if (filteredValues.creationDate.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.record_create_date", filteredValues.creationDate));
                }

                if (filteredValues.age.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.age", filteredValues.age));
                }

                List<string> tempList = new List<string>();

                if (columnsAndValuesList.Any())
                {
                    tempList.Add(" (" + string.Join(" AND ", columnsAndValuesList.Select(tuple => JoinMultipleSelectedValues(tuple.Item1, tuple.Item2))) + ")");
                }

                if (tempList.Count > 0)
                {
                    stringBuilder.Append(" WHERE " + string.Join(" AND ", tempList));
                }

                stringBuilder.Append(" GROUP BY r.patient_nric LIMIT 200;");

                return dataDAL.RetrievePatients(stringBuilder.ToString());
            }
            return null;
        }

        public DataTable GetPatientsForDownload(FilteredValues filteredValues)
        {
            if (AccountBLL.IsResearcher())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT r.patient_nric, ra.marital_status, ra.gender, ra.sex, ra.age, ra.postal, 
                              (SELECT GROUP_CONCAT(DISTINCT pd.diagnosis_code SEPARATOR ',') 
	                            FROM patient_diagnosis pd
	                            WHERE pd.patient_nric = r.patient_nric) as patient_diagnosis_code,
                              r.title, r.type, r.description, ra.record_create_date, r.content, GROUP_CONCAT(DISTINCT rd.diagnosis_code SEPARATOR ',') as record_diagnoses_codes, ra.record_id
                              FROM records_anonymized ra 
                              INNER JOIN record r ON ra.record_id = r.id 
                              LEFT JOIN record_diagnosis rd ON r.id = rd.record_id ");

                List<Tuple<string, List<string>>> columnsAndValuesList = new List<Tuple<string, List<string>>>();
                if (filteredValues.sex.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.sex", filteredValues.sex));
                }

                if (filteredValues.gender.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.gender", filteredValues.gender));
                }

                if (filteredValues.maritalStatus.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.marital_status", filteredValues.maritalStatus));
                }

                if (filteredValues.postal.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.postal", filteredValues.postal));
                }

                if (filteredValues.diagnoses.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("pd.diagnosis_code", filteredValues.recordDiagnoses));
                }

                if (filteredValues.recordType.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("r.type", filteredValues.recordType));
                }

                if (filteredValues.recordDiagnoses.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("rd.diagnosis_code", filteredValues.recordDiagnoses));
                }

                if (filteredValues.creationDate.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.record_create_date", filteredValues.creationDate));
                }

                if (filteredValues.age.Count > 0)
                {
                    columnsAndValuesList.Add(new Tuple<string, List<string>>("ra.age", filteredValues.age));
                }

                List<string> tempList = new List<string>();

                if (columnsAndValuesList.Any())
                {
                    tempList.Add(" (" + string.Join(" AND ", columnsAndValuesList.Select(tuple => JoinMultipleSelectedValues(tuple.Item1, tuple.Item2))) + ")");
                }

                if (tempList.Count > 0)
                {
                    stringBuilder.Append(" WHERE " + string.Join(" AND ", tempList));
                }

                stringBuilder.Append(" GROUP BY r.id");
                stringBuilder.Append(" ORDER BY r.patient_nric;");

                DataTable anonPatientsTable = dataDAL.RetrieveAnonPatients(stringBuilder.ToString());

                anonPatientsTable.Columns.Add("data", typeof(string));

                foreach (DataRow row in anonPatientsTable.Rows)
                {
                    long recordId = Convert.ToInt64(row["record_id"]);
                    RecordType recordType = RecordType.Get(Convert.ToString(row["type"]));

                    if (recordType.isContent)
                    {
                        row["data"] = Convert.ToString(row["content"]);
                    }
                    else
                    {
                        row["data"] = "~/Researcher/Download.ashx?record=" + recordId.ToString();
                    }
                }
                anonPatientsTable.Columns.Remove("content");

                // Renaming the columns in the datatable
                anonPatientsTable.Columns["patient_nric"].ColumnName = "patient id";
                anonPatientsTable.Columns["patient_diagnosis_code"].ColumnName = "patient diagnoses";
                anonPatientsTable.Columns["type"].ColumnName = "record type";
                anonPatientsTable.Columns["description"].ColumnName = "record description";
                anonPatientsTable.Columns["record_diagnoses_codes"].ColumnName = "record diagnoses";
                anonPatientsTable.Columns["record_id"].ColumnName = "record id";
                anonPatientsTable.Columns["record_create_date"].ColumnName = "record creation date";

                return anonPatientsTable;
            }
            return null;
        }

        public List<PatientDiagnosis> GetPatientDiagnoses(List<long> recordIDs)
        {
            if (AccountBLL.IsResearcher())
            {
                IEnumerable<Tuple<string, long>> recordIDsParameterized = from recordID in recordIDs
                                                                          select (new Tuple<string, long>("@" + recordID.ToString().Replace(" ", string.Empty), recordID));

                List<PatientDiagnosis> result = dataDAL.RetrievePatientDiagnoses(recordIDsParameterized);
                logAccountBLL.LogEvent(AccountBLL.GetNRIC(), "View Patient Diagnoses", "Record IDs: " + string.Join(", ", recordIDs) + ".");
                return result;
            }

            return null;
        }

        public List<Record> GetRecords(List<long> recordIDs)
        {
            if (AccountBLL.IsResearcher())
            {
                IEnumerable<Tuple<string, long>> recordIDsParameterized = from recordID in recordIDs
                                                                          select (new Tuple<string, long>("@" + recordID.ToString().Replace(" ", string.Empty), recordID));

                List<Record> result = dataDAL.RetrieveRecords(recordIDsParameterized);

                logRecordBLL.LogEvent(AccountBLL.GetNRIC(), "View Records", "Record IDs: " + string.Join(", ", recordIDs) + ".");
                return result;
            }

            return null;
        }
        public Record GetRecord(long recordID)
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveRecord(recordID);
            }
            return null;
        }

        public List<RecordDiagnosis> GetRecordDiagnoses(long recordID)
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveRecordDiagnoses(recordID);
            }

            return null;
        }

        private string JoinMultipleSelectedValues(string columnName, List<string> valuesList)
        {
            StringBuilder sb = new StringBuilder();
            List<string> tempList = new List<string>();
            if (valuesList.Any())
            {
                tempList.Add(" (" + string.Join(" OR ", valuesList.Select(value => columnName + " = " + "'" + value + "'")) + ")");
            }

            if (tempList.Count > 0)
            {
                sb.Append(string.Join(" OR ", tempList));
            }

            return sb.ToString();
        }

        public DataTable GetDiagnoses()
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveDiagnoses();
            }

            return null;
        }

        public DataTable GetRecordDiagnoses()
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveRecordDiagnoses();
            }

            return null;
        }

        public DataTable GetPostal()
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrievePostal();
            }

            return null;
        }

        public DataTable GetRecordCreationDate()
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveCreationDate();
            }

            return null;
        }

        public GeneralizedSetting GetGeneralizedSettingFromDb()
        {
            if (AccountBLL.IsResearcher())
            {
                return dataDAL.RetrieveGeneralizationLevel();
            }

            return null;
        }

        private class Anonymizer
        {
            private Dictionary<string, Tree> dgh;

            // Key: Quasi-Identifiers Sequence, Value: List of Tuple<Diagnosis, Content, File Name, File Extension>
            private Dictionary<List<string>, List<Tuple<string>>> sequencesFrequency;
            private Dictionary<string, HashSet<string>> valuesInTableForEachQuasi;
            private Dictionary<string, int> generalizationLevel;

            private IList<string> quasiIdentifiers = new List<string>(new string[] { "Age", "Sex", "Gender", "Marital Status", "Postal", "Record Creation Date" });
            private readonly List<string> quasiIdentifiersFilePaths;

            private readonly string FILE_AGE_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/age_hierarchy.csv");
            private readonly string FILE_SEX_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/sex_hierarchy.csv");
            private readonly string FILE_GENDER_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/gender_hierarchy.csv");
            private readonly string FILE_MARITAL_STATUS_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/marital_status_hierarchy.csv");
            private readonly string FILE_POSTAL_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/postal_hierarchy.csv");
            private readonly string FILE_RECORD_DATE_HIERARCHY = HttpContext.Current.Server.MapPath("~/Data-Hierarchy/record_date_hierarchy.csv");

            public Anonymizer()
            {
                dgh = new Dictionary<string, Tree>();
                SequenceListEqualityComparer seqListEqualityComparer = new SequenceListEqualityComparer();
                sequencesFrequency = new Dictionary<List<string>, List<Tuple<string>>>(seqListEqualityComparer);
                valuesInTableForEachQuasi = new Dictionary<string, HashSet<string>>();
                generalizationLevel = new Dictionary<string, int>();

                // read files
                //FILE_AGE_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/age_hierarchy.csv"));
                //FILE_SEX_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/sex_hierarchy.csv"));
                //FILE_GENDER_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/gender_hierarchy.csv"));
                //FILE_MARITAL_STATUS_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/marital_status_hierarchy.csv"));
                //FILE_POSTAL_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/postal_hierarchy.csv"));
                //FILE_RECORD_DATE_HIERARCHY = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Data-Hierarchy/record_date_hierarchy.csv"));

                quasiIdentifiersFilePaths = new List<string>(new string[] { FILE_AGE_HIERARCHY, FILE_SEX_HIERARCHY, FILE_GENDER_HIERARCHY, FILE_MARITAL_STATUS_HIERARCHY, FILE_POSTAL_HIERARCHY, FILE_RECORD_DATE_HIERARCHY });
            }

            private int GetAge(DateTime dob, DateTime recordDate)
            {
                DateTime zeroTime = new DateTime(1, 1, 1);
                TimeSpan timespan = recordDate.Subtract(dob);
                int age = (zeroTime + timespan).Year - 1;

                return age;
            }

            private string GetDate(DateTime createdDateTime)
            {
                string createdDate = createdDateTime.ToString("yyyy-M-d");
                return createdDate;
            }

            private void InitializeAnonymizer()
            {
                for (int i = 0; i < quasiIdentifiers.Count; i++)
                {
                    // Fill up DGH with quasi-identifier and its respective tree
                    string quasiIdentifier = quasiIdentifiers[i];
                    string quasiIdentifierHierarchyFilePath = quasiIdentifiersFilePaths[i];
                    Tree quasiHierarchyTree = new Tree();
                    quasiHierarchyTree.BuildTree(quasiIdentifierHierarchyFilePath);
                    dgh[quasiIdentifier] = quasiHierarchyTree;

                    // Initialize the dictionary with an empty hashset
                    valuesInTableForEachQuasi[quasiIdentifier] = new HashSet<string>();

                    // Initialize the dictionary with a value of 0 for all quasi-identifiers
                    generalizationLevel[quasiIdentifier] = 0;
                }
            }

            /// <summary>
            /// Anonymizes a data table and returns the anonymized data table and a dictionary of generalization level of every quasi-identifier
            /// </summary>
            public Tuple<DataTable, Dictionary<string, int>> anonymize(DataTable dt, int k, double suppressionThreshold)
            {
                //for (int i = 0; i < quasiIdentifiers.Count; i++)
                //{
                //  // Fill up DGH with quasi-identifier and its respective tree
                //  string quasiIdentifier = quasiIdentifiers[i];
                //  string quasiIdentifierHierarchyFilePath = quasiIdentifiersFilePaths[i];
                //  Tree quasiHierarchyTree = new Tree();
                //  quasiHierarchyTree.BuildTree(quasiIdentifierHierarchyFilePath);
                //  dgh[quasiIdentifier] = quasiHierarchyTree;

                //  // Initialize the dictionary with an empty hashset
                //  valuesInTableForEachQuasi[quasiIdentifier] = new HashSet<string>();

                //  // Initialize the dictionary with a value of 0 for all quasi-identifiers
                //  generalizationLevel[quasiIdentifier] = 0;
                //}
                InitializeAnonymizer();

                int numberOfRows = 0;
                foreach (DataRow row in dt.Rows)
                {
                    numberOfRows++;
                    string gender = row["gender"].ToString();
                    string sex = row["sex"].ToString();
                    string maritalStatus = row["marital_status"].ToString();
                    string dob = row["dob"].ToString();
                    string postal = row["postal"].ToString();
                    string createdDateTime = row["record_created_time"].ToString();
                    string recordId = row["record_id"].ToString();

                    string recordCreatedDate = GetDate(DateTime.ParseExact(createdDateTime, "MM/dd/yyyy HH:mm:ss", null));
                    string age = GetAge(Convert.ToDateTime(DateTime.ParseExact(dob, "MM/dd/yyyy", null)), Convert.ToDateTime(recordCreatedDate)).ToString();

                    List<string> quasiList = new List<string>(new string[] { age, sex, gender, maritalStatus, postal, recordCreatedDate });

                    if (sequencesFrequency.ContainsKey(quasiList))
                    {
                        sequencesFrequency[quasiList].Add(new Tuple<string>(recordId));
                    }
                    else
                    {
                        sequencesFrequency[quasiList] = new List<Tuple<string>>();
                        sequencesFrequency[quasiList].Add(new Tuple<string>(recordId));
                    }

                    for (int i = 0; i < quasiIdentifiers.Count; i++)
                    {
                        string quasiIdentifierForSet = quasiIdentifiers[i];
                        HashSet<string> set = valuesInTableForEachQuasi[quasiIdentifierForSet];
                        string itemToAdd = quasiList[i];
                        set.Add(itemToAdd);
                    }
                }

                int maxRecordsToSuppress = Convert.ToInt32(Math.Ceiling(suppressionThreshold * numberOfRows));

                while (true)
                {
                    int totalCountLessThanK = 0;
                    foreach (KeyValuePair<List<string>, List<Tuple<string>>> seqAndCountEntry in sequencesFrequency)
                    {
                        int seqCount = seqAndCountEntry.Value.Count;
                        if (seqCount < k)
                        {
                            totalCountLessThanK += k;
                        }
                    }

                    // Generalize
                    if (totalCountLessThanK > maxRecordsToSuppress)
                    {
                        string quasiToGeneralize = "";
                        int maxQuasiCount = 0;
                        foreach (KeyValuePair<string, HashSet<string>> quasiAndValuesEntry in valuesInTableForEachQuasi)
                        {
                            string quasi = quasiAndValuesEntry.Key;
                            Console.WriteLine("quasi:" + quasi);
                            int currentCount = quasiAndValuesEntry.Value.Count;
                            Console.WriteLine("count:" + string.Join(", ", quasiAndValuesEntry.Value));

                            if (currentCount > maxQuasiCount)
                            {
                                quasiToGeneralize = quasi;
                                maxQuasiCount = currentCount;
                            }
                        }

                        // if unable to generalize anymore, move on to suppression
                        if (maxQuasiCount == 0)
                        {
                            break;
                        }

                        generalizationLevel[quasiToGeneralize] = generalizationLevel[quasiToGeneralize] + 1;
                        valuesInTableForEachQuasi[quasiToGeneralize] = new HashSet<string>();
                        Console.WriteLine("quasi to generalize: " + quasiToGeneralize);
                        Console.WriteLine("max quasi count: " + maxQuasiCount);
                        List<List<string>> sequencesKeysList = new List<List<string>>(sequencesFrequency.Keys);
                        // Generalize the quasi-identifier with the most number of distinct values
                        foreach (List<string> sequence in sequencesKeysList)
                        {
                            int quasiIndex = quasiIdentifiers.IndexOf(quasiToGeneralize);
                            Tree quasiTree = dgh[quasiToGeneralize];
                            string valueToGeneralize = sequence[quasiIndex];
                            Tuple<string, int> generalizedTuple = quasiTree.root[valueToGeneralize];
                            string newGeneralizedValue = generalizedTuple.Item1;
                            if (newGeneralizedValue != null)
                            {
                                List<string> newSequence = new List<string>();
                                for (int i = 0; i < quasiIndex; i++)
                                {
                                    newSequence.Add(sequence[i]);
                                }
                                newSequence.Add(newGeneralizedValue);
                                // if quasiIndex is not the index to the last element
                                if (quasiIndex < sequence.Count - 1)
                                {
                                    for (int i = quasiIndex + 1; i < sequence.Count; i++)
                                    {
                                        newSequence.Add(sequence[i]);
                                    }
                                }

                                if (sequencesFrequency.ContainsKey(newSequence))
                                {
                                    List<Tuple<string>> sequenceTuples = sequencesFrequency[sequence];
                                    List<Tuple<string>> newSequenceTuples = sequencesFrequency[newSequence];
                                    // Append tuples of current ungeneralized sequence to the new generalized sequence
                                    newSequenceTuples.AddRange(sequenceTuples);
                                }
                                else
                                {
                                    sequencesFrequency[newSequence] = sequencesFrequency[sequence];
                                }
                                sequencesFrequency.Remove(sequence);
                                valuesInTableForEachQuasi[quasiToGeneralize].Add(newGeneralizedValue);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // Suppress entries whose quasis make up less than k records
                DataTable anonymizedDataTable = new DataTable();
                anonymizedDataTable.Columns.Add("Age", typeof(string));
                anonymizedDataTable.Columns.Add("Sex", typeof(string));
                anonymizedDataTable.Columns.Add("Gender", typeof(string));
                anonymizedDataTable.Columns.Add("Marital Status", typeof(string));
                anonymizedDataTable.Columns.Add("Postal", typeof(string));
                anonymizedDataTable.Columns.Add("Created Date", typeof(string));

                anonymizedDataTable.Columns.Add("Record ID", typeof(string));


                foreach (KeyValuePair<List<string>, List<Tuple<string>>> entry in sequencesFrequency)
                {
                    if (entry.Value.Count >= k)
                    {
                        for (int i = 0; i < entry.Value.Count; i++)
                        {
                            DataRow anonymisedRow = anonymizedDataTable.NewRow();
                            anonymisedRow["Age"] = entry.Key[0];
                            anonymisedRow["Sex"] = entry.Key[1];
                            anonymisedRow["Gender"] = entry.Key[2];
                            anonymisedRow["Marital Status"] = entry.Key[3];
                            anonymisedRow["Postal"] = entry.Key[4];
                            anonymisedRow["Created Date"] = entry.Key[5];
                            anonymisedRow["Record ID"] = entry.Value[i].Item1;

                            anonymizedDataTable.Rows.Add(anonymisedRow);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                return new Tuple<DataTable, Dictionary<string, int>>(anonymizedDataTable, generalizationLevel);
            }
        }

        private class SequenceListEqualityComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string> ls1, List<string> ls2)
            {
                for (int i = 0; i < ls1.Count; i++)
                {
                    if (!string.Equals(ls1[i], ls2[i]))
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(List<string> ls)
            {
                int prime = 31;
                int hashcode = 1;
                foreach (string s in ls)
                {
                    hashcode = hashcode * prime + s.GetHashCode();
                }
                return hashcode;
            }
        }

        private class Tree
        {

            public Dictionary<string, Tuple<string, int>> root;

            public Tree()
            {
                // Dictionary stores the parent and height of a node
                root = new Dictionary<string, Tuple<string, int>>();
            }

            public void BuildTree(string hierarchyFilePath)
            {
                StreamReader file = new StreamReader(hierarchyFilePath);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] lineArray = line.Split(',');
                    int height = lineArray.Length - 1;
                    int index = 0;
                    string parent = null;
                    for (int i = lineArray.Length - 1; i >= 0; i--)
                    {
                        string value = lineArray[i];
                        root[value] = new Tuple<string, int>(parent, height - index);
                        parent = value;
                        index++;
                    }
                }
            }
        }
    }
}
