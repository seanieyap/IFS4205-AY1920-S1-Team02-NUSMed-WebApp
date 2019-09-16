using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
                    
                    // not correct method, change to different method with authorization control.
                    Account account = new AccountBLL().GetAccount(nric);

                    // Personal Details
                    LabelInformationNRIC.Text = account.nric;
                    inputNRIC.Value = account.nric;
                    DateofBirth.Value = account.dateOfBirth.ToString("MM/dd/yyyy");
                    FirstName.Value = account.firstName;
                    LastName.Value = account.lastName;
                    CountryofBirth.Value = account.countryOfBirth;
                    Nationality.Value = account.nationality;
                    Sex.Value = account.sex;
                    Gender.Value = account.gender;
                    MartialStatus.Value = account.martialStatus;

                    // Contact Details
                    Address.Value = account.address;
                    PostalCode.Value = account.addressPostalCode;
                    EmailAddress.Value = account.email;
                    ContactNumber.Value = account.contactNumber;

                    // Patient NOK Details
                    NOKName.Value = account.nokName;
                    NOKContact.Value = account.nokContact;

                    UpdatePanelInformation.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Information Modal", "$('#modalInformation').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Information View.');", true);
                }
            }
            //else if (e.CommandName.Equals("ViewRecords"))
            //{

            //}

            Bind_GridViewPatient();
        }
        protected void GridViewPatient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatient.PageIndex = e.NewPageIndex;
            GridViewPatient.DataSource = ViewState["GridViewPatient"];
            GridViewPatient.DataBind();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            Bind_GridViewPatient();
        }
        protected void GridViewPatient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int16 permissionApproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionApproved"));
                Int16 permissionUnapproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionUnapproved"));
                Label LabelName = (Label)e.Row.FindControl("LabelName");
                Label LabelNameStatus = (Label)e.Row.FindControl("LabelNameStatus");
                LinkButton LinkButtonViewInformation = (LinkButton)e.Row.FindControl("LinkButtonViewInformation");
                LinkButton LinkButtonViewRecords = (LinkButton)e.Row.FindControl("LinkButtonViewRecords");
                Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

                if (permissionApproved == 0)
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
                else if (permissionApproved != 0)
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

            if (patient.permissionUnapproved > 0 && patient.requestTime != null)
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
                    permission += HeightMeasurement.permissionFlag;
                }
                if (CheckBoxTypeWeightMeasurement.Checked)
                {
                    permission += WeightMeasurement.permissionFlag;
                }
                if (CheckBoxTypeTemperatureReading.Checked)
                {
                    permission += TemperatureReading.permissionFlag;
                }
                if (CheckBoxTypeBloodPressureReading.Checked)
                {
                    permission += BloodPressureReading.permissionFlag;
                }
                if (CheckBoxTypeECGReading.Checked)
                {
                    permission += ECGReading.permissionFlag;
                }
                if (CheckBoxTypeMRI.Checked)
                {
                    permission += MRI.permissionFlag;
                }
                if (CheckBoxTypeXRay.Checked)
                {
                    permission += XRay.permissionFlag;
                }
                if (CheckBoxTypeGait.Checked)
                {
                    permission += Gait.permissionFlag;
                }

                therapistBLL.UpdateRequest(nric, permission);
                Bind_GridViewPatient();
                Update_UpdatePanelPermissions(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Request Submitted / Updated to " + nric + " for Permissions.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting / Updating Request.');", true);
            }

        }

    }
}