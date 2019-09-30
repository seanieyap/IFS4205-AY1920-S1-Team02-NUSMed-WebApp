using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;

namespace NUSMed_WebApp.Researcher
{
    public partial class Search_Data : Page
    {
        private readonly DataBLL dataBLL = new DataBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveResearcherSearchData();

            if (!IsPostBack)
            {
                Bind_GridViewPatientAnonymised();
            }
        }

        protected void Bind_GridViewPatientAnonymised()
        {
            List<PatientAnonymised> recordAnonymised = dataBLL.getAnonymizedTableFromDb();
            ViewState["GridViewPatientAnonymised"] = recordAnonymised;
            GridViewPatientAnonymised.DataSource = recordAnonymised;
            GridViewPatientAnonymised.DataBind();
        }

        protected void GridViewPatientAnonymised_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientAnonymised.PageIndex = e.NewPageIndex;
            GridViewPatientAnonymised.DataSource = ViewState["GridViewPatientAnonymised"];
            GridViewPatientAnonymised.DataBind();
        }

        protected void GridViewPatientAnonymised_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        string recordId = DataBinder.Eval(e.Row.DataItem, "id").ToString();
            //        RecordType recordType = RecordType.Get(DataBinder.Eval(e.Row.DataItem, "record_type").ToString());
            //        if (recordType.isContent)
            //        {
            //            Label LabelContent = (Label)e.Row.FindControl("LabelContent");
            //            string content = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "content"));
            //            string unit = recordType.prefix;

            //            LabelContent.Text = content + unit;
            //            LabelContent.Visible = true;
            //        }
            //        else if (!recordType.isContent)
            //        {
            //            LinkButton LinkbuttonFileView = (LinkButton)e.Row.FindControl("LinkbuttonFileView");
            //            HtmlAnchor FileDownloadLink = (HtmlAnchor)e.Row.FindControl("FileDownloadLink");

            //            //LinkbuttonFileView.CommandName = "FileView";
            //            //LinkbuttonFileView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
            //            //LinkbuttonFileView.Visible = true;
            //            //LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View "
            //            //    + DataBinder.Eval(e.Row.DataItem, "fileType") +
            //            //    "</span>";

            //            FileDownloadLink.HRef = "~/Patient/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
            //            FileDownloadLink.Visible = true;
            //        }
            //    }
        }

        protected void GridViewPatientAnonymised_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string id = e.CommandArgument.ToString();
            //ViewState["GridViewGridViewMedicalNoteSelectedID"] = id;

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

            //Bind_GridViewMedicalNote();
        }

    }
}
