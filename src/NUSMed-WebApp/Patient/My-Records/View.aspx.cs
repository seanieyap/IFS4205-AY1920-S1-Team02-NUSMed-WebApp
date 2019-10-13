using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient.My_Records
{
    public partial class View : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();
        private readonly PatientBLL patientBLL = new PatientBLL();

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
                LinkButton LinkButtonViewFineGrain = (LinkButton)e.Row.FindControl("LinkButtonViewFineGrain");
                LinkButtonViewFineGrain.CommandName = "FineGrainView";
                LinkButtonViewFineGrain.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                LinkButton LinkbuttonDiagnosisView = (LinkButton)e.Row.FindControl("LinkbuttonDiagnosisView");
                LinkbuttonDiagnosisView.CommandName = "DiagnosisView";
                LinkbuttonDiagnosisView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();

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
                    LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i><span class=\"d-none d-lg-inline-block\">View "
                        + DataBinder.Eval(e.Row.DataItem, "fileType") +
                        "</span>";

                    FileDownloadLink.HRef = "~/Patient/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    FileDownloadLink.Visible = true;
                }

                // emergency
                bool isEmergency = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEmergency"));
                if (isEmergency)
                {
                    Label LabelIsEmergency = (Label)e.Row.FindControl("LabelIsEmergency");
                    LabelIsEmergency.Visible = true;
                    LabelIsEmergency.Attributes.Add("title", "This Record was added by a therapist whom you did not approve permissions.");
                }
            }
        }
        protected void GridViewRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("FileView"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    Record record = recordBLL.GetRecord(id);

                    modalFileViewImage.Visible = false;
                    modalFileViewVideo.Visible = false;
                    modalFileViewPanelText.Visible = false;

                    labelRecordName.Text = record.title;
                    modalFileViewLabelFileName.Text = record.fileName + record.fileExtension;
                    modalFileViewLabelFileSize.Text = record.fileSizeMegabytes;
                    FileDownloadLinkviaModal.HRef = "~/Patient/Download.ashx?record=" + record.id.ToString();

                    if (record.fileIsImage)
                    {
                        modalFileViewImage.Visible = true;
                        modalFileViewImage.ImageUrl = "~/Patient/Download.ashx?record=" + record.id;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').modal('show');", true);
                    }
                    else if (record.fileIsText)
                    {
                        modalFileViewPanelText.Visible = true;
                        if (record.IsFileSafe())
                        {
                            string js = record.type.GetTextPlotJS(File.ReadAllText(record.fullpath));

                            ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').on('shown.bs.modal', function (e) {  "+js+ "}); $('#modalFileView').modal('show');", true);
                        }
                    }
                    else if (record.fileIsVideo)
                    {
                        modalFileViewVideo.Visible = true;
                        modalFileViewVideoSource.Attributes.Add("src", "~/Patient/Download.ashx?record=" + record.id);

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').modal('show');", true);
                    }

                    UpdatePanelFileView.Update();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View File Modal", "toastr['error']('Error Opening File Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("FineGrainView"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    ViewState["GridViewRecordsSelected"] = id;
                    Update_FineGrainModal();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Fine Grain Permissions Modal", "$('#modalFineGrain').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Fine Grain Permissions Modal", "toastr['error']('Error Opening Fine Grain Permissions Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("DiagnosisView"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    ViewState["GridViewRecordsSelected"] = id;
                    List<RecordDiagnosis> recordDiagnosis = recordBLL.GetRecordDiagnoses(id);
                    ViewState["GridViewRecordDiagnoses"] = recordDiagnosis;
                    GridViewRecordDiagnoses.DataSource = recordDiagnosis;
                    GridViewRecordDiagnoses.DataBind();
                    UpdatePanelRecordDiagnosis.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Diagnosis Modal", "$('#modalDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Diagnosis Modal", "toastr['error']('Error Opening View Diagnosis Modal.');", true);
                }
            }
        }

        #region FineGrainModal
        protected void Update_FineGrainModal()
        {
            long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelected"]);
            Record record = recordBLL.GetRecord(recordID);
            modalLabelFineGrainRecordTitle.Text = record.title;

            if (record.status == 0)
            {
                LinkButtonStatusDisable.CssClass = ("btn disabled");
                LinkButtonStatusEnable.CssClass = ("btn btn-success");
            }
            else if (record.status == 1)
            {
                LinkButtonStatusDisable.CssClass = ("btn btn-danger");
                LinkButtonStatusEnable.CssClass = ("btn disabled");
            }

            string termAllowed = TextboxSearchFineGrainAllow.Text.Trim().ToLower();
            List<Classes.Entity.Therapist> therapistCurrent = patientBLL.GetCurrentTherapistsFineGrain(termAllowed, recordID);
            GridViewFineGrain.DataSource = therapistCurrent;
            GridViewFineGrain.DataBind();

            ViewState["UpdatePanelFineGrain"] = therapistCurrent;

            UpdatePanelFineGrain.Update();
        }
        protected void GridViewFineGrain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                short? recordPermissionStatus = (short?)DataBinder.Eval(e.Row.DataItem, "recordPermissionStatus");
                LinkButton LinkButtonRecordStatusDefault = (LinkButton)e.Row.FindControl("LinkButtonRecordStatusDefault");
                LinkButton LinkButtonRecordStatusEnable = (LinkButton)e.Row.FindControl("LinkButtonRecordStatusEnable");
                LinkButton LinkButtonRecordStatusDisable = (LinkButton)e.Row.FindControl("LinkButtonRecordStatusDisable");

                if (recordPermissionStatus == null)
                {
                    LinkButtonRecordStatusDisable.CssClass = ("btn btn-danger");
                    LinkButtonRecordStatusEnable.CssClass = ("btn btn-success");
                    LinkButtonRecordStatusDefault.CssClass = ("btn disabled");
                }
                else if (recordPermissionStatus == 0)
                {
                    LinkButtonRecordStatusDisable.CssClass = ("btn disabled");
                    LinkButtonRecordStatusEnable.CssClass = ("btn btn-success");
                    LinkButtonRecordStatusDefault.CssClass = ("btn btn-info");
                }
                else if (recordPermissionStatus == 1)
                {
                    LinkButtonRecordStatusDisable.CssClass = ("btn btn-danger");
                    LinkButtonRecordStatusEnable.CssClass = ("btn disabled");
                    LinkButtonRecordStatusDefault.CssClass = ("btn btn-info");
                }
            }
        }
        protected void GridViewFineGrain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string therapistNRIC = e.CommandArgument.ToString();
            long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelected"]);

            if (e.CommandName.Equals("DefaultTherapist"))
            {
                try
                {
                    recordBLL.UpdateRecordTherapistDefault(recordID, therapistNRIC);
                    Update_FineGrainModal();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Therapist Fine Grain Permissions has been Set to \"Default\" to access specified record.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Seting Therapist Fine Grain Permissions to \"Default\" to access record.');", true);
                }
            }
            else if (e.CommandName.Equals("AllowTherapist"))
            {
                try
                {
                    recordBLL.UpdateRecordTherapistAllow(recordID, therapistNRIC);
                    Update_FineGrainModal();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Therapist Fine Grain Permissions has been Set to \"Allow\" to access specified record.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Seting Therapist Fine Grain Permissions to \"Allow\" to access record.');", true);
                }

            }
            else if (e.CommandName.Equals("DisallowTherapist"))
            {
                try
                {
                    recordBLL.UpdateRecordTherapistDisallow(recordID, therapistNRIC);
                    Update_FineGrainModal();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Therapist Fine Grain Permissions has been Set to \"Disallow\" to access specified record.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Seting Therapist Fine Grain Permissions to \"Disallow\" to access record.');", true);
                }
            }
        }
        protected void GridViewFineGrain_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewFineGrain.PageIndex = e.NewPageIndex;
            GridViewFineGrain.DataSource = ViewState["UpdatePanelFineGrain"];
            GridViewFineGrain.DataBind();
            UpdatePanelFineGrain.Update();
        }
        protected void LinkButtonFineGrainAllow_Click(object sender, EventArgs e)
        {
            string termAllowed = TextboxSearchFineGrainAllow.Text.Trim().ToLower();
            List<Classes.Entity.Therapist> therapistsAllowed = patientBLL.GetCurrentTherapists(termAllowed);
            GridViewFineGrain.DataSource = therapistsAllowed;
            GridViewFineGrain.DataBind();

            UpdatePanelFineGrain.Update();
        }
        protected void LinkButtonStatusDisable_Click(object sender, EventArgs e)
        {
            long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelected"]);
            try
            {
                recordBLL.UpdateRecordDisable(recordID);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Status of specified Record has been set to \"Disabled\".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Error setting status of Record to \"Disabled\".');", true);
            }
            Update_FineGrainModal();
        }
        protected void LinkButtonStatusEnable_Click(object sender, EventArgs e)
        {
            long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelected"]);
            try
            {
                recordBLL.UpdateRecordEnable(recordID);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Status of specified Record has been set to \"Enabled\".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Error setting status of Record to \"Enabled\".');", true);
            }

            Update_FineGrainModal();
        }
        #endregion

        #region Diagnosis Modal
        protected void GridViewRecordDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecordDiagnoses.PageIndex = e.NewPageIndex;
            GridViewRecordDiagnoses.DataSource = ViewState["GridViewRecordDiagnoses"];
            GridViewRecordDiagnoses.DataBind();
        }
        #endregion
    }
}