using NUSMed_WebApp.Classes.DAL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUSMed_WebApp.Classes.BLL
{
    public class LogAccountBLL
    {
        private readonly LogAccountDAL logDAL = new LogAccountDAL();

        public void LogEvent(string creatorNRIC, string action, string description)
        {
            logDAL.Insert(creatorNRIC, action, description);
        }

        public List<LogEvent> GetAccountLogs(List<string> subjectNRICs, List<string> actions, DateTime? dateTimeFrom, DateTime? dateTimeTo)
        {
            if (AccountBLL.IsAdministrator())
            {
                List<Tuple<string, string>> subjectNRICsValidated = new List<Tuple<string, string>>();
                Dictionary<string, string> subjectNRICsValidatedDictionary = GetCreatorNRICs().ToDictionary(x => x);
                foreach (string subjectNRIC in subjectNRICs)
                {
                    if (subjectNRICsValidatedDictionary.ContainsKey(subjectNRIC))
                    {
                        subjectNRICsValidated.Add(new Tuple<string, string>("@" + subjectNRIC.Replace(" ", String.Empty), subjectNRIC));
                    }
                }

                List<Tuple<string, string>> actionsValidated = new List<Tuple<string, string>>();
                Dictionary<string, string> actionsValidatedDictionary = GetActions().ToDictionary(x => x);
                foreach (string action in actions)
                {
                    if (actionsValidatedDictionary.ContainsKey(action))
                    {
                        actionsValidated.Add(new Tuple<string, string>("@" + action.Replace(" ", String.Empty), action));
                    }
                }

                string dateTimeFromValidated = string.Empty;
                if (dateTimeFrom != null)
                {
                    dateTimeFromValidated = dateTimeFrom?.ToString("yyyy-MM-dd HH:mm:ss");
                }
                string dateTimeToValidated = string.Empty;
                if (dateTimeTo != null)
                {
                    dateTimeToValidated = dateTimeTo?.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Build Query
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(@"SELECT id, creator_nric, action, description, create_time
                    FROM account ");

                if (subjectNRICsValidated.Count > 0 || actionsValidated.Count > 0 || dateTimeFrom != null || dateTimeTo != null)
                {
                    stringBuilder.Append(" WHERE ");
                }

                List<string> temp = new List<string>();

                if (subjectNRICsValidated.Count > 0)
                {
                    temp.Add("(" + string.Join(" OR ", subjectNRICsValidated.Select(t => "creator_nric = " + t.Item1)) + ")");
                }
                if (actionsValidated.Count > 0)
                {
                    temp.Add("(" + string.Join(" OR ", actionsValidated.Select(t => "action = " + t.Item1)) + ")");
                }

                if (!string.IsNullOrEmpty(dateTimeFromValidated))
                {
                    temp.Add("create_time >= @dateTimeFromValidated");
                }
                if (!string.IsNullOrEmpty(dateTimeToValidated))
                {
                    temp.Add("create_time <= @dateTimeToValidated");
                }

                stringBuilder.Append(string.Join(" AND ", temp));

                stringBuilder.Append(" ORDER BY create_time DESC LIMIT 200;");

                return logDAL.Retrieve(stringBuilder.ToString(), subjectNRICsValidated, actionsValidated, dateTimeFromValidated, dateTimeToValidated);
            }

            return null;
        }

        public List<string> GetCreatorNRICs()
        {
            if (AccountBLL.IsAdministrator())
            {
                return logDAL.RetrieveCreatorNRICs();
            }

            return null;
        }
        public List<string> GetActions()
        {
            if (AccountBLL.IsAdministrator())
            {
                return logDAL.RetrieveActions();
            }

            return null;
        }
    }
}