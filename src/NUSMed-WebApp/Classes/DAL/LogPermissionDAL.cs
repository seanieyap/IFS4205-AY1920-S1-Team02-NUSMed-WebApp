using MySql.Data.MySqlClient;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;

namespace NUSMed_WebApp.Classes.DAL
{
    public class LogPermissionDAL : DALLog
    {
        /// <summary>
        /// Insert Permission Log Event
        /// </summary>
        public void Insert(string creatorNRIC, string action, string description)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"INSERT INTO permission
                    (creator_nric, action, description)
                    VALUES (@creatorNRIC, @action, @description);";

                cmd.Parameters.AddWithValue("@creatorNRIC", creatorNRIC);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@description", description);

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Retrieve Permission Log Events
        /// </summary>
        public List<LogEvent> Retrieve(string query, List<Tuple<string, string>> subjectNRICsValidated, List<Tuple<string, string>> actionsValidated, string dateTimeFromValidated, string dateTimeToValidated)
        {
            List<LogEvent> result = new List<LogEvent>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = query;

                foreach (Tuple<string, string> tuple in subjectNRICsValidated)
                {
                    cmd.Parameters.AddWithValue(tuple.Item1, tuple.Item2);
                }
                foreach (Tuple<string, string> tuple in actionsValidated)
                {
                    cmd.Parameters.AddWithValue(tuple.Item1, tuple.Item2);
                }
                if (!string.IsNullOrEmpty(dateTimeFromValidated))
                {
                    cmd.Parameters.AddWithValue("@dateTimeFromValidated", dateTimeFromValidated);
                }
                if (!string.IsNullOrEmpty(dateTimeToValidated))
                {
                    cmd.Parameters.AddWithValue("@dateTimeToValidated", dateTimeToValidated);
                }

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LogEvent logEvent = new LogEvent
                            {
                                id = Convert.ToInt64(reader["id"]),
                                creatorNRIC = Convert.ToString(reader["creator_nric"]),
                                action = Convert.ToString(reader["action"]),
                                description = Convert.ToString(reader["description"]),
                                createTime = Convert.ToDateTime(reader["create_time"]),
                            };
                            result.Add(logEvent);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve all distinct creator nric
        /// </summary>
        public List<string> RetrieveCreatorNRICs()
        {
            List<string> result = new List<string>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT distinct creator_nric
                    FROM permission
                    ORDER BY creator_nric DESC;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Convert.ToString(reader["creator_nric"]));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieve all distinct actions
        /// </summary>
        public List<string> RetrieveActions()
        {
            List<string> result = new List<string>();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"SELECT distinct action
                    FROM permission
                    ORDER BY action DESC;";

                using (cmd.Connection = connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(Convert.ToString(reader["action"]));
                        }
                    }
                }
            }

            return result;
        }

    }
}