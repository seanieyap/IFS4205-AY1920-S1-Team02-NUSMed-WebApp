﻿using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Load(object sender, EventArgs e)
        {
            AccountBLL accountBLL = new AccountBLL();
            if (accountBLL.IsAuthenticated())
            {
                if (accountBLL.IsMultiple())
                {
                    navLinksSwitchRole.Visible = false;
                    navLinksAccountProfile.Visible = false;
                    navLinksAccountChangePassword.Visible = false;
                }
                else if (accountBLL.IsPatient())
                {
                    navLinksPatientDashboard.Visible = true;
                    navLinksPatientTherapist.Visible = true;
                    navLinksPatientsMyRecords.Visible = true;
                }
                else if (accountBLL.IsTherapist())
                {
                    navLinksTherapistDashboard.Visible = true;
                    navLinksTherapistMyPatients.Visible = true;
                }
                else if (accountBLL.IsResearcher())
                {
                    navLinksResearcherDashboard.Visible = true;
                    navLinksResearcherAggregatedSearch.Visible = true;
                    navLinksResearcherRecordSearch.Visible = true;
                }
                else if (accountBLL.IsAdministrator())
                {
                    navLinksAdminDashboard.Visible = true;
                    navLinksAdminManageAccounts.Visible = true;
                    navLinksAdminManageLogs.Visible = true;
                }

                navLinksAccount.Visible = true;
                LabelNRIC.Text = new AccountBLL().GetNRIC();
                LabelRole.Text = new AccountBLL().GetRole();

                // Timeout Timer
                sessionWarningModal.Visible = true;
                sessionTimeoutModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "timer",
                    @"setTimeout(function() {$('.modal').modal('hide'); $('#sessionWarningModal').modal('show');}, " + (HttpContext.Current.Session.Timeout * 60000 - 60000).ToString() + @");
                    setTimeout(function() {$('.modal').modal('hide'); $('#sessionTimeoutModal').modal('show');}, " + (HttpContext.Current.Session.Timeout * 60000).ToString() + ");"
                    , true);
            }
            else
            {
                navLinksHome.Visible = true;
                //navLinksData.Visible = true;
                navLinksAbout.Visible = true;
            }

            // Toastr Notifications 
            if (Session["toastr"] != null)
            {
                if (string.Equals(Session["toastr"].ToString(), "logout"))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['info']('You have been Logged Out.');", true);
                    Session.Abandon();
                }
                else if (string.Equals(Session["toastr"].ToString(), "login"))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['info']('You have been Logged In as " + new AccountBLL().GetNRIC() + ".');", true);
                    Session.Remove("toastr");
                }
            }
        }

        #region UI
        #region My Account
        public void LiActiveMyProfile()
        {
            navLinksAccountProfile.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActiveChangePassword()
        {
            navLinksAccountChangePassword.Attributes.Add("class", "dropdown-item active");
        }
        #endregion
        #region Patient
        public void LiActivePatientDashboard()
        {
            navLinksPatientDashboard.Attributes.Add("class", "nav-link active");
        }
        public void LiActivePatientMyTherapists()
        {
            navLinksPatientTherapist.Attributes.Add("class", "nav-link active");
        }
        public void LiActivePatientMyRecords()
        {
            navLinksPatientMyRecordsDropdown.Attributes.Add("class", "nav-link active");
        }
        public void LiActivePatientMyRecordView()
        {
            navLinksPatientMyRecordView.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActivePatientMyRecordNew()
        {
            navLinksPatientMyRecordNew.Attributes.Add("class", "dropdown-item active");
        }
        #endregion
        #region Therapist
        public void LiActiveTherapistDashboard()
        {
            navLinksTherapistDashboard.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveTherapistMyPatients()
        {
            navLinksTherapistMyPatientsDropdown.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveTherapistMyPatientsView()
        {
            navLinksTherapistMyPatientsView.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActiveTherapistMyPatientsNewRequest()
        {
            navLinksTherapistMyPatientsNewRequest.Attributes.Add("class", "dropdown-item active");
        }
        #endregion
        #region Researcher
        public void LiActiveResearcherDashboard()
        {
            navLinksResearcherDashboard.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveResearcherAggregatedSearch()
        {
            navLinksResearcherDashboard.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveResearcherRecordSearch()
        {
            navLinksResearcherDashboard.Attributes.Add("class", "nav-link active");
        }
        #endregion
        #region Admin
        public void LiActiveAdminDashboard()
        {
            navLinksAdminDashboard.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveAdminManageLogs()
        {
            navLinksAdminManageLogsDropdown.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveAdminViewAccountLogs()
        {
            navLinksAdminViewAccountLogs.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActiveAdminViewRecordLogs()
        {
            navLinksAdminViewRecordLogs.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActiveAdminManageAccounts()
        {
            navLinksAdminManageAccountsDropdown.Attributes.Add("class", "nav-link active");
        }
        public void LiActiveAdminViewAccounts()
        {
            navLinksAdminViewAccounts.Attributes.Add("class", "dropdown-item active");
        }
        public void LiActiveAdminAccountRegistration()
        {
            navLinksAdminAccountRegistration.Attributes.Add("class", "dropdown-item active");
        }
        #endregion
        #endregion

        protected void ButtonLogout_ServerClick(object sender, EventArgs e)
        {
            new AccountBLL().Logout();

            // Set session for toastr notification
            Session["toastr"] = "logout";

            FormsAuthentication.RedirectToLoginPage();
        }

        protected void buttonSwitchRole_ServerClick(object sender, EventArgs e)
        {
            try
            {
                // Check if account has role
                AccountBLL accountBLL = new AccountBLL();
                Account account = accountBLL.GetStatus();

                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                string nric = authTicket.Name;
                List<string> userData = new List<string>(authTicket.UserData.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries));
                string guid = string.Empty;

                if (userData.Any()) //prevent IndexOutOfRangeException for empty list
                {
                    guid = userData.ElementAt(userData.Count - 1);
                    userData.RemoveAt(userData.Count - 1);
                }

                accountBLL.Login(nric, "Multiple");
                Response.Redirect("~/Role-Selection", false);
            }
            catch
            {
                new AccountBLL().Logout();
                Response.Redirect("~/");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches 
            //in the view state form field in the Page.PreLoad method.
            if (requestCookie != null
                && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,
                    Domain = FormsAuthentication.CookieDomain,
                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL &&
                    Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += Master_Page_PreLoad;
        }

        protected void Master_Page_PreLoad(object sender, EventArgs e)
        {
            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] =
                       Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] !=
                         (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of " +
                                        "Anti-XSRF token failed.");
                }
            }
        }
    }
}