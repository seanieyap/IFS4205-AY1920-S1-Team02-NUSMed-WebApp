using System;
using System.Collections.Generic;
using System.Web.UI;
using System.IO;
using System.Configuration;

namespace NUSMed_WebApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAbout();
        }
    }
}
