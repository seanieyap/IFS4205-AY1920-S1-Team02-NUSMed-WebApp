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

            if (PanelContent.Visible == true)
            {
                record.content = Server.HtmlEncode(inputMethodContent.Text.Trim());
            }
            else
            {
                // Reset the other error message
                //LabelMethodListDefault();

                //if (!inputMethodFile.HasFile)
                //{
                //    inputMethodFile.CssClass = "custom-file-input is-invalid";
                //    LabelMethodFile.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>Please choose a file.";
                //    LabelMethodFile.CssClass = "invalid-feedback";
                //    //ShowError("There are Errors detected in the form.");
                //    return;
                //}

                // If CSV
                //string fileExtension = Path.GetExtension(inputMethodFile.PostedFile.FileName).Substring(1);

                //if (fileExtension == "csv" || fileExtension == ".txt")
                //{
                //    StreamReader csvreader = new StreamReader(inputMethodFile.FileContent);

                //while (!csvreader.EndOfStream)
                //{
                //    string line = csvreader.ReadLine();
                //    List<string> words = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(word => word.Trim()).ToList();

                //    foreach (string word in new HashSet<string>(words))
                //    {
                //        dataBLL.SelectWord(word);
                //    }
                //}
                //}
            }

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
                if (string.IsNullOrEmpty(record.content))
                {
                    validate[2] = false;
                    inputMethodContent.CssClass = "form-control is-invalid";
                    LabelMethodContent.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>Please Enter some Content.";
                    LabelMethodContent.CssClass = "invalid-feedback";
                }
                else
                {
                    LabelMethodFileDefault();
                }
            }
            else
            {
                if (!inputMethodFile.HasFile)
                {
                    validate[2] = false;
                    inputMethodFile.CssClass = "custom-file-input is-invalid";
                    LabelMethodFile.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>Please choose a file.";
                    LabelMethodFile.CssClass = "invalid-feedback";
                }
                else
                {
                    LabelMethodListDefault();
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

            if (RadioButtonType01.Checked)
            {
                IsContent = true;
            }
            else if (RadioButtonType02.Checked)
            {
                IsContent = true;
            }
            else if (RadioButtonType03.Checked)
            {
                IsContent = true;
            }
            else if (RadioButtonType04.Checked)
            {
                IsContent = true;
            }
            else if (RadioButtonType05.Checked)
            {
                IsContent = true;
            }
            else if (RadioButtonType06.Checked)
            {
                IsContent = false;
            }
            else if (RadioButtonType07.Checked)
            {
                IsContent = false;
            }
            else if (RadioButtonType08.Checked)
            {
                IsContent = false;
            }
            else if (RadioButtonType09.Checked)
            {
                IsContent = false;
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
            RadioButtonType01.Checked = true;
            RadioButtonType02.Checked = false;
            RadioButtonType03.Checked = false;
            RadioButtonType04.Checked = false;
            RadioButtonType05.Checked = false;
            RadioButtonType06.Checked = false;
            RadioButtonType07.Checked = false;
            RadioButtonType08.Checked = false;
            RadioButtonType09.Checked = false;
            ScriptManager.RegisterStartupScript(this, GetType(), "Reset Panels", "document.forms[0].reset();", true);
        }
        private RecordType GetSelectedType()
        {
            if (RadioButtonType01.Checked == true)
            {
                return new MedicalNote();
            }
            else if (RadioButtonType02.Checked == true)
            {
                return new HeightMeasurement();
            }
            else if (RadioButtonType03.Checked == true)
            {
                return new WeightMeasurement();
            }
            else if (RadioButtonType04.Checked == true)
            {
                return new TemperatureReading();
            }
            else if (RadioButtonType05.Checked == true)
            {
                return new BloodPressureReading();
            }
            else if (RadioButtonType06.Checked == true)
            {
                return new ECGReading();
            }
            else if (RadioButtonType07.Checked == true)
            {
                return new MRI();
            }
            else if (RadioButtonType08.Checked == true)
            {
                return new XRay();
            }
            //else if (RadioButtonType09.Checked == true)
            //{
            //    return new Gait();
            //}

            return new Gait();
        }

        protected void buttonRefresh_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        #region UI

        private void LabelMethodFileDefault()
        {
            LabelMethodFile.Text = "Note: Words should be seperated by commas.";
            LabelMethodFile.CssClass = "small text-muted";
            inputMethodFile.CssClass = "custom-file-input";
        }

        private void LabelMethodListDefault()
        {
            inputMethodContent.Text = "Note: Words should be seperated by line-breaks or spaces.";
            LabelMethodContent.CssClass = "small text-muted";
            LabelMethodContent.CssClass = "form-control";
        }

        //private void ShowError(string text)
        //{
        //    PanelSecondaryInstructions.CssClass = "alert alert-danger small d-block py-2 px-2 mb-0 mt-1 align-bottom";
        //    LabelSecondaryInstructions.Text = "<i class=\"fas fa-fw fa-exclamation-circle\"></i>" + text;
        //}

        #endregion
    }
}