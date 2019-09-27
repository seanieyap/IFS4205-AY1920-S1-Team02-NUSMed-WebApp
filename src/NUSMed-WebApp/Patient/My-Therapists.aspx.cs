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
                Int16 permissionApproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionApproved"));
                Int16 permissionUnapproved = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "permissionUnapproved"));
                Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

                if (permissionUnapproved > 0)
                {
                    LabelPermissionStatus.Attributes.Add("title", "Pending Approval requested on " + Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "requestTime")));
                    LabelPermissionStatus.CssClass = "text-warning";
                }
                else if (permissionUnapproved == 0)
                {
                    LabelPermissionStatus.Attributes.Add("title", "Not Pending Approval for Permissions");
                    LabelPermissionStatus.CssClass = "text-info";
                }

                bool isEmergency = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEmergency"));
                if (isEmergency)
                {
                    LabelPermissionStatus.Attributes.Add("title", "This therapist has permissions whom you did not approve.<br />To remove this warning, please approve his/her permissions.");
                    LabelPermissionStatus.CssClass = "text-danger";
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

            if (therapist.permissionUnapproved > 0 && therapist.requestTime != null)
            {
                modalPermissionStatus.Text = therapist.lastName + " " + therapist.firstName + " sent request on " + therapist.requestTime;
            }
            else
            {
                modalPermissionStatus.Text = "No Request received by " + therapist.lastName + " " + therapist.firstName + ".";
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
    }
}