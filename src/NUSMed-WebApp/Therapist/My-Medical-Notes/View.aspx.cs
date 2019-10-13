using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
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
            long id = Convert.ToInt64(e.CommandArgument.ToString());
            ViewState["GridViewGridViewMedicalNoteSelectedID"] = id;

            if (e.CommandName.Equals("ViewNote"))
            {
                try
                {
                    Note note = therapistBLL.GetNote(id);

                    // Note Details
                    inputTitle.Value = note.title;
                    TextBoxContent.Text = note.content;
                    inputCreateBy.Value = note.creator.lastName + " " + note.creator.firstName;
                    inputCreateTime.Value = note.createTime.ToString();
                    inputPatientNRIC.Value = note.patient.nric;

                    if (note.patient.approvedTime == null)
                    {
                        inputPatientName.Value = "Redacted";

                        PanelNoteUnauthorized.Visible = true;
                        PanelPatientPersonalInformation.Visible = false;
                        PanelPatientDiagnosis.Visible = false;
                        PanelNoteRecords.Visible = false;
                    }
                    else
                    {
                        inputPatientName.Value = note.patient.lastName + " " + note.patient.firstName;

                        // Personal Details
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

                        // Patient Diagnoses
                        List<PatientDiagnosis> patientDiagnoses = therapistBLL.GetPatientDiagnoses(note.patient.nric, id);
                        ViewState["GridViewPatientDiagnoses"] = patientDiagnoses;
                        GridViewPatientDiagnoses.DataSource = patientDiagnoses;
                        GridViewPatientDiagnoses.DataBind();

                        // Records
                        List<Record> records = new RecordBLL().GetRecords(note.patient.nric, note.id);
                        ViewState["GridViewRecords"] = records;
                        GridViewRecords.DataSource = records;
                        GridViewRecords.DataBind();
                    }

                    ViewState["GridViewPatientSelectedNRIC"] = note.patient.nric;

                    UpdatePanelNote.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Note Modal", "$('#modalNote').modal('show'); $('#NoteInformation').collapse('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Note View.');", true);
                }
            }
            else if (e.CommandName.Equals("ViewSendNoteModal"))
            {
                try
                {
                    Bind_GridViewTherapistSendNote();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Note Modal", "$('#modalSendNote').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Send Note View.');", true);
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
                DateTime? patientApprovedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "patient.approvedTime");
                Label LabelPatientName = (Label)e.Row.FindControl("LabelPatientName");

                if (patientApprovedTime == null)
                {
                    LabelPatientName.Text = "Redacted";
                }
                else
                {
                    LabelPatientName.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "patient.lastName")) + " " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "patient.firstName"));
                }
            }
        }
        #endregion

        #region Patient Diagnosis Functions
        protected void GridViewPatientDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientDiagnoses.PageIndex = e.NewPageIndex;
            GridViewPatientDiagnoses.DataSource = ViewState["GridViewPatientDiagnoses"];
            GridViewPatientDiagnoses.DataBind();
        }

        protected void GridViewPatientDiagnoses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label LabelPatientDiagnosesEnd = (Label)e.Row.FindControl("LabelPatientDiagnosesEnd");
                DateTime? endDateTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "end");

                if (endDateTime == null)
                {
                    LabelPatientDiagnosesEnd.Text = "Present";
                }
                else
                {
                    LabelPatientDiagnosesEnd.Text = endDateTime.ToString();
                }
            }
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

            ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#NoteRecords').collapse('show');", true);
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
                    FileDownloadLinkviaModal.HRef = "~/Therapist/Download.ashx?record=" + record.id.ToString();

                    if (record.fileExtension == ".png" || record.fileExtension == ".jpg" || record.fileExtension == ".jpeg")
                    {
                        modalFileViewImage.Visible = true;
                        modalFileViewImage.ImageUrl = "~/Therapist/Download.ashx?record=" + record.id;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalNote').modal('hide'); $('#modalFileView').modal('show'); $('#NoteRecords').collapse('show');", true);
                    }
                    else if (record.fileExtension == ".txt")
                    {
                        modalFileViewPanelText.Visible = true;
                        if (record.IsFileSafe())
                        {
                            string js = record.type.GetTextPlotJS(File.ReadAllText(record.fullpath));

                            ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').on('shown.bs.modal', function (e) {  " + js + "}); $('#modalNote').modal('hide'); $('#modalFileView').modal('show'); $('#NoteRecords').collapse('show');", true);
                        }
                    }
                    else if (record.fileExtension == ".mp4")
                    {
                        modalFileViewVideo.Visible = true;
                        modalFileViewVideoSource.Attributes.Add("src", "~/Patient/Download.ashx?record=" + record.id);

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalNote').modal('hide'); $('#modalFileView').modal('show'); $('#NoteRecords').collapse('show');", true);
                    }

                    UpdatePanelFileView.Update();
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

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Record Diagnosis Modal", "$('#modalNote').modal('hide'); $('#modalRecordDiagnosisView').modal('show'); $('#NoteRecords').collapse('show');", true);
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
            long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelectedRecord"]);
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

        #region
        protected void Bind_GridViewTherapistSendNote()
        {
            string term = TextBoxSearchTherapist.Text.Trim().ToLower();
            List<Classes.Entity.Therapist> therapists = therapistBLL.GetTherapists(term);
            ViewState["GridViewTherapistSendNote"] = therapists;
            GridViewTherapistSendNote.DataSource = therapists;
            GridViewTherapistSendNote.DataBind();
            UpdatePanelSendNote.Update();
        }
        protected void GridViewTherapistSendNote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectTherapist"))
            {
                try
                {
                    string nric = e.CommandArgument.ToString();

                    if (ViewState["GridViewTherapistSendNoteSelectedNRIC"] != null)
                    {
                        HashSet<string> selectedTherapists = (HashSet<string>)ViewState["GridViewTherapistSendNoteSelectedNRIC"];
                        selectedTherapists.Add(nric);
                        ViewState["GridViewTherapistSendNoteSelectedNRIC"] = selectedTherapists;
                    }
                    else
                    {
                        HashSet<string> recordSelectedIDs = new HashSet<string>();
                        recordSelectedIDs.Add(nric);
                        ViewState["GridViewTherapistSendNoteSelectedNRIC"] = recordSelectedIDs;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Selecting Therapist.');", true);
                }
            }
            else if (e.CommandName.Equals("DeselectTherapist"))
            {
                try
                {
                    string nric = e.CommandArgument.ToString();

                    if (ViewState["GridViewTherapistSendNoteSelectedNRIC"] != null)
                    {
                        HashSet<string> selectedTherapists = (HashSet<string>)ViewState["GridViewTherapistSendNoteSelectedNRIC"];
                        selectedTherapists.Remove(nric);
                        ViewState["GridViewTherapistSendNoteSelectedNRIC"] = selectedTherapists;
                    }
                    else
                    {
                        ViewState["GridViewTherapistSendNoteSelectedNRIC"] = new HashSet<string>();
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error De-selecting Therapist.');", true);
                }
            }

            Bind_GridViewTherapistSendNote();
        }
        protected void GridViewTherapistSendNote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewTherapistSendNote.PageIndex = e.NewPageIndex;
            GridViewTherapistSendNote.DataSource = ViewState["GridViewTherapistSendNote"];
            GridViewTherapistSendNote.DataBind();
        }
        protected void GridViewTherapistSendNote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton LinkButtonViewSelectTherapist = (LinkButton)e.Row.FindControl("LinkButtonViewSelectTherapist");
                LinkButtonViewSelectTherapist.CssClass = "btn btn-primary btn-sm";
                LinkButtonViewSelectTherapist.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();

                if (ViewState["GridViewTherapistSendNoteSelectedNRIC"] != null)
                {
                    if (((HashSet<string>)ViewState["GridViewTherapistSendNoteSelectedNRIC"]).Contains(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nric"))))
                    {
                        e.Row.CssClass = "table-success";
                        LinkButtonViewSelectTherapist.CommandName = "DeselectTherapist";
                        LinkButtonViewSelectTherapist.Text = "<i class=\"fas fa-fw fa-minus-square\"></i><span class=\"d-none d-lg-inline-block\">Deselect</span>";
                    }
                    else
                    {
                        LinkButtonViewSelectTherapist.CommandName = "SelectTherapist";
                        LinkButtonViewSelectTherapist.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
                    }
                }
                else
                {
                    LinkButtonViewSelectTherapist.CommandName = "SelectTherapist";
                    LinkButtonViewSelectTherapist.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
                }
            }
        }
        protected void ButtonSearchSendNote_Click(object sender, EventArgs e)
        {
            Bind_GridViewTherapistSendNote();
        }
        protected void buttonSendNote_ServerClick(object sender, EventArgs e)
        {
            try
            {
                HashSet<string> selectedTherapists = new HashSet<string>();

                if (ViewState["GridViewTherapistSendNoteSelectedNRIC"] != null)
                {
                    selectedTherapists = (HashSet<string>)ViewState["GridViewTherapistSendNoteSelectedNRIC"];
                }

                long noteID = (long) ViewState["GridViewGridViewMedicalNoteSelectedID"];

                therapistBLL.SendNote(noteID, selectedTherapists);

                ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modalSendNote').modal('hide'); toastr['success']('Success in Sending Medical Note to other Therapist(s)');", true);

            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Sending a Medical Note');", true);
            }
        }

        #endregion
    }
}