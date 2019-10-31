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

      if (dataBLL.IsGeneralizedSettingInvalid())
      {
        LinkbuttonDataGenerate.Enabled = false;
        ScriptManager.RegisterStartupScript(this, GetType(), "Anonymising Records Unavailable", "toastr['error']('Anonymization of data is currently unavailable. Please try again later.');", true);
      }
    }

    protected void LinkbuttonDataGenerate_Click(object sender, EventArgs e)
    {
      if (dataBLL.IsGeneralizedSettingInvalid())
      {
        LinkbuttonDataGenerate.Enabled = false;
        return;
      }

      try
      {
        dataBLL.InsertAnonymizedTableToDb();
        ScriptManager.RegisterStartupScript(this, GetType(), "Anonymising Records Success", "toastr['success']('The database have been re-populated with Anonymised Patient Data.');", true);
      }
      catch
      {
        ScriptManager.RegisterStartupScript(this, GetType(), "Anonymising Records Error", "toastr['error']('Error occured when Anonymising Patient Data.');", true);
      }
    }
  }
}
