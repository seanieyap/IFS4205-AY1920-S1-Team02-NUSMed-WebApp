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
        RecordBLL recordBLL = new RecordBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyRecords();
            Master.LiActivePatientMyRecordNew();

            if (!IsPostBack)
            {
                ResetPanel();
            }
        }

        protected void buttonSubmit_ServerClick(object sender, EventArgs e)
        {
            Record record = new Record();
            record.title = Server.HtmlEncode(inputTitle.Value.Trim());
            record.description = Server.HtmlEncode(inputDescription.Value.Trim());
            record.content = string.Empty;
            record.type = GetSelectedType();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 3).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(record.title))
            {
                validate[0] = false;
                inputTitle.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputTitle.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(record.description))
            {
                validate[1] = false;
                inputDescription.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputDescription.Attributes.Add("class", "form-control is-valid");

            if (record.type.isContent)
            {
                record.content = inputContent.Value.Trim();

                if (!record.IsContentValid())
                {
                    validate[2] = false;
                    inputContent.Attributes.Add("class", "form-control is-invalid");
                }
            }
            else
            {
                if (inputFile.HasFile)
                {
                    // validate extension via file extension and size
                    // no need to care for directory traversal due to server hardening
                    record.extension = Path.GetExtension(inputFile.PostedFile.FileName).Substring(1);
                    if (record.IsFileValid())
                    {
                        validate[2] = false;

                    }
                }
                else
                {
                    validate[2] = false;
                    inputFile.CssClass = "custom-file-input is-invalid";
                    LabelFileHelper.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>Please choose a file.";
                    LabelFileHelper.CssClass = "invalid-feedback";
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
                    recordBLL.SubmitRecord(record);
                    ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modelSuccess').modal('show')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Submitting a Record');", true);
                }
            }
        }

        protected void RadioButtonType_CheckedChanged(object sender, EventArgs e)
        {
            SwitchPanel();
        }

        private void SwitchPanel()
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
                inputContent.Attributes.Add("placeholder", ".txt, .jpeg, .png");
            }
            else if (RadioButtonTypeXRay.Checked)
            {
                IsContent = false;
                LabelFile.Text = "X-Ray";
                LabelFileHelper.Text = "(File type: Image. Accepted formats: .jpg, .jpeg, .png)";
                inputContent.Attributes.Add("placeholder", ".txt, .jpeg, .png");
            }
            else if (RadioButtonTypeGait.Checked)
            {
                IsContent = false;
                LabelFile.Text = "Gait";
                LabelFileHelper.Text = "(File type: Timeseries or Movie. Accepted formats: .txt, .mp4)";
                inputContent.Attributes.Add("placeholder", ".txt, .mp4");
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

            // TODO reset to Height Measurement details
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

        #region UI

        private void LabelMethodFileDefault()
        {
            LabelFileHelper.Text = "Note: Words should be seperated by commas.";
            LabelFileHelper.CssClass = "small text-muted";
            inputFile.CssClass = "custom-file-input";
        }

        private void LabelMethodListDefault()
        {
            inputContent.Value = string.Empty;
        }

        //private void ShowError(string text)
        //{
        //    PanelSecondaryInstructions.CssClass = "alert alert-danger small d-block py-2 px-2 mb-0 mt-1 align-bottom";
        //    LabelSecondaryInstructions.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>" + text;
        //}

        #endregion
    }
}