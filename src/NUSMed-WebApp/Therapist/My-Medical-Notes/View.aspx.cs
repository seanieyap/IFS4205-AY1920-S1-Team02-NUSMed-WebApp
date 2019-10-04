using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Therapist.My_Medical_Notes
{
    public partial class View : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();
        private readonly TherapistBLL therapistBLL = new TherapistBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyMedicalNotes();
            Master.LiActiveTherapistMyMedicalNotesView();

            if (!IsPostBack)
            {
                Bind_GridViewMedicalNote();
            }
        }

        #region GridViewMedicalNote Functions
        protected void Bind_GridViewMedicalNote()
        {
            string term = TextboxSearch.Text.Trim().ToLower();
            List<Note> notes = therapistBLL.GetNotes(term);
            ViewState["GridViewMedicalNote"] = notes;
            GridViewMedicalNote.DataSource = notes;
            GridViewMedicalNote.DataBind();
            UpdatePanelMedicalNote.Update();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            Bind_GridViewMedicalNote();
        }
        protected void GridViewMedicalNote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            ViewState["GridViewGridViewMedicalNoteSelectedID"] = id;

            if (e.CommandName.Equals("ViewNote"))
            {
                try
                {
                    Update_UpdatePanelNote(Convert.ToInt32(id));
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Note Modal", "$('#modalNote').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Note View.');", true);
                }
            }

            Bind_GridViewMedicalNote();
        }
        protected void GridViewMedicalNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewMedicalNote.PageIndex = e.NewPageIndex;
            GridViewMedicalNote.DataSource = ViewState["GridViewMedicalNote"];
            GridViewMedicalNote.DataBind();
        }
        protected void GridViewMedicalNote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton LinkButtonNote = (LinkButton)e.Row.FindControl("LinkButtonNote");
                DateTime? approvedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "patient.approvedTime");
                //DateTime? requestTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "patient.requestTime");

                if (approvedTime == null)
                {
                    //LabelName.Text = "Redacted";
                    //LinkButtonViewInformation.CssClass = "btn btn-secondary btn-sm disabled";
                    //LinkButtonViewInformation.Enabled = false;
                    //LinkButtonViewDiagnosis.CssClass = "btn btn-secondary btn-sm disabled";
                    //LinkButtonViewDiagnosis.Enabled = false;
                    LinkButtonNote.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonNote.Enabled = false;
                    //LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
                    //LinkButtonNewRecord.Enabled = false;
                }
                else
                {
                    //LabelName.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lastName")) + " " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "firstName"));
                    //LinkButtonViewInformation.CssClass = "btn btn-success btn-sm";
                    //LinkButtonViewInformation.CommandName = "ViewInformation";
                    //LinkButtonViewInformation.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                    //LinkButtonViewDiagnosis.CssClass = "btn btn-success btn-sm";
                    //LinkButtonViewDiagnosis.CommandName = "ViewDiagnosis";
                    //LinkButtonViewDiagnosis.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                    LinkButtonNote.CssClass = "btn btn-success btn-sm";
                    LinkButtonNote.CommandName = "ViewNote";
                    LinkButtonNote.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                    //Int16 permissionApproved = (Int16)DataBinder.Eval(e.Row.DataItem, "permissionApproved");
                    //if (permissionApproved == 0)
                    //{
                    //    LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
                    //    LinkButtonNewRecord.Enabled = false;
                    //    LinkButtonNewRecord.Attributes.Add("TabIndex", "0");
                    //    LinkButtonNewRecord.Attributes.Add("data-toggle", "tooltip");
                    //    LinkButtonNewRecord.Attributes.Add("title", "You do not have any record type permissions.");
                    //}
                    //else
                    //{
                    //    LinkButtonNewRecord.CssClass = "btn btn-info btn-sm";
                    //    LinkButtonNewRecord.NavigateUrl = "~/Therapist/My-Patients/New-Record?Patient-NRIC=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nric"));
                    //}
                }
            }
        }

        private void Update_UpdatePanelNote(int id)
        {
            Note note = therapistBLL.GetNote(id);

            // Note Details
            inputTitle.Value = note.title;
            TextBoxContent.Text = note.content;


            // Personal Details
            LabelInformationNRIC.Text = note.patient.nric;
            inputNRIC.Value = note.patient.nric;
            DateofBirth.Value = note.patient.dateOfBirth.ToString("MM/dd/yyyy");
            FirstName.Value = note.patient.firstName;
            LastName.Value = note.patient.lastName;
            CountryofBirth.Value = note.patient.countryOfBirth;
            Nationality.Value = note.patient.nationality;
            Sex.Value = note.patient.sex;
            Gender.Value = note.patient.gender;
            MaritalStatus.Value = note.patient.maritalStatus;

            // Contact Details
            Address.Value = note.patient.address;
            PostalCode.Value = note.patient.addressPostalCode;
            EmailAddress.Value = note.patient.email;
            ContactNumber.Value = note.patient.contactNumber;

            // Patient NOK Details
            NOKName.Value = note.patient.nokName;
            NOKContact.Value = note.patient.nokContact;

            // Records
            List<Record> records = new RecordBLL().GetRecords(note.id, note.patient.nric);
            ViewState["GridViewRecords"] = records;
            GridViewRecords.DataSource = records;
            GridViewRecords.DataBind();

            ViewState["GridViewPatientSelectedNRIC"] = note.patient.nric;

            UpdatePanelNote.Update();
        }

        #endregion

        #region Record Functions
        protected void GridViewRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool permited = (bool)DataBinder.Eval(e.Row.DataItem, "permited");

                if (permited)
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
                            + DataBinder.Eval(e.Row.DataItem, "fileType") +
                            "</span>";

                        FileDownloadLink.HRef = "~/Therapist/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
                        FileDownloadLink.Visible = true;
                    }

                    LinkButton LinkButtonRecordDiagnosisView = (LinkButton)e.Row.FindControl("LinkButtonRecordDiagnosisView");
                    LinkButtonRecordDiagnosisView.CommandName = "RecordDiagnosisView";
                    LinkButtonRecordDiagnosisView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    LinkButtonRecordDiagnosisView.Visible = true;
                    LinkButtonRecordDiagnosisView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View</span>";
                }
                else
                {
                    short status = (short)DataBinder.Eval(e.Row.DataItem, "status");
                    short? recordPermissionStatus = (short?)DataBinder.Eval(e.Row.DataItem, "recordPermissionStatus");

                    Label LabelRecordPermissionStatusContent = (Label)e.Row.FindControl("LabelRecordPermissionStatusContent");
                    LabelRecordPermissionStatusContent.CssClass = "text-danger";
                    LabelRecordPermissionStatusContent.Visible = true;
                    Label LabelRecordPermissionStatusDiagnosis = (Label)e.Row.FindControl("LabelRecordPermissionStatusDiagnosis");
                    LabelRecordPermissionStatusDiagnosis.CssClass = "text-danger";
                    LabelRecordPermissionStatusDiagnosis.Visible = true;

                    if (status == 0)
                    {
                        LabelRecordPermissionStatusDiagnosis.Attributes.Add("title", "Patient has Disabled Record Access To all Therapists");
                        LabelRecordPermissionStatusContent.Attributes.Add("title", "Patient has Disabled Record Access To all Therapists");
                    }
                    else if (recordPermissionStatus == 0)
                    {
                        LabelRecordPermissionStatusDiagnosis.Attributes.Add("title", "Patient has Disabled Record Access via Fine Grain Permissions");
                        LabelRecordPermissionStatusContent.Attributes.Add("title", "Patient has Disabled Record Access via Fine Grain Permissions");
                    }
                    else
                    {
                        LabelRecordPermissionStatusDiagnosis.Attributes.Add("title", "You do not have Access to this Record Type");
                        LabelRecordPermissionStatusContent.Attributes.Add("title", "You do not have Access to this Record Type");
                    }
                }
            }
        }
        protected void GridViewRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecords.PageIndex = e.NewPageIndex;
            GridViewRecords.DataSource = ViewState["GridViewRecords"];
            GridViewRecords.DataBind();
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
                    modalFileViewLabelText.Visible = false;

                    if (record.fileExtension == ".png" || record.fileExtension == ".jpg" || record.fileExtension == ".jpeg")
                    {
                        modalFileViewImage.Visible = true;
                        modalFileViewImage.ImageUrl = "~/Therapist/Download.ashx?record=" + record.id;
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
                        modalFileViewVideoSource.Attributes.Add("src", "~/Therapist/Download.ashx?record=" + record.id);
                    }

                    labelRecordName.Text = record.title;
                    modalFileViewLabelFileName.Text = record.fileName + record.fileExtension;
                    modalFileViewLabelFileSize.Text = record.fileSizeMegabytes;
                    FileDownloadLinkviaModal.HRef = "~/Therapist/Download.ashx?record=" + record.id.ToString();

                    UpdatePanelFileView.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalNote').modal('hide'); $('#modalFileView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View File Modal", "toastr['error']('Error Opening File Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("RecordDiagnosisView"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    ViewState["GridViewRecordsSelectedRecord"] = id;
                    Bind_GridViewRecordDiagnoses();

                    UpdatePanelRecordDiagnosisView.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Record Diagnosis Modal", "$('#modalNote').modal('hide'); $('#modalRecordDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Record Diagnosis Modal", "toastr['error']('Error Opening Record Diagnosis Modal.');", true);
                }
            }
        }

        protected void CloseModalFileView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View File Modal", " $('#modalFileView').modal('hide'); $('#modalNote').modal('show');$('body').addClass('modal-open');", true);
        }
        #endregion

        #region Record Diagnosis Functions
        protected void Bind_GridViewRecordDiagnoses()
        {
            int recordID = Convert.ToInt32(ViewState["GridViewRecordsSelectedRecord"]);
            Record record = recordBLL.GetRecord(recordID);
            labelRecordNameDiagnosis.Text = record.title;

            List<RecordDiagnosis> recordDiagnoses = recordBLL.GetRecordDiagnoses(recordID);
            ViewState["GridViewRecordDiagnoses"] = recordDiagnoses;
            GridViewRecordDiagnoses.DataSource = recordDiagnoses;
            GridViewRecordDiagnoses.DataBind();

            UpdatePanelRecordDiagnosisView.Update();
        }

        protected void GridViewRecordDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecordDiagnoses.PageIndex = e.NewPageIndex;
            GridViewRecordDiagnoses.DataSource = ViewState["GridViewRecordDiagnoses"];
            GridViewRecordDiagnoses.DataBind();
        }

        protected void ButtonSearchDiagnosisForRecord_Click(object sender, EventArgs e)
        {
            Bind_GridViewRecordDiagnoses();
        }
        protected void CloseModalRecordDiagnosisView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View Record Diagnosis Modal", " $('#modalRecordDiagnosisView').modal('hide'); $('#modalNote').modal('show');", true);
        }
        #endregion
    }
}