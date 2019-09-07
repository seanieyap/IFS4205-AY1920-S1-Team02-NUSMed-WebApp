using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountBLL accountBLL = new AccountBLL();
            if (accountBLL.IsAuthenticated())
            {
                if (accountBLL.IsMultiple())
                {
                    Response.Redirect("~/Role-Selection");
                }
                else if (accountBLL.IsPatient())
                {
                    Response.Redirect("~/Patient/Dashboard");
                }
                else if (accountBLL.IsTherapist())
                {
                    Response.Redirect("~/Therapist/Dashboard");
                }
                else if (accountBLL.IsResearcher())
                {
                    Response.Redirect("~/Researcher/Dashboard");
                }
                else if (accountBLL.IsAdministrator())
                {
                    Response.Redirect("~/Admin/Dashboard");
                }
            }
            else if (Request.QueryString["fail-auth"] != null && Request.QueryString["fail-auth"].ToString().Equals("true"))
            {
                failAuthModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "fail-auth", "$('#failAuthModal').modal('show');", true);
            }
            else if (Request.QueryString["multiple-logins"] != null && Request.QueryString["multiple-logins"].ToString().Equals("true"))
            {

                multipleLoginsModal.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "Multiple logins detected", "$('#multipleLoginsModal').modal('show');", true);
            }
        }

        protected void ButtonLogin_ServerClick(object sender, EventArgs e)
        {
            string nric = inputNRIC.Value.Trim();

            using (SecureString password = new SecureString())
            {
                foreach (char c in inputPassword.Value)
                {
                    password.AppendChar(c);
                }

                password.MakeReadOnly();
                AccountBLL accountBLL = new AccountBLL();

                if (!string.IsNullOrEmpty(nric) && !string.IsNullOrEmpty(password.ToString()))
                {
                    Account account = accountBLL.GetStatus(nric, AccountBLL.ConvertToUnsecureString(password));

                    if (account.status == 0)
                    {
                        AuthenticationError();
                        return;
                    }
                    else
                    {
                        accountBLL.Update1FALogin(nric);

                        // Trigger MFA
                        if (account.status == 1)
                        {

                        }

                        // If omitted from mfa
                        else if (account.status == 2)
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
                    }

                }
                else
                {
                    AuthenticationError();
                }
            }
        }

        protected void buttonCloseModal_ServerClick(object sender, EventArgs e)
        {
            var nameValueCollection = HttpUtility.ParseQueryString(HttpContext.Current.Request.QueryString.ToString());
            nameValueCollection.Remove("fail-auth");
            nameValueCollection.Remove("multiple-logins");
            string url = HttpContext.Current.Request.Path + "?" + nameValueCollection;

            Response.Redirect(url);
        }

        #region Helpers
        private void AuthenticationError()
        {
            inputNRIC.Attributes.Add("class", "form-control is-invalid");
            inputPassword.Attributes.Add("class", "form-control is-invalid");
            spanMessage.Visible = true;
        }
        #endregion
    }
}