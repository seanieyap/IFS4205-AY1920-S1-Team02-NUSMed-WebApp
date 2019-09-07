using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin.Account
{
    public partial class View : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminManageAccounts();
            Master.LiActiveAdminViewAccounts();

            if (!IsPostBack)
            {
                string term = TextboxSearch.Text.Trim().ToLower();
                Bind_GridViewAccounts(term);
            }
        }

        #region GridViewAccounts Functions
        protected void Bind_GridViewAccounts(string term)
        {
            List<Classes.Entity.Account> accounts = accountBLL.GetAllAccounts(term);
            ViewState["GridViewAccounts"] = accounts;
            GridViewAccounts.DataSource = accounts;
            GridViewAccounts.DataBind();
        }
        protected void GridViewAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string nric = GridViewAccounts.DataKeys[e.RowIndex].Values["nric"].ToString();

            #region Validation
            if (string.Equals(nric, accountBLL.GetNRIC()))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Unable to Delete the current Account \"" + nric + "\".')", true);
                return;
            }
            #endregion

            try
            {
                accountBLL.DeleteAccount(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account, \"" + nric + "\", was Deleted successfully');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Deleting an Account');", true);
            }

            GridViewAccounts.EditIndex = -1;

            string term = TextboxSearch.Text.Trim().ToLower();

            Bind_GridViewAccounts(term);
        }
        protected void GridViewAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ViewPersonal"))
            {
                string nric = e.CommandArgument.ToString();
                try
                {
                    Classes.Entity.Account account = accountBLL.GetPersonalInformation(nric);
                    labelPersonalNRIC.Text = nric;
                    inputPersonalNRIC.Value = account.nric;
                    inputPersonalDoB.Value = account.dateOfBirth.ToString("dd/MM/yyyy");
                    inputPersonalFirstName.Value = account.firstName;
                    inputPersonalLastName.Value = account.lastName;
                    inputPersonalCountryofBirth.Value = account.countryOfBirth;
                    inputPersonalNationality.Value = account.nationality;
                    inputPersonalSex.Value = account.sex;
                    inputPersonalGender.Value = account.gender;
                    inputPersonalMartialStatus.Value = account.martialStatus;

                    UpdatePanelPersonal.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Personal Modal", "$('#modalPersonal').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Personal Information of " + nric + ".');", true);
                }
            }
            if (e.CommandName.Equals("ViewContact"))
            {
                string nric = e.CommandArgument.ToString();
                try
                {
                    Classes.Entity.Account account = accountBLL.GetContactInformation(nric);
                    labelContactNRIC.Text = nric;
                    inputAddress.Value = account.address;
                    inputPostalCode.Value = account.addressPostalCode;
                    inputEmailAddress.Value = account.email;
                    inputContactNumber.Value = account.contactNumber;

                    UpdatePanelContact.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Contact Modal", "$('#modalContact').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Contact Information of " + nric + ".');", true);
                }
            }
            if (e.CommandName.Equals("ViewPatient"))
            {
                string nric = e.CommandArgument.ToString();
                try
                {
                    Classes.Entity.Account account = accountBLL.GetPatientInformation(nric);
                    // Just info
                    labelPatientNRIC.Text = nric;
                    inputPatientNokName.Value = account.nokName;
                    inputPatientNokContact.Value = account.nokContact;

                    //Emergency
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;
                    string therapistTerm = TextboxSearchTherapist.Text.Trim().ToLower();
                    Bind_GridViewTherapists(nric, therapistTerm);
                    Bind_GridViewTherapists2(nric);

                    UpdatePanelPatient.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Patient Modal", "$('#modalPatient').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Patient Information of " + nric + ".');", true);
                }
            }

            if (e.CommandName.Equals("ViewTherapist"))
            {
                string nric = e.CommandArgument.ToString();
                try
                {
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;
                    inputTherapistJobTitle.Attributes.Add("class", "form-control");
                    inputTherapistDepartment.Attributes.Add("class", "form-control");
                    spanMessageTherapistDetailsUpdate.Visible = false;
                    RefreshTherapistModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Therapist Modal", "$('#modalTherapist').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Therapist Information of " + nric + ".');", true);
                }
            }
            if (e.CommandName.Equals("ViewResearcher"))
            {
                string nric = e.CommandArgument.ToString();
                try
                {
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;
                    inputResearcherJobTitle.Attributes.Add("class", "form-control");
                    inputResearcherDepartment.Attributes.Add("class", "form-control");
                    spanMessageResearcherUpdate.Visible = false;
                    RefreshResearcherModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Researcher Modal", "$('#modalResearcher').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Researcher Information of " + nric + ".');", true);
                }
            }

            #region MFA
            if (e.CommandName.Equals("MFATokenIDUpdate"))
            {
                string[] ca = e.CommandArgument.ToString().Split(';');
                string nric = ca[0];
                GridViewRow row = GridViewAccounts.Rows[Convert.ToInt32(ca[1])];
                TextBox textBoxTokenID = row.FindControl("TextboxMFATokenIDUpdate") as TextBox;

                try
                {
                    accountBLL.MFATokenIDUpdate(nric, textBoxTokenID.Text.Trim());
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('MFA Token ID of " + nric + " has been set accordingly.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when setting MFA Token ID of " + nric + ".');", true);
                }
            }
            if (e.CommandName.Equals("MFADeviceIDUpdate"))
            {
                string[] ca = e.CommandArgument.ToString().Split(';');
                string nric = ca[0];
                GridViewRow row = GridViewAccounts.Rows[Convert.ToInt32(ca[1])];
                TextBox textBoxDeviceID = row.FindControl("TextboxMFADeviceIDUpdate") as TextBox;

                try
                {
                    accountBLL.MFADeviceIDUpdate(nric, textBoxDeviceID.Text.Trim());
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('MFA Device ID of " + nric + " has been set accordingly.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when setting MFA Device ID of " + nric + ".');", true);
                }
            }
            #endregion

            #region Status Update
            if (e.CommandName.Equals("StatusDisable"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusDisable(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Account Status of " + nric + " has been set to \"Disabled\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when setting Account status of " + nric + " to \"Disabled\".');", true);
                }

            }
            else if (e.CommandName.Equals("StatusEnable"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusEnable(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Account Status of " + nric + " has been set to \"Enabled\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when setting Account status of " + nric + " to \"Enabled\".');", true);
                }
            }
            else if (e.CommandName.Equals("StatusEnableWoMFA"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusEnableWithoutMFA(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Account Status of " + nric + " has been set to \"Enabled, Omitted from MFA\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals();  toastr['error']('Error occured when setting Account status of " + nric + " to \"Enabled, Omitted from MFA\".');", true);
                }
            }
            #endregion

            #region Role Update
            #region Role Enable
            else if (e.CommandName.Equals("RoleEnablePatient"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnablePatient(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Patient\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Enabling Role of \"Patient\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableTherapist"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableTherapist(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Therapist\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Enabling Role of \"Therapist\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableResearcher"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableResearcher(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Researcher\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Enabling Role of \"Researcher\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableAdmin"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableAdmin(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Administrator\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Enabling Role of \"Administrator\" to " + nric + ".');", true);
                }
            }
            #endregion

            #region Role Disable
            else if (e.CommandName.Equals("RoleDisablePatient"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisablePatient(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Patient\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Disabling Role of \"Patient\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleDisableTherapist"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableTherapist(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Therapist\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Disabling Role of \"Therapist\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleDisableResearcher"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableResearcher(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Researcher\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Disabling Role of \"Researcher\" to " + nric + ".');", true);
                }

            }
            else if (e.CommandName.Equals("RoleDisableAdmin"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableAdmin(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['success']('Role of \"Researcher\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "hideGridViewModals(); toastr['error']('Error occured when Disabling Role of \"Researcher\" to " + nric + ".');", true);
                }
            }
            #endregion
            #endregion

            string term = TextboxSearch.Text.Trim().ToLower();
            Bind_GridViewAccounts(term);
        }
        protected void GridViewAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAccounts.PageIndex = e.NewPageIndex;
            GridViewAccounts.DataSource = ViewState["GridViewAccounts"];
            GridViewAccounts.DataBind();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string term = TextboxSearch.Text.Trim().ToLower();
            Bind_GridViewAccounts(term);
        }
        #endregion

        #region GridViewTherapists
        protected void Bind_GridViewTherapists(string patientNRIC, string term)
        {
            List<Classes.Entity.Account> accounts = accountBLL.GetTherapists(patientNRIC, term);
            ViewState["GridViewTherapists"] = accounts;
            GridViewTherapists.DataSource = accounts;
            GridViewTherapists.DataBind();
        }
        protected void GridViewTherapists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddEmergencyTherapist"))
            {
                string patientNRIC = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                try
                {
                    string therapistNRIC = e.CommandArgument.ToString();

                    accountBLL.AddEmergencyTherapist(patientNRIC, therapistNRIC);
                    string therapistTerm = TextboxSearchTherapist.Text.Trim().ToLower();
                    Bind_GridViewTherapists(patientNRIC, therapistTerm);
                    Bind_GridViewTherapists2(patientNRIC);
                    UpdatePanelPatient.Update();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Emergency Therapist," + therapistNRIC + ", has been assigned to " + patientNRIC + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when assigning emergency Therapist to " + patientNRIC + ".');", true);
                }
            }
        }
        protected void GridViewTherapists_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewTherapists.PageIndex = e.NewPageIndex;
            GridViewTherapists.DataSource = ViewState["GridViewTherapists"];
            GridViewTherapists.DataBind();
            UpdatePanelPatient.Update();
        }
        protected void ButtonSearchTherapist_Click(object sender, EventArgs e)
        {
            string term = TextboxSearchTherapist.Text.Trim().ToLower();
            string patientNRIC = ViewState["GridViewAccountsSelectedNRIC"].ToString();
            Bind_GridViewTherapists(patientNRIC, term);
        }
        #endregion

        #region GridViewAllTherapists2
        protected void Bind_GridViewTherapists2(string nric)
        {
            List<Classes.Entity.Account> accounts = accountBLL.GetEmergencyTherapists(nric);
            ViewState["GridViewTherapists2"] = accounts;
            GridViewTherapists2.DataSource = accounts;
            GridViewTherapists2.DataBind();
        }
        protected void GridViewTherapists2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("RemoveEmergencyTherapist"))
            {
                string patientNRIC = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                try
                {
                    string therapistNRIC = e.CommandArgument.ToString();

                    accountBLL.RemoveEmergencyTherapist(therapistNRIC);
                    string therapistTerm = TextboxSearchTherapist.Text.Trim().ToLower();
                    Bind_GridViewTherapists(patientNRIC, therapistTerm);
                    Bind_GridViewTherapists2(patientNRIC);
                    UpdatePanelPatient.Update();

                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Emergency Therapist," + therapistNRIC + ", assigned to " + patientNRIC + " has been removed.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when removing emergency Therapist assigned to " + patientNRIC + ".');", true);
                }
            }
        }
        protected void GridViewTherapists2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewTherapists2.PageIndex = e.NewPageIndex;
            GridViewTherapists2.DataSource = ViewState["GridViewTherapists2"];
            GridViewTherapists2.DataBind();
            UpdatePanelPatient.Update();
        }
        #endregion

        protected void buttonTherapistUpdate_ServerClick(object sender, EventArgs e)
        {
            string jobTitle = inputTherapistJobTitle.Value.Trim();
            string department = inputTherapistDepartment.Value.Trim();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 2).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(jobTitle))
            {
                validate[0] = false;
                inputTherapistJobTitle.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputTherapistJobTitle.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(department))
            {
                validate[1] = false;
                inputTherapistDepartment.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputTherapistDepartment.Attributes.Add("class", "form-control is-valid");
            #endregion

            if (validate.Contains(false))
            {
                spanMessageTherapistDetailsUpdate.Visible = true;
            }
            else
            {
                spanMessageTherapistDetailsUpdate.Visible = false;

                try
                {
                    string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();
                    accountBLL.UpdateTherapistDetails(nric, jobTitle, department);
                    RefreshTherapistModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Therapist Details were successfully Updated');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Therapist Details');", true);
                }
            }
        }
        protected void buttonResearcherUpdate_ServerClick(object sender, EventArgs e)
        {
            string jobTitle = inputResearcherJobTitle.Value.Trim();
            string department = inputResearcherDepartment.Value.Trim();

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 2).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(jobTitle))
            {
                validate[0] = false;
                inputResearcherJobTitle.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputResearcherJobTitle.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(department))
            {
                validate[1] = false;
                inputResearcherDepartment.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputResearcherDepartment.Attributes.Add("class", "form-control is-valid");
            #endregion

            if (validate.Contains(false))
            {
                spanMessageResearcherUpdate.Visible = true;
            }
            else
            {
                spanMessageResearcherUpdate.Visible = false;

                try
                {
                    string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();
                    accountBLL.UpdateResearcherDetails(nric, jobTitle, department);
                    RefreshResearcherModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Researcher Details were successfully Updated');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Researcher Details');", true);
                }
            }
        }
        private void RefreshTherapistModal(string nric)
        {
            Classes.Entity.Account account = accountBLL.GetTherapistInformation(nric);
            labelTherapistNRIC.Text = nric;
            inputTherapistJobTitle.Value = account.therapistJobTitle;
            inputTherapistDepartment.Value = account.therapistDepartment;

            UpdatePanelTherapist.Update();
        }
        private void RefreshResearcherModal(string nric)
        {
            Classes.Entity.Account account = accountBLL.GetResearcherInformation(nric);
            labelResearcherNRIC.Text = nric;
            inputResearcherJobTitle.Value = account.researcherJobTitle;
            inputResearcherDepartment.Value = account.researcherDepartment;

            UpdatePanelResearcher.Update();
        }
    }
}