using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NUSMed_WebApp.Classes.Entity
{
  public class Data
  {
    // query items
    public string from { get; set; }
    public string primaryKey { get; set; }
    public List<string> innerJoinItems { get; set; }
    public List<string> selectItemsWord { get; set; }
    public List<string> selectItemsNonword { get; set; }
    public List<string> selectSoundDurationWord { get; set; }
    public List<string> selectSoundDurationNonword { get; set; }
    public List<string> selectSoundItemsWord { get; set; }
    public List<string> selectSoundItemsNonword { get; set; }
    public List<Tuple<string, decimal>> whereItemsFrom { get; set; }
    public List<Tuple<string, decimal>> whereItemsTo { get; set; }
    public List<string> whereWordsEquals { get; set; }
    public List<Tuple<string, string>> whereWordsContains { get; set; }

    public Data()
    {
      selectItemsWord = new List<string>();
      selectItemsNonword = new List<string>();
      selectSoundItemsWord = new List<string>();
      selectSoundItemsNonword = new List<string>();
      selectSoundDurationWord = new List<string>();
      selectSoundDurationNonword = new List<string>();
      innerJoinItems = new List<string>();
      whereItemsFrom = new List<Tuple<string, decimal>>();
      whereItemsTo = new List<Tuple<string, decimal>>();
      whereWordsEquals = new List<string>();
      whereWordsContains = new List<Tuple<string, string>>();
    }
  }
}
