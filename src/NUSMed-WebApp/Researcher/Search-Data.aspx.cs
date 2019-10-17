using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                if (postalCodeTable.Rows.Contains("*"))
                {
                    inputPostal.Attributes.Add("disabled", "true");
                }
                else
                {
                    foreach (DataRow postalCode in postalCodeTable.Rows)
                    {
                        inputPostal.Items.Add(new ListItem(postalCode["postal"].ToString(), postalCode["postal"].ToString()));
                    }
                }

                // Diagnoses
                DataTable diagnosesTable = dataBLL.GetDiagnoses();
                foreach (DataRow diagnosis in diagnosesTable.Rows)
                {
                    string buffer = diagnosis["diagnosis_code"].ToString();
                    inputDiagnosis.Items.Add(new ListItem(buffer + ": " + diagnosis["diagnosis_description_short"].ToString(), buffer));
                }

                // Record Type
                inputRecordType.Items.Add(new ListItem("Blood Pressure Reading", "Blood Pressure Reading"));
                inputRecordType.Items.Add(new ListItem("ECG", "ECG"));
                inputRecordType.Items.Add(new ListItem("Gait", "Gait"));
                inputRecordType.Items.Add(new ListItem("Height Measurement", "Height Measurement"));
                inputRecordType.Items.Add(new ListItem("MRI", "MRI"));
                inputRecordType.Items.Add(new ListItem("Weight Measurement", "Weight Measurement"));
                inputRecordType.Items.Add(new ListItem("X-ray", "X-ray"));

                // Record Diagnoses
                DataTable recordDiagnosesTable = dataBLL.GetRecordDiagnoses();
                foreach (DataRow diagnosis in recordDiagnosesTable.Rows)
                {
                    string buffer = diagnosis["diagnosis_code"].ToString();
                    inputRecordDiagnosis.Items.Add(new ListItem(buffer + ": " + diagnosis["diagnosis_description_short"].ToString(), buffer));
                }

                // Creation Date
                DataTable creationDateTable = dataBLL.GetRecordCreationDate();
                if (creationDateTable.Rows.Contains("*"))
                {
                    inputCreationDate.Attributes.Add("disabled", "true");
                }
                else
                {
                    foreach (DataRow creationDate in creationDateTable.Rows)
                    {
                        inputCreationDate.Items.Add(new ListItem(creationDate["record_create_date"].ToString(), creationDate["record_create_date"].ToString()));
                    }
                }

                GridViewPatientAnonymised.DataBind();
                #endregion
            }

        }

        #region GridViewPatientAnonymised Functions
        protected void GridViewPatientAnonymised_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientAnonymised.PageIndex = e.NewPageIndex;
            GridViewPatientAnonymised.DataSource = ViewState["GridViewPatientAnonymised"];
            GridViewPatientAnonymised.DataBind();
        }
        protected void GridViewPatientAnonymised_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ViewRecords"))
            {
                try
                {
                    string recordIDsString = e.CommandArgument.ToString();

                    List<long> recordIDs = recordIDsString.Split(',').Select(long.Parse).ToList();
                    List<Record> records = dataBLL.GetRecords(recordIDs);

                    ViewState["GridViewRecords"] = records;
                    GridViewRecords.DataSource = records;
                    GridViewRecords.DataBind();
                    UpdatePanelRecords.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Select Records Modal", "$('#modalRecords').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Records Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("ViewDiagnosis"))
            {
                try
                {
                    string recordIDsString = e.CommandArgument.ToString();

                    List<long> recordIDs = recordIDsString.Split(',').Select(long.Parse).ToList();
                    List<PatientDiagnosis> patientDiagnoses = dataBLL.GetPatientDiagnoses(recordIDs);

                    GridViewPatientDiagnoses.DataSource = patientDiagnoses;
                    GridViewPatientDiagnoses.DataBind();
                    UpdatePanelDiagnosisView.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Diagnosis Modal", "$('#modalDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error Opening Diagnosis Modal.');", true);
                }
            }
        }
        protected void ButtonFilter_ServerClick(object sender, EventArgs e)
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

                // Postal
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

                // Diagnosis
                DataTable diagnosesTable = dataBLL.GetDiagnoses();
                foreach (ListItem item in inputDiagnosis.Items)
                {
                    if (item.Selected)
                    {
                        if (diagnosesTable.AsEnumerable().Any(row => row.Field<string>("diagnosis_code").Equals(item.Value.Trim())))
                        {
                            filteredValues.diagnoses.Add(item.Value.Trim());
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

                // Record Diagnosis
                DataTable recordDiagnosesTable = dataBLL.GetRecordDiagnoses();
                foreach (ListItem item in inputRecordDiagnosis.Items)
                {
                    if (item.Selected)
                    {
                        if (recordDiagnosesTable.AsEnumerable().Any(row => row.Field<string>("diagnosis_code").Equals(item.Value.Trim())))
                        {
                            filteredValues.recordDiagnoses.Add(item.Value.Trim());
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
                PanelViewHeader.Visible = true;
                PanelView.Visible = true;
                UpdatePanelPatientAnonymised.Update();

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Data successfully displayed.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when displaying data.');", true);
            }
        }
        protected void ButtonDownload_Click(object sender, EventArgs e)
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

                // Postal
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

                // Diagnosis
                DataTable diagnosesTable = dataBLL.GetDiagnoses();
                foreach (ListItem item in inputDiagnosis.Items)
                {
                    if (item.Selected)
                    {
                        if (diagnosesTable.AsEnumerable().Any(row => row.Field<string>("diagnosis_code").Equals(item.Value.Trim())))
                        {
                            filteredValues.diagnoses.Add(item.Value.Trim());
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

                // Record Diagnosis
                DataTable recordDiagnosesTable = dataBLL.GetRecordDiagnoses();
                foreach (ListItem item in inputRecordDiagnosis.Items)
                {
                    if (item.Selected)
                    {
                        if (recordDiagnosesTable.AsEnumerable().Any(row => row.Field<string>("diagnosis_code").Equals(item.Value.Trim())))
                        {
                            filteredValues.recordDiagnoses.Add(item.Value.Trim());
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

                DataTable anonPatientsTable = dataBLL.GetPatientsForDownload(filteredValues);

                string delimiter = ",";

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                    {
                        for (int i = 0; i < anonPatientsTable.Columns.Count; i++)
                        {
                            streamWriter.Write(anonPatientsTable.Columns[i].ColumnName);
                            streamWriter.Write((i < anonPatientsTable.Columns.Count - 1) ? delimiter : Environment.NewLine);
                        }

                        foreach (DataRow row in anonPatientsTable.Rows)
                        {
                            for (int i = 0; i < anonPatientsTable.Columns.Count; i++)
                            {
                                if (row[i].ToString().IndexOf(",") > -1)
                                {
                                    streamWriter.Write("\"" + row[i].ToString() + "\"");
                                }
                                else
                                {
                                    streamWriter.Write(row[i].ToString());
                                }
                                streamWriter.Write((i < anonPatientsTable.Columns.Count - 1) ? delimiter : Environment.NewLine);
                            }
                        }
                    }

                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "text/csv";
                    Response.Charset = string.Empty;
                    Response.Cache.SetCacheability(HttpCacheability.Public);
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"anon_data.csv\"");
                    Response.BinaryWrite(memoryStream.ToArray());
                }

                ViewState["GridViewPatientAnonymised"] = null;
                GridViewPatientAnonymised.DataSource = null;
                GridViewPatientAnonymised.DataBind();
                PanelViewHeader.Visible = false;
                PanelView.Visible = false;
                UpdatePanelPatientAnonymised.Update();
                Response.Flush();
                Response.Close();

                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Data download has been successfully initiated.');", true);
            }
            catch
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when intiating data download.');", true);
            }
        }
        #endregion

        private MemoryStream GetStream(DataTable dt)
        {
            MemoryStream memoryStream = new MemoryStream();
            if (dt.Rows.Count > 0)
            {
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                foreach (DataColumn col in dt.Columns)
                {
                    streamWriter.Write(col.ColumnName.ToString() + ",");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (row[i].ToString().IndexOf(",") > -1)
                        {
                            streamWriter.Write("\"" + row[i].ToString() + "\"");
                        }
                        else
                        {
                            streamWriter.Write(row[i].ToString() + ",");
                        }
                    }
                    streamWriter.WriteLine();
                }
            }
            return memoryStream;
        }

        #region Patient Diagnosis Functions
        protected void GridViewPatientDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPatientDiagnoses.PageIndex = e.NewPageIndex;
            GridViewPatientDiagnoses.DataSource = ViewState["GridViewPatientDiagnoses"];
            GridViewPatientDiagnoses.DataBind();
        }
        #endregion

        #region Record Functions
        protected void GridViewRecords_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RecordType recordType = (RecordType)DataBinder.Eval(e.Row.DataItem, "type");

                if (recordType.isContent)
                {
                    Label LabelContent = (Label)e.Row.FindControl("LabelContent");
                    string content = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "content"));

                    LabelContent.Text = content + " " + recordType.prefix;
                    LabelContent.Visible = true;
                }
                else if (!recordType.isContent)
                {
                    LinkButton LinkbuttonFileView = (LinkButton)e.Row.FindControl("LinkbuttonFileView");
                    HtmlAnchor FileDownloadLink = (HtmlAnchor)e.Row.FindControl("FileDownloadLink");

                    LinkbuttonFileView.CommandName = "FileView";
                    LinkbuttonFileView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    LinkbuttonFileView.Visible = true;
                    LinkbuttonFileView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View "
                        + DataBinder.Eval(e.Row.DataItem, "fileType") +
                        "</span>";

                    FileDownloadLink.HRef = "~/Researcher/Download.ashx?record=" + DataBinder.Eval(e.Row.DataItem, "id").ToString();
                    FileDownloadLink.Visible = true;
                }

                LinkButton LinkButtonRecordDiagnosisView = (LinkButton)e.Row.FindControl("LinkButtonRecordDiagnosisView");
                LinkButtonRecordDiagnosisView.CommandName = "RecordDiagnosisView";
                LinkButtonRecordDiagnosisView.CommandArgument = DataBinder.Eval(e.Row.DataItem, "id").ToString();
                LinkButtonRecordDiagnosisView.Visible = true;
                LinkButtonRecordDiagnosisView.Text = "<i class=\"fas fa-fw fa-eye\"></i></i><span class=\"d-none d-lg-inline-block\">View</span>";
            }
        }
        protected void GridViewRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecords.PageIndex = e.NewPageIndex;
            GridViewRecords.DataSource = ViewState["GridViewRecords"];
            GridViewRecords.DataBind();
        }
        protected void GridViewRecords_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("FileView"))
            {
                try
                {
                    long recordID = Convert.ToInt64(e.CommandArgument);
                    Record record = dataBLL.GetRecord(recordID);

                    modalFileViewImage.Visible = false;
                    modalFileViewVideo.Visible = false;
                    modalFileViewPanelText.Visible = false;

                    labelRecordName.Text = record.title;
                    modalFileViewLabelFileName.Text = record.fileName + record.fileExtension;
                    modalFileViewLabelFileSize.Text = record.fileSizeMegabytes;
                    FileDownloadLinkviaModal.HRef = "~/Researcher/Download.ashx?record=" + record.id.ToString();

                    if (record.fileExtension == ".png" || record.fileExtension == ".jpg" || record.fileExtension == ".jpeg")
                    {
                        modalFileViewImage.Visible = true;
                        modalFileViewImage.ImageUrl = "~/Researcher/Download.ashx?record=" + record.id;

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').modal('show');", true);
                    }
                    else if (record.fileExtension == ".txt")
                    {
                        modalFileViewPanelText.Visible = true;
                        if (record.IsFileSafe())
                        {
                            string js = record.type.GetTextPlotJS(File.ReadAllText(record.fullpath));

                            ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').on('shown.bs.modal', function (e) {  " + js + "}); $('#modalFileView').modal('show');", true);
                        }
                    }
                    else if (record.fileExtension == ".mp4")
                    {
                        modalFileViewVideo.Visible = true;
                        modalFileViewVideoSource.Attributes.Add("src", "~/Researcher/Download.ashx?record=" + record.id);

                        ScriptManager.RegisterStartupScript(this, GetType(), "Open View File Modal", "$('#modalRecords').modal('hide'); $('#modalFileView').modal('show');", true);
                    }

                    UpdatePanelFileView.Update();
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View File Modal", "toastr['error']('Error Opening File Modal.');", true);
                }
            }
            else if (e.CommandName.Equals("RecordDiagnosisView"))
            {
                try
                {
                    long recordID = Convert.ToInt64(e.CommandArgument);

                    Record record = dataBLL.GetRecord(recordID);
                    labelRecordNameDiagnosis.Text = record.title;

                    List<RecordDiagnosis> recordDiagnoses = dataBLL.GetRecordDiagnoses(recordID);
                    ViewState["GridViewRecordDiagnoses"] = recordDiagnoses;
                    GridViewRecordDiagnoses.DataSource = recordDiagnoses;
                    GridViewRecordDiagnoses.DataBind();

                    UpdatePanelRecordDiagnosisView.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "Open View Record Diagnosis Modal", "$('#modalRecords').modal('hide'); $('#modalRecordDiagnosisView').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error Opening View Record Diagnosis Modal", "toastr['error']('Error Opening Record Diagnosis Modal.');", true);
                }
            }
        }

        protected void CloseModalFileView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View File Modal", " $('#modalFileView').modal('hide'); $('#modalRecords').modal('show');", true);
        }
        #endregion

        #region Record Diagnosis Functions
        protected void GridViewRecordDiagnoses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewRecordDiagnoses.PageIndex = e.NewPageIndex;
            GridViewRecordDiagnoses.DataSource = ViewState["GridViewRecordDiagnoses"];
            GridViewRecordDiagnoses.DataBind();
        }
        protected void CloseModalRecordDiagnosisView_ServerClick(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Close View Record Diagnosis Modal", " $('#modalRecordDiagnosisView').modal('hide'); $('#modalRecords').modal('show');", true);
        }
        #endregion
    }
}
