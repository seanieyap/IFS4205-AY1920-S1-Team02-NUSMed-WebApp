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
    public partial class New_Request : Page
    {
        private readonly TherapistBLL therapistBLL = new TherapistBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyPatients();
            Master.LiActiveTherapistMyPatientsNewRequest();

            if (!IsPostBack)
            {
                Bind_GridViewPatient();
            }
        }

        #region GridViewPatient Functions
        protected void Bind_GridViewPatient()
        {
            string term = TextboxSearch.Text.Trim().ToLower();
            List<Classes.Entity.Patient> patients = therapistBLL.GetUnrequestedPatients(term);
            ViewState["GridViewPatient"] = patients;
            GridViewPatient.DataSource = patients;
            GridViewPatient.DataBind();
            UpdatePanelAccounts.Update();
        }
        protected void GridViewPatient_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string nric = e.CommandArgument.ToString();

            if (e.CommandName.Equals("Request"))
            {
                try
                {
                    CheckBoxTypeHeightMeasurement.Checked = false;
                    CheckBoxTypeWeightMeasurement.Checked = false;
                    CheckBoxTypeTemperatureReading.Checked = false;
                    CheckBoxTypeBloodPressureReading.Checked = false;
                    CheckBoxTypeECGReading.Checked = false;
                    CheckBoxTypeMRI.Checked = false;
                    CheckBoxTypeXRay.Checked = false;
                    CheckBoxTypeGait.Checked = false;
                    UpdatePanelSelectPermissions.Update();

                    ViewState["GridViewPatientSelectedNRIC"] = nric;

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Permission Modal", "$('#modalSelectPermissions').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Select Permission View.');", true);
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
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            Bind_GridViewPatient();
        }
        protected void GridViewPatient_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool acceptRequest = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "acceptNewRequest"));
                LinkButton linkButtonRequest = (LinkButton)e.Row.FindControl("LinkButtonRequest");

                if (acceptRequest)
                {
                    linkButtonRequest.CssClass = "btn btn-success btn-sm";
                    linkButtonRequest.Text = "<i class=\"fas fa-fw fa-user-friends\"></i>Request";
                    linkButtonRequest.CommandName = "Request";
                    linkButtonRequest.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
                }
                else
                {
                    linkButtonRequest.CssClass = "btn btn-secondary btn-sm disabled";
                    linkButtonRequest.Text = "Requested / Granted";
                }
            }
        }
        #endregion

        protected void buttonRequest_ServerClick(object sender, EventArgs e)
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

                therapistBLL.SubmitRequest(nric, permission);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "$('#modalSelectPermissions').modal('hide');toastr['success']('Request Submitted to " + nric + " for Permissions.');", true);
                Bind_GridViewPatient();
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "$('#modalSelectPermissions').modal('hide');toastr['error']('Error occured when Submitting Request.');", true);
            }

        }
    }
}