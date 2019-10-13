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
                GeneralizedSetting generalizedSetting = dataBLL.GetGeneralizedSettingFromDb();

                #region Initialize Controls
                // Age
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
                inputRecordType.Items.Add(new ListItem("Blood Pressure Reading", "Blood Pressure Reading"));
                inputRecordType.Items.Add(new ListItem("ECG", "ECG"));
                inputRecordType.Items.Add(new ListItem("Gait", "Gait"));
                inputRecordType.Items.Add(new ListItem("Height Measurement", "Height Measurement"));
                inputRecordType.Items.Add(new ListItem("MRI", "MRI"));
                inputRecordType.Items.Add(new ListItem("Weight Measurement", "Weight Measurement"));
                inputRecordType.Items.Add(new ListItem("X-ray", "X-ray"));

                // Diagnoses
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

                GridViewPatientAnonymised.DataBind();
                #endregion
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

        protected void buttonFilter_ServerClick(object sender, EventArgs e)
        {
            try
            {
                // Retrieve gen settings
                GeneralizedSetting generalizedSetting = dataBLL.GetGeneralizedSettingFromDb();

                #region Validation: Match and Get only valid inputs
                FilteredValues filteredValues = new FilteredValues();

                // Marital Status
                foreach (ListItem item in inputMaritalStatusLevel.Items)
                {
                    if (item.Selected)
                    {
                        if (generalizedSetting.maritalStatusOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
                        {
                            filteredValues.maritalStatus.Add(item.Value.Trim());
                        }
                    }
                }

                // Sex
                foreach (ListItem item in inputSexLevel.Items)
                {
                    if (item.Selected)
                    {
                        if (generalizedSetting.sexOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
                        {
                            filteredValues.sex.Add(item.Value.Trim());
                        }
                    }
                }

                // Gender
                foreach (ListItem item in inputGenderLevel.Items)
                {
                    if (item.Selected)
                    {
                        if (generalizedSetting.genderOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
                        {
                            filteredValues.gender.Add(item.Value.Trim());
                        }
                    }
                }

                // Age
                foreach (ListItem item in inputAgeLevel.Items)
                {
                    if (item.Selected)
                    {
                        if (generalizedSetting.ageOptions.Any(t => t.Item1.Equals(item.Value.Trim())))
                        {
                            filteredValues.age.Add(item.Value.Trim());
                        }
                    }
                }

                DataTable postalTable = dataBLL.GetPostal();
                foreach (ListItem item in inputPostal.Items)
                {
                    if (item.Selected)
                    {
                        if (postalTable.AsEnumerable().Any(row => row.Field<string>("postal").Equals(item.Value.Trim())))
                        {
                            filteredValues.postal.Add(item.Value.Trim());
                        }
                    }
                }

                // Record Type
                foreach (ListItem item in inputRecordType.Items)
                {
                    if (item.Selected)
                    {
                        filteredValues.recordType.Add(item.Value.Trim());
                    }
                }

                // Diagnosis
                DataTable diagnosesTable = dataBLL.GetDiagnoses();
                foreach (ListItem item in inputDiagnosis.Items)
                {
                    if (item.Selected)
                    {
                        if (diagnosesTable.AsEnumerable().Any(row => row.Field<string>("diagnosis_code").Equals(item.Value.Trim())))
                        {
                            filteredValues.diagnosis.Add(item.Value.Trim());
                        }
                    }
                }

                // Creation Date
                DataTable creationDateTable = dataBLL.GetRecordCreationDate();
                foreach (ListItem item in inputCreationDate.Items)
                {
                    if (item.Selected)
                    {
                        if (creationDateTable.AsEnumerable().Any(row => row.Field<string>("record_create_date").Equals(item.Value.Trim())))
                        {
                            filteredValues.creationDate.Add(item.Value.Trim());
                        }
                    }
                }
                #endregion

                List<PatientAnonymised> recordAnonymised = dataBLL.GetPatients(filteredValues);
                ViewState["GridViewPatientAnonymised"] = recordAnonymised;
                GridViewPatientAnonymised.DataSource = recordAnonymised;
                GridViewPatientAnonymised.DataBind();
                UpdatePanelPatientAnonymised.Update();

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Data successfully displayed.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when displaying data.');", true);
            }
        }
    }
}
