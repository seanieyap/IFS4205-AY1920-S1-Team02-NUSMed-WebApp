using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient
{
    public partial class My_Diagnoses : Page
    {
        private readonly PatientBLL patientBLL = new PatientBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyDiagnoses();

            if (!IsPostBack)
            {
                Bind_GridViewPatientDiagnoses();
            }
        }

        #region GridViewDiagnosis Functions
        protected void Bind_GridViewPatientDiagnoses()
        {
            List<PatientDiagnosis> patientDiagnoses = patientBLL.GetDiagnoses();
            ViewState["GridViewPatientDiagnoses"] = patientDiagnoses;
            GridViewPatientDiagnoses.DataSource = patientDiagnoses;
            GridViewPatientDiagnoses.DataBind();
            UpdatePanelDiagnoses.Update();
        }
     
        protected void GridViewPatientDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientDiagnoses.PageIndex = e.NewPageIndex;
            GridViewPatientDiagnoses.DataSource = ViewState["GridViewPatientDiagnoses"];
            GridViewPatientDiagnoses.DataBind();
        }
        #endregion
    }
}