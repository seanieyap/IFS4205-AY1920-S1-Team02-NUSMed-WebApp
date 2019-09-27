using NUSMed_WebApp.Classes.BLL;
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
        private readonly DataBLL dataBLL = new DataBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminResearcherData();
        }

        protected void LinkbuttonDataGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                dataBLL.InsertAnonymizedTableToDb();
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('The database have been re-populated with Anonymised Records.');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Anonymising Records." + ex + "');", true);
            }
        }
    }
}