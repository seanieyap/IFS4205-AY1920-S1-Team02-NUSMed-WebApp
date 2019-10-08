using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        Bind_GridViewPatientAnonymised(new FilteredValues());
      }
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

    protected void Bind_GridViewPatientAnonymised(FilteredValues fv)
    {
      GeneralizedSetting generalizedSetting = dataBLL.GetGeneralizedSettingFromDb();

      // precheck if -1

      // Age
      inputAgeLevel.Items.Clear();
      labelTitleAge.Attributes.Add("title", "This quasi-identifier has been generalized to Level " + generalizedSetting.age);
      if (generalizedSetting.ageOptions != null)
      {
        foreach (Tuple<string, string> option in generalizedSetting.ageOptions)
        {
          inputAgeLevel.Items.Add(new ListItem(option.Item2, option.Item1));
        }
      }
      else
      {
        inputAgeLevel.Attributes.Add("disabled", "true");
      }

      // Marital Status
      inputMaritalStatusLevel.Items.Clear();
      labelTitleMaritalStatus.Attributes.Add("title", "This quasi-identifier has been generalized to Level " + generalizedSetting.maritalStatus);
      if (generalizedSetting.maritalStatusOptions != null)
      {
        foreach (Tuple<string, string> option in generalizedSetting.maritalStatusOptions)
        {
          inputMaritalStatusLevel.Items.Add(new ListItem(option.Item2, option.Item1));
        }
      }
      else
      {
        inputMaritalStatusLevel.Attributes.Add("disabled", "true");
      }

      // Gender
      inputGenderLevel.Items.Clear();
      labelTitleGender.Attributes.Add("title", "This quasi-identifier has been generalized to Level " + generalizedSetting.gender);
      if (generalizedSetting.genderOptions != null)
      {
        foreach (Tuple<string, string> option in generalizedSetting.genderOptions)
        {
          inputGenderLevel.Items.Add(new ListItem(option.Item2, option.Item1));
        }
      }
      else
      {
        inputGenderLevel.Attributes.Add("disabled", "true");
      }

      // Sex
      inputSexLevel.Items.Clear();
      labelTitleSex.Attributes.Add("title", "This quasi-identifier has been generalized to Level " + generalizedSetting.sex);
      if (generalizedSetting.sexOptions != null)
      {
        foreach (Tuple<string, string> option in generalizedSetting.sexOptions)
        {
          inputSexLevel.Items.Add(new ListItem(option.Item2, option.Item1));
        }
      }
      else
      {
        inputSexLevel.Attributes.Add("disabled", "true");
      }

      // Postal
      inputPostal.Items.Clear();
      DataTable postalCodeTable = dataBLL.GetPostal();
      foreach (DataRow postalCode in postalCodeTable.Rows)
      {
        if (Equals(postalCode["postal"].ToString(), "*"))
        {
          inputPostal.Attributes.Add("disabled", "true");
          break;
        }

        inputPostal.Items.Add(new ListItem(postalCode["postal"].ToString(), postalCode["postal"].ToString()));
      }

      // Record Type
      inputRecordType.Items.Clear();
      inputRecordType.Items.Add(new ListItem("Blood Pressure Reading", "Blood Pressure Reading"));
      inputRecordType.Items.Add(new ListItem("ECG", "ECG"));
      inputRecordType.Items.Add(new ListItem("Gait", "Gait"));
      inputRecordType.Items.Add(new ListItem("Height Measurement", "Height Measurement"));
      inputRecordType.Items.Add(new ListItem("MRI", "MRI"));
      inputRecordType.Items.Add(new ListItem("Weight Measurement", "Weight Measurement"));
      inputRecordType.Items.Add(new ListItem("X-ray", "X-ray"));

      // Diagnoses
      inputDiagnosis.Items.Clear();
      DataTable diagnosesTable = dataBLL.GetDiagnoses();
      foreach (DataRow diagnosis in diagnosesTable.Rows)
      {
        inputDiagnosis.Items.Add(new ListItem(diagnosis["diagnosis_description_short"].ToString(), diagnosis["diagnosis_code"].ToString()));
      }

      // Creation Date
      inputCreationDate.Items.Clear();
      DataTable creationDateTable = dataBLL.GetRecordCreationDate();
      foreach (DataRow creationDate in creationDateTable.Rows)
      {
        if (Equals(creationDate["record_create_date"].ToString(), "*"))
        {
          inputCreationDate.Attributes.Add("disabled", "true");
          break;
        }
        inputCreationDate.Items.Add(new ListItem(creationDate["record_create_date"].ToString(), creationDate["record_create_date"].ToString()));
      }


      // List<PatientAnonymised> recordAnonymised = dataBLL.GetPatients(new List<Tuple<string, string>>());
      //List<PatientAnonymised> recordAnonymised = dataBLL.GetPatients(new FilteredValues());
      List<PatientAnonymised> recordAnonymised = dataBLL.GetPatients(fv);
      ViewState["GridViewPatientAnonymised"] = recordAnonymised;
      GridViewPatientAnonymised.DataSource = recordAnonymised;
      GridViewPatientAnonymised.DataBind();
      UpdatePanelPatientAnonymised.Update();
    }

    protected void buttonFilter_ServerClick(object sender, EventArgs e)
    {
      // Retrieve gen settings
      GeneralizedSetting generalizedSetting = dataBLL.GetGeneralizedSettingFromDb();

      #region Validation: Match and Get only valid inputs
      FilteredValues fv = new FilteredValues();

      // Marital Status
      List<string> inputMaritalStatusValues = new List<string>();
      foreach (ListItem item in inputMaritalStatusLevel.Items)
      {
        if (item.Selected)
        {
          if (generalizedSetting.maritalStatusOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
          {
            inputMaritalStatusValues.Add(item.Value.Trim());
          }
        }
      }
      fv.maritalStatus = inputMaritalStatusValues;

      // Sex
      List<string> inputSexValues = new List<string>();
      foreach (ListItem item in inputSexLevel.Items)
      {
        if (item.Selected)
        {
          if (generalizedSetting.sexOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
          {
            inputSexValues.Add(item.Value.Trim());
          }
        }
      }
      fv.sex = inputSexValues;

      // Gender
      List<string> inputGenderValues = new List<string>();
      foreach (ListItem item in inputGenderLevel.Items)
      {
        if (item.Selected)
        {
          if (generalizedSetting.genderOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
          {
            inputGenderValues.Add(item.Value.Trim());
          }
        }
      }
      fv.gender = inputGenderValues;


      // Age
      List<string> inputAgeValues = new List<string>();
      foreach (ListItem item in inputAgeLevel.Items)
      {
        if (item.Selected)
        {
          if (generalizedSetting.ageOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
          {
            inputAgeValues.Add(item.Value.Trim());
          }
        }
      }
      fv.age = inputAgeValues;

      // TO DO: Postal Validation
      List<string> inputPostalValues = new List<string>();
      foreach (ListItem item in inputPostal.Items)
      {
        if (item.Selected)
        {
          inputPostalValues.Add(item.Value.Trim());
        }
      }
      fv.postal = inputPostalValues;

      // Record Type
      List<string> inputRecordTypeValues = new List<string>();
      foreach (ListItem item in inputRecordType.Items)
      {
        if (item.Selected)
        {
          inputRecordTypeValues.Add(item.Value.Trim());
        }
      }
      fv.recordType = inputRecordTypeValues;

      // Diagnosis
      List<string> inputDiagnosisValues = new List<string>();
      foreach (ListItem item in inputDiagnosis.Items)
      {
        if (item.Selected)
        {
            inputDiagnosisValues.Add(item.Value.Trim());
        }
      }
      fv.diagnosis = inputDiagnosisValues;

      // Creation Date
      List<string> inputCreationDateValues = new List<string>();
      foreach (ListItem item in inputCreationDate.Items)
      {
        if (item.Selected)
        {
          inputCreationDateValues.Add(item.Value.Trim());
        }
      }
      fv.creationDate = inputCreationDateValues;


      //dataBLL.GetPatients(fv);


      #endregion


      // validate values you have got
      Bind_GridViewPatientAnonymised(fv);
    }
  }
}
