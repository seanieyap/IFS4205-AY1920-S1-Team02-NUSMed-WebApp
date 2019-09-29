using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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
            string id = e.CommandArgument.ToString();
            ViewState["GridViewGridViewMedicalNoteSelectedID"] = id;

            //if (e.CommandName.Equals("ViewPermission"))
            //{
            //    try
            //    {
            //        Update_UpdatePanelPermissions(nric);
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Permission Modal", "$('#modalPermissions').modal('show');", true);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Permission View.');", true);
            //    }
            //}
            //else if (e.CommandName.Equals("ViewInformation"))
            //{
            //    try
            //    {
            //        // todo add additional permission checks.
            //        Classes.Entity.Patient patient = therapistBLL.GetPatientInformation(nric);

            //        // Personal Details
            //        LabelInformationNRIC.Text = patient.nric;
            //        inputNRIC.Value = patient.nric;
            //        DateofBirth.Value = patient.dateOfBirth.ToString("MM/dd/yyyy");
            //        FirstName.Value = patient.firstName;
            //        LastName.Value = patient.lastName;
            //        CountryofBirth.Value = patient.countryOfBirth;
            //        Nationality.Value = patient.nationality;
            //        Sex.Value = patient.sex;
            //        Gender.Value = patient.gender;
            //        MaritalStatus.Value = patient.maritalStatus;

            //        // Contact Details
            //        Address.Value = patient.address;
            //        PostalCode.Value = patient.addressPostalCode;
            //        EmailAddress.Value = patient.email;
            //        ContactNumber.Value = patient.contactNumber;

            //        // Patient NOK Details
            //        NOKName.Value = patient.nokName;
            //        NOKContact.Value = patient.nokContact;

            //        UpdatePanelInformation.Update();

            //        ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Information Modal", "$('#modalInformation').modal('show');", true);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Information View.');", true);
            //    }
            //}
            //else if (e.CommandName.Equals("ViewRecords"))
            //{
            //    try
            //    {
            //        List<Record> records = new RecordBLL().GetRecords(nric);
            //        LabelRecordsNRIC.Text = nric;
            //        modalRecordsHyperlinkNewRecord.NavigateUrl = "~/Therapist/My-Patients/New-Record?Patient-NRIC=" + nric;

            //        ViewState["GridViewRecords"] = records;
            //        GridViewRecords.DataSource = records;
            //        GridViewRecords.DataBind();
            //        UpdatePanelRecords.Update();

            //        ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Records Modal", "$('#modalRecords').modal('show');", true);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Records Modal.');", true);
            //    }
            //}
            //else if (e.CommandName.Equals("ViewDiagnosis"))
            //{
            //    try
            //    {
            //        TextboxSearchDiagnosis.Text = string.Empty;
            //        Bind_GridViewPatientDiagnoses(nric);
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Open Diagnosis Modal", "$('#modalDiagnosisView').modal('show');", true);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Diagnosis Modal.');", true);
            //    }
            //}

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
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DateTime? approvedTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "approvedTime");
            //    DateTime? requestTime = (DateTime?)DataBinder.Eval(e.Row.DataItem, "requestTime");
            //    Label LabelName = (Label)e.Row.FindControl("LabelName");
            //    LinkButton LinkButtonViewInformation = (LinkButton)e.Row.FindControl("LinkButtonViewInformation");
            //    LinkButton LinkButtonViewDiagnosis = (LinkButton)e.Row.FindControl("LinkButtonViewDiagnosis");
            //    LinkButton LinkButtonViewRecords = (LinkButton)e.Row.FindControl("LinkButtonViewRecords");
            //    HyperLink LinkButtonNewRecord = (HyperLink)e.Row.FindControl("LinkButtonNewRecord");
            //    Label LabelPermissionStatus = (Label)e.Row.FindControl("LabelPermissionStatus");

            //    if (approvedTime == null)
            //    {
            //        LabelName.Text = "Redacted";
            //        LinkButtonViewInformation.CssClass = "btn btn-secondary btn-sm disabled";
            //        LinkButtonViewInformation.Enabled = false;
            //        LinkButtonViewDiagnosis.CssClass = "btn btn-secondary btn-sm disabled";
            //        LinkButtonViewDiagnosis.Enabled = false;
            //        LinkButtonViewRecords.CssClass = "btn btn-secondary btn-sm disabled";
            //        LinkButtonViewRecords.Enabled = false;
            //        LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
            //        LinkButtonNewRecord.Enabled = false;
            //    }
            //    else
            //    {
            //        LabelName.Text = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lastName")) + " " + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "firstName"));
            //        LinkButtonViewInformation.CssClass = "btn btn-success btn-sm";
            //        LinkButtonViewInformation.CommandName = "ViewInformation";
            //        LinkButtonViewInformation.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
            //        LinkButtonViewDiagnosis.CssClass = "btn btn-success btn-sm";
            //        LinkButtonViewDiagnosis.CommandName = "ViewDiagnosis";
            //        LinkButtonViewDiagnosis.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();
            //        LinkButtonViewRecords.CssClass = "btn btn-success btn-sm";
            //        LinkButtonViewRecords.CommandName = "ViewRecords";
            //        LinkButtonViewRecords.CommandArgument = DataBinder.Eval(e.Row.DataItem, "nric").ToString();

            //        Int16 permissionApproved = (Int16)DataBinder.Eval(e.Row.DataItem, "permissionApproved");
            //        if (permissionApproved == 0)
            //        {
            //            LinkButtonNewRecord.CssClass = "btn btn-secondary btn-sm disabled";
            //            LinkButtonNewRecord.Enabled = false;
            //            LinkButtonNewRecord.Attributes.Add("TabIndex", "0");
            //            LinkButtonNewRecord.Attributes.Add("data-toggle", "tooltip");
            //            LinkButtonNewRecord.Attributes.Add("title", "You do not have any record type permissions.");
            //        }
            //        else
            //        {
            //            LinkButtonNewRecord.CssClass = "btn btn-info btn-sm";
            //            LinkButtonNewRecord.NavigateUrl = "~/Therapist/My-Patients/New-Record?Patient-NRIC=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "nric"));
            //        }
            //    }

            //    if (requestTime == null)
            //    {
            //        if (approvedTime == null)
            //        {
            //            LabelPermissionStatus.CssClass = "text-secondary";
            //            LabelPermissionStatus.Attributes.Add("title", "You have no permissions to this patient.");
            //        }
            //        else
            //        {
            //            LabelPermissionStatus.CssClass = "text-info";
            //            LabelPermissionStatus.Attributes.Add("title", "You have permissions to this patient and have not requested for any new permissions.");
            //        }
            //    }
            //    else
            //    {
            //        if (approvedTime == null)
            //        {
            //            LabelPermissionStatus.CssClass = "text-danger";
            //            LabelPermissionStatus.Attributes.Add("title", "You have no permissions to this patient and have requested for new permissions.");
            //        }
            //        else
            //        {
            //            LabelPermissionStatus.CssClass = "text-warning";
            //            LabelPermissionStatus.Attributes.Add("title", "You have requested for new permissions.");
            //        }
            //    }

            //    bool isEmergency = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEmergency"));
            //    if (isEmergency)
            //    {
            //        Label LabelPermissionEmergencyStatus = (Label)e.Row.FindControl("LabelPermissionEmergencyStatus");
            //        LabelPermissionEmergencyStatus.Attributes.Add("title", "This patient is an emergency patient whom did not approve of any permissions.");
            //        LabelPermissionEmergencyStatus.CssClass = "text-danger";
            //        LabelPermissionEmergencyStatus.Visible = true;
            //    }
            //}
        }
        #endregion
    }
}