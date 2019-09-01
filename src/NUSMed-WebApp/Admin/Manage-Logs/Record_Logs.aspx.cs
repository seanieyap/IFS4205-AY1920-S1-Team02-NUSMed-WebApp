using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin.Manage_Logs
{
    public partial class Record_Logs : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminManageLogs();
            Master.LiActiveAdminViewRecordLogs();


        }
    }
}