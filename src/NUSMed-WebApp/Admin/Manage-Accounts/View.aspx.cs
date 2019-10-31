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
        //protected void GridViewAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    string nric = GridViewAccounts.DataKeys[e.RowIndex].Values["nric"].ToString();

        //    try
        //    {
        //        accountBLL.DeleteAccount(nric);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account, \"" + nric + "\", was Deleted successfully');", true);
        //    }
        //    catch
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Deleting an Account');", true);
        //    }

        //    GridViewAccounts.EditIndex = -1;

        //    string term = TextboxSearch.Text.Trim().ToLower();

        //    Bind_GridViewAccounts(term);
        //}
        protected void GridViewAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string nric = e.CommandArgument.ToString();

            if (e.CommandName.Equals("ViewPersonal"))
            {
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
                    inputPersonalMaritalStatus.Value = account.maritalStatus;

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
                try
                {
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;
                    inputTherapistJobTitle.Attributes.Add("class", "form-control form-control-sm");
                    inputTherapistDepartment.Attributes.Add("class", "form-control form-control-sm");
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
                try
                {
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;
                    inputResearcherJobTitle.Attributes.Add("class", "form-control form-control-sm");
                    inputResearcherDepartment.Attributes.Add("class", "form-control form-control-sm");
                    spanMessageResearcherUpdate.Visible = false;
                    RefreshResearcherModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Researcher Modal", "$('#modalResearcher').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Researcher Information of " + nric + ".');", true);
                }
            }
            if (e.CommandName.Equals("ViewStatus"))
            {
                try
                {
                    ViewState["GridViewAccountsSelectedNRIC"] = nric;

                    RefreshStatusModal(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Open Status Modal", "$('#modalStatus').modal('show');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error viewing Status Information of " + nric + ".');", true);
                }
            }

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
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Emergency Therapist, " + therapistNRIC + ", has been assigned to " + patientNRIC + ".');", true);
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

                    accountBLL.RemoveEmergencyTherapist(patientNRIC, therapistNRIC);
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
                inputTherapistJobTitle.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputTherapistJobTitle.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(department))
            {
                validate[1] = false;
                inputTherapistDepartment.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputTherapistDepartment.Attributes.Add("class", "form-control form-control-sm is-valid");
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
        private void RefreshTherapistModal(string nric)
        {
            Classes.Entity.Account account = accountBLL.GetTherapistInformation(nric);
            labelTherapistNRIC.Text = nric;
            inputTherapistJobTitle.Value = account.therapistJobTitle;
            inputTherapistDepartment.Value = account.therapistDepartment;

            UpdatePanelTherapist.Update();
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
                inputResearcherJobTitle.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputResearcherJobTitle.Attributes.Add("class", "form-control form-control-sm is-valid");

            if (string.IsNullOrEmpty(department))
            {
                validate[1] = false;
                inputResearcherDepartment.Attributes.Add("class", "form-control form-control-sm is-invalid");
            }
            else
                inputResearcherDepartment.Attributes.Add("class", "form-control form-control-sm is-valid");
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Researcher Details of " + nric + " were successfully Updated');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Researcher Details');", true);
                }
            }
        }
        private void RefreshResearcherModal(string nric)
        {
            Classes.Entity.Account account = accountBLL.GetResearcherInformation(nric);
            labelResearcherNRIC.Text = nric;
            inputResearcherJobTitle.Value = account.researcherJobTitle;
            inputResearcherDepartment.Value = account.researcherDepartment;

            UpdatePanelResearcher.Update();
        }

        protected void LinkButtonStatusDisable_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.StatusDisable(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been updated to \"Disabled\".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when updating Account status.');", true);
            }
        }
        protected void LinkButtonStatusEnable_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.StatusEnable(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been updated to \"Enabled\".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when updating Account status.');", true);
            }
        }
        //TODO: For Dev Only
        //protected void LinkButtonStatusEnableWoMFA_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

        //        accountBLL.StatusEnableWithoutMFA(nric);
        //        RefreshStatusModal(nric);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been updated to \"Enabled, Omitted from MFA\".');", true);
        //    }
        //    catch
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when updating Account status.');", true);
        //    }
        //}
        protected void LinkButtonTokenIDUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.MFATokenIDUpdate(nric, TextboxMFATokenIDUpdate.Text.Trim());
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('MFA Token ID of " + nric + " has been set accordingly.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when updating MFA Token ID.');", true);
            }

        }
        protected void LinkButtonDeviceIDUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.MFADeviceIDUpdate(nric, TextboxMFADeviceIDUpdate.Text.Trim());
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('MFA Device ID of " + nric + " has been set accordingly.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when updating MFA Device ID.');", true);
            }
        }
        protected void LinkButtonRoleEnablePatient_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleEnablePatient(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Patient\" has been Enabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role.');", true);
            }
        }
        protected void LinkButtonRoleEnableTherapist_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleEnableTherapist(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Therapist\" has been Enabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role.');", true);
            }
        }
        protected void LinkButtonRoleEnableResearcher_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleEnableResearcher(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Researcher\" has been Enabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role.');", true);
            }
        }
        protected void LinkButtonRoleRoleEnableAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleEnableAdmin(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Administrator\" has been Enabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role.');", true);
            }
        }
        protected void LinkButtonRoleDisablePatient_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleDisablePatient(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Patient\" has been Disabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role.');", true);
            }
        }
        protected void LinkButtonRoleDisableTherapist_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleDisableTherapist(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Therapist\" has been Disabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role.');", true);
            }
        }
        protected void LinkButtonRoleDisableResearcher_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleDisableResearcher(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Researcher\" has been Disabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role.');", true);
            }
        }
        protected void LinkButtonRoleRoleDisableAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string nric = ViewState["GridViewAccountsSelectedNRIC"].ToString();

                accountBLL.RoleDisableAdmin(nric);
                RefreshStatusModal(nric);
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Administrator\" has been Disabled for " + nric + ".');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role.');", true);
            }
        }
        private void RefreshStatusModal(string nric)
        {
            Classes.Entity.Account account = accountBLL.GetStatusInformation(nric);
            labelStatusNRIC.Text = nric;
            inputStatusCreateTime.Value = account.createTime.ToString();
            inputStatusLastLogin.Value = account.lastFullLogin.ToString();

            if (account.status == 0)
            {
                LinkButtonStatusDisable.CssClass = ("btn disabled");
                LinkButtonStatusEnable.CssClass = ("btn btn-success");
                //TODO: For Dev Only
                //LinkButtonStatusEnableWoMFA.CssClass = ("btn btn-warning");
            }
            else if (account.status == 1)
            {
                LinkButtonStatusDisable.CssClass = ("btn btn-danger");
                LinkButtonStatusEnable.CssClass = ("btn disabled");
                //TODO: For Dev Only
                //LinkButtonStatusEnableWoMFA.CssClass = ("btn btn-warning");
            }
            //else if (account.status == 2)
            //{
            //    LinkButtonStatusDisable.CssClass = ("btn btn-danger");
            //    LinkButtonStatusEnable.CssClass = ("btn btn-success");
            //    //TODO: For Dev Only
            //    LinkButtonStatusEnableWoMFA.CssClass = ("btn disabled");
            //}
            LabelMFATokenStatus.Text = account.MFATokenStatus;
            LabelMFADeviceStatus.Text = account.MFADeviceStatus;
            TextboxMFATokenIDUpdate.Text = string.Empty;
            TextboxMFADeviceIDUpdate.Text = string.Empty;
            if (account.patientStatus == 0)
            {
                LabelRolePatient.Text = "Disabled";
                LinkButtonRolePatientDisable.CssClass = "btn btn-sm btn-danger float-right disabled";
                LinkButtonRolePatientEnable.CssClass = "btn btn-sm btn-success float-right mr-2";
            }
            else if (account.patientStatus == 1)
            {
                LabelRolePatient.Text = "Enabled";
                LinkButtonRolePatientDisable.CssClass = "btn btn-sm btn-danger float-right";
                LinkButtonRolePatientEnable.CssClass = "btn btn-sm btn-success float-right disabled mr-2";
            }
            if (account.therapistStatus == 0)
            {
                LabelRoleTherapist.Text = "Disabled";
                LinkButtonRoleTherapistDisable.CssClass = "btn btn-sm btn-danger float-right disabled";
                LinkButtonRoleTherapistEnable.CssClass = "btn btn-sm btn-success float-right mr-2";
            }
            else if (account.therapistStatus == 1)
            {
                LabelRoleTherapist.Text = "Enabled";
                LinkButtonRoleTherapistDisable.CssClass = "btn btn-sm btn-danger float-right";
                LinkButtonRoleTherapistEnable.CssClass = "btn btn-sm btn-success float-right disabled mr-2";
            }
            if (account.researcherStatus == 0)
            {
                LabelRoleResearcher.Text = "Disabled";
                LinkButtonRoleResearcherDisable.CssClass = "btn btn-sm btn-danger float-right disabled";
                LinkButtonRoleResearcherEnable.CssClass = "btn btn-sm btn-success float-right mr-2";
            }
            else if (account.researcherStatus == 1)
            {
                LabelRoleResearcher.Text = "Enabled";
                LinkButtonRoleResearcherDisable.CssClass = "btn btn-sm btn-danger float-right";
                LinkButtonRoleResearcherEnable.CssClass = "btn btn-sm btn-success float-right disabled mr-2";
            }
            if (account.adminStatus == 0)
            {
                LabelRoleAdmin.Text = "Disabled";
                LinkButtonRoleAdminDisable.CssClass = "btn btn-sm btn-danger float-right disabled";
                LinkButtonRoleAdminEnable.CssClass = "btn btn-sm btn-success float-right mr-2";
            }
            else if (account.adminStatus == 1)
            {
                LabelRoleAdmin.Text = "Enabled";
                LinkButtonRoleAdminDisable.CssClass = "btn btn-sm btn-danger float-right";
                LinkButtonRoleAdminEnable.CssClass = "btn btn-sm btn-success float-right disabled mr-2";
            }

            UpdatePanelStatus.Update();
        }
    }
}