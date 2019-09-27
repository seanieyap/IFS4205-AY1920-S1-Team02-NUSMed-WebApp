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
    public partial class My_Diagnoses : Page
    {
        private readonly DiagnosisBLL diagnosisBLL = new DiagnosisBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyDiagnoses();

            if (!IsPostBack)
            {
                Bind_GridViewPatientDiagnoses();
            }
        }

        #region GridViewPatient Functions
        protected void Bind_GridViewPatientDiagnoses()
        {
            List<PatientDiagnosis> patientDiagnosis = diagnosisBLL.GetDiagnosis();
            ViewState["GridViewPatientDiagnoses"] = patientDiagnosis;
            GridViewPatientDiagnoses.DataSource = patientDiagnosis;
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