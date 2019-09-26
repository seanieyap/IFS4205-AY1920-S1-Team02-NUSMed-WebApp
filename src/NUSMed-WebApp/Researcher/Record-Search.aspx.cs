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
      Bind_GridViewAnonRecords();

    }

    protected void Bind_GridViewAnonRecords()
    {
      DataTable anonymizedDt = dataBLL.getAnonymizedTableFromDb();
      GridViewAnonRecords.DataSource = anonymizedDt;
      GridViewAnonRecords.DataBind();
    }

    protected void GridViewAnonRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
      GridViewAnonRecords.PageIndex = e.NewPageIndex;
      GridViewAnonRecords.DataSource = ViewState["GridViewAnonRecords"];
      GridViewAnonRecords.DataBind();
    }
  }
}
