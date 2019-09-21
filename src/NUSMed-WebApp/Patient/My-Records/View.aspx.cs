using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient.My_Records
{
    public partial class View : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyRecords();
            Master.LiActivePatientMyRecordView();


            if (!IsPostBack)
            {
                Bind_GridViewRecords();
            }
        }

        protected void Bind_GridViewRecords()
        {
            List<Record> records = recordBLL.GetRecords();
            ViewState["GridViewRecords"] = records;
            GridViewRecords.DataSource = records;
            GridViewRecords.DataBind();
            UpdatePanelRecords.Update();
        }
        protected void GridViewRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecords.PageIndex = e.NewPageIndex;
            GridViewRecords.DataSource = ViewState["GridViewRecords"];
            GridViewRecords.DataBind();
        }

        protected void GridViewRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RecordType recordType = (RecordType)DataBinder.Eval(e.Row.DataItem, "type");

                if (recordType.isContent)
                {
                    Label LabelContent = (Label)e.Row.FindControl("LabelContent");
                    string content = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "content"));

                    LabelContent.Text = content + " " + recordType.prefix;
                    LabelContent.Visible = true;
                }
                else if (!recordType.isContent)
                {
                    LinkButton LinkbuttonFileView = (LinkButton)e.Row.FindControl("LinkbuttonFileView");
                    HtmlAnchor FileDownloadLink = (HtmlAnchor)e.Row.FindControl("FileDownloadLink");

                    LinkbuttonFileView.CommandName = "FileView";
                    LinkbuttonFileView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    LinkbuttonFileView.Visible = true;
                    LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View "
                        + DataBinder.Eval(e.Row.DataItem, "fileType")/*"test"*/ +
                        "</span>";

                    FileDownloadLink.HRef = "~/Patient/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    FileDownloadLink.Visible = true;
                }
            }
        }

        protected void GridViewRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            ViewState["GridViewRecordsSelectedID"] = id;

            if (e.CommandName.Equals("FileView"))
            {
                Record record = recordBLL.GetRecordFileInformation(id);

                modalFileViewImage.Visible = false;
                modalFileViewVideo.Visible = false;
                modalFileViewLabelText.Visible = false;

                if (record.fileExtension == ".png" || record.fileExtension == ".jpg" || record.fileExtension == ".jpeg")
                {
                    modalFileViewImage.Visible = true;
                    modalFileViewImage.ImageUrl = "~/Patient/Download.ashx?record=" + record.id;
                }
                else if (record.fileExtension == ".txt")
                {
                    // todo, create timeseries
                    modalFileViewLabelText.Visible = true;
                    if (record.IsFileSafe())
                    {
                        modalFileViewLabelText.Text = File.ReadAllText(record.fullpath);
                    }
                    else
                    {
                        modalFileViewLabelText.Text = "File Corrupted";
                    }
                }
                else if (record.fileExtension == ".mp4")
                {
                    modalFileViewVideo.Visible = true;
                    so.Attributes.Add("src", "~/Patient/Download.ashx?record=" + record.id);
                }

                labelRecordName.Text = record.title;
                modalFileViewLabelFileName.Text = record.fileName + record.fileExtension;
                modalFileViewLabelFileSize.Text = record.fileSizeMegabytes;
                FileDownloadLinkviaModal.HRef = "~/Patient/Download.ashx?record=" + record.id.ToString();

                UpdatePanelFileView.Update();
                ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').modal('show');", true);
            }
        }
    }
}