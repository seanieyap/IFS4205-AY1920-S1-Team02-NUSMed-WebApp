using NUSMed_WebApp.Classes.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Therapist.My_Patients
{
    public partial class New_Request : Page
    {
        private readonly AccountBLL accountBLL = new AccountBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyPatients();
            Master.LiActiveTherapistMyPatientsNewRequest();

            if (!IsPostBack)
            {
                string term = TextboxSearch.Text.Trim().ToLower();
                Bind_GridViewAccounts(term);
            }
        }

        #region GridViewAccounts Functions
        protected void Bind_GridViewAccounts(string term)
        {
            List<Classes.Entity.Account> accounts = accountBLL.GetAllPatients(term);
            ViewState["GridViewAccounts"] = accounts;
            GridViewAccounts.DataSource = accounts;
            GridViewAccounts.DataBind();
        }
        protected void GridViewAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string nric = GridViewAccounts.DataKeys[e.RowIndex].Values["nric"].ToString();

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
        protected void GridViewAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool acceptRequest = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "acceptNewRequest"));
                LinkButton linkButtonRequest = (LinkButton)e.Row.FindControl("LinkButtonRequest");

                if (acceptRequest)
                {
                    linkButtonRequest.CssClass = "view-modal btn btn-success btn-sm";
                    linkButtonRequest.Text = "<i class=\"fas fa-fw fa-user-friends\"></i>Request";
                }
                else
                {
                    linkButtonRequest.CssClass = "view-modal btn btn-secondary btn-sm disabled";
                    linkButtonRequest.Text = "Already Pending";
                }
            }
        }
        #endregion
    }
}