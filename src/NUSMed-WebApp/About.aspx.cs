using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.associatedDeviceID = "8b5ceafc-efcc-4db3-b906-0b488ab1038b";
            account.associatedTokenID = "K4dUkJlFROWAtGvt17E1fA==";
            HttpContext.Current.Cache.Insert("F0040672P" + "_MFAAttempt", account, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
        }

        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            Session.Abandon();
        }

        protected void Unnamed_Click2(object sender, EventArgs e)
        {
            label1.Text = Session.SessionID;
        }

    protected void LinkButtonGenerate_Click(object sender, EventArgs e)
    {
      DataBLL dataBLL = new DataBLL();

      dataBLL.InsertAnonymizedTableToDb();
    }
  }
}
