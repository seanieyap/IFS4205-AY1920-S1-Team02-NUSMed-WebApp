using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Researcher
{
    public partial class Record_Search : Page
    {

        private readonly DataBLL dataBLL = new DataBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveResearcherRecordSearch();

            if (!IsPostBack)
            {
                Bind_GridViewAnonRecords();
            }
        }

        protected void Bind_GridViewAnonRecords()
        {
            List<RecordAnonymised> recordAnonymised = dataBLL.getAnonymizedTableFromDb();
            ViewState["GridViewAnonRecords"] = recordAnonymised;
            GridViewAnonRecords.DataSource = recordAnonymised;
            GridViewAnonRecords.DataBind();
        }

        protected void GridViewAnonRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAnonRecords.PageIndex = e.NewPageIndex;
            GridViewAnonRecords.DataSource = ViewState["GridViewAnonRecords"];
            GridViewAnonRecords.DataBind();
        }

        protected void GridViewRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string recordId = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                RecordType recordType = RecordType.Get(DataBinder.Eval(e.Row.DataItem, "record_type").ToString());
                if (recordType.isContent)
                {
                    Label LabelContent = (Label)e.Row.FindControl("LabelContent");
                    string content = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "content"));
                    string unit = recordType.prefix;

                    LabelContent.Text = content + unit;
                    LabelContent.Visible = true;
                }
                else if (!recordType.isContent)
                {
                    LinkButton LinkbuttonFileView = (LinkButton)e.Row.FindControl("LinkbuttonFileView");
                    HtmlAnchor FileDownloadLink = (HtmlAnchor)e.Row.FindControl("FileDownloadLink");

                    //LinkbuttonFileView.CommandName = "FileView";
                    //LinkbuttonFileView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    //LinkbuttonFileView.Visible = true;
                    //LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View "
                    //    + DataBinder.Eval(e.Row.DataItem, "fileType") +
                    //    "</span>";

                    FileDownloadLink.HRef = "~/Patient/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    FileDownloadLink.Visible = true;
                }
            }
        }
    }
}
