using System;
using System.Collections.Generic;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace NUSMed_WebApp.Therapist.My_Medical_Notes
{
    public partial class New_Medical_Note : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();
        private readonly TherapistBLL therapistBLL = new TherapistBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyMedicalNotes();
            Master.LiActiveTherapistMyMedicalNotesNew();

            if (!IsPostBack)
            {
                Bind_GridViewPatientAndRecord();
            }
        }

        #region GridViewPatient Functions
        protected void Bind_GridViewPatientAndRecord()
        {
            string nric = string.Empty;
            if (ViewState["GridViewPatientSelectedPatientNRIC"] != null)
            {
                nric = Convert.ToString(ViewState["GridViewPatientSelectedPatientNRIC"]);
            }
            List<Record> records = new RecordBLL().GetRecords(nric);

            ViewState["GridViewRecords"] = records;
            GridViewRecords.DataSource = records;
            GridViewRecords.DataBind();

            string term = TextboxSearch.Text.Trim().ToLower();
            List<Classes.Entity.Patient> patients = therapistBLL.GetCurrentPatients(term);
            ViewState["GridViewPatient"] = patients;
            GridViewPatient.DataSource = patients;
            GridViewPatient.DataBind();
            UpdatePanelNewMedicalNote.Update();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            ViewState["GridViewPatientSelectedNRIC"] = null;
            Bind_GridViewPatientAndRecord();
        }
        protected void GridViewPatient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string nric = e.CommandArgument.ToString();
            ViewState["GridViewPatientSelectedNRIC"] = nric;

            if (e.CommandName.Equals("ViewInformation"))
            {
                try
                {
                    Classes.Entity.Patient patient = therapistBLL.GetPatientInformation(nric);

                    // Personal Details
                    LabelInformationNRIC.Text = patient.nric;
                    inputNRIC.Value = patient.nric;
                    DateofBirth.Value = patient.dateOfBirth.ToString("MM/dd/yyyy");
                    FirstName.Value = patient.firstName;
                    LastName.Value = patient.lastName;
                    CountryofBirth.Value = patient.countryOfBirth;
                    Nationality.Value = patient.nationality;
                    Sex.Value = patient.sex;
                    Gender.Value = patient.gender;
                    MaritalStatus.Value = patient.maritalStatus;

                    // Contact Details
                    Address.Value = patient.address;
                    PostalCode.Value = patient.addressPostalCode;
                    EmailAddress.Value = patient.email;
                    ContactNumber.Value = patient.contactNumber;

                    // Patient NOK Details
                    NOKName.Value = patient.nokName;
                    NOKContact.Value = patient.nokContact;

                    UpdatePanelInformation.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Information Modal", "$('#modalInformation').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Information View.');", true);
                }
            }
            else if (e.CommandName.Equals("SelectPatient"))
            {
                try
                {
                    ViewState["GridViewPatientSelectedPatientNRIC"] = nric;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Selecting Patient.');", true);
                }
            }
            else if (e.CommandName.Equals("DeselectPatient"))
            {
                try
                {
                    ViewState["GridViewPatientSelectedPatientNRIC"] = null;
                    ViewState["GridViewRecordSelectedIDs"] = null;
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error De-selecting Patient.');", true);
                }
            }
            else if (e.CommandName.Equals("ViewDiagnosis"))
            {
                try
                {
                    List<PatientDiagnosis> patientDiagnoses = therapistBLL.GetPatientDiagnoses(nric);
                    labelDiagnosisName.Text = nric;

                    ViewState["GridViewPatientDiagnoses"] = patientDiagnoses;
                    GridViewPatientDiagnoses.DataSource = patientDiagnoses;
                    GridViewPatientDiagnoses.DataBind();

                    UpdatePanelDiagnosisView.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Diagnosis Modal", "$('#modalDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Diagnosis Modal.');", true);
                }
            }

            Bind_GridViewPatientAndRecord();
        }
        protected void GridViewPatient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatient.PageIndex = e.NewPageIndex;
            GridViewPatient.DataSource = ViewState["GridViewPatient"];
            GridViewPatient.DataBind();
        }
        protected void GridViewPatient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime? approvedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "approvedTime");
                Label LabelName = (Label)e.Row.FindControl("LabelName");
                LinkButton LinkButtonViewInformation = (LinkButton)e.Row.FindControl("LinkButtonViewInformation");
                LinkButton LinkButtonViewDiagnosis = (LinkButton)e.Row.FindControl("LinkButtonViewDiagnosis");
                LinkButton LinkButtonViewSelectPatient = (LinkButton)e.Row.FindControl("LinkButtonViewSelectPatient");

                if (approvedTime == null)
                {
                    LabelName.Text = "Redacted";
                    LinkButtonViewInformation.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewInformation.Enabled = false;
                    LinkButtonViewDiagnosis.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewDiagnosis.Enabled = false;
                    LinkButtonViewSelectPatient.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewSelectPatient.Enabled = false;
                }
                else
                {
                    LabelName.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lastName")) + " " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "firstName"));
                    LinkButtonViewInformation.CssClass = "btn btn-success btn-sm";
                    LinkButtonViewInformation.CommandName = "ViewInformation";
                    LinkButtonViewInformation.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                    LinkButtonViewDiagnosis.CssClass = "btn btn-success btn-sm";
                    LinkButtonViewDiagnosis.CommandName = "ViewDiagnosis";
                    LinkButtonViewDiagnosis.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();

                    LinkButtonViewSelectPatient.CssClass = "btn btn-primary btn-sm";
                    LinkButtonViewSelectPatient.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();

                    if (ViewState["GridViewPatientSelectedPatientNRIC"] != null)
                    {
                        if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nric")).Equals(Convert.ToString(ViewState["GridViewPatientSelectedPatientNRIC"])))
                        {
                            e.Row.CssClass = "table-success";
                            LinkButtonViewSelectPatient.CommandName = "DeselectPatient";
                            LinkButtonViewSelectPatient.Text = "<i class=\"fas fa-fw fa-minus-square\"></i><span class=\"d-none d-lg-inline-block\">Deselect</span>";
                        }
                        else
                        {
                            LinkButtonViewSelectPatient.CommandName = "SelectPatient";
                            LinkButtonViewSelectPatient.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
                        }
                    }
                    else
                    {
                        LinkButtonViewSelectPatient.CommandName = "SelectPatient";
                        LinkButtonViewSelectPatient.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
                    }
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

                        LinkbuttonFileView.CommandName = "FileView";
                        LinkbuttonFileView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                        LinkbuttonFileView.Visible = true;
                        LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View "
                            + DataBinder.Eval(e.Row.DataItem, "fileType") +
                            "</span>";
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

                LinkButton LinkButtonViewSelectRecord = (LinkButton)e.Row.FindControl("LinkButtonViewSelectRecord");
                LinkButtonViewSelectRecord.CssClass = "btn btn-primary btn-sm";
                LinkButtonViewSelectRecord.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();

                if (ViewState["GridViewRecordSelectedIDs"] != null)
                {
                    if (((HashSet<long>)ViewState["GridViewRecordSelectedIDs"]).Contains(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id"))))
                    {
                        e.Row.CssClass = "table-success";
                        LinkButtonViewSelectRecord.CommandName = "DeselectRecord";
                        LinkButtonViewSelectRecord.Text = "<i class=\"fas fa-fw fa-minus-square\"></i><span class=\"d-none d-lg-inline-block\">Deselect</span>";
                    }
                    else
                    {
                        LinkButtonViewSelectRecord.CommandName = "SelectRecord";
                        LinkButtonViewSelectRecord.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
                    }
                }
                else
                {
                    LinkButtonViewSelectRecord.CommandName = "SelectRecord";
                    LinkButtonViewSelectRecord.Text = "<i class=\"fas fa-fw fa-hand-pointer\"></i><span class=\"d-none d-lg-inline-block\">Select</span>";
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
                    //FileDownloadLinkviaModal.HRef = "~/Therapist/Download.ashx?record=" + record.id.ToString();

                    UpdatePanelFileView.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').modal('show');", true);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Record Diagnosis Modal", "$('#modalRecords').modal('hide'); $('#modalRecordDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Record Diagnosis Modal", "toastr['error']('Error Opening Record Diagnosis Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("SelectRecord"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    if (ViewState["GridViewRecordSelectedIDs"] != null)
                    {
                        HashSet<long> recordSelectedIDs = (HashSet<long>)ViewState["GridViewRecordSelectedIDs"];
                        recordSelectedIDs.Add(id);
                        ViewState["GridViewRecordSelectedIDs"] = recordSelectedIDs;
                    }
                    else
                    {
                        HashSet<long> recordSelectedIDs = new HashSet<long>();
                        recordSelectedIDs.Add(id);
                        ViewState["GridViewRecordSelectedIDs"] = recordSelectedIDs;
                    }

                    Bind_GridViewPatientAndRecord();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Selecting Record.');", true);
                }
            }
            else if (e.CommandName.Equals("DeselectRecord"))
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);

                    if (ViewState["GridViewRecordSelectedIDs"] != null)
                    {
                        HashSet<long> recordSelectedIDs = (HashSet<long>)ViewState["GridViewRecordSelectedIDs"];
                        recordSelectedIDs.Remove(id);
                        ViewState["GridViewRecordSelectedIDs"] = recordSelectedIDs;
                    }
                    else
                    {
                        ViewState["GridViewRecordSelectedIDs"] = new HashSet<long>();
                    }

                    Bind_GridViewPatientAndRecord();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error De-Selecting Record.');", true);
                }
            }

        }
        protected void CloseModalFileView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View File Modal", " $('#modalFileView').modal('hide');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View Record Diagnosis Modal", " $('#modalRecordDiagnosisView').modal('hide');", true);
        }
        #endregion

        protected void buttonSubmit_ServerClick(object sender, EventArgs e)
        {
            Note note = new Note();
            note.title = inputTitle.Value.Trim();
            note.content = TextBoxContent.Text.Trim();
            HashSet<long> selectedRecords = new HashSet<long>();

            if (ViewState["GridViewPatientSelectedPatientNRIC"] != null)
            {
                note.patient.nric = Convert.ToString(ViewState["GridViewPatientSelectedPatientNRIC"]);
            }

            if (ViewState["GridViewRecordSelectedIDs"] != null)
            {
                selectedRecords = (HashSet<long>)ViewState["GridViewRecordSelectedIDs"];
            }

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 3).ToArray();

            // If any fields are empty
            if (!note.IsTitleValid())
            {
                validate[0] = false;
                inputTitle.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputTitle.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (!note.IsContentValid())
            {
                validate[1] = false;
                TextBoxContent.CssClass = "form-control form-control-sm is-invalid";
            }
            else
                TextBoxContent.CssClass="form-control form-control-sm is-valid";

            if (!AccountBLL.IsNRICValid(note.patient.nric))
            {
                validate[2] = false;
                spanGridViewPatientMessage.Visible = true;
            }
            else
            {
                spanGridViewPatientMessage.Visible = false;
            }

            #endregion

            if (validate.Contains(false))
            {
                spanMessage.Visible = true;
            }
            else
            {
                spanMessage.Visible = false;

                try
                {
                    foreach (int recordID in selectedRecords)
                    {
                        Record newRecord = new Record();
                        newRecord.id = recordID;
                        newRecord.patientNRIC = note.patient.nric;
                        note.records.Add(newRecord);
                    }

                    if (therapistBLL.AddNote(note))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modelSuccess').modal('show');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting a Record');", true);
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting a Record');", true);
                }
            }
        }

        protected void buttonSuccessCreateAnother_ServerClick(object sender, EventArgs e)
        {
            if (Master.IsLocalUrl(Request.RawUrl))
            {
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}