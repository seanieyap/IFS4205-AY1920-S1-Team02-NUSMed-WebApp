using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class My_Profile : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveMyProfile();

            if (!IsPostBack)
            {
                UpdatePanelMyProfile_Bind();
            }
        }
        private void UpdatePanelMyProfile_Bind()
        {
            Account account = accountBLL.GetAccount(AccountBLL.GetNRIC());

            // Personal Details
            nric.Value = account.nric;
            DateofBirth.Value = account.dateOfBirth.ToString("MM/dd/yyyy");
            FirstName.Value = account.firstName;
            LastName.Value = account.lastName;
            CountryofBirth.Value = account.countryOfBirth;
            Nationality.Value = account.nationality;
            Sex.Value = account.sex;
            Gender.Value = account.gender;
            MaritalStatus.Value = account.maritalStatus;

            // Contact Details
            Address.Value = account.address;
            PostalCode.Value = account.addressPostalCode;
            EmailAddress.Value = account.email;
            ContactNumber.Value = account.contactNumber;

            // Account Details
            LastFullLogin.Value = account.lastFullLogin.ToString();
            Registration.Value = account.createTime.ToString();

            if (AccountBLL.IsPatient())
            {
                NOKName.Value = account.nokName;
                NOKContact.Value = account.nokContact;
                HeaderPatient.Visible = true;
                DivPatient.Visible = true;
            }
            else if (AccountBLL.IsTherapist())
            {
                // Therapist Details
                TherapistJobTile.Value = account.therapistJobTitle;
                TherapistDepartment.Value = account.therapistDepartment;
                HeaderTherapist.Visible = true;
                DivTherapist.Visible = true;
            }
            else if (AccountBLL.IsResearcher())
            {
                // Therapist Details
                ResearcherJobTitle.Value = account.researcherJobTitle;
                ResearcherDepartment.Value = account.researcherDepartment;
                HeaderResearcher.Visible = true;
                DivResearcher.Visible = true;
            }
            UpdatePanelMyProfile.Update();
        }

        #region Contact Details
        protected void buttonContactDetailsEdit_ServerClick(object sender, EventArgs e)
        {
            spanMessagePatientDetailsUpdate.Visible = false;

            AddressEdit.Attributes.Add("class", "form-control form-control-sm");
            PostalCodeEdit.Attributes.Add("class", "form-control form-control-sm");
            EmailAddressEdit.Attributes.Add("class", "form-control form-control-sm");
            ContactNumberEdit.Attributes.Add("class", "form-control form-control-sm");

            Account account = accountBLL.GetAccount(AccountBLL.GetNRIC());
            AddressEdit.Value = account.address;
            PostalCodeEdit.Value = account.addressPostalCode;
            EmailAddressEdit.Value = account.email;
            ContactNumberEdit.Value = account.contactNumber;

            UpdatePanelMyProfile_Bind();
            ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modalContactDetails').modal('show')", true);
        }
        protected void buttonContactDetailsUpdate_ServerClick(object sender, EventArgs e)
        {
            string address = AddressEdit.Value.Trim();
            string addressPostalCode = PostalCodeEdit.Value.Trim();
            string email = EmailAddressEdit.Value.Trim();
            string contactNumber = ContactNumberEdit.Value.Trim();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 4).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(address))
            {
                validate[0] = false;
                AddressEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                AddressEdit.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(addressPostalCode) || !AccountBLL.IsAddressPostalCode(addressPostalCode))
            {
                validate[1] = false;
                PostalCodeEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                PostalCodeEdit.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(email) || !AccountBLL.IsEmailAddress(email))
            {
                validate[2] = false;
                EmailAddressEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                EmailAddressEdit.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(contactNumber) || !AccountBLL.IsContactNumber(contactNumber))
            {
                validate[3] = false;
                ContactNumberEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                ContactNumberEdit.Attributes.Add("class", "form-control form-control-sm is-valid");
            #endregion

            if (validate.Contains(false))
            {
                spanMessageContactDetailsUpdate.Visible = true;
            }
            else
            {
                spanMessageContactDetailsUpdate.Visible = false;

                try
                {
                    accountBLL.UpdateContactDetails(address, addressPostalCode, email, contactNumber);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Contact Details were successfully Updated');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Registering an Account');", true);
                }
            }
        }
        #endregion

        #region Patient Details
        protected void buttonPatientDetailsEdit_ServerClick(object sender, EventArgs e)
        {
            spanMessagePatientDetailsUpdate.Visible = false;

            NOKNameEdit.Attributes.Add("class", "form-control form-control-sm");
            NOKContactEdit.Attributes.Add("class", "form-control form-control-sm");

            Account account = accountBLL.GetPatientInformation(AccountBLL.GetNRIC());
            NOKNameEdit.Value = account.nokName;
            NOKContactEdit.Value = account.nokContact;

            ScriptManager.RegisterStartupScript(this, GetType(), "open modal", "$('#modalPatientDetails').modal('show')", true);
        }
        protected void buttonPatientDetailsUpdate_ServerClick(object sender, EventArgs e)
        {
            string nokName = NOKNameEdit.Value.Trim();
            string nokContact = NOKContactEdit.Value.Trim();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 2).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(nokName))
            {
                validate[0] = false;
                NOKNameEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                NOKNameEdit.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(nokContact) || !AccountBLL.IsContactNumber(nokContact))
            {
                validate[1] = false;
                NOKContactEdit.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                NOKContactEdit.Attributes.Add("class", "form-control form-control-sm is-valid");
            #endregion

            if (validate.Contains(false))
            {
                spanMessagePatientDetailsUpdate.Visible = true;
            }
            else
            {
                spanMessagePatientDetailsUpdate.Visible = false;

                try
                {
                    accountBLL.UpdatePatientDetails(nokName, nokContact);
                    UpdatePanelMyProfile_Bind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Next of Kin Details were successfully Updated');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Next of Kin Details');", true);
                }
            }
        }

        #endregion

    }
}