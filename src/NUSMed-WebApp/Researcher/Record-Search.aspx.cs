using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUSMed_WebApp.Classes.BLL;

namespace NUSMed_WebApp.Researcher
{
  public partial class Record_Search : Page
    {

    private readonly DataBLL dataBLL = new DataBLL();
    protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveResearcherRecordSearch();

        }

        protected void Bind_GridViewRecords()
        {
          DataTable anonymizedDt = dataBLL.GetAnonymizedTable();
          ViewState["GridViewAnonRecords"] = anonymizedDt;
          GridViewAnonRecords.DataSource = anonymizedDt;
          GridViewAnonRecords.DataBind();
    }
  }
}
