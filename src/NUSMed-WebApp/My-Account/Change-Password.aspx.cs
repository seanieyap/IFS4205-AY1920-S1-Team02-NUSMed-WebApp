using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.My_Account
{
    public partial class Change_Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveChangePassword();
        }

        protected void buttonChangePassword_ServerClick(object sender, EventArgs e)
        {
            AccountBLL accountBLL = new AccountBLL();
            string passwordCurrent = inputPasswordCurrent.Value;
            string passwordNew = inputPasswordNew.Value;
            string passwordNewRepeat = inputPasswordNewRepeat.Value;

            #region Validation
            bool[] validate = Enumerable.Repeat(true, 2).ToArray();

            // If any fields are empty
            if (string.IsNullOrEmpty(passwordCurrent) || accountBLL.GetStatus(accountBLL.GetNRIC(), passwordCurrent).status == 0)
            {
                validate[0] = false;
                inputPasswordCurrent.Attributes.Add("class", "form-control is-invalid");
            }
            else
                inputPasswordCurrent.Attributes.Add("class", "form-control is-valid");

            if (string.IsNullOrEmpty(passwordNew) || string.IsNullOrEmpty(passwordNewRepeat)
                || !AccountBLL.IsPassword(passwordNew, passwordNewRepeat) || passwordCurrent.Equals(passwordNew))
            {
                validate[1] = false;
                inputPasswordNew.Attributes.Add("class", "form-control is-invalid");
                inputPasswordNewRepeat.Attributes.Add("class", "form-control is-invalid");
            }
            else
            {
                inputPasswordNew.Attributes.Add("class", "form-control is-valid");
                inputPasswordNewRepeat.Attributes.Add("class", "form-control is-valid");
            }

            if (!validate[0])
            {
                labelMessage.Text = "Current Password is incorrect.";
            }
            else if (!validate[1])
            {
                labelMessage.Text = "New password is invalid.";
            }
            #endregion

            if (validate.Contains(false))
            {
                spanMessage.Visible = true;
            }
            else
            {
                try
                {
                    accountBLL.ChangePassword(passwordNew);

                    spanMessage.Visible = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Password were successfully Updated');", true);
                    UpdatePanelChangePassword.Update();

                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when Updating Password');", true);
                }

            }
        }

    }
}