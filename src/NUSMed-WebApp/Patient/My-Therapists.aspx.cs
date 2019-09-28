using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient
{
    public partial class My_Therapists : Page
    {
        private readonly PatientBLL patientBLL = new PatientBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyPatients();
            Master.LiActiveTherapistMyPatientsView();

            if (!IsPostBack)
            {
                Bind_GridViewTherapist();
            }
        }

        #region GridViewPatient Functions
        protected void Bind_GridViewTherapist()
        {
            string term = TextboxSearch.Text.Trim().ToLower();
            List<Classes.Entity.Therapist> therapists = patientBLL.GetCurrentTherapists(term);
            ViewState["GridViewTherapist"] = therapists;
            GridViewTherapist.DataSource = therapists;
            GridViewTherapist.DataBind();
            UpdatePanelAccounts.Update();
        }
        protected void GridViewTherapist_RowCommand(object sender, GridViewCommandEventArgs e)
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

            Bind_GridViewTherapist();
        }
        protected void GridViewTherapist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewTherapist.PageIndex = e.NewPageIndex;
            GridViewTherapist.DataSource = ViewState["GridViewPatient"];
            GridViewTherapist.DataBind();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            Bind_GridViewTherapist();
        }
        protected void GridViewTherapist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime? approvedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "approvedTime");
                DateTime? requestTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "requestTime");
                Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

                if (requestTime == null)
                {
                    if (approvedTime == null)
                    {
                        LabelPermissionStatus.CssClass = "text-secondary";
                        LabelPermissionStatus.Attributes.Add("title", "Therapist has no permissions");
                    }
                    else
                    {
                        LabelPermissionStatus.CssClass = "text-info";
                        LabelPermissionStatus.Attributes.Add("title", "Therapist has permissions and is not requesting for new permissions.");
                    }
                }
                else
                {
                    if (approvedTime == null)
                    {
                        LabelPermissionStatus.CssClass = "text-danger";
                        LabelPermissionStatus.Attributes.Add("title", "Therapist has no permissions and is requesting for new permissions.");
                    }
                    else
                    {
                        LabelPermissionStatus.CssClass = "text-warning";
                        LabelPermissionStatus.Attributes.Add("title", "Therapist is requesting for new permissions.");
                    }
                }

                bool isEmergency = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEmergency"));
                if (isEmergency)
                {
                    Label LabelPermissionEmergencyStatus = (Label)e.Row.FindControl("LabelPermissionEmergencyStatus");
                    LabelPermissionEmergencyStatus.Attributes.Add("title", "This therapist got permissions via the emergency system whom you did not approve.<br />To remove this warning, approve his/her permissions.");
                    LabelPermissionEmergencyStatus.CssClass = "text-danger";
                    LabelPermissionEmergencyStatus.Visible = true;
                }

            }
        }
        #endregion
        private void Update_UpdatePanelPermissions(string nric)
        {
            Classes.Entity.Therapist therapist = patientBLL.GetTherapistPermission(nric);

            LabelTherapistName.Text = therapist.lastName + " " + therapist.firstName;

            CheckBoxTypeHeightMeasurementApproved.Checked = therapist.hasHeightMeasurementPermissionsApproved;
            CheckBoxTypeWeightMeasurementApproved.Checked = therapist.hasWeightMeasurementPermissionsApproved;
            CheckBoxTypeTemperatureReadingApproved.Checked = therapist.hasTemperatureReadingPermissionsApproved;
            CheckBoxTypeBloodPressureReadingApproved.Checked = therapist.hasBloodPressureReadingPermissionsApproved;
            CheckBoxTypeECGReadingApproved.Checked = therapist.hasECGReadingPermissionsApproved;
            CheckBoxTypeMRIApproved.Checked = therapist.hasMRIPermissionsApproved;
            CheckBoxTypeXRayApproved.Checked = therapist.hasXRayPermissionsApproved;
            CheckBoxTypeGaitApproved.Checked = therapist.hasGaitPermissionsApproved;

            CheckBoxTypeHeightMeasurement.Checked = therapist.hasHeightMeasurementPermissions;
            CheckBoxTypeWeightMeasurement.Checked = therapist.hasWeightMeasurementPermissions;
            CheckBoxTypeTemperatureReading.Checked = therapist.hasTemperatureReadingPermissions;
            CheckBoxTypeBloodPressureReading.Checked = therapist.hasBloodPressureReadingPermissions;
            CheckBoxTypeECGReading.Checked = therapist.hasECGReadingPermissions;
            CheckBoxTypeMRI.Checked = therapist.hasMRIPermissions;
            CheckBoxTypeXRay.Checked = therapist.hasXRayPermissions;
            CheckBoxTypeGait.Checked = therapist.hasGaitPermissions;

            if (therapist.requestTime == null)
            {
                if (therapist.approvedTime == null)
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-secondary my-2 text-center small");
                    modalPermissionStatus.Text = therapist.lastName + " " + therapist.firstName + " has no permissions.";
                }
                else
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-info my-2 text-center small");
                    modalPermissionStatus.Text = therapist.lastName + " " + therapist.firstName +  " has permissions and has not submitted any new request.";
                }
            }
            else
            {
                if (therapist.approvedTime == null)
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-danger my-2 text-center small");
                    modalPermissionStatus.Text = therapist.lastName + " " + therapist.firstName + " has no permissions and has requested for permissions sent on " + therapist.requestTime;
                }
                else
                {
                    DivModalPermissionStatus.Attributes.Add("class", "alert alert-warning my-2 text-center small");
                    modalPermissionStatus.Text = therapist.lastName + " " + therapist.firstName + " has permissions and has requested for new permissions sent on " + therapist.requestTime;
                }
            }

            UpdatePanelPermissions.Update();
        }
        protected void buttonPermissionApprove_ServerClick(object sender, EventArgs e)
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

                patientBLL.ApproveRequest(nric, permission);
                Bind_GridViewTherapist();
                Update_UpdatePanelPermissions(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Request Approved, Permissions of " + nric + " has been Updated.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Permissions.');", true);
            }

        }
        protected void buttonPermissionRevoke_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewPatientSelectedNRIC"].ToString();

                patientBLL.RevokePermissions(nric);
                Bind_GridViewTherapist();
                Update_UpdatePanelPermissions(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Permissions of " + nric + " has been Rescinded.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Rescinded Permissions of a therapist.');", true);
            }

        }
    }
}