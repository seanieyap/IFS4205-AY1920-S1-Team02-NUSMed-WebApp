using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin
{
    public partial class Researcher_Data : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminResearcherData();
        }

        protected void LinkbuttonDataGenerate_Click(object sender, EventArgs e)
        {
            // todo
        }
    }
}