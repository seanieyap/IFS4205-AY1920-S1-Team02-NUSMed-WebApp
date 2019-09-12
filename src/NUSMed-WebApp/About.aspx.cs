using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Caching;

namespace NUSMed_WebApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Cache.Insert("s90000004" + "_MFAAttempt", "Approved", null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration);
        }

        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            Session.Abandon();
        }

        protected void Unnamed_Click2(object sender, EventArgs e)
        {
            label1.Text = Session.SessionID;
        }
    }
}