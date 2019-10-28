using System;
using System.Web.UI;

namespace NUSMed_WebApp.Patient
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientDashboard();

        }
    }
}