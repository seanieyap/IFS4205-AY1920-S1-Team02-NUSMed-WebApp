using System;
using System.Web.UI;

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
