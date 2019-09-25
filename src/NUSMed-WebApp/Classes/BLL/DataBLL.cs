using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using NUSMed_WebApp.Classes.DAL;

namespace NUSMed_WebApp.Classes.BLL
{
  public class DataBLL
  {
    private readonly DataDAL dataDAL = new DataDAL();

    public class Anonymizer
    {

      public Dictionary<string, Tree> dgh;

      // Key: Quasi-Identifiers Sequence, Value: List of Tuple<Diagnosis, Content, File Name, File Extension>
      public Dictionary<List<string>, List<Tuple<string>>> sequencesFrequency;
      public Dictionary<string, HashSet<string>> valuesInTableForEachQuasi;
      public Dictionary<string, int> generalizationLevel;

      public IList<string> quasiIdentifiers = new List<string>(new string[] { "Age", "Sex", "Gender", "Marital Status", "Postal", "Record Creation Date" });
      public IList<string> quasiIdentifiersFilePaths = new List<string>(new string[] { FILE_AGE_HIERARCHY, FILE_SEX_HIERARCHY, FILE_GENDER_HIERARCHY, FILE_MARITAL_STATUS_HIERARCHY, FILE_POSTAL_HIERARCHY, FILE_RECORD_DATE_HIERARCHY });

      public const string FILE_AGE_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/age_hierarchy.csv";
      public const string FILE_SEX_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/sex_hierarchy.csv";
      public const string FILE_GENDER_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/gender_hierarchy.csv";
      public const string FILE_MARITAL_STATUS_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/marital_status_hierarchy.csv";
      public const string FILE_POSTAL_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/postal_hierarchy.csv";
      public const string FILE_RECORD_DATE_HIERARCHY = "C:/Users/Cheryl Toh/Desktop/IFS4205/project/IFS4205-AY1920-S1-Team02-NUSMed-WebApp/src/NUSMed-WebApp/Data-Hierarchy/record_date_hierarchy.csv";

      public Anonymizer()
      {
        dgh = new Dictionary<string, Tree>();
        SequenceListEqualityComparer seqListEqualityComparer = new SequenceListEqualityComparer();
        sequencesFrequency = new Dictionary<List<string>, List<Tuple<string>>>(seqListEqualityComparer);
        valuesInTableForEachQuasi = new Dictionary<string, HashSet<string>>();
        generalizationLevel = new Dictionary<string, int>();
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


      /// <summary>
      /// Anonymizes a data table and returns the anonymized data table and a dictionary of generalization level of every quasi-identifier
      /// </summary>
      public Tuple<DataTable, Dictionary<string, int>> anonymize(DataTable dt, int k, double suppressionThreshold)
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

      public void PrintDictionary(Dictionary<List<string>, int> d)
      {
        foreach (KeyValuePair<List<string>, int> entry in d)
        {
          for (int i = 0; i < entry.Value; i++)
          {
            Console.WriteLine("List:");
            PrintList(entry.Key);
          }
        }
      }

      public void PrintList(List<string> ls)
      {
        foreach (string item in ls)
        {
          Console.WriteLine(item);
        }
      }
    }

    class SequenceListEqualityComparer : IEqualityComparer<List<string>>
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

    public class Tree
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

    public DataTable GetAnonymizedTable()
    {
      DataTable dt = dataDAL.RetrieveColumns();
      Anonymizer anonymizer = new Anonymizer();
      Tuple<DataTable, Dictionary<string, int>> anonDtAndGenLevel = anonymizer.anonymize(dt, 3, 0.05);
      DataTable anonymizedDataTable = anonDtAndGenLevel.Item1;
      Dictionary<string, int> genLevel = anonDtAndGenLevel.Item2;
      //dataDAL.InsertIntoAnonymizedTable(anonymizedDataTable);
      dataDAL.InsertGeneralizationLevel(genLevel);
      return anonymizedDataTable;
    }
  }
}
