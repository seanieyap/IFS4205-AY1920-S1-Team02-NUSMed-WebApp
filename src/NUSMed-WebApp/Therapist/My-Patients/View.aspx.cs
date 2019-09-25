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

namespace NUSMed_WebApp.Therapist.My_Patients
{
    public partial class View : Page
    {
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
                    // todo add additional permission checks.
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
                    Update_UpdatePanelRecords(nric);

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Records Modal", "$('#modalRecords').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Records Modal.');", true);
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
                //Int16 permissionApproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionApproved"));
                DateTime? approvedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "approvedTime");
                Int16 permissionUnapproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionUnapproved"));
                Label LabelName = (Label)e.Row.FindControl("LabelName");
                Label LabelNameStatus = (Label)e.Row.FindControl("LabelNameStatus");
                LinkButton LinkButtonViewInformation = (LinkButton)e.Row.FindControl("LinkButtonViewInformation");
                LinkButton LinkButtonViewRecords = (LinkButton)e.Row.FindControl("LinkButtonViewRecords");
                Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

                // todo testing
                if (approvedTime == null)
                {
                    LabelName.Text = "Redacted";
                    LabelNameStatus.Visible = true;
                    LinkButtonViewInformation.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewInformation.Enabled = false;
                    LinkButtonViewRecords.CssClass = "btn btn-secondary btn-sm disabled";
                    LinkButtonViewRecords.Enabled = false;
                    LabelPermissionStatus.Attributes.Add("title", "Permissions Approved on " + Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "approvedTime")));
                    LabelPermissionStatus.CssClass = "text-success";
                }
                else
                {
                    LabelName.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lastName")) + " " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "firstName"));
                    LabelNameStatus.Visible = false;
                    LinkButtonViewInformation.CssClass = "btn btn-success btn-sm";
                    LinkButtonViewInformation.CommandName = "ViewInformation";
                    LinkButtonViewInformation.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                    LinkButtonViewRecords.CssClass = "btn btn-success btn-sm";
                    LinkButtonViewRecords.CommandName = "ViewRecords";
                    LinkButtonViewRecords.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                }

                if (permissionUnapproved > 0)
                {
                    LabelPermissionStatus.Attributes.Add("title", "Pending Approval requested on " + Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "requestTime")));
                    LabelPermissionStatus.CssClass = "text-warning";
                }
                else
                {
                    LabelPermissionStatus.Attributes.Add("title", "Not Pending Approval for Permissions");
                    LabelPermissionStatus.CssClass = "text-info";
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
        #endregion

        #region Record Functions
        private void Update_UpdatePanelRecords(string nric)
        {
            List<Record> records = new RecordBLL().GetRecords(nric);
            LabelRecordsNRIC.Text = nric;

            ViewState["GridViewRecords"] = records;
            GridViewRecords.DataSource = records;
            GridViewRecords.DataBind();
            UpdatePanelRecords.Update();
        }
        protected void GridViewRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool permited = (bool)DataBinder.Eval(e.Row.DataItem, "permited");

                // todo
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
                }
                else
                {
                    short status = (short)DataBinder.Eval(e.Row.DataItem, "status");
                    short? recordPermissionStatus = (short?)DataBinder.Eval(e.Row.DataItem, "recordPermissionStatus");

                    Label LabelRecordPermissionStatus = (Label)e.Row.FindControl("LabelRecordPermissionStatus");
                    LabelRecordPermissionStatus.CssClass = "text-danger";
                    LabelRecordPermissionStatus.Visible = true;

                    if (status == 0)
                    {
                        LabelRecordPermissionStatus.Attributes.Add("title", "Patient has Disabled Record Access To all Therapists");
                    }
                    else if (recordPermissionStatus == 0)
                    {
                        LabelRecordPermissionStatus.Attributes.Add("title", "Patient has Disabled Record Access via Fine Grain Permissions");
                    }
                    else
                    {
                        LabelRecordPermissionStatus.Attributes.Add("title", "You do not have Access to this Record Type");
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
                    string patientNRIC = ViewState["GridViewPatientSelectedNRIC"].ToString();
                    int id = Convert.ToInt32(e.CommandArgument);
                    Record record = new RecordBLL().GetRecord(id);

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
                        so.Attributes.Add("src", "~/Therapist/Download.ashx?record=" + record.id);
                    }

                    labelRecordName.Text = record.title;
                    modalFileViewLabelFileName.Text = record.fileName + record.fileExtension;
                    modalFileViewLabelFileSize.Text = record.fileSizeMegabytes;
                    FileDownloadLinkviaModal.HRef = "~/Therapist/Download.ashx?record=" + record.id.ToString();

                    UpdatePanelFileView.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalFileView').modal('show');", true);
            }
                catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View File Modal", "toastr['error']('Error Opening File Modal.');", true);
            }
        }
        }
        #endregion
    }
}