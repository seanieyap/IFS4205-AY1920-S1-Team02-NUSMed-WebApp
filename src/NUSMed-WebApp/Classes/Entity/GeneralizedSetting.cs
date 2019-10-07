using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
    [Serializable]
    public class GeneralizedSetting
    {
        #region Age Values
        private static readonly IList<Tuple<string, string>> ageOptionsLevel1 = new ReadOnlyCollection<Tuple<string, string>>(
                new List<Tuple<string, string>> {
                        Tuple.Create("0-9", "0 - 9"),
                        Tuple.Create("10-19", "10 - 19"),
                        Tuple.Create("20-29", "20 - 29"),
                        Tuple.Create("30-39", "30 - 39"),
                        Tuple.Create("40-49", "40 - 49"),
                        Tuple.Create("50-59", "50 - 59"),
                        Tuple.Create("60-69", "60 - 69"),
                        Tuple.Create("70-79", "70 - 79"),
                        Tuple.Create("80-89", "80 - 89"),
                        Tuple.Create("90-99", "90 - 99")
                });

        private static readonly IList<Tuple<string, string>> ageOptionsLevel2 = new ReadOnlyCollection<Tuple<string, string>>(
                new List<Tuple<string, string>> {
                        Tuple.Create("0-19", "0 - 19"),
                        Tuple.Create("20-39", "20 - 39"),
                        Tuple.Create("40-59", "40 - 59"),
                        Tuple.Create("60-79", "60 - 79"),
                        Tuple.Create("80-99", "80 - 99"),
                });
        #endregion


        public int maritalStatus { get; set; } = -1;
        public int gender { get; set; } = -1;
        public int sex { get; set; } = -1;
        public int postal { get; set; } = -1;
        public int age { get; set; } = -1;
        public IList<Tuple<string, string>> ageOptions
        {
            get
            {
                if (age == 1)
                {
                    return ageOptionsLevel1;
                }
                else if (age == 2)
                {
                    return ageOptionsLevel2;
                }

                return null;
            }
        }
        public int recordCreationDate { get; set; } = -1;

    }
}
