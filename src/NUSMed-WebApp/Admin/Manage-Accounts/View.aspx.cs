﻿using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin.Account
{
    public partial class View : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();
        //private List<Classes.Entity.Account> accounts;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminManageAccounts();
            Master.LiActiveAdminViewAccounts();

            if (!IsPostBack)
            {
                string term = TextboxSearch.Text.Trim().ToLower();
                Bind_GridViewAccounts(term);
            }
            else
            {
                GridViewAccounts.DataSource = null;
                GridViewAccounts.DataBind();
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
            if (string.Equals(nric, new AccountBLL().GetNRIC()))
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
            string term = TextboxSearch.Text.Trim().ToLower();

            // MFA
            if (e.CommandName.Equals("MFATokenIDEdit"))
            {
                string nric = e.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                TextBox textBoxUserID = (TextBox)row.FindControl("TextboxMFATokenIDEdit");

                try
                {
                    //accountBLL.StatusDisable(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('MFA Token ID of " + nric + " has been set accordingly.');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when setting MFA Token ID of " + nric + ".');", true);
                }
            }

            // Status Change
            if (e.CommandName.Equals("StatusDisable"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusDisable(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been set to \"Disabled\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when setting Account status of " + nric + " to \"Disabled\".');", true);
                }

            }
            else if (e.CommandName.Equals("StatusEnable"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusEnable(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been set to \"Enabled\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when setting Account status of " + nric + " to \"Enabled\".');", true);
                }
            }
            else if (e.CommandName.Equals("StatusEnableWoMFA"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.StatusEnableWithoutMFA(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Account Status of " + nric + " has been set to \"Enabled, Omitted from MFA\".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when setting Account status of " + nric + " to \"Enabled, Omitted from MFA\".');", true);
                }
            }

            // Role Enable
            else if (e.CommandName.Equals("RoleEnablePatient"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnablePatient(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Patient\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role of \"Patient\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableTherapist"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableTherapist(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Therapist\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role of \"Therapist\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableResearcher"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableResearcher(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Researcher\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role of \"Researcher\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleEnableAdmin"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleEnableAdmin(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Administrator\" has been Enabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Enabling Role of \"Administrator\" to " + nric + ".');", true);
                }
            }

            // Role Disable
            else if (e.CommandName.Equals("RoleDisablePatient"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisablePatient(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Patient\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role of \"Patient\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleDisableTherapist"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableTherapist(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Therapist\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role of \"Therapist\" to " + nric + ".');", true);
                }
            }
            else if (e.CommandName.Equals("RoleDisableResearcher"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableResearcher(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Researcher\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role of \"Researcher\" to " + nric + ".');", true);
                }

            }
            else if (e.CommandName.Equals("RoleDisableAdmin"))
            {
                string nric = e.CommandArgument.ToString();

                try
                {
                    accountBLL.RoleDisableAdmin(nric);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Role of \"Researcher\" has been Disabled for " + nric + ".');", true);
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Disabling Role of \"Researcher\" to " + nric + ".');", true);
                }

            }

            Bind_GridViewAccounts(term);
        }
        #endregion

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string term = TextboxSearch.Text.Trim().ToLower();

            Bind_GridViewAccounts(term);
        }

        protected void GridViewAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //string term = TextboxSearch.Text.Trim().ToLower();

            GridViewAccounts.PageIndex = e.NewPageIndex;
            GridViewAccounts.DataSource = ViewState["GridViewAccounts"];
            GridViewAccounts.DataBind();
        }
    }
}