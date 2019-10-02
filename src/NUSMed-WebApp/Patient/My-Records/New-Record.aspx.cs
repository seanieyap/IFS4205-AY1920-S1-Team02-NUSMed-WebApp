using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient.My_Records
{
    public partial class New_Record : Page
    {
        private readonly RecordBLL recordBLL = new RecordBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyRecords();
            Master.LiActivePatientMyRecordNew();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            // Show success modal 
            if (Session["NewRecordSuccess"] != null)
            {
                if (string.Equals(Session["NewRecordSuccess"].ToString(), "success"))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modelSuccess').modal('show');", true);
                    Session.Remove("NewRecordSuccess");
                }
                else if (string.Equals(Session["NewRecordSuccess"].ToString(), "error"))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting a Record');", true);
                    Session.Remove("NewRecordSuccess");
                }
            }

            // store FileUpload object in Session to enable persistence between postbacks. 
            if (Session["inputFile"] == null && inputFile.HasFile)
            {
                Session["inputFile"] = inputFile;
            }
            else if (Session["inputFile"] != null && (!inputFile.HasFile))
            {
                inputFile = (FileUpload)Session["inputFile"];
            }
            else if (inputFile.HasFile)
            {
                Session["inputFile"] = inputFile;
            }

            if (!IsPostBack)
            {
                ResetPanel();
            }
        }

        protected void buttonSubmit_ServerClick(object sender, EventArgs e)
        {
            Record record = new Record();
            record.patientNRIC = AccountBLL.GetNRIC();
            record.title = inputTitle.Value.Trim();
            record.description = inputDescription.Value.Trim();
            record.content = string.Empty;
            record.type = GetSelectedType();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 3).ToArray();

            // If any fields are empty
            if (!record.IsTitleValid())
            {
                validate[0] = false;
                inputTitle.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputTitle.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (!record.IsDescriptionValid())
            {
                validate[1] = false;
                inputDescription.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputDescription.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (record.type.isContent)
            {
                record.content = inputContent.Value.Trim();

                if (!record.IsContentValid())
                {
                    validate[2] = false;
                    inputContent.Attributes.Add("class", "form-control form-control-sm is-invalid");
                }
                else
                {
                    inputContent.Attributes.Add("class", "form-control form-control-sm is-valid");
                }
            }
            else
            {
                inputContent.Attributes.Add("class", "form-control form-control-sm is-invalid");

                record.fileName = Path.GetFileNameWithoutExtension(inputFile.FileName);
                record.fileExtension = Path.GetExtension(inputFile.FileName);
                record.fileSize = inputFile.PostedFile.ContentLength;

                //record.fileMimeType = inputFile.PostedFile.ContentType;

                if (!inputFile.HasFile)
                {
                    validate[2] = false;
                    LabelFileError.Visible = true;
                    LabelFileError.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>No file chosen.";
                }
                else if (!record.IsFileValid())
                {
                    validate[2] = false;
                    LabelFileError.Visible = true;
                    LabelFileError.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>Chosen file is of incorrect format for this type of record.";
                }
                else
                {
                    LabelFileError.Visible = false;
                }
            }

            #endregion

            if (validate.Contains(false))
            {
                spanMessage.Visible = true;
            }
            else
            {
                spanMessage.Visible = false;

                try
                {
                    if (!record.type.isContent)
                    {
                        record.createTime = DateTime.Now;

                        Directory.CreateDirectory(record.GetFileServerPath() + "\\" + record.GetFileDirectoryNameHash());

                        inputFile.SaveAs(record.fullpath);
                    }

                    recordBLL.AddRecord(record);

                    Session["NewRecordSuccess"] = "success";
                }
                catch
            {
                Session["NewRecordSuccess"] = "error";
            }
            Response.Redirect(Request.RawUrl);
            }
        }

        protected void RadioButtonType_CheckedChanged(object sender, EventArgs e)
        {
            bool IsContent = false;

            if (RadioButtonTypeHeightMeasurement.Checked)
            {
                IsContent = true;
                LabelContent.Text = "Height Measurement";
                LabelContentHelper.Text = "(Format: Centimetre, cm. Values: 0 - 280)";
                inputContent.Attributes.Add("placeholder", "cm");
            }
            else if (RadioButtonTypeWeightMeasurement.Checked)
            {
                IsContent = true;
                LabelContent.Text = "Weight Measurement";
                LabelContentHelper.Text = "(Format: Kilogram, KG. Values: 0 - 650)";
                inputContent.Attributes.Add("placeholder", "kg");
            }
            else if (RadioButtonTypeTemperatureReading.Checked)
            {
                IsContent = true;
                LabelContent.Text = "Temperature Reading";
                LabelContentHelper.Text = "(Format: Degree Celsius. Values: 0 - 100 °C)";
                inputContent.Attributes.Add("placeholder", "°C");
            }
            else if (RadioButtonTypeBloodPressureReading.Checked)
            {
                IsContent = true;
                LabelContent.Text = "Blood Pressure Reading";
                LabelContentHelper.Text = "(Format: Systolic Pressure (mmHG) over Diastolic Pressure (mmHG). Values: 0 - 250 / 0 - 250)";
                inputContent.Attributes.Add("placeholder", "Systolic Pressure (mmHG) / Diastolic Pressure (mmHG)");
            }
            else if (RadioButtonTypeECGReading.Checked)
            {
                IsContent = false;
                LabelFile.Text = "ECG Reading";
                LabelFileHelper.Text = "(File type: Timeseries, Format: .txt)";
                inputFile.Attributes.Add("accept", ".txt");
            }
            else if (RadioButtonTypeMRI.Checked)
            {
                IsContent = false;
                LabelFile.Text = "MRI";
                LabelFileHelper.Text = "(File type: Image. Accepted formats: .jpg, .jpeg, .png)";
                inputFile.Attributes.Add("accept", ".jpg, .jpeg, .png");
            }
            else if (RadioButtonTypeXRay.Checked)
            {
                IsContent = false;
                LabelFile.Text = "X-Ray";
                LabelFileHelper.Text = "(File type: Image. Accepted formats: .jpg, .jpeg, .png)";
                inputFile.Attributes.Add("accept", ".jpg, .jpeg, .png");
            }
            else if (RadioButtonTypeGait.Checked)
            {
                IsContent = false;
                LabelFile.Text = "Gait";
                LabelFileHelper.Text = "(File type: Timeseries or Movie. Accepted formats: .txt, .mp4)";
                inputFile.Attributes.Add("accept", ".txt, .mp4");
            }

            if (IsContent)
            {
                PanelContent.Visible = true;
                PanelFile.Visible = false;
            }
            else
            {
                PanelContent.Visible = false;
                PanelFile.Visible = true;
            }
            UpdatePanelNewRecord.Update();
        }

        private void ResetPanel()
        {
            RadioButtonTypeHeightMeasurement.Checked = true;
            RadioButtonTypeWeightMeasurement.Checked = false;
            RadioButtonTypeTemperatureReading.Checked = false;
            RadioButtonTypeBloodPressureReading.Checked = false;
            RadioButtonTypeECGReading.Checked = false;
            RadioButtonTypeMRI.Checked = false;
            RadioButtonTypeXRay.Checked = false;
            RadioButtonTypeGait.Checked = false;

            ScriptManager.RegisterStartupScript(this, GetType(), "Reset Panels", "document.forms[0].reset();", true);
        }

        protected void buttonRefresh_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        private RecordType GetSelectedType()
        {
            if (RadioButtonTypeHeightMeasurement.Checked == true)
                return new HeightMeasurement();
            else if (RadioButtonTypeWeightMeasurement.Checked == true)
                return new WeightMeasurement();
            else if (RadioButtonTypeTemperatureReading.Checked == true)
                return new TemperatureReading();
            else if (RadioButtonTypeBloodPressureReading.Checked == true)
                return new BloodPressureReading();
            else if (RadioButtonTypeECGReading.Checked == true)
                return new ECGReading();
            else if (RadioButtonTypeMRI.Checked == true)
                return new MRI();
            else if (RadioButtonTypeXRay.Checked == true)
                return new XRay();

            return new Gait();
        }
    }
}