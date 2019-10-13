using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Therapist.My_Patients
{
    public partial class View : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();
        private readonly TherapistBLL therapistBLL = new TherapistBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyPatients();
            Master.LiActiveTherapistMyPatientsView();

            if (!IsPostBack)
            {
                Bind_GridViewPatient();
            }
        }

        #region GridViewPatient Functions
        protected void Bind_GridViewPatient()
        {
            string term = TextboxSearch.Text.Trim().ToLower();
            List<Classes.Entity.Patient> patients = therapistBLL.GetCurrentPatients(term);
            ViewState["GridViewPatient"] = patients;
            GridViewPatient.DataSource = patients;
            GridViewPatient.DataBind();
            UpdatePanelAccounts.Update();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            Bind_GridViewPatient();
        }
        protected void GridViewPatient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string nric = e.CommandArgument.ToString();
            ViewState["GridViewPatientSelectedNRIC"] = nric;

            if (e.CommandName.Equals("ViewPermission"))
            {
                try
                {
                    Update_UpdatePanelPermissions(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Permission Modal", "$('#modalPermissions').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Permission View.');", true);
                }
            }
            else if (e.CommandName.Equals("ViewInformation"))
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
            else if (e.CommandName.Equals("ViewRecords"))
            {
                try
                {
                    List<Record> records = new RecordBLL().GetRecords(nric);
                    LabelRecordsNRIC.Text = nric;
                    modalRecordsHyperlinkNewRecord.NavigateUrl = "~/Therapist/My-Patients/New-Record?Patient-NRIC=" + nric;

                    ViewState["GridViewRecords"] = records;
                    GridViewRecords.DataSource = records;
                    GridViewRecords.DataBind();
                    UpdatePanelRecords.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Records Modal", "$('#modalRecords').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Records Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("ViewDiagnosis"))
            {
                try
                {
                    TextboxSearchDiagnosis.Text = string.Empty;
                    Bind_GridViewPatientDiagnoses(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Diagnosis Modal", "$('#modalDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Diagnosis Modal.');", true);
                }
            }

            Bind_GridViewPatient();
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
                DateTime? requestTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "requestTime");
                Label LabelName = (Label)e.Row.FindControl("LabelName");
                LinkButton LinkButtonViewInformation = (LinkButton)e.Row.FindControl("LinkButtonViewInformation");
                LinkButton LinkButtonViewDiagnosis = (LinkButton)e.Row.FindControl("LinkButtonViewDiagnosis");
                LinkButton LinkButtonViewRecords = (LinkButton)e.Row.FindControl("LinkButtonViewRecords");
                HyperLink LinkButtonNewRecord = (HyperLink)e.Row.FindControl("LinkButtonNewRecord");
                Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

                if (approvedTime == null)
                {
                    LabelName.Text = "Redacted";
                    LinkButtonViewInformation.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewInformation.Enabled = false;
                    LinkButtonViewDiagnosis.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewDiagnosis.Enabled = false;
                    LinkButtonViewRecords.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewRecords.Enabled = false;
                    LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonNewRecord.Enabled = false;
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
                    LinkButtonViewRecords.CssClass = "btn btn-success btn-sm";
                    LinkButtonViewRecords.CommandName = "ViewRecords";
                    LinkButtonViewRecords.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();

                    Int16 permissionApproved = (Int16)DataBinder.Eval(e.Row.DataItem, "permissionApproved");
                    if (permissionApproved == 0)
                    {
                        LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
                        LinkButtonNewRecord.Enabled = false;
                        LinkButtonNewRecord.Attributes.Add("TabIndex", "0");
                        LinkButtonNewRecord.Attributes.Add("data-toggle", "tooltip");
                        LinkButtonNewRecord.Attributes.Add("title", "You do not have any record type permissions.");
                    }
                    else
                    {
                        LinkButtonNewRecord.CssClass = "btn btn-info btn-sm";
                        LinkButtonNewRecord.NavigateUrl = "~/Therapist/My-Patients/New-Record?Patient-NRIC=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nric"));
                    }
                }

                if (requestTime == null)
                {
                    if (approvedTime == null)
                    {
                        LabelPermissionStatus.CssClass = "text-secondary";
                        LabelPermissionStatus.Attributes.Add("title", "You have no permissions to this patient.");
                    }
                    else
                    {
                        LabelPermissionStatus.CssClass = "text-info";
                        LabelPermissionStatus.Attributes.Add("title", "You have permissions to this patient and have not requested for any new permissions.");
                    }
                }
                else
                {
                    if (approvedTime == null)
                    {
                        LabelPermissionStatus.CssClass = "text-danger";
                        LabelPermissionStatus.Attributes.Add("title", "You have no permissions to this patient and have requested for new permissions.");
                    }
                    else
                    {
                        LabelPermissionStatus.CssClass = "text-warning";
                        LabelPermissionStatus.Attributes.Add("title", "You have requested for new permissions.");
                    }
                }

                bool isEmergency = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEmergency"));
                if (isEmergency)
                {
                    Label LabelPermissionEmergencyStatus = (Label)e.Row.FindControl("LabelPermissionEmergencyStatus");
                    LabelPermissionEmergencyStatus.Attributes.Add("title", "This patient is an emergency patient whom did not approve of any permissions.");
                    LabelPermissionEmergencyStatus.CssClass = "text-danger";
                    LabelPermissionEmergencyStatus.Visible = true;
                }
            }
        }
        #endregion

        #region Permission Functions
        private void Update_UpdatePanelPermissions(string nric)
        {
            Classes.Entity.Patient patient = therapistBLL.GetPatientPermissions(nric);

            LabelPatientNRIC.Text = patient.nric;

            CheckBoxTypeHeightMeasurementApproved.Checked = patient.hasHeightMeasurementPermissionsApproved;
            CheckBoxTypeWeightMeasurementApproved.Checked = patient.hasWeightMeasurementPermissionsApproved;
            CheckBoxTypeTemperatureReadingApproved.Checked = patient.hasTemperatureReadingPermissionsApproved;
            CheckBoxTypeBloodPressureReadingApproved.Checked = patient.hasBloodPressureReadingPermissionsApproved;
            CheckBoxTypeECGReadingApproved.Checked = patient.hasECGReadingPermissionsApproved;
            CheckBoxTypeMRIApproved.Checked = patient.hasMRIPermissionsApproved;
            CheckBoxTypeXRayApproved.Checked = patient.hasXRayPermissionsApproved;
            CheckBoxTypeGaitApproved.Checked = patient.hasGaitPermissionsApproved;

            CheckBoxTypeHeightMeasurement.Checked = patient.hasHeightMeasurementPermissions;
            CheckBoxTypeWeightMeasurement.Checked = patient.hasWeightMeasurementPermissions;
            CheckBoxTypeTemperatureReading.Checked = patient.hasTemperatureReadingPermissions;
            CheckBoxTypeBloodPressureReading.Checked = patient.hasBloodPressureReadingPermissions;
            CheckBoxTypeECGReading.Checked = patient.hasECGReadingPermissions;
            CheckBoxTypeMRI.Checked = patient.hasMRIPermissions;
            CheckBoxTypeXRay.Checked = patient.hasXRayPermissions;
            CheckBoxTypeGait.Checked = patient.hasGaitPermissions;

            if (patient.requestTime != null)
            {
                modalPermissionStatus.Text = "Request for Permissions were sent on " + patient.requestTime;
            }
            else
            {
                modalPermissionStatus.Text = "No Request sent to " + patient.nric;
            }

            if (patient.requestTime == null)
            {
                if (patient.approvedTime == null)
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-secondary my-2 text-center small");
                    modalPermissionStatus.Text = "You have no approved permissions.";
                }
                else
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-info my-2 text-center small");
                    modalPermissionStatus.Text = "You have been granted permissions and have not submitted any new requests for permissions.";
                }
            }
            else
            {
                if (patient.approvedTime == null)
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-danger my-2 text-center small");
                    modalPermissionStatus.Text = "You have not been granted permissions and have requested for permissions sent on " + patient.requestTime + ".";
                }
                else
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-warning my-2 text-center small");
                    modalPermissionStatus.Text = "You have been granted permissions and have requested for permissions sent on " + patient.requestTime + ".";
                }
            }

            UpdatePanelPermissions.Update();
        }
        protected void buttonPermissionRequest_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewPatientSelectedNRIC"].ToString();

                short permission = 0;

                if (CheckBoxTypeHeightMeasurement.Checked)
                {
                    permission += new HeightMeasurement().permissionFlag;
                }
                if (CheckBoxTypeWeightMeasurement.Checked)
                {
                    permission += new WeightMeasurement().permissionFlag;
                }
                if (CheckBoxTypeTemperatureReading.Checked)
                {
                    permission += new TemperatureReading().permissionFlag;
                }
                if (CheckBoxTypeBloodPressureReading.Checked)
                {
                    permission += new BloodPressureReading().permissionFlag;
                }
                if (CheckBoxTypeECGReading.Checked)
                {
                    permission += new ECGReading().permissionFlag;
                }
                if (CheckBoxTypeMRI.Checked)
                {
                    permission += new MRI().permissionFlag;
                }
                if (CheckBoxTypeXRay.Checked)
                {
                    permission += new XRay().permissionFlag;
                }
                if (CheckBoxTypeGait.Checked)
                {
                    permission += new Gait().permissionFlag;
                }

                therapistBLL.UpdateRequest(nric, permission);
                Bind_GridViewPatient();
                Update_UpdatePanelPermissions(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Request Submitted to " + nric + " for Permissions.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting Request for Permissions.');", true);
            }

        }
        protected void buttonPermissionRescind_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewPatientSelectedNRIC"].ToString();

                therapistBLL.RescindPermissions(nric);
                Bind_GridViewPatient();
                Update_UpdatePanelPermissions(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Request Submitted to " + nric + " for Permissions.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting Request for Permissions.');", true);
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

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').modal('show');", true);
                    }
                    else if (record.fileExtension == ".txt")
                    {
                        modalFileViewPanelText.Visible = true;
                        if (record.IsFileSafe())
                        {
                            string js = record.type.GetTextPlotJS(File.ReadAllText(record.fullpath));

                            ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').on('shown.bs.modal', function (e) {  " + js + "}); $('#modalFileView').modal('show');", true);
                        }
                    }
                    else if (record.fileExtension == ".mp4")
                    {
                        modalFileViewVideo.Visible = true;
                        modalFileViewVideoSource.Attributes.Add("src", "~/Therapist/Download.ashx?record=" + record.id);

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').modal('show');", true);
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

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Record Diagnosis Modal", "$('#modalRecords').modal('hide'); $('#modalRecordDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Record Diagnosis Modal", "toastr['error']('Error Opening Record Diagnosis Modal.');", true);
                }
            }
        }

        protected void CloseModalFileView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View File Modal", " $('#modalFileView').modal('hide'); $('#modalRecords').modal('show');", true);
        }
        #endregion

        #region Patient Diagnosis Functions
        protected void Bind_GridViewPatientDiagnoses(string nric)
        {
            List<PatientDiagnosis> patientDiagnoses = therapistBLL.GetPatientDiagnoses(nric);
            labelDiagnosisName.Text = nric;

            ViewState["GridViewPatientDiagnoses"] = patientDiagnoses;
            GridViewPatientDiagnoses.DataSource = patientDiagnoses;
            GridViewPatientDiagnoses.DataBind();

            string term = TextboxSearchDiagnosis.Text.Trim().ToLower();
            string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
            List<Diagnosis> diagnoses = therapistBLL.GetDiagnoses(term, patientNRIC, patientDiagnoses);
            ViewState["GridViewPatientDiagnosisAdd"] = diagnoses;
            GridViewPatientDiagnosisAdd.DataSource = diagnoses;
            GridViewPatientDiagnosisAdd.DataBind();

            UpdatePanelDiagnosisView.Update();
        }

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
                    LinkButton LinkButtonPatientDiagnosesEnd = (LinkButton)e.Row.FindControl("LinkButtonPatientDiagnosesEnd");
                }
                else
                {
                    LabelPatientDiagnosesEnd.Text = endDateTime.ToString();
                }
            }
        }
        protected void GridViewPatientDiagnoses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("UpdateEndPatientDiagnosis"))
            {

                try
                {
                    string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
                    string code = e.CommandArgument.ToString();

                    therapistBLL.UpdatePatientDiagnosis(patientNRIC, code);

                    Bind_GridViewPatientDiagnoses(patientNRIC);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Diagnosis attributed to " + patientNRIC + " has been Sucessfully Declared to have ended.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Declaring end of Diagnosis.');", true);
                }
            }

        }
        protected void GridViewPatientDiagnosesAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientDiagnosisAdd.PageIndex = e.NewPageIndex;
            GridViewPatientDiagnosisAdd.DataSource = ViewState["GridViewPatientDiagnosisAdd"];
            GridViewPatientDiagnosisAdd.DataBind();
        }

        protected void GridViewPatientDiagnosisAdd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddPatientDiagnosis"))
            {
                try
                {
                    string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
                    string code = e.CommandArgument.ToString();

                    therapistBLL.AddPatientDiagnosis(patientNRIC, code);

                    Bind_GridViewPatientDiagnoses(patientNRIC);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Diagnosis has been Successfully Attributed to " + patientNRIC + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Attributing Diagnosis to Patient.');", true);
                }
            }
        }

        protected void ButtonSearchDiagnosis_Click(object sender, EventArgs e)
        {
            string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
            Bind_GridViewPatientDiagnoses(patientNRIC);
        }
        #endregion

        #region Record Diagnosis Functions
        protected void Bind_GridViewRecordDiagnoses()
        {
            long recordID = Convert.ToInt64( ViewState["GridViewRecordsSelectedRecord"]);
            Record record = recordBLL.GetRecord(recordID);
            labelRecordNameDiagnosis.Text = record.title;

            List<RecordDiagnosis> recordDiagnoses = recordBLL.GetRecordDiagnoses(recordID);
            ViewState["GridViewRecordDiagnoses"] = recordDiagnoses;
            GridViewRecordDiagnoses.DataSource = recordDiagnoses;
            GridViewRecordDiagnoses.DataBind();

            string term = TextboxSearchDiagnosisForRecord.Text.Trim().ToLower();
            string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
            List<Diagnosis> diagnoses = therapistBLL.GetDiagnoses(term, patientNRIC, recordDiagnoses);
            ViewState["GridViewRecordDiagnosesAdd"] = diagnoses;
            GridViewRecordDiagnosesAdd.DataSource = diagnoses;
            GridViewRecordDiagnosesAdd.DataBind();

            UpdatePanelRecordDiagnosisView.Update();
        }

        protected void GridViewRecordDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecordDiagnoses.PageIndex = e.NewPageIndex;
            GridViewRecordDiagnoses.DataSource = ViewState["GridViewRecordDiagnoses"];
            GridViewRecordDiagnoses.DataBind();
        }

        protected void GridViewRecordDiagnosesAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecordDiagnosesAdd.PageIndex = e.NewPageIndex;
            GridViewRecordDiagnosesAdd.DataSource = ViewState["GridViewRecordDiagnosesAdd"];
            GridViewRecordDiagnosesAdd.DataBind();
        }

        protected void GridViewRecordDiagnosesAdd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddRecordDiagnosis"))
            {
                try
                {
                    string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
                    string code = e.CommandArgument.ToString();
                    long recordID = Convert.ToInt64(ViewState["GridViewRecordsSelectedRecord"]);

                    recordBLL.AddRecordDiagnosis(patientNRIC, recordID, code);

                    Bind_GridViewRecordDiagnoses();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Diagnosis has been Successfully Rttributed to Record.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Attributing Diagnosis to Record.');", true);
                }
            }
        }

        protected void ButtonSearchDiagnosisForRecord_Click(object sender, EventArgs e)
        {
            Bind_GridViewRecordDiagnoses();
        }

        protected void CloseModalRecordDiagnosisView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View Record Diagnosis Modal", " $('#modalRecordDiagnosisView').modal('hide'); $('#modalRecords').modal('show');", true);
        }
        #endregion
    }
}