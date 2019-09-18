using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class Register : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminManageAccounts();
            Master.LiActiveAdminAccountRegistration();

            // set date min for date of birth
            inputDoB.Attributes.Add("max", DateTime.Now.ToString("yyyy-MM-dd"));

            if (!IsPostBack)
            {
                UpdatePanelRegistration.Update();
            }
        }

        protected void ButtonRegister_ServerClick(object sender, EventArgs e)
        {
            string nric = inputNRIC.Value.Trim().ToUpper();
            string password = inputPassword.Value.Trim();
            string passwordConfirm = inputPasswordConfirm.Value.Trim();
            string associatedTokenID = inputAssociatedTokenID.Value.Trim();
            string firstName = inputFirstName.Value.Trim().ToLower();
            string lastName = inputLastName.Value.Trim().ToLower();
            string countryOfBirth = inputCountryofBirth.Value.Trim();
            string nationality = inputNationality.Value.Trim();
            string sex = "";
            if (RadioButtonSexMale.Checked)
                sex = "Male";
            else if (RadioButtonSexFemale.Checked)
                sex = "Female";
            string gender = "Prefer not to say";
            if (RadioButtonGenderMale.Checked)
                gender = "Male";
            else if (RadioButtonGenderFemale.Checked)
                gender = "Female";
            else if (RadioButtonGenderTrans.Checked)
                gender = "Trans";
            else if (RadioButtonGenderOther.Checked)
                gender = "Other";
            string maritalStatus = "Widowed";
            if (RadioButtonMaritalStatusSingle.Checked)
                maritalStatus = "Single";
            if (RadioButtonMaritalStatusMarried.Checked)
                maritalStatus = "Married";
            if (RadioButtonMaritalStatusDivorced.Checked)
                maritalStatus = "Divorced";
            if (RadioButtonMaritalStatusWidowed.Checked)
                maritalStatus = "Married";
            string address = inputAddress.Value.Trim();
            string addressPostalCode = inputPostalCode.Value.Trim();
            string email = inputEmail.Value.Trim();
            string contactNumber = inputContactNumber.Value.Trim();
            string doB = inputDoB.Value.Trim();
            DateTime dateOfBirth = new DateTime();
            List<string> roles = new List<string>();
            if (CheckBoxRolePatient.Checked)
                roles.Add("Patient");
            if (CheckBoxRoleTherapist.Checked)
                roles.Add("Therapist");
            if (CheckBoxRoleResearcher.Checked)
                roles.Add("Researcher");
            if (CheckBoxRoleAdmin.Checked)
                roles.Add("Administrator");

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 12).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(nric) || accountBLL.IsRegistered(nric) 
                || !AccountBLL.IsNRICValid(nric))
            {
                validate[0] = false;
                inputNRIC.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputNRIC.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(firstName))
            {
                validate[1] = false;
                inputFirstName.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputFirstName.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(lastName))
            {
                validate[2] = false;
                inputLastName.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputLastName.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(countryOfBirth))
            {
                validate[3] = false;
                inputCountryofBirth.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputCountryofBirth.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(nationality))
            {
                validate[4] = false;
                inputNationality.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputNationality.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(address))
            {
                validate[5] = false;
                inputAddress.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputAddress.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(email) || !AccountBLL.IsEmailAddress(email))
            {
                validate[6] = false;
                inputEmail.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputEmail.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(contactNumber) || !AccountBLL.IsContactNumber(contactNumber))
            {
                validate[7] = false;
                inputContactNumber.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputContactNumber.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(doB) || !AccountBLL.IsDateOfBirthValid(doB, ref dateOfBirth))
            {
                validate[8] = false;
                inputDoB.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputDoB.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm) 
                || !AccountBLL.IsPassword(password, passwordConfirm))
            {
                validate[9] = false;
                inputPassword.Attributes.Add("class", "form-control is-invalid");
                inputPasswordConfirm.Attributes.Add("class", "form-control is-invalid");
            }
            else
            {
                inputPassword.Attributes.Add("class", "form-control is-valid");
                inputPasswordConfirm.Attributes.Add("class", "form-control is-valid");
            }

            if (!string.IsNullOrEmpty(associatedTokenID) && !AccountBLL.IsTokenValid(associatedTokenID))
            {
                validate[10] = false;
                inputAssociatedTokenID.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputAssociatedTokenID.Attributes.Add("class", "form-control is-valid");


            if (string.IsNullOrEmpty(addressPostalCode) || !AccountBLL.IsAddressPostalCode(addressPostalCode))
            {
                validate[11] = false;
                inputPostalCode.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputPostalCode.Attributes.Add("class", "form-control is-valid");

            #endregion

            if (validate.Contains(false))
            {
                inputPassword.Attributes.Add("class", "form-control is-invalid");
                if (!validate[9])
                    inputPasswordConfirm.Attributes.Add("class", "form-control");
                spanMessage.Visible = true;
            }
            else
            {
                spanMessage.Visible = false;

                try {
                    accountBLL.Register(nric, password, associatedTokenID, firstName, lastName, countryOfBirth, nationality, sex, gender,
                        maritalStatus, address, addressPostalCode, email, contactNumber, dateOfBirth, roles);
                    ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modelSuccess').modal('show')", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Registering an Account');", true);
                }
            }
        }

        protected void buttonRefresh_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}
