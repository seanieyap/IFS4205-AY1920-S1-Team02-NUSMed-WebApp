﻿using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Web.Security;
using System.Collections.Specialized;

namespace NUSMed_WebApp
{
    public partial class _Default : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveHome();

            if (AccountBLL.IsAuthenticated())
            {
                if (AccountBLL.IsMultiple())
                {
                    Response.Redirect("~/Role-Selection");
                }
                else if (AccountBLL.IsPatient())
                {
                    Response.Redirect("~/Patient/Dashboard");
                }
                else if (AccountBLL.IsTherapist())
                {
                    Response.Redirect("~/Therapist/Dashboard");
                }
                else if (AccountBLL.IsResearcher())
                {
                    Response.Redirect("~/Researcher/Dashboard");
                }
                else if (AccountBLL.IsAdministrator())
                {
                    Response.Redirect("~/Admin/Dashboard");
                }
            }
            else if (Request.QueryString["fail-mfa"] != null && Request.QueryString["fail-mfa"].ToString().Equals("true"))
            {
                MFAFailModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "fail-mfa", "$('#MFAFailModal').modal('show');", true);
            }
            else if (Request.QueryString["fail-auth"] != null && Request.QueryString["fail-auth"].ToString().Equals("true"))
            {
                failAuthModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "fail-auth", "$('#failAuthModal').modal('show');", true);
            }
            else if (Request.QueryString["multiple-logins"] != null && Request.QueryString["multiple-logins"].ToString().Equals("true"))
            {

                multipleLoginsModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "multiple-logins", "$('#multipleLoginsModal').modal('show');", true);
            }
        }

        protected void ButtonLogin_ServerClick(object sender, EventArgs e)
        {
            string nric = inputNRIC.Value.Trim().ToUpper();
            string password = inputPassword.Value;

            if (!string.IsNullOrEmpty(nric) && !string.IsNullOrEmpty(password.ToString()))
            {
                Account account = accountBLL.GetStatus(nric, password);

                if (account.status == 0)
                {
                    AuthenticationError();
                }
                else
                {
                    // Remove attemps counter
                    HttpContext.Current.Cache.Remove(nric + "_LoginAttempt");

                    // Trigger MFA
                    if (account.status == 1)
                    {
                        try
                        {
                            HttpContext.Current.Cache.Insert(nric + "_MFAAttempt", "Awaiting", null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                            HttpContext.Current.Cache.Insert(nric + "_MFAAttempt_Password", password, null, DateTime.Now.AddSeconds(30), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                            Session["nric_MFAAttempt"] = nric;
                            Session["countdown"] = 30;
                            TimerMFA.Enabled = true;
                            ScriptManager.RegisterStartupScript(this, GetType(), "MFA", "$('#modalMFA').modal('show');", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when attempting to launch MFA');", true);
                        }
                    }

                    //TODO: For Dev Only
                    // If omitted from mfa
                    else if (account.status == 2)
                    {
                        try
                        {
                            // Check roles
                            int numberOfRoles = account.roles.Count();
                            if (numberOfRoles == 0)
                            {
                                NoRoleModal.Visible = true;
                                ScriptManager.RegisterStartupScript(this, GetType(), "No roles", "$('#NoRoleModal').modal('show');", true);
                                return;
                            }
                            else if (numberOfRoles == 1)
                            {
                                accountBLL.Login(nric, account.roles[0]);
                                Session["toastr"] = "login";
                            }
                            else
                            {
                                accountBLL.Login(nric, "Multiple");
                            }
                            Response.Redirect("~/");
                        }
                        catch
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when attempting to Login');", true);
                        }
                    }
                }
            }
            else
            {
                AuthenticationError();
            }
        }
        protected void TimerMFA_Tick(object sender, EventArgs e)
        {
            if (Session["nric_MFAAttempt"] == null)
            {
                TimerMFA.Enabled = false;
                Session.Abandon();
                FormsAuthentication.RedirectToLoginPage("fail-mfa=true");
                return;
            }

            if (HttpContext.Current.Cache.Get(Session["nric_MFAAttempt"].ToString() + "_MFAAttempt") == null)
            {
                LabelTimer.Text = "<i class=\"fas fa-8x fa-times-circle\"></i>";
                LabelTimer.CssClass = "nus-orange";
                LabelSeconds.Visible = false;
                pSubMessage.InnerText = "30 Seconds have passed. Please Try Again from the start.";
                TimerMFA.Enabled = false;
                return;
            }

            string nric = Session["nric_MFAAttempt"].ToString();

            if (!HttpContext.Current.Cache.Get(nric + "_MFAAttempt").ToString().Equals("Awaiting")
                && HttpContext.Current.Cache.Get(nric + "_MFAAttempt_Password") != null)
            {
                TimerMFA.Enabled = false;

                try
                {
                    string password = HttpContext.Current.Cache.Get(nric + "_MFAAttempt_Password").ToString();
                    Account account = (Account)HttpContext.Current.Cache.Get(nric + "_MFAAttempt");
                    account = accountBLL.GetStatus(nric, password, account.associatedDeviceID, account.associatedTokenID);

                    if (account.status != 1)
                    {
                        HttpContext.Current.Cache.Remove(nric + "_MFAAttempt");
                        HttpContext.Current.Cache.Remove(nric + "_MFAAttempt_Password");
                        Session.Abandon();
                        FormsAuthentication.RedirectToLoginPage("fail-mfa=true");
                        return;
                    }
                    else
                    {
                        try
                        {
                            // Check roles
                            int numberOfRoles = account.roles.Count();
                            if (numberOfRoles == 0)
                            {
                                NoRoleModal.Visible = true;
                                ScriptManager.RegisterStartupScript(this, GetType(), "No roles", "$('.modal').modal('hide'); $('#NoRoleModal').modal('show');", true);
                                return;
                            }
                            else if (numberOfRoles == 1)
                            {
                                accountBLL.Login(nric, account.roles[0]);
                                Session["toastr"] = "login";
                            }
                            else
                            {
                                accountBLL.Login(nric, "Multiple");
                            }

                            Response.Redirect("~/");
                        }
                        catch
                        {
                            HttpContext.Current.Cache.Remove(nric + "_MFAAttempt");
                            HttpContext.Current.Cache.Remove(nric + "_MFAAttempt_Password");
                            Session.Abandon();
                            FormsAuthentication.RedirectToLoginPage("fail-mfa=true");
                        }
                    }
                }
                catch
                {
                    TimerMFA.Enabled = false;
                    Session.Abandon();
                    FormsAuthentication.RedirectToLoginPage("fail-auth=true");
                }
            }
            else
            {
                int count = Convert.ToInt32(Session["countdown"]) - 1;
                LabelTimer.Text = count.ToString();

                Session["countdown"] = count;
            }
        }

        #region Helpers
        protected void buttonCloseModal_ServerClick(object sender, EventArgs e)
        {
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString());
            nameValueCollection.Remove("fail-auth");
            nameValueCollection.Remove("fail-mfa");
            nameValueCollection.Remove("multiple-logins");
            string url = HttpContext.Current.Request.Path + "?" + nameValueCollection;

            if (Master.IsLocalUrl(url))
            {
                Response.Redirect(url);
            }
        }
        private void AuthenticationError()
        {
            inputNRIC.Value = string.Empty;
            inputPassword.Value = string.Empty;
            inputNRIC.Attributes.Add("class", "form-control form-control-sm is-invalid");
            inputPassword.Attributes.Add("class", "form-control form-control-sm is-invalid");
            spanMessage.Visible = true;
        }
        #endregion
    }
}