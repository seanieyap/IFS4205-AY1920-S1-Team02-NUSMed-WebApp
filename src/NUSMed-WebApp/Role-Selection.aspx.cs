using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp
{
    public partial class RoleSelection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountBLL accountBLL = new AccountBLL();
            Account account = accountBLL.GetStatus();

            if (account.patientStatus == 1)
            {
                buttonLoginPatient.Visible = true;
            }
            if (account.therapistStatus == 1)
            {
                buttonLoginTherapist.Visible = true;
            }
            if (account.researcherStatus == 1)
            {
                buttonLoginResearcher.Visible = true;
            }
            if (account.adminStatus == 1)
            {
                buttonLoginAdmin.Visible = true;
            }
        }

        protected void buttonLoginPatient_ServerClick(object sender, EventArgs e)
        {
            Select("Patient");
            Response.Redirect("~/Patient/Dashboard");
        }

        protected void buttonLoginTherapist_ServerClick(object sender, EventArgs e)
        {
            Select("Therapist");
            Response.Redirect("~/Therapist/Dashboard");
        }

        protected void buttonLoginResearcher_ServerClick(object sender, EventArgs e)
        {
            Select("Researcher");
            Response.Redirect("~/Researcher/Dashboard");
        }

        protected void buttonLoginAdmin_ServerClick(object sender, EventArgs e)
        {
            Select("Administrator");
            Response.Redirect("~/Admin/Dashboard");
        }

        protected void Select(string role)
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

                accountBLL.Login(nric, role);
            }
            catch
            {
                new AccountBLL().Logout();
                Response.Redirect("~/");
            }
        }
    }
}