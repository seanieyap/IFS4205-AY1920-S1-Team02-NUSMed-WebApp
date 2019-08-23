using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (new AccountBLL().IsAuthenticated())
            //{
            //    Response.Redirect("/admin-panel");
            //}
        }

        protected void ButtonLogin_ServerClick(object sender, EventArgs e)
        {
            //string[] loginID = inputLoginID.Value.Trim().Split('\\');
            //string domain = loginID[0];
            //string userID = loginID[1];

            ////string password = inputPassword.Value;
            //using (SecureString password = new SecureString())
            //{
            //    foreach (char c in inputPassword.Value)
            //    {
            //        password.AppendChar(c);
            //    }

            //    password.MakeReadOnly();
            //    AccountBLL accountBLL = new AccountBLL();

            //    if (!string.IsNullOrEmpty(userID)
            //        && !string.IsNullOrEmpty(password.ToString())
            //        && accountBLL.IsAuthenticated(userID, domain, AccountBLL.ConvertToUnsecureString(password)))
            //    {
            //        accountBLL.Login(userID);
            //        Session["toastr"] = "login";
            //        Response.Redirect("/admin-panel");
            //    }
            //    else
            //    {
            //        inputLoginID.Attributes.Add("class", "form-control is-invalid");
            //        inputPassword.Attributes.Add("class", "form-control is-invalid");
            //        spanMessage.Visible = true;
            //    }
            //}
        }
    }
}