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
    private static readonly IList<Tuple<string, string>> ageOptionsLevel0 = new ReadOnlyCollection<Tuple<string, string>>(
        new List<Tuple<string, string>> {
                         Tuple.Create("0", "0"),
                         Tuple.Create("1", "1"),
                         Tuple.Create("2", "2"),
                         Tuple.Create("3", "3"),
                         Tuple.Create("4", "4"),
                         Tuple.Create("5", "5"),
                         Tuple.Create("6", "6"),
                         Tuple.Create("7", "7"),
                         Tuple.Create("8", "8"),
                         Tuple.Create("9", "9"),
                         Tuple.Create("10", "10"),
                         Tuple.Create("11", "11"),
                         Tuple.Create("12", "12"),
                         Tuple.Create("13", "13"),
                         Tuple.Create("14", "14"),
                         Tuple.Create("15", "15"),
                         Tuple.Create("16", "16"),
                         Tuple.Create("17", "17"),
                         Tuple.Create("18", "18"),
                         Tuple.Create("19", "19"),
                         Tuple.Create("20", "20"),
                         Tuple.Create("21", "21"),
                         Tuple.Create("22", "22"),
                         Tuple.Create("23", "23"),
                         Tuple.Create("24", "24"),
                         Tuple.Create("25", "25"),
                         Tuple.Create("26", "26"),
                         Tuple.Create("27", "27"),
                         Tuple.Create("28", "28"),
                         Tuple.Create("29", "29"),
                         Tuple.Create("30", "30"),
                         Tuple.Create("31", "31"),
                         Tuple.Create("32", "32"),
                         Tuple.Create("33", "33"),
                         Tuple.Create("34", "34"),
                         Tuple.Create("35", "35"),
                         Tuple.Create("36", "36"),
                         Tuple.Create("37", "37"),
                         Tuple.Create("38", "38"),
                         Tuple.Create("39", "39"),
                         Tuple.Create("40", "40"),
                         Tuple.Create("41", "41"),
                         Tuple.Create("42", "42"),
                         Tuple.Create("43", "43"),
                         Tuple.Create("44", "44"),
                         Tuple.Create("45", "45"),
                         Tuple.Create("46", "46"),
                         Tuple.Create("47", "47"),
                         Tuple.Create("48", "48"),
                         Tuple.Create("49", "49"),
                         Tuple.Create("50", "50"),
                         Tuple.Create("51", "51"),
                         Tuple.Create("52", "52"),
                         Tuple.Create("53", "53"),
                         Tuple.Create("54", "54"),
                         Tuple.Create("55", "55"),
                         Tuple.Create("56", "56"),
                         Tuple.Create("57", "57"),
                         Tuple.Create("58", "58"),
                         Tuple.Create("59", "59"),
                         Tuple.Create("60", "60"),
                         Tuple.Create("61", "61"),
                         Tuple.Create("62", "62"),
                         Tuple.Create("63", "63"),
                         Tuple.Create("64", "64"),
                         Tuple.Create("65", "65"),
                         Tuple.Create("66", "66"),
                         Tuple.Create("67", "67"),
                         Tuple.Create("68", "68"),
                         Tuple.Create("69", "69"),
                         Tuple.Create("70", "70"),
                         Tuple.Create("71", "71"),
                         Tuple.Create("72", "72"),
                         Tuple.Create("73", "73"),
                         Tuple.Create("74", "74"),
                         Tuple.Create("75", "75"),
                         Tuple.Create("76", "76"),
                         Tuple.Create("77", "77"),
                         Tuple.Create("78", "78"),
                         Tuple.Create("79", "79"),
                         Tuple.Create("80", "80"),
                         Tuple.Create("81", "81"),
                         Tuple.Create("82", "82"),
                         Tuple.Create("83", "83"),
                         Tuple.Create("84", "84"),
                         Tuple.Create("85", "85"),
                         Tuple.Create("86", "86"),
                         Tuple.Create("87", "87"),
                         Tuple.Create("88", "88"),
                         Tuple.Create("89", "89"),
                         Tuple.Create("90", "90"),
                         Tuple.Create("91", "91"),
                         Tuple.Create("92", "92"),
                         Tuple.Create("93", "93"),
                         Tuple.Create("94", "94"),
                         Tuple.Create("95", "95"),
                         Tuple.Create("96", "96"),
                         Tuple.Create("97", "97"),
                         Tuple.Create("98", "98"),
                         Tuple.Create("99", "99")
        });

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

    #region Marital Status Values
    private static readonly IList<Tuple<string, string>> maritalStatusOptionsLevel0 = new ReadOnlyCollection<Tuple<string, string>>(
            new List<Tuple<string, string>> {
                        Tuple.Create("SINGLE", "Single"),
                        Tuple.Create("WIDOWED", "Widowed"),
                        Tuple.Create("DIVORCED", "Divorced"),
                        Tuple.Create("MARRIED", "Married"),
            });

    private static readonly IList<Tuple<string, string>> maritalStatusOptionsLevel1 = new ReadOnlyCollection<Tuple<string, string>>(
            new List<Tuple<string, string>> {
                        Tuple.Create("NEVER MARRIED", "Never married"),
                        Tuple.Create("BEEN MARRIED", "Been married"),
            });
    #endregion

    #region Gender Values
    private static readonly IList<Tuple<string, string>> genderOptionsLevel0 = new ReadOnlyCollection<Tuple<string, string>>(
            new List<Tuple<string, string>> {
                        Tuple.Create("MALE", "Male"),
                        Tuple.Create("FEMALE", "Female"),
                        Tuple.Create("TRANS", "Trans"),
                        Tuple.Create("OTHERS", "Others"),
                        Tuple.Create("PREFER NOT TO SAY", "Prefer not to say"),
            });
    #endregion

    #region Sex Values
    private static readonly IList<Tuple<string, string>> sexOptionsLevel0 = new ReadOnlyCollection<Tuple<string, string>>(
            new List<Tuple<string, string>> {
                        Tuple.Create("MALE", "Male"),
                        Tuple.Create("FEMALE", "Female"),
            });
    #endregion

    public int maritalStatus { get; set; } = -1;

    public IList<Tuple<string, string>> maritalStatusOptions
    {
      get
      {
        if (maritalStatus == 0)
        {
          return maritalStatusOptionsLevel0;
        }
        else if (maritalStatus == 1)
        {
          return maritalStatusOptionsLevel1;
        }

        return null;
      }
    }
    public int gender { get; set; } = -1;

    public IList<Tuple<string, string>> genderOptions
    {
      get
      {
        if (gender == 0)
        {
          return genderOptionsLevel0;
        }
        return null;
      }
    }

    public int sex { get; set; } = -1;

    public IList<Tuple<string, string>> sexOptions
    {
      get
      {
        if (sex == 0)
        {
          return sexOptionsLevel0;
        }

        return null;
      }
    }

    public int postal { get; set; } = -1;
    // return list here
    public int age { get; set; } = -1;
    public IList<Tuple<string, string>> ageOptions
    {
      get
      {
        if (age == 0)
        {
          return ageOptionsLevel0;
        }
        else if (age == 1)
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

  [Serializable]
  public class FilteredValues
  {
    // Patient
    public List<string> sex { get; set; } = new List<string>();
    public List<string> gender { get; set; } = new List<string>();
    public List<string> maritalStatus { get; set; } = new List<string>();
    public List<string> postal { get; set; } = new List<string>();

    // Record
    public List<string> recordType { get; set; } = new List<string>();
    public List<string> diagnosis { get; set; } = new List<string>();
    public List<string> creationDate { get; set; } = new List<string>();
    public List<string> age { get; set; } = new List<string>();

  }
}
